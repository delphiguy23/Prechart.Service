using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Models;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Email.Models;
using Prechart.Service.Globals.Helper;
using Prechart.Service.Globals.Interfaces;
using Prechart.Service.Globals.Interfaces.Documents;
using Prechart.Service.Globals.Interfaces.Loonheffings;
using Prechart.Service.Globals.Interfaces.Werkgever;
using Prechart.Service.Globals.Models.Email;
using Prechart.Service.Globals.Models.Klant;
using Prechart.Service.Globals.Models.Loonheffings;
using Prechart.Service.Globals.Models.Person;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Globals.Models.Xsd.Loonheffings;
using Prechart.Service.Loonheffings.Models;
using Prechart.Service.Loonheffings.Repositories;
using AdresBinnenlandType = Prechart.Service.Globals.Models.Loonheffings.Xsd.Xsd2022.AdresBinnenlandType;
using AdresBuitenlandType = Prechart.Service.Globals.Models.Loonheffings.Xsd.Xsd2022.AdresBuitenlandType;
using Gesl = Prechart.Service.Globals.Models.Loonheffings.Enums.Gesl;
using KlantModel = Prechart.Service.Loonheffings.Models.KlantModel;
using Loonaangifte = Prechart.Service.Globals.Models.Loonheffings.Xsd.Xsd2022.Loonaangifte;

namespace Prechart.Service.Loonheffings.Service;

public partial class LoonheffingsService : ILoonheffingsService
{
    private readonly IXsdHelper _helper;
    private readonly ILogger<LoonheffingsService> _logger;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILoonheffingsRepository _repository;

    public LoonheffingsService(ILogger<LoonheffingsService> logger,
        IPublishEndpoint publishEndpoint,
        IXsdHelper helper,
        IMapper mapper,
        ILoonheffingsRepository repository)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _helper = helper;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<IFluentResults<List<ProcessXmlResult>>> HandleAsync(ProcessUploadedXmls request, CancellationToken cancellationToken = default)
    {
        var sw = new Stopwatch();
        sw.Start();

        var results = new List<ProcessXmlResult>();
        var filename = string.Empty;

        try
        {
            if (await _repository.HandleAsync(new LoonheffingsRepository.GetUnProcessUploadedTaxFiling(), cancellationToken) is { } result
                && result.IsSuccess()
                && (result?.Value?.Any() ?? false)
                && (result?.Value.Any(v => v.Person is not null) ?? false))
            {
                try
                {
                    _logger.LogInformation("Start processing unprocessed taxfiling {@Unprocessed}", result.Value);

                    var onBoarding = new List<EmployeeOnBoardingModel>();
                    var klants = new List<KlantModel>();
                    var werkgevers = new List<IMongoWerkgever>();

                    foreach (var r in result.Value)
                    {
                        filename = r.FileName;

                        werkgevers.Add(new MongoWerkgeverModel
                        {
                            Collectieve = ExtractCollectieveAangifte(r),
                            Klant = new WerkgeverKlantModel
                            {
                                KlantId = ObjectId.GenerateNewId().ToString(),
                                KlantName = r.Klant,
                            },

                            Naam = r.TaxNo,
                            Sector = 0,
                            FiscaalNummer = r.TaxNo,
                            LoonheffingenExtentie = r.TaxNo.Length > 3 ? r.TaxNo[^3..] : string.Empty,
                            OmzetbelastingExtentie = r.TaxNo.Length > 3 ? "B01" : string.Empty,
                            DatumActiefVanaf = new DateTime(2022, 1, 1),
                            DatumActiefTot = new DateTime(2023, 12, 31),
                            Actief = true,
                        });

                        onBoarding.AddRange(ExtractEmployeesForOnBoarding(r));
                    }

                    await _publishEndpoint.Publish<IMongoWerkgevers>(new MongoWerkgeversModel {Werkgevers = werkgevers});

                    if (onBoarding.Any())
                    {
                        await _publishEndpoint.Publish<IXmlToPersons>(new {Persons = onBoarding});
                    }

                    _logger.LogInformation("End processing unprocessed taxfiling");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    results.Add(new ProcessXmlResult
                    {
                        FileName = filename,
                        Processed = false,
                        ProcessErrors = e.Message,
                        EmployeesInserted = 0,
                        EmployeesUpdated = 0,
                    });
                }
            }

            await _repository.HandleAsync(new LoonheffingsRepository.GetUnProcessUploadedTaxFilingWithoutEmployees(), cancellationToken);

            sw.Stop();
            _logger.LogInformation($"Time elapsed: {sw.Elapsed.TotalSeconds} s");
            return ResultsTo.Success(results);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            sw.Stop();
            _logger.LogInformation($"Time elapsed: {sw.Elapsed.TotalSeconds} s");
            return ResultsTo.Failure<List<ProcessXmlResult>>().FromException(e);
        }
    }

