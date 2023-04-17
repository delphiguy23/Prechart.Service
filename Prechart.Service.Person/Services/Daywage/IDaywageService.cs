using System.Collections.Generic;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Models.Person.Daywages;
using Prechart.Service.Person.Models.Daywage;

namespace Prechart.Service.Person.Services.Daywage;

public interface IDaywageService :
    IHandlerAsync<DaywageService.GetReferencePeriod, IFluentResults<ReferencePeriodeModel>>,
    IHandlerAsync<DaywageService.CalculateWithInReferencePeriod, IFluentResults<DaywageWithInReferencePeriode>>,
    IHandlerAsync<DaywageService.GetDaywage, IFluentResults<List<PersonDaywageHistoryModel>>>
{
}
