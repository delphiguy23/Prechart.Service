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
    public class WhenGetInhoudingBoth : BelastingTabellenWitGroenServiceSpec
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
        public class AndGetTaxRecordFound : WhenGetInhoudingBoth
        {
            public Given GetParamameters => () =>
            {
                getInhouding.InkomenWit = 300;
                getInhouding.InkomenGroen = 300;
                getInhouding.BasisDagen = 1;
                getInhouding.AlgemeneHeffingsKortingIndicator = true;
                getInhouding.Loontijdvak = TaxPeriodEnum.Day;
                getInhouding.WoondlandBeginselId = 1;
                getInhouding.ProcesDatum = DateTime.Today;
                getInhouding.Geboortedatum = new DateTime(1961, 11, 1);
            };

            public And CaseFoundGreen => () => Repository.HandleAsync(Arg.Any<BelastingTabellenWitGroenRepository.GetInhoudingGreen>(), CancellationToken.None).Returns<IFluentResults<BerekenInhoudingModel>>(
                ResultsTo.Something(
                    new BerekenInhoudingModel
                    {
                        InhoudingWit = 108.20m,
                        AlgemeneHeffingsKorting = 1,
                        ArbeidsKorting = 7.07m,
                        InhoudingGroen = 66.80m,
                        BasisDagen = 1,
                        AlgemeneHeffingsKortingIndicator = true,
                        Loontijdvak = (int)TaxPeriodEnum.Day,
                        WoonlandbeginselId = 1,
                        WoonlandbeginselNaam = "Nederlands",
                        InhoudingType = TaxRecordType.Green,
                    })
                );

            public And CaseFoundWhite => () => Repository.HandleAsync(Arg.Any<BelastingTabellenWitGroenRepository.GetInhoudingWhite>(), CancellationToken.None).Returns(
                ResultsTo.Something(
                    new BerekenInhoudingModel
                    {
                        InhoudingWit = 108.20m,
                        AlgemeneHeffingsKorting = 1,
                        ArbeidsKorting = 7.07m,
                        InhoudingGroen = 66.80m,
                        BasisDagen = 1,
                        AlgemeneHeffingsKortingIndicator = true,
                        Loontijdvak = (int)TaxPeriodEnum.Day,
                        WoonlandbeginselId = 1,
                        WoonlandbeginselNaam = "Nederlands",
                        InhoudingType = TaxRecordType.White,
                    })
                );

            public And CaseFoundBoth => () => Repository.HandleAsync(Arg.Any<BelastingTabellenWitGroenRepository.GetInhoudingBoth>(), CancellationToken.None).Returns(
                ResultsTo.Something(
                    new BerekenInhoudingModel
                    {
                        InhoudingWit = 108.20m,
                        AlgemeneHeffingsKorting = 1,
                        ArbeidsKorting = 7.07m,
                        InhoudingGroen = 66.80m,
                        BasisDagen = 1,
                        AlgemeneHeffingsKortingIndicator = true,
                        Loontijdvak = (int)TaxPeriodEnum.Day,
                        WoonlandbeginselId = 1,
                        WoonlandbeginselNaam = "Nederlands",
                        InhoudingType = TaxRecordType.Both,
                    })
                );

            [Fact]
            public void ThenResultShouldBeSome()
            {
                Result.Value.InhoudingType.ToString().Should().Be("Both");
            }

            [Fact]
            public void ThenResultShouldBeTrue()
            {
                Result.IsSuccess().Should().BeTrue();
            }
        }

        public class AndGetTaxRecordNull : WhenGetInhoudingBoth
        {
            public Given GetParamameters => () =>
            {
                getInhouding = null;
            };

            public And CaseFound => () => Repository.HandleAsync(Arg.Any<BelastingTabellenWitGroenRepository.GetInhoudingBoth>(), CancellationToken.None).Returns(ResultsTo.NotFound<BerekenInhoudingModel>());

            [Fact]
            public void ThenResultShouldBeTrue()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }

        public class AndGetTaxRecordNotFound : WhenGetInhoudingBoth
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

            public And CaseFound => () => Repository.HandleAsync(Arg.Any<BelastingTabellenWitGroenRepository.GetInhoudingBoth>(), CancellationToken.None).Returns(ResultsTo.NotFound<BerekenInhoudingModel>());

            [Fact]
            public void ThenResultShouldBeTrue()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }
    }
}
