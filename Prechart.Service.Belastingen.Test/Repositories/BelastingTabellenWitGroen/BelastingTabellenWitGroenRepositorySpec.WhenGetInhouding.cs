using FluentAssertions;
using Prechart.Service.Belastingen.Database.Models;
using Prechart.Service.Belastingen.Models;
using Prechart.Service.Belastingen.Repositories.BelastingTabellenWitGroen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Models.Belastingen;
using System;
using System.Threading;
using Xunit;

namespace Prechart.Service.Belastingen.Test.Repositories;

public partial class BelastingTabellenWitGroenRepositorySpec
{
    public class GetInhouding : BelastingTabellenWitGroenRepositorySpec
    {
        public IFluentResults<BerekenInhoudingModel> Result { get; private set; }

        public class AndGetInhoudingWhite : GetInhouding
        {
            public And Found => () =>
            {
                Context.White.Add(new White
                {
                    Year = 2022,
                    CountryId = 1,
                    TypeId = 1,
                    Tabelloon = 420.58M,
                    ZonderLoonheffingskorting = 175.00M,
                    MetLoonheffingskorting = 175.00M,
                    VerrekendeArbeidskorting = 0,
                    EerderZonderLoonheffingskorting = 149.94M,
                    EerderMetLoonheffingskorting = 149.94M,
                    EerderVerrekendeArbeidskorting = 0,
                    LaterZonderLoonheffingskorting = 150.58M,
                    LaterMetLoonheffingskorting = 150.58M,
                    LaterVerrekendeArbeidskorting = 0M,
                    Active = true,
                });

                Context.Woonlandbeginsel.Add(new Woonlandbeginsel
                {
                    Id = 1,
                    WoonlandbeginselCode = "NL",
                    WoonlandbeginselBenaming = "Nederlands",
                    WoonlandbeginselBelastingCode = 2,
                    Active = true,
                });

                Context.SaveChanges();
            };

            public When GetTax => async () => Result = await Subject.HandleAsync(new BelastingTabellenWitGroenRepository.GetInhoudingWhite
            {
                InkomenWit = 3000,
                BasisDagen = 1,
                AhkInd = true,
                Loontijdvak = TaxPeriodEnum.Day,
                WoondlandBeginselId = 1,
                Jaar = 2022,
                Geboortedatum = new DateTime(1950, 11, 3),
            }, CancellationToken.None);

            [Fact]
            public void ThenShouldReturnSome()
            {
                Result.Value.InhoudingType.ToString().Should().Be("White");
            }

            [Fact]
            public void ThenShouldReturnTrue()
            {
                Result.IsSuccess().Should().BeTrue();
            }
        }

        public class AndGetInhoudingGreen : GetInhouding
        {
            public And Found => () =>
            {
                Context.Green.Add(new Green
                {
                    Year = 2022,
                    CountryId = 1,
                    TypeId = 1,
                    Tabelloon = 420.58M,
                    ZonderLoonheffingskorting = 175.00M,
                    MetLoonheffingskorting = 175.00M,
                    VerrekendeArbeidskorting = 0,
                    EerderZonderLoonheffingskorting = 149.94M,
                    EerderMetLoonheffingskorting = 149.94M,
                    EerderVerrekendeArbeidskorting = 0,
                    LaterZonderLoonheffingskorting = 150.58M,
                    LaterMetLoonheffingskorting = 150.58M,
                    LaterVerrekendeArbeidskorting = 0M,
                    Active = true,
                });

                Context.Woonlandbeginsel.Add(new Woonlandbeginsel
                {
                    Id = 2,
                    WoonlandbeginselCode = "NL",
                    WoonlandbeginselBenaming = "Nederlands",

                    WoonlandbeginselBelastingCode = 2,
                    Active = true,
                });

                Context.SaveChanges();
            };

            public When GetTax => async () => Result = await Subject.HandleAsync(new BelastingTabellenWitGroenRepository.GetInhoudingGreen
            {
                InkomenGroen = 3000,
                BasisDagen = 1,
                AhkInd = true,
                Loontijdvak = TaxPeriodEnum.Day,
                WoondlandBeginselId = 1,
                Jaar = 2022,
                Geboortedatum = new DateTime(1950, 11, 3),
            }, CancellationToken.None);

            [Fact]
            public void ThenShouldReturnSome()
            {
                Result.Value.InhoudingType.ToString().Should().Be("Green");
            }

