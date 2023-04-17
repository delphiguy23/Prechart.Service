using AutoMapper;
using HandlebarsDotNet;
using HandlebarsDotNet.Helpers;
using HandlebarsDotNet.Helpers.Enums;
using MassTransit;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Globals.Interfaces.Klant;
using Prechart.Service.Globals.Models;
using Prechart.Service.Globals.Models.Loonheffings;
using Prechart.Service.Globals.Models.Person;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Werkgever.Extensions;
using Prechart.Service.Werkgever.Models;
using Prechart.Service.Werkgever.Repository;

namespace Prechart.Service.Werkgever.Service;

public partial class WerkgeverService : IWerkgeverService
{
    private readonly IRequestClient<IGetKlant> _getKlantEvent;
    private readonly ILogger<WerkgeverService> _logger;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IWerkgeverRepository _repository;

    public WerkgeverService(ILogger<WerkgeverService> logger,
        IWerkgeverRepository repository,
        IMapper mapper,
        IRequestClient<IGetKlant> getKlantEvent,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _getKlantEvent = getKlantEvent;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<IFluentResults<List<MongoWerkgeverModel>>> HandleAsync(GetMongoWerkgever request, CancellationToken cancellationToken = default)
    {
        try
        {
            var werkgevers = await _repository.HandleAsync(new WerkgeverRepository.GetMongoWerkgever {Taxno = request.Taxno}, cancellationToken);

            if (werkgevers.IsNotFoundOrBadRequest() || werkgevers.IsFailure())
            {
                return ResultsTo.Something(werkgevers);
            }

            foreach (var werkgever in werkgevers.Value)
            {
                if (werkgever.WhkPremies is null)
                {
                    continue;
                }

                foreach (var whkPremie in werkgever.WhkPremies)
                {
                    whkPremie.WgaVastWerkgever = whkPremie.WgaVastWerkgever.ConvertToPercentage();
                    whkPremie.WgaVastWerknemer = whkPremie.WgaVastWerknemer.ConvertToPercentage();
                    whkPremie.FlexWerkgever = whkPremie.FlexWerkgever.ConvertToPercentage();
                    whkPremie.FlexWerknemer = whkPremie.FlexWerknemer.ConvertToPercentage();
                    whkPremie.ZwFlex = whkPremie.ZwFlex.ConvertToPercentage();
                }

                if (werkgever.Collectieve is null)
                {
                    continue;
                }

                foreach (var collectieve in werkgever.Collectieve ?? new List<CollectieveAangifteModel>())
                {
                    collectieve.TotSaldo = collectieve.SaldoCorrectiesVoorgaandTijdvak.Sum(s => s.Saldo);
                }
            }

            return ResultsTo.Something(werkgevers);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<List<MongoWerkgeverModel>>().FromException(e);
        }
    }

    public async Task<IFluentResults<List<MongoWerkgeverModel>>> HandleAsync(UpsertMongoWerkgevers request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.HandleAsync(new WerkgeverRepository.UpsertWerkgevers
            {
                Werkgevers = request.Werkgevers,
            }, cancellationToken);

            if (result.IsNotFoundOrBadRequest())
            {
                return ResultsTo.Something(result);
            }

            foreach (var mongoWerkgeverModel in result.Value)
            {
                if (mongoWerkgeverModel.Klant is null)
                {
                    continue;
                }

                var klants = result.Value.Select(r =>
                    new KlantModel
                    {
                        Id = r.Klant.KlantId.ToObjectId(),
                        KlantNaam = r.Klant.KlantName,
                        Werkgevers = new List<string> {r.FiscaalNummer},
                        ContactPersons = new List<PersonModel>(),
                        Active = true,
                        DateCreated = DateTime.Now,
                    }).ToList();

                await _publishEndpoint.Publish<IXmlToKlants>(new
                {
                    Klants = klants,
                });
            }

            await HandleAsync(new SyncFromMongo
            {
                Werkgevers = result.Value,
            }, CancellationToken.None);

            return ResultsTo.Something(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<List<MongoWerkgeverModel>>().FromException(e);
        }
    }

    public async Task<IFluentResults<bool>> HandleAsync(SyncFromSql request, CancellationToken cancellationToken = default)
    {
        try
        {
            var mongoWerkgevers = new List<MongoWerkgeverModel>();

            if (await _repository.HandleAsync(new WerkgeverRepository.GetSqlWerkgevers(), cancellationToken) is { } werkgevers && werkgevers.IsSuccess())
            {
                var _werkgevers = werkgevers.Value.ToList();
                foreach (var werkgever in _werkgevers)
                {
                    _logger.LogInformation(werkgever?.KlantMongoId ?? "");
                    if (await _repository.HandleAsync(new WerkgeverRepository.GetSqlWerkgeversWhkPremies
                            {WerkgeverId = werkgever.Id}, cancellationToken) is { } whk &&
                        whk.IsSuccess() && whk.Value is not null && whk.Value.Any())
                    {
                        var mongoWerkgever = _mapper.Map<MongoWerkgeverModel>(werkgever);

                        var response = await _getKlantEvent.GetResponse<IKlantResponse, NotFound>(new
                            {
                                KlantId = string.IsNullOrWhiteSpace(werkgever.KlantMongoId) ? string.Empty : werkgever.KlantMongoId,
                                KlantName = string.Empty,
                                TaxNo = werkgever.FiscaalNummer,
                            },
                            timeout: RequestTimeout.After(m: 5));

                        if (response.Is(out Response<IKlantResponse> klantResponse))
                        {
                            mongoWerkgever.Klant = new WerkgeverKlantModel
                            {
                                KlantId = klantResponse.Message.Id,
                                KlantName = klantResponse.Message.KlantNaam,
                            };
                        }

                        mongoWerkgever.WhkPremies = whk.Value.Select(x => _mapper.Map<MongoWhkPremie>(x)).ToList();

                        mongoWerkgevers.Add(mongoWerkgever);
                    }
                }

                _logger.LogInformation("test");
            }

            if (mongoWerkgevers.Any())
            {
                await HandleAsync(new UpsertMongoWerkgevers
                {
                    Werkgevers = mongoWerkgevers,
                }, CancellationToken.None);
            }

            return ResultsTo.Success(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults<bool>> HandleAsync(UpsertMongoWhk request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.HandleAsync(new WerkgeverRepository.UpsertMongoWhk
            {
                WerkgeverId = request.WerkgeverId,
                Id = request.Id,
                WgaVastWerkgever = request.WgaVastWerkgever.ConvertToRawPercentage(),
                WgaVastWerknemer = request.WgaVastWerknemer.ConvertToRawPercentage(),
                FlexWerkgever = request.FlexWerkgever.ConvertToRawPercentage(),
                FlexWerknemer = request.FlexWerknemer.ConvertToRawPercentage(),
                ZwFlex = request.ZwFlex.ConvertToRawPercentage(),
                ActiefVanaf = request.ActiefVanaf,
                ActiefTot = request.ActiefTot,
                SqlId = request.SqlId,
                Actief = request.Actief,
            }, cancellationToken);

            return ResultsTo.Something(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults<List<CollectieveAangifteModel>>> HandleAsync(GetCollectieve request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.TaxNo))
            {
                return ResultsTo.BadRequest<List<CollectieveAangifteModel>>("TaxNo is required").WithMessage("Argument is null or empty");
            }

            var result = await _repository.HandleAsync(new WerkgeverRepository.GetCollectieve
            {
                TaxNo = request.TaxNo,
            }, cancellationToken);

            if (result.IsNotFoundOrBadRequest())
            {
                return ResultsTo.Something(result);
            }

            var collectieves = new List<CollectieveAangifteModel>();

            foreach (var periode in result.Value.Select(r => r.Periode).Distinct())
            {
                var periodeCollectieve = result.Value.Where(r => r.Periode == periode).OrderByDescending(r => r.ProcessedDate).FirstOrDefault();

                if (periodeCollectieve is not null)
                {
                    collectieves.Add(periodeCollectieve);
                }
            }

            return ResultsTo.Something(collectieves.OrderByDescending(c => c.Periode).ToList());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<List<CollectieveAangifteModel>>().FromException(e);
        }
    }

    public async Task<IFluentResults<bool>> HandleAsync(UpdateKlantWerkgever request, CancellationToken cancellationToken = default)
    {
        try
        {
            return ResultsTo.Something(await _repository.HandleAsync(new WerkgeverRepository.UpdateKlantWerkgever
            {
                KlantId = request.KlantId,
                KlantName = request.KlantName,
                TaxNo = request.TaxNo,
            }, cancellationToken));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults<string>> HandleAsync(Print request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request?.TaxNo))
        {
            return ResultsTo.BadRequest<string>().WithMessage("Invalid Argument");
        }

        var result = await HandleAsync(new GetCollectieve
        {
            TaxNo = request.TaxNo,
        }, CancellationToken.None);

        var werkgever = HeaderTemplate() + CollectieveHeaderTemplate() + CollectieveDataTemplate();

        var handlebarsContext = Handlebars.Create();
        HandlebarsHelpers.Register(handlebarsContext, Category.String);
        var template = handlebarsContext.Compile(werkgever);

        if (result.Value.GroupByTaxNumber().FirstOrDefault(g => g.TaxNo == request.TaxNo) is { } grouped)
        {
            var html = template(grouped);
            return ResultsTo.Something(html);
        }

        return ResultsTo.NotFound<string>();
    }

    public async Task<IFluentResults<bool>> HandleAsync(SyncFromMongo request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (request.Werkgevers is null || !request.Werkgevers.Any())
            {
                return ResultsTo.BadRequest<bool>("Werkgevers is required").WithMessage("Argument is null or empty");
            }

            var werkgevers = request.Werkgevers;

            foreach (var werkgever in werkgevers)
            {
                if (await _repository.HandleAsync(new WerkgeverRepository.UpsertFromMongoWerkgever
                    {
                        Werkgever = werkgever,
                    }, cancellationToken) is { } werkgeverId)
                {
                    await _repository.HandleAsync(new WerkgeverRepository.UpsertFromMongoWerkgeverWhk
                    {
                        Werkgever = werkgever,
                        WerkgeverId = werkgeverId.Value,
                    }, cancellationToken);
                }
            }

            return ResultsTo.Success(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    private static string HeaderTemplate()
    {
        var header =
            @"
            <h1 style=""text-align: center;"">Wergever Information</h1>
            <table style=""height: 64px; width: 100%; border-collapse: collapse; border-style: solid;"" border=""1"">
            <tbody>
            <tr style=""height: 18px;"">
            <td style=""width: 20%; height: 10px; border-style: none;"">Naam</td>
            <td style=""width: 20%; height: 10px; border-style: none;"">{{TaxNo}}</td>
            <td style=""width: 20%; height: 10px; border-style: none;"">
            <div class=""col-6"">Loonheffingen Extentie</div>
            </td>
            <td style=""width: 20%; height: 10px; border-style: none;"">&nbsp;</td>
            </tr>
            <tr style=""height: 18px;"">
            <td style=""width: 20%; height: 18px; border-style: none;"">Sector</td>
            <td style=""width: 20%; height: 18px; border-style: none;"">&nbsp;</td>
            <td style=""width: 20%; height: 18px; border-style: none;"">Omzetbelasting Extentie</td>
            <td style=""width: 20%; height: 18px; border-style: none;"">&nbsp;</td>
            </tr>
            <tr style=""height: 18px;"">
            <td style=""width: 20%; height: 18px; border-style: none;"">Fiscaalnummer</td>
            <td style=""width: 20%; height: 18px; border-style: none;"">{{TaxNo}}</td>
            <td style=""width: 20%; height: 18px; border-style: none;"">
            <div class=""col-6"">Datum Actief Vanaf</div>
            </td>
            <td style=""width: 20%; height: 18px; border-style: none;"">&nbsp;</td>
            </tr>
            <tr style=""height: 18px;"">
            <td style=""width: 20%; height: 18px; border-style: none;"">Klantnaam</td>
            <td style=""width: 20%; height: 18px; border-style: none;"">&nbsp;</td>
            <td style=""width: 20%; height: 18px; border-style: none;"">
            <div class=""col-6"">Datum Actief Tot</div>
            </td>
            <td style=""width: 20%; height: 18px; border-style: none;"">&nbsp;</td>
            </tr>
            </tbody>
            </table>
            <p>&nbsp;</p>
        ";

        return header;
    }

    private static string CollectieveHeaderTemplate()
    {
        var template =
            @"
            <h3>Collectieve Aangifte</h3>
            ";
        return template;
    }

    private static string CollectieveDataTemplate()
    {
        var template =
            @"
            {{#each CollectieveAangifte}}
                <table style=""border-collapse: collapse; width: 100%;"" border=""1"">
                <tbody>
                <tr>
                <td style=""width: 33.333333%;"">TaxPeriode:{{Periode}}</td>
                <td style=""width: 33.333333%;"">Date:{{ProcessedDate}}</td>
                <td style=""width: 33.333333%;"">Type:{{CollectieveType}}</td>
                </tr>
                </tbody>
                </table>
                <p>&nbsp;</p>

                <table style=""border-collapse: collapse; width: 100%; height: 270px;"" border=""1"">
                <tbody>
                <tr style=""height: 18px;"">
                <td style=""width: 25%; height: 18px;""><strong>TotLnLbPh</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotLnLbPh \""#,###,##0.00\""}}</td>
                <td style=""width: 25%; height: 18px;""><strong>VrlAvso</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format VrlAvso \""#,###,##0.00\""}}</td>
                </tr>
                <tr style=""height: 18px;"">
                <td style=""width: 25%; height: 18px;""><strong>TotLnSv</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotLnSv \""#,###,##0.00\""}}</td>
                <td style=""width: 25%; height: 18px;""><strong>TotPrAofLg</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotPrAofLg \""#,###,##0.00\""}}</td>
                </tr>
                <tr style=""height: 18px;"">
                <td style=""width: 25%; height: 18px;""><strong>TotPrlnAofAnwLg</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotPrlnAofAnwLg \""#,###,##0.00\""}}</td>
                <td style=""width: 25%; height: 18px;""><strong>TotPrAofHg</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotPrAofHg \""#,###,##0.00\""}}</td>
                </tr>
                <tr style=""height: 18px;"">
                <td style=""width: 25%; height: 18px;""><strong>TotPrlnAofAnwHg</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotPrlnAofAnwHg \""#,###,##0.00\""}}</td>
                <td style=""width: 25%; height: 18px;""><strong>TotPrAofUit</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotPrAofUit \""#,###,##0.00\""}}</td>
                </tr>
                <tr style=""height: 18px;"">
                <td style=""width: 25%; height: 18px;""><strong>TotPrlnAofAnwUit</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotPrlnAofAnwUit \""#,###,##0.00\""}}</td>
                <td style=""width: 25%; height: 18px;""><strong>TotOpslWko</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotOpslWko \""#,###,##0.00\""}}</td>
                </tr>
                <tr style=""height: 18px;"">
                <td style=""width: 25%; height: 18px;""><strong>TotPrlnAwfAnwLg</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotPrlnAwfAnwLg \""#,###,##0.00\""}}</td>
                <td style=""width: 25%; height: 18px;""><strong>TotPrGediffWhk</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotPrGediffWhk \""#,###,##0.00\""}}</td>
                </tr>
                <tr style=""height: 18px;"">
                <td style=""width: 25%; height: 18px;""><strong>TotPrlnAwfAnwHg</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotPrlnAwfAnwHg \""#,###,##0.00\""}}</td>
                <td style=""width: 25%; height: 18px;""><strong>TotPrAwfLg</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotPrAwfLg \""#,###,##0.00\""}}</td>
                </tr>
                <tr style=""height: 18px;"">
                <td style=""width: 25%; height: 18px;""><strong>TotPrlnAwfAnwHz</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotPrlnAwfAnwHz \""#,###,##0.00\""}}</td>
                <td style=""width: 25%; height: 18px;""><strong>TotPrAwfHg</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotPrAwfHg \""#,###,##0.00\""}}</td>
                </tr>
                <tr style=""height: 18px;"">
                <td style=""width: 25%; height: 18px;""><strong>PrLnUFO</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format PrLnUFO \""#,###,##0.00\""}}</td>
                <td style=""width: 25%; height: 18px;""><strong>TotPrAwfHz</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotPrAwfHz \""#,###,##0.00\""}}</td>
                </tr>
                <tr style=""height: 18px;"">
                <td style=""width: 25%; height: 18px;""><strong>IngLbPh</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format IngLbPh \""#,###,##0.00\""}}</td>
                <td style=""width: 25%; height: 18px;""><strong>PrUfo</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format PrUfo \""#,###,##0.00\""}}</td>
                </tr>
                <tr style=""height: 18px;"">
                <td style=""width: 25%; height: 18px;""><strong>EHPubUitk</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format EHPubUitk \""#,###,##0.00\""}}</td>
                <td style=""width: 25%; height: 18px;""><strong>IngBijdrZvw</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format IngBijdrZvw \""#,###,##0.00\""}}</td>
                </tr>
                <tr style=""height: 18px;"">
                <td style=""width: 25%; height: 18px;""><strong>EHGebrAuto</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format EHGebrAuto \""#,###,##0.00\""}}</td>
                <td style=""width: 25%; height: 18px;""><strong>TotWghZvw</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotWghZvw \""#,###,##0.00\""}}</td>
                </tr>
                <tr style=""height: 18px;"">
                <td style=""width: 25%; height: 18px;""><strong>EHVUT</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format EHVUT \""#,###,##0.00\""}}</td>
                <td style=""width: 25%; height: 18px;""><strong>TotTeBet</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotTeBet \""#,###,##0.00\""}}</td>
                </tr>
                <tr style=""height: 18px;"">
                <td style=""width: 25%; height: 18px;""><strong>EhOvsFrfWrkkstrg</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format EhOvsFrfWrkkstrg \""#,###,##0.00\""}}</td>
                <td style=""width: 25%; height: 18px;""><strong>TotSaldo</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotSaldo \""#,###,##0.00\""}}</td>
                </tr>
                <tr style=""height: 18px;"">
                <td style=""width: 25%; height: 18px;""><strong>AVZeev</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format AVZeev \""#,###,##0.00\""}}</td>
                <td style=""width: 25%; height: 18px;""><strong>TotGen</strong></td>
                <td style=""width: 25%; height: 18px; text-align: right;"">{{String.Format TotGen \""#,###,##0.00\""}}</td>
                </tr>
                </tbody>
                </table>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
            {{/each}}        
            ";

        return template;
    }
}
