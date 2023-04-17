using FluentAssertions;
using Prechart.Service.Belastingen.Database.Models;
using Prechart.Service.Belastingen.Repositories.BelastingTabellenWitGroen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.TestBase;
using System.Threading;
using Xunit;

namespace Prechart.Service.Belastingen.Test.Repositories;

public partial class BelastingTabellenWitGroenRepositorySpec
{
    public class GetWoonlandbeginselCode : BelastingTabellenWitGroenRepositorySpec
    {
        public IFluentResults<int> Result { get; private set; }

        public When GetWoonlandbeginselIds => async () => Result = await Subject.HandleAsync(new BelastingTabellenWitGroenRepository.GetWoonlandbeginselCode { WoonlandbeginselCode = "NL" }, CancellationToken.None);

        public class AndFound : GetWoonlandbeginselCode
        {
            public And Found => () =>
            {
                Context.Woonlandbeginsel.Add(new Woonlandbeginsel
                {
                    WoonlandbeginselCode = "NL",
                    WoonlandbeginselBenaming = "Netherlands",
                    WoonlandbeginselBelastingCode = 2,
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
                Result.Value.Should().Be(1);
            }
        }

        public class AndNotFound : GetWoonlandbeginselCode
        {
            [Fact]
            public void ThenShouldReturnFalse()
            {
                Result.IsNotFound().Should().BeFalse();
            }
        }
    }
}
