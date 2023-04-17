using FluentAssertions;
using NSubstitute;
using Prechart.Service.Belastingen.Models;
using Prechart.Service.Belastingen.Repositories.BelastingTabellenWitGroen;
using Prechart.Service.Belastingen.Services.BelastingTabellenWitGroen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Interfaces.Belastingen;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace Prechart.Service.Belastingen.Test.Service;

public partial class BelastingTabellenWitGroenServiceSpec
{
    private IFluentResults<bool> result;
    private List<ITaxRecord> taxRecords;


    public class WhenUpsertToTable : BelastingTabellenWitGroenServiceSpec
    {
        public class AndGetTaxRecordFound : WhenUpsertToTable
        {
            public When GetTaxRecord => async () => result = await Subject.HandleAsync(new BelastingTabellenWitGroenService.UpsertToTable
            {
                TaxType = "Green",
                TaxTable = taxRecords,
            }, CancellationToken.None);

            public And Data => () =>
            {
                taxRecords = new List<ITaxRecord>
                {
                    new InhoudingModel {TypeId = 1, CountryId = 1, Year = 2022},
                };
            };

            public And CaseFound => () => Repository.HandleAsync(Arg.Any<BelastingTabellenWitGroenRepository.UpsertToTable>(), CancellationToken.None).Returns(ResultsTo.Something(1));


            [Fact]
            public void ThenShouldUpsertToTableSuccess()
            {
                result.Value.Should().BeTrue();
            }
        }

        public class AndGetTaxRecordRequestNull : WhenUpsertToTable
        {
            public When GetTaxRecord => async () => result = await Subject.HandleAsync(new BelastingTabellenWitGroenService.UpsertToTable(), CancellationToken.None);

            [Fact]
            public void ThenShouldUpsertToTableSuccess()
            {
                result.Value.Should().BeFalse();
            }
        }
    }
}