    public async Task<IFluentResults<UploadXmlResults>> HandleAsync(ValidateXml request, CancellationToken cancellationToken = default)
    {
        try
        {
            var sendXml = new List<SendMailXml>();
            _logger.LogInformation($"Start {nameof(ValidateXml)}");
            if (request.Files is null || !request.Files.Any() || request.XsdYear == 0)
            {
                _logger.LogInformation($"End {nameof(ValidateXml)}, No Files or Year provided");
                return ResultsTo.NotFound<UploadXmlResults>().WithMessage("Argument is null or empty");
            }

            if (_helper.GetSchemaStream($"Prechart.Service.Globals.Models.Loonheffings.Xsd.Files.Loonaangifte{request.XsdYear}v2.0.xsd") is null)
            {
                return ResultsTo.Failure<UploadXmlResults>().WithMessage($"No XSD Schema found for {request.XsdYear}");
            }

            var xmlResults = new List<ILoonaangifteUploadResult>();

            foreach (var file in request.Files)
            {
                _logger.LogInformation($"Validating XmlFile {file.FileName}");

                var results = await _helper.Validate(file.Stream, request.XsdYear);

                xmlResults.Add(new UploadXmlResult
                {
                    FileName = file.FileName,
                    IsValid = !results?.Any() ?? false,
                    Errors = results is null ? new List<string> {"Validator returns null."} : results.Any() ? results.Select(r => r).ToList() : null,
                });

                sendXml.Add(new SendMailXml
                {
                    FileName = file.FileName,
                    Stream = file.Stream,
                    Error = results is null ? "Validator returns null." : results.Any() ? results.Select(r => r).FirstOrDefault() : null,
                    IsValid = !results?.Any() ?? false,
                });

                if (results?.Any() ?? true)
                {
                    continue;
                }

                var serializedXml = new XmlSerializer(typeof(Loonaangifte));

                var ms = new MemoryStream(file.Stream);

                await _repository.HandleAsync(new LoonheffingsRepository.UpsertTaxFiling2022
                {
                    TaxFiling = new XmlLoonaangifteUpload
                    {
                        FileName = file.FileName,
                        IsValid = !results?.Any() ?? false,
                        Errors = results?.Any() ?? false ? results : null,
                        Loonaangifte = (Loonaangifte) serializedXml.Deserialize(ms) ?? null,
                        EmployeesInserted = 0,
                        EmployeesUpdated = 0,
                        Processed = false,
                        UploadedDate = DateTime.Now,
                    },
                }, cancellationToken);
            }

            await SendEmailNotification("XmlOnBoardingProcess", "XML Validations.", EmailEventType.XmlOnBoardingProcess, sendXml);

            return ResultsTo.Success(new UploadXmlResults {Status = xmlResults});
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<UploadXmlResults>().FromException(e);
        }
    }

