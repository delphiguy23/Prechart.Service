using System.Collections.Generic;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Models.Loonheffings;
using Prechart.Service.Globals.Models.Person.Daywages;

namespace Prechart.Service.Person.Repositories.Daywage;

public interface IDaywageRepository :
    IHandlerAsync<DaywageRepository.GetTaxDetails, IFluentResults<List<TaxPaymentDetails>>>,
    IHandlerAsync<DaywageRepository.UpsertDaywage, IFluentResults>,
    IHandlerAsync<DaywageRepository.GetDaywage, IFluentResults<List<PersonDaywageModel>>>
{
}
