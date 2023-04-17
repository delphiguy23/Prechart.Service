using FluentAssertions;
using Prechart.Service.Belastingen.Database.Models;
using Prechart.Service.Belastingen.Repositories.BelastingTabellenWitGroen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.TestBase;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace Prechart.Service.Belastingen.Test.Repositories;

public partial class BelastingTabellenWitGroenRepositorySpec
{
    public class GetTaxYear : BelastingTabellenWitGroenRepositorySpec
    {
        public IFluentResults<List<int>> Result { get; private set; }
        public When GetCountry => async () => Result = await Subject.HandleAsync(new BelastingTabellenWitGroenRepository.GetTaxYear(), CancellationToken.None);

        public class AndFound : GetTaxYear
        {
            public And Found => () =>
            {
                Context.Green.Add(new Green
                {
                    Year = 2022,
                    Active = true,
                }
                );

                Context.White.Add(new White
                {
                    Year = 2021,
                    Active = true,
                }
                );

                Context.SaveChanges();
            };

            [Fact]
            public void ThenShouldReturnTrue()
            {
                Result.IsSuccess().Should().BeTrue();
            }

            [Fact]
            public void ThenShouldReturnSome()
            {
                Result.Value.Capacity.Should().BeGreaterThan(0);
            }
        }

        public class AndNotFound : GetTaxYear
        {
            [Fact]
            public void ThenShouldReturnNone()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }
    }
}