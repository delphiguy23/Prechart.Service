using FluentAssertions;
using NSubstitute;
using Prechart.Service.Belastingen.Models;
using Prechart.Service.Belastingen.Repositories.BelastingTabellenWitGroen;
using Prechart.Service.Belastingen.Services.BelastingTabellenWitGroen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Models.Belastingen;
using System;
using System.Threading;
using Xunit;

namespace Prechart.Service.Belastingen.Test.Service;

public partial class BelastingTabellenWitGroenServiceSpec
{
    public class WhenGetInhoudingGreen : BelastingTabellenWitGroenServiceSpec
    {
        private IFluentResults<BerekenInhoudingModel> Result;

        private BelastingTabellenWitGroenService.GetInhouding getInhouding = new()
        {
            InkomenWit = new decimal(13),
            InkomenGroen = new decimal(13),
            BasisDagen = new decimal(13),
            AlgemeneHeffingsKortingIndicator = new bool(),
            Loontijdvak = new TaxPeriodEnum(),
            WoondlandBeginselId = new int(),
            ProcesDatum = new DateTime(DateTime.Now.Year, 1, DateTime.DaysInMonth(DateTime.Now.Year, 1)),
            Geboortedatum = new DateTime(DateTime.Now.Year, 1, DateTime.DaysInMonth(DateTime.Now.Year, 1)),
        };

        public When GetTaxRecord => async () => Result = await Subject.HandleAsync(getInhouding, CancellationToken.None);

        public class AndGetTaxRecordFound : WhenGetInhoudingGreen
        {
            public Given GetParamameters => () =>
            {
                getInhouding.InkomenWit = 0;
                getInhouding.InkomenGroen = 300;
                getInhouding.BasisDagen = 1;
                getInhouding.AlgemeneHeffingsKortingIndicator = true;
                getInhouding.Loontijdvak = TaxPeriodEnum.Day;
                getInhouding.WoondlandBeginselId = 1;
                getInhouding.ProcesDatum = DateTime.Today;
                getInhouding.Geboortedatum = new DateTime(1961, 11, 1);
            };

            public And CaseFound => () => Repository.HandleAsync(Arg.Any<BelastingTabellenWitGroenRepository.GetInhoudingGreen>(), CancellationToken.None).Returns<IFluentResults<BerekenInhoudingModel>>(
                ResultsTo.Something(
                    new BerekenInhoudingModel
                    {
                        InhoudingWit = 0M,
                        AlgemeneHeffingsKorting = 0M,
                        ArbeidsKorting = 0M,
                        InhoudingGroen = 115.27M,
                        BasisDagen = 0M,
                        AlgemeneHeffingsKortingIndicator = true,
                        Loontijdvak = (int)TaxPeriodEnum.Day,
                        WoonlandbeginselId = 1,
                        WoonlandbeginselNaam = "Nederlands",
                        InhoudingType = TaxRecordType.Green,
                    })
                );

            [Fact]
            public void ThenResultShouldBeSome()
            {
                Result.Value.InhoudingType.ToString().Should().Be("Green");
            }

            [Fact]
            public void ThenResultShouldBeTrue()
            {
                Result.IsSuccess().Should().BeTrue();
            }
        }

        public class AndGetTaxRecordFoundNull : WhenGetInhoudingGreen
        {
            public Given GetParamameters => () =>
            {
                getInhouding = null;
            };

            public And CaseFound => () => Repository.HandleAsync(Arg.Any<BelastingTabellenWitGroenRepository.GetInhoudingGreen>(), CancellationToken.None).Returns(ResultsTo.NotFound<BerekenInhoudingModel>());

            [Fact]
            public void ThenResultShouldBeTrue()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }

        public class AndGetTaxRecordNotFound : WhenGetInhoudingGreen
        {
            public Given GetParamameters => () =>
            {
                getInhouding.InkomenWit = 0;
                getInhouding.InkomenGroen = 0;
                getInhouding.BasisDagen = 1;
                getInhouding.AlgemeneHeffingsKortingIndicator = true;
                getInhouding.Loontijdvak = TaxPeriodEnum.Day;
                getInhouding.WoondlandBeginselId = 1;
                getInhouding.ProcesDatum = DateTime.Today;
                getInhouding.Geboortedatum = new DateTime(1961, 11, 1);
            };

            public And CaseFound => () => Repository.HandleAsync(Arg.Any<BelastingTabellenWitGroenRepository.GetInhoudingGreen>(), CancellationToken.None).Returns(ResultsTo.NotFound<BerekenInhoudingModel>());

            [Fact]
            public void ThenResultShouldBeTrue()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }
    }
}
