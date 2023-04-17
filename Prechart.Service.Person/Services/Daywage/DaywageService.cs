using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Globals.Helper;
using Prechart.Service.Globals.Models.Loonheffings;
using Prechart.Service.Globals.Models.Person.Daywages;
using Prechart.Service.Person.Models.Daywage;
using Prechart.Service.Person.Repositories.Daywage;

namespace Prechart.Service.Person.Services.Daywage;

public partial class DaywageService : IDaywageService
{
    private readonly ILogger<DaywageService> _logger;
    private readonly IDaywageRepository _repository;

    public DaywageService(ILogger<DaywageService> logger, IDaywageRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<IFluentResults<DaywageWithInReferencePeriode>> HandleAsync(CalculateWithInReferencePeriod request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("CalculateWithInReferencePeriod");

        if (request.StartOfSickLeave.Year < 2000)
        {
            return ResultsTo.BadRequest<DaywageWithInReferencePeriode>("StartOfSickLeave is null or year is less than 2000");
        }

        var refencePeriodes = request.StartOfSickLeave.ToReferencePeriod();

        var result = await _repository.HandleAsync(new DaywageRepository.GetTaxDetails
        {
            PersonId = request.PersonId,
            TaxNumber = request.TaxNumber,
        }, cancellationToken);

        if (result.IsNotFoundOrBadRequest() || result?.Value is null)
        {
            return ResultsTo.NotFound<DaywageWithInReferencePeriode>().FromResults(result);
        }

        var taxDetailsInPeriodes = result.Value
            .Where(t => t.TaxNo == request.TaxNumber)
            .Where(t => t.CollectieveType == CollectieveType.Normaal)
            .Where(t => refencePeriodes.ToPeriodList().Contains(t.TaxPeriode))
            .OrderBy(t => t.TaxPeriode)
            .ToList();

        if (!taxDetailsInPeriodes.Any() || string.IsNullOrWhiteSpace(taxDetailsInPeriodes?.FirstOrDefault()?.TaxNo ?? string.Empty))
        {
            return ResultsTo
                .NotFound<DaywageWithInReferencePeriode>()
                .WithMessage("No Tax Details found for the given Tax Number and Reference Period.");
        }

        var totalPaidInReference = taxDetailsInPeriodes.Sum(t => t.Werknemersgegevens.LnSv).Round(2);
        var totalDaysInReference = taxDetailsInPeriodes.Sum(t => t.Werknemersgegevens.AantVerlU / 8).Round(2);

        var daywageWithInReferencePeriod = new DaywageWithInReferencePeriode
        {
            PersonId = request.PersonId,
            DaywageRecord = new PersonDaywageModel
            {
                TaxNo = taxDetailsInPeriodes?.FirstOrDefault()?.TaxNo ?? string.Empty,
                StartOfSickLeave = request.StartOfSickLeave,
                StartOfRefencePeriode = refencePeriodes.startReference,
                EndOfReferencePeriode = refencePeriodes.endReference,
                DaysInReferencePeriode = totalDaysInReference,
                TotalPaidInReferencePeriode = totalPaidInReference,
                DaywageBasedOnReferencePeriode = (totalPaidInReference / totalDaysInReference).Round(2),
                TaxDetails = taxDetailsInPeriodes,
                CalculatedOn = DateTime.Now,
                Active = true,
            },
        };

        await _repository.HandleAsync(new DaywageRepository.UpsertDaywage
        {
            DaywageWithInReferencePeriode = daywageWithInReferencePeriod,
        });

        return ResultsTo.Success(daywageWithInReferencePeriod);
    }

    public async Task<IFluentResults<ReferencePeriodeModel>> HandleAsync(GetReferencePeriod request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("GetReferencePeriod");

        if (request.StartOfSickLeave.Year < 2000)
        {
            return ResultsTo.BadRequest<ReferencePeriodeModel>("StartOfSickLeave is null or year is less than 2000");
        }

        var referencePeriod = request.StartOfSickLeave.ToReferencePeriod();

        return ResultsTo.Success(new ReferencePeriodeModel
        {
            StartOfReferencePeriode = referencePeriod.startReference,
            EndOfReferencePeriode = referencePeriod.endReference,
        });
    }

    public async Task<IFluentResults<List<PersonDaywageHistoryModel>>> HandleAsync(GetDaywage request, CancellationToken cancellationToken = default)
    {
        var result = await _repository.HandleAsync(new DaywageRepository.GetDaywage
        {
            PersonId = request.PersonId,
            TaxNumber = request.TaxNumber,
        }, cancellationToken);

        if (result.IsNotFoundOrBadRequest())
        {
            return ResultsTo.NotFound<List<PersonDaywageHistoryModel>>();
        }

        var history = result.Value.Select(d => new PersonDaywageHistoryModel
        {
            DaywageId = d.DaywageId,
            TaxNo = d.TaxNo,
            StartOfSickLeave = d.StartOfSickLeave,
            StartOfReferencePeriode = d.StartOfRefencePeriode,
            EndOfReferencePeriode = d.EndOfReferencePeriode,
            DaysInReferencePeriode = d.DaysInReferencePeriode,
            TotalPaidInReferencePeriode = d.TotalPaidInReferencePeriode,
            DaywageBasedOnReferencePeriode = d.DaywageBasedOnReferencePeriode,
            CalculatedOn = d.CalculatedOn,
            TaxDetails = d.TaxDetails.Select(t => new TaxPaymentDaywageDetails
            {
                TaxNo = t.TaxNo,
                NumIv = t.NumIv,
                PersonNr = t.PersonNr,
                TaxFileProcessDate = t.TaxFileProcessDate,
                TaxPeriode = t.TaxPeriode,
                CollectieveType = t.CollectieveType,
                Werknemersgegevens = new DaywageWerknemersgegevensType
                {
                    LnSv = t.Werknemersgegevens.LnSv,
                    AantVerlU = t.Werknemersgegevens.AantVerlU,
                },
            }).ToList(),
            Active = d.Active,
        });

        return ResultsTo.Something(history.ToList());
    }
}
