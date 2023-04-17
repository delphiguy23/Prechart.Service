using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Prechart.Service.Belastingen.Database.Models;
using Prechart.Service.Belastingen.Models;
using Prechart.Service.Belastingen.Repositories.BelastingTabellenWitGroen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Documents.Upload.Models;
using Prechart.Service.Globals.Helper;
using Prechart.Service.Globals.Models.Batch;
using Prechart.Service.Globals.Models.Belastingen;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Belastingen.Services.BelastingTabellenWitGroen;

public partial class BelastingTabellenWitGroenService : IBelastingTabellenWitGroenService
{
    private readonly IBatchHelper _batchHelper;
    private readonly ILogger<BelastingTabellenWitGroenService> _logger;
    private readonly IBelastingTabellenWitGroenRepository _repository;

    public BelastingTabellenWitGroenService(ILogger<BelastingTabellenWitGroenService> logger,
        IBelastingTabellenWitGroenRepository repository,
        IBatchHelper batchHelper)
    {
        _logger = logger;
        _repository = repository;
        _batchHelper = batchHelper;
    }

    public async Task<IFluentResults<BerekenInhoudingModel>> HandleAsync(GetInhouding request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                return ResultsTo.NotFound<BerekenInhoudingModel>();
            }

            if (!new[] { 0, 1, 2, 3, 4, 5 }.Contains((int)request.Loontijdvak))
            {
                return ResultsTo.Failure<BerekenInhoudingModel>("Invalid Type Id");
            }

            Func<GetInhouding, Task<List<BerekenInhoudingModel>>> inhoudingRecord = null;
            var inkomenValues = (White: request.InkomenWit, Green: request.InkomenGroen);

            inhoudingRecord = inkomenValues switch
            {
                ( < 0M or > 0M, 0M) => GetTaxRecordTypeWhite,
                (0M, < 0M or > 0M) => GetTaxRecordTypeGreen,
                ( < 0M or > 0M, < 0M or > 0M) => GetTaxRecordTypeBoth,
                _ => null,
            };

            if (inhoudingRecord is null)
            {
                return ResultsTo.NotFound<BerekenInhoudingModel>();
            }

            var resultInhoudings = await inhoudingRecord(request);
            switch (inkomenValues)
            {
                case ( < 0M or > 0M, 0M): //white
                    if (resultInhoudings is not null && resultInhoudings.Any())
                    {
                        var resultWhite = resultInhoudings.FirstOrDefault(i => i.InhoudingType == TaxRecordType.White);
                        resultWhite.NettoBetaling = request.InkomenWit + request.InkomenGroen - (resultWhite.InhoudingWit + resultWhite.InhoudingGroen) + resultWhite.ArbeidsKorting;

                        return ResultsTo.Something(resultWhite);
                    }

                    break;
                case (0M, < 0M or > 0M): //green
                    if (resultInhoudings is not null && resultInhoudings.Any())
                    {
                        var resultGreen = resultInhoudings.FirstOrDefault(i => i.InhoudingType == TaxRecordType.Green);
                        resultGreen.NettoBetaling = request.InkomenWit + request.InkomenGroen - (resultGreen.InhoudingWit + resultGreen.InhoudingGroen) + resultGreen.ArbeidsKorting;
                        return ResultsTo.Something(resultGreen);
                    }

                    break;
                case ( < 0M or > 0M, < 0M or > 0M): //both
                    if (resultInhoudings is not null && resultInhoudings.Any())
                    {
                        var inhWit = resultInhoudings.FirstOrDefault(i => i.InhoudingType == TaxRecordType.Both)?.InhoudingWit;
                        var ahk = resultInhoudings.FirstOrDefault(i => i.InhoudingType == TaxRecordType.Both)?.AlgemeneHeffingsKorting;

                        var result = resultInhoudings.Where(i => i.InhoudingType == TaxRecordType.White).Select(i =>
                            new BerekenInhoudingModel
                            {
                                InhoudingWit = i.InhoudingWit,
                                InhoudingGroen = (inhWit ?? 0M) - i.InhoudingWit,
                                AlgemeneHeffingsKorting = ahk ?? 0M,
                                ArbeidsKorting = i.ArbeidsKorting,
                                AlgemeneHeffingsKortingIndicator = i.AlgemeneHeffingsKortingIndicator,
                                InhoudingType = TaxRecordType.Both,
                                WoonlandbeginselId = request.WoondlandBeginselId,
                                WoonlandbeginselNaam = i.WoonlandbeginselNaam,
                            }).FirstOrDefault();

                        result.NettoBetaling = request.InkomenWit + request.InkomenGroen - (result.InhoudingWit + result.InhoudingGroen) + result.ArbeidsKorting;
                        return ResultsTo.Success(result);
                    }

                    break;
            }

