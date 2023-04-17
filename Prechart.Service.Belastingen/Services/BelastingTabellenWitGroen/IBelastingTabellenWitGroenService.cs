using Prechart.Service.Belastingen.Database.Models;
using Prechart.Service.Belastingen.Models;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using System.Collections.Generic;

namespace Prechart.Service.Belastingen.Services.BelastingTabellenWitGroen;

public interface IBelastingTabellenWitGroenService :
    IHandlerAsync<BelastingTabellenWitGroenService.GetInhouding, IFluentResults<BerekenInhoudingModel>>,
    IHandlerAsync<BelastingTabellenWitGroenService.GetAlleWoonlandbeginsel, IFluentResults<List<Woonlandbeginsel>>>,
    IHandlerAsync<BelastingTabellenWitGroenService.UpsertToTable, IFluentResults<bool>>,
    IHandlerAsync<BelastingTabellenWitGroenService.GetTaxYear, IFluentResults<List<int>>>,
    IHandlerAsync<BelastingTabellenWitGroenService.ProcessPendingCsv, IFluentResults<bool>>
{
}