    public async Task<IFluentResults<UpdateResult>> HandleAsync(LoonheffingsProcessedResult request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Start {nameof(LoonheffingsProcessedResult)}");

            return ResultsTo.Something(await _repository.HandleAsync(new LoonheffingsRepository.UpdateProcessedXml
            {
                FileName = request.FileName,
                EmployeesInserted = request.EmployeesInserted,
                EmployeesUpdated = request.EmployeesUpdated,
                Errors = request.ProcessErrors,
            }, cancellationToken));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<UpdateResult>().FromException(e);
        }
    }

    private List<CollectieveAangifteModel> ExtractCollectieveAangifte(UnprocessedUploads request)
    {
        if (request?.CollectieveAangifteCorrection is null && request?.CollectieveAangifteNormal is null)
        {
            return null;
        }

        var collectives = _mapper.Map<CollectieveAangifteModel>(request?.CollectieveAangifteNormal ?? request?.CollectieveAangifteCorrection);
        collectives.TaxNo = request.TaxNo;
        collectives.Periode = $"{request.PeriodEnd.Year}-{request.PeriodEnd.Month:00}";
        collectives.ProcessedDate = request.TaxFileProcessDate;
        collectives.CollectieveType = request?.CollectieveAangifteNormal is not null ? CollectieveType.Normaal : CollectieveType.Correctie;

        return new List<CollectieveAangifteModel> {collectives};
    }

    private List<EmployeeOnBoardingModel> ExtractEmployeesForOnBoarding(UnprocessedUploads unprocessed)
    {
        var onBoarding = new List<EmployeeOnBoardingModel>();

        if (unprocessed?.Person is null || !unprocessed.Person.Any())
        {
            return onBoarding;
        }

        foreach (var empDetail in unprocessed.Person)
        {
            var e = empDetail.NatuurlijkPersoon;
            var employee = new EmployeeOnBoardingModel
            {
                SofiNr = e.SofiNr,
                Voorletter = e.Voorl,
                Voorvoegsel = e.Voorv,
                SignificantAchternaam = e.SignNm,
                Geboortedatum = e.Gebdat,
                Nationaliteit = e.Nat,
                Geslacht = (Gesl) e.Gesl,
                AdresBinnenland = e.Item is AdresBinnenlandType ? _mapper.Map<Globals.Models.Xsd.Loonheffings.AdresBinnenlandType>(e.Item as AdresBinnenlandType) : null,
                AdresBuitenland = e.Item is AdresBuitenlandType ? _mapper.Map<Globals.Models.Xsd.Loonheffings.AdresBuitenlandType>(e.Item as AdresBuitenlandType) : null,
                PersonType = PersonType.Employee,
                Werkgever = new List<KlantWerkgeverModel>
                {
                    new()
                    {
                        Klant = unprocessed.Klant,
                        LoonheffingsNr = unprocessed.TaxNo,
                    },
                },
                TaxPaymentDetails = ExtractEmployeesCalculatedAmount(unprocessed, empDetail),
                TaxFileName = unprocessed.FileName,
            };

            onBoarding.Add(employee);
        }

        return onBoarding;
    }

    private List<TaxPaymentDetails> ExtractEmployeesCalculatedAmount(UnprocessedUploads unprocessed, NatuurlijkPersoonDetails details)
    {
        var numIvs = GetDistinctNumIv(unprocessed);

        var taxPaymentDetails = new List<TaxPaymentDetails>
        {
            new()
            {
                TaxNo = unprocessed.TaxNo,
                NumIv = details.NumIV,
                PersonNr = details.PersNr,
                TaxFileProcessDate = unprocessed.TaxFileProcessDate,
                TaxPeriode = $"{unprocessed.PeriodEnd.Year}-{unprocessed.PeriodEnd.Month:00}",
                CollectieveType = details.CollectieveType,
                Inkomstenperiode = _mapper.Map<List<InkomstenPeriodeModel>>(details.Inkomstenperiode),
                Werknemersgegevens = _mapper.Map<WerknemersgegevensType>(details.Werknemersgegevens),
            },
        };

        return taxPaymentDetails;
    }

    private static List<string> GetDistinctNumIv(UnprocessedUploads unprocessed)
    {
        return unprocessed.Person.Select(p => p.NumIV).Distinct().ToList();
    }

    private static List<NatuurlijkPersoonDetails> GetPersonsByNumIv(string numIv, UnprocessedUploads unprocessed)
    {
        return unprocessed.Person.Where(p => p.NumIV == numIv).ToList();
    }

    private async Task SendEmailNotification(string subject, string body, EmailEventType emailEventType, List<SendMailXml> sendMailXml)
    {
        try
        {
            var emailBody = body;
            List<EmailAttachmentModel> files = new();

            if (sendMailXml is not null)
            {
                emailBody = EmailHelper.EmailBodyCreator(body, "Filename;Error").ToString().Replace("{__emailbody__}", CreateSendMailXmlEmailBody(sendMailXml));

                foreach (var attachment in sendMailXml)
                {
                    files.Add(new EmailAttachmentModel {Filename = attachment.FileName, FileData = attachment.Stream});
                }
            }

            await _publishEndpoint.Publish<IEmailEvent>(
                new EmailEventModel
                {
                    Subject = subject,
                    Body = emailBody,
                    EmailEventType = emailEventType,
                    Attachments = null,
                });
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }

    private string CreateSendMailXmlEmailBody(List<SendMailXml> sendMailXml)
    {
        var body = new StringBuilder();
        var formatProvider = CultureInfo.InvariantCulture;

        try
        {
            if (sendMailXml is null)
            {
                return body.ToString();
            }

            foreach (var c in sendMailXml)
            {
                body.AppendLine(@"<tr>");
                body.AppendLine(formatProvider, $@"<td>{c.FileName}</td>");
                body.AppendLine(formatProvider, $@"<td>{c.Error}</td>");
                body.AppendLine(@"</TR>");
            }
        }
        catch (Exception ex)
        {
            body.Clear();
            _logger.LogError(ex, "Error Occurred during the creation of email body");
        }

        return body.ToString();
    }
}