            return ResultsTo.NotFound<BerekenInhoudingModel>();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            return ResultsTo.Failure<BerekenInhoudingModel>(e.Message);
        }
    }

    public async Task<IFluentResults<bool>> HandleAsync(UpsertToTable request, CancellationToken cancellationToken)
    {
        if (request.TaxTable is null || request.TaxType is null)
        {
            return ResultsTo.Success(false);
        }

        var result = await _repository.HandleAsync(new BelastingTabellenWitGroenRepository.UpsertToTable
        { TaxTable = request.TaxTable, TaxType = request.TaxType }, CancellationToken.None);

        return result?.Value == null ? ResultsTo.Success(false) : ResultsTo.Success((result?.Value > 0));
    }

    public async Task<IFluentResults<List<Woonlandbeginsel>>> HandleAsync(GetAlleWoonlandbeginsel request, CancellationToken cancellationToken)
    {
        return await _repository.HandleAsync(new BelastingTabellenWitGroenRepository.GetWoonlandbeginsel(), CancellationToken.None);
    }

    public async Task<IFluentResults<List<int>>> HandleAsync(GetTaxYear request, CancellationToken cancellationToken = default)
    {
        return await _repository.HandleAsync(new BelastingTabellenWitGroenRepository.GetTaxYear(), cancellationToken);
    }

    public async Task<IFluentResults<bool>> HandleAsync(ProcessPendingCsv request, CancellationToken cancellationToken)
    {
        var woonlands = await HandleAsync(new GetAlleWoonlandbeginsel(), CancellationToken.None);

        if (request?.BatchRecords is null || woonlands?.Value is null)
        {
            return ResultsTo.BadRequest<bool>().WithMessage("BatchRecords or Woonlandbeginsel is null");
        }

        var record = request.BatchRecords;
        await _batchHelper.UpdateBatchStatus(record.Id.ToString(), BatchProcessStatus.CurrentlyProcessing);

        try
        {
            var payload = JsonConvert.DeserializeObject<CsvBatchRecord>(record.Payload);

            if (payload is null || payload.CsvFile is null)
            {
                await _batchHelper.UpdateBatchErrors(record.Id.ToString(), new List<string> { "Unable to deserialize message." });
                await _batchHelper.UpdateBatchStatus(record.Id.ToString(), BatchProcessStatus.CompletedWithFailure);
                await _batchHelper.UpdateProgress(record.Id.ToString(), 1, 1);

                return ResultsTo.BadRequest<bool>().WithMessage("");
            }

            _logger.LogInformation("Processing CsvFile : {PayloadFileName}", payload?.FileName ?? string.Empty);

            var csvStream = new MemoryStream(payload.CsvFile);

            if (csvStream is null)
            {
                await _batchHelper.UpdateBatchErrors(record.Id.ToString(), new List<string> { "Memory Stream is null." });
                await _batchHelper.UpdateBatchStatus(record.Id.ToString(), BatchProcessStatus.CompletedWithFailure);
                await _batchHelper.UpdateProgress(record.Id.ToString(), 1, 1);

                return ResultsTo.BadRequest<bool>().WithMessage("Memory Stream is null.");
            }

            var filename = Path.GetFileNameWithoutExtension(payload.FileName);
            var parsedName = filename.Split('_');

            if (!TaxFilingCsvProcessorHelper.CsvFileCheck(parsedName))
            {
                await _batchHelper.UpdateBatchErrors(record.Id.ToString(), new List<string> { "Invalid Filename." });
                await _batchHelper.UpdateBatchStatus(record.Id.ToString(), BatchProcessStatus.CompletedWithFailure);
                await _batchHelper.UpdateProgress(record.Id.ToString(), 1, 1);

                return ResultsTo.BadRequest<bool>().WithMessage("Invalid Filename.");
            }

            var typeId = parsedName[1].ToLower() switch
            {
                "4wk" => (int)TaxPeriodEnum.FourWeekly,
                "dag" => (int)TaxPeriodEnum.Day,
                "mnd" => (int)TaxPeriodEnum.Month,
                "wk" => (int)TaxPeriodEnum.Week,
                "kw" => (int)TaxPeriodEnum.Quarter,
                _ => (int)TaxPeriodEnum.None,
            };

            var code = woonlands.Value.FirstOrDefault(c => c.WoonlandbeginselCode == parsedName[2].ToUpper());
            var taxType = string.Equals(parsedName[0], TaxTypeEnum.Wit.ToString(), StringComparison.InvariantCultureIgnoreCase)
                ? TaxTypeEnum.Wit.ToString()
                : TaxTypeEnum.Groen.ToString();

            if (code is not null && typeId > 0)
            {
                var year = DateTime.ParseExact(parsedName[5], "yyyyMMdd", CultureInfo.InvariantCulture).Year;

                var taxTable = TaxFilingCsvProcessorHelper.TaxFilingCsvProcessor(csvStream, code.Id, typeId, year, parsedName[0]);

                if (!taxTable.Any())
                {
                    await _batchHelper.UpdateBatchErrors(record.Id.ToString(), new List<string> { "No Record Found" });
                    await _batchHelper.UpdateBatchStatus(record.Id.ToString(), BatchProcessStatus.CompletedWithFailure);
                    await _batchHelper.UpdateProgress(record.Id.ToString(), 1, 1);

                    return ResultsTo.BadRequest<bool>().WithMessage("No Record Found");
                }

                await HandleAsync(new UpsertToTable
                {
                    TaxTable = taxTable,
                    TaxType = taxType,
                }, CancellationToken.None);

                await _batchHelper.UpdateProgress(record.Id.ToString(), 1, 1);
                await _batchHelper.UpdateBatchStatus(record.Id.ToString(), BatchProcessStatus.CompletedSuccessful);

                return ResultsTo.Success<bool>(true);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            await _batchHelper.UpdateBatchErrors(record.Id.ToString(), new List<string> { e.StackTrace, e.Message });
            await _batchHelper.UpdateBatchStatus(record.Id.ToString(), BatchProcessStatus.CompletedWithFailure);
            await _batchHelper.UpdateProgress(record.Id.ToString(), 1, 1);

            return ResultsTo.BadRequest<bool>().WithMessage(e.Message);
        }

        return ResultsTo.Success<bool>(true);
    }


    private async Task<List<BerekenInhoudingModel>> GetTaxRecordTypeWhite(GetInhouding request)
    {
        var white = await _repository.HandleAsync(new BelastingTabellenWitGroenRepository.GetInhoudingWhite
        {
            InkomenWit = request.InkomenWit,
            BasisDagen = request.BasisDagen,
            Geboortedatum = request.Geboortedatum,
            AhkInd = request.AlgemeneHeffingsKortingIndicator,
            Loontijdvak = request.Loontijdvak,
            WoondlandBeginselId = request.WoondlandBeginselId,
            Jaar = request.ProcesDatum.Year,
        }, CancellationToken.None);

        return new List<BerekenInhoudingModel> { white.Value }; ;
    }

    private async Task<List<BerekenInhoudingModel>> GetTaxRecordTypeGreen(GetInhouding request)
    {
        var green = await _repository.HandleAsync(new BelastingTabellenWitGroenRepository.GetInhoudingGreen
        {
            InkomenGroen = request.InkomenGroen,
            BasisDagen = request.BasisDagen,
            Geboortedatum = request.Geboortedatum,
            AhkInd = request.AlgemeneHeffingsKortingIndicator,
            Loontijdvak = request.Loontijdvak,
            WoondlandBeginselId = request.WoondlandBeginselId,
            Jaar = request.ProcesDatum.Year,
        }, CancellationToken.None);

        return new List<BerekenInhoudingModel> { green.Value };
    }

    private async Task<List<BerekenInhoudingModel>> GetTaxRecordTypeBoth(GetInhouding request)
    {
        var result = new List<BerekenInhoudingModel>();
        result.AddRange(await GetTaxRecordTypeWhite(request));
        result.AddRange(await GetTaxRecordTypeGreen(request));

        if (await _repository.HandleAsync(new BelastingTabellenWitGroenRepository.GetInhoudingBoth
        {
            InkomenWit = request.InkomenWit,
            InkomenGroen = request.InkomenGroen,
            BasisDagen = request.BasisDagen,
            Geboortedatum = request.Geboortedatum,
            AhkInd = request.AlgemeneHeffingsKortingIndicator,
            Loontijdvak = request.Loontijdvak,
            WoondlandBeginselId = request.WoondlandBeginselId,
            Jaar = request.ProcesDatum.Year,
        }, CancellationToken.None) is { } both && both.IsSuccess())
        {
            result.Add(both.Value);
        }


        return result;
    }
}