            [Fact]
            public void ThenShouldReturnTrue()
            {
                Result.IsSuccess().Should().BeTrue();
            }
        }

        public class AndGetInhoudingBoth : GetInhouding
        {
            public And Found => () =>
            {
                Context.White.Add(new White
                {
                    Year = 2022,
                    CountryId = 1,
                    TypeId = 1,
                    Tabelloon = 420.58M,
                    ZonderLoonheffingskorting = 175.00M,
                    MetLoonheffingskorting = 175.00M,
                    VerrekendeArbeidskorting = 0,
                    EerderZonderLoonheffingskorting = 149.94M,
                    EerderMetLoonheffingskorting = 149.94M,
                    EerderVerrekendeArbeidskorting = 0,
                    LaterZonderLoonheffingskorting = 150.58M,
                    LaterMetLoonheffingskorting = 150.58M,
                    LaterVerrekendeArbeidskorting = 0M,
                    Active = true,
                });

                Context.Green.Add(new Green
                {
                    Year = 2022,
                    CountryId = 1,
                    TypeId = 1,
                    Tabelloon = 420.58M,
                    ZonderLoonheffingskorting = 175.00M,
                    MetLoonheffingskorting = 175.00M,
                    VerrekendeArbeidskorting = 0,
                    EerderZonderLoonheffingskorting = 149.94M,
                    EerderMetLoonheffingskorting = 149.94M,
                    EerderVerrekendeArbeidskorting = 0,
                    LaterZonderLoonheffingskorting = 150.58M,
                    LaterMetLoonheffingskorting = 150.58M,
                    LaterVerrekendeArbeidskorting = 0M,
                    Active = true,
                });

                Context.Woonlandbeginsel.Add(new Woonlandbeginsel
                {
                    Id = 1,
                    WoonlandbeginselCode = "NL",
                    WoonlandbeginselBenaming = "Nederlands",
                    WoonlandbeginselBelastingCode = 2,
                    Active = true,
                });

                Context.SaveChanges();
            };

            public When GetTax => async () => Result = await Subject.HandleAsync(new BelastingTabellenWitGroenRepository.GetInhoudingBoth
            {
                InkomenGroen = 3000,
                InkomenWit = 3000,
                BasisDagen = 1,
                AhkInd = true,
                Loontijdvak = TaxPeriodEnum.Day,
                WoondlandBeginselId = 1,
                Jaar = 2022,
                Geboortedatum = new DateTime(1950, 11, 3),
            }, CancellationToken.None);

            [Fact]
            public void ThenShouldReturnSome()
            {
                Result.Value.InhoudingType.ToString().Should().Be("Both");
            }

            [Fact]
            public void ThenShouldReturnTrue()
            {
                Result.IsSuccess().Should().BeTrue();
            }
        }

        public class AndNotFoundWhite : GetInhouding
        {
            public When GetTax => async () => Result = await Subject.HandleAsync(new BelastingTabellenWitGroenRepository.GetInhoudingWhite
            {
                InkomenWit = 0,
                BasisDagen = 1,
                AhkInd = true,
                Loontijdvak = TaxPeriodEnum.Day,
                WoondlandBeginselId = 1,
                Jaar = 2022,
            }, CancellationToken.None);

            [Fact]
            public void ThenShouldReturnNone()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }

        public class AndNotFoundGreen : GetInhouding
        {
            public When GetTax => async () => Result = await Subject.HandleAsync(new BelastingTabellenWitGroenRepository.GetInhoudingGreen
            {
                InkomenGroen = 0,
                BasisDagen = 1,
                AhkInd = true,
                Loontijdvak = TaxPeriodEnum.Day,
                WoondlandBeginselId = 1,
                Jaar = 2022,
            }, CancellationToken.None);

            [Fact]
            public void ThenShouldReturnNone()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }

        public class AndNotFoundBoth : GetInhouding
        {
            public When GetTax => async () => Result = await Subject.HandleAsync(new BelastingTabellenWitGroenRepository.GetInhoudingBoth
            {
                InkomenWit = 0,
                InkomenGroen = 0,
                BasisDagen = 1,
                AhkInd = true,
                Loontijdvak = TaxPeriodEnum.Day,
                WoondlandBeginselId = 1,
                Jaar = 2022,
            }, CancellationToken.None);

            [Fact]
            public void ThenShouldReturnNone()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }
    }
}
