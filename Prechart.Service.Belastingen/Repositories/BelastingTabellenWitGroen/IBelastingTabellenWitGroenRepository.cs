using Prechart.Service.Belastingen.Database.Models;
using Prechart.Service.Belastingen.Models;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using System;
using System.Collections.Generic;

namespace Prechart.Service.Belastingen.Repositories.BelastingTabellenWitGroen;

public interface IBelastingTabellenWitGroenRepository :
    IHandlerAsync<BelastingTabellenWitGroenRepository.GetWoonlandbeginselId, IFluentResults<string>>,
    IHandlerAsync<BelastingTabellenWitGroenRepository.GetWoonlandbeginselCode, IFluentResults<int>>,
    IHandlerAsync<BelastingTabellenWitGroenRepository.GetWoonlandbeginsel, IFluentResults<List<Woonlandbeginsel>>>,
    IHandlerAsync<BelastingTabellenWitGroenRepository.UpsertToTable, IFluentResults<int>>,
    IHandlerAsync<BelastingTabellenWitGroenRepository.GetInhoudingGreen, IFluentResults<BerekenInhoudingModel>>,
    IHandlerAsync<BelastingTabellenWitGroenRepository.GetInhoudingWhite, IFluentResults<BerekenInhoudingModel>>,
    IHandlerAsync<BelastingTabellenWitGroenRepository.GetInhoudingBoth, IFluentResults<BerekenInhoudingModel>>,
    IHandlerAsync<BelastingTabellenWitGroenRepository.GetTaxYear, IFluentResults<List<int>>>,
    IHandlerAsync<BelastingTabellenWitGroenRepository.IsAow, IFluentResults<DateTime>>
{
}
