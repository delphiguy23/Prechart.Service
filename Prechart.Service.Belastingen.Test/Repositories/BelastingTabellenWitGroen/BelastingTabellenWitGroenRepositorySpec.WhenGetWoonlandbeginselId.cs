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
    public class GetWoonlandbeginselId : BelastingTabellenWitGroenRepositorySpec
    {
        private readonly int woonlandbeginselId = 1;
        public IFluentResults<string> Result { get; private set; }

        public When GetWoonlandbeginselIds => async () => Result = await Subject.HandleAsync(new BelastingTabellenWitGroenRepository.GetWoonlandbeginselId { WoonlandbeginselId = woonlandbeginselId }, CancellationToken.None);

        public class AndFound : GetWoonlandbeginselId
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
            public void ThenShouldReturnSome()
            {
                Result.Value.Should().Be("Netherlands");
            }

            [Fact]
            public void ThenShouldReturnTrue()
            {
                Result.IsSuccess().Should().BeTrue();
            }
        }

        public class AndNotFound : GetWoonlandbeginselId
        {
            [Fact]
            public void ThenShouldReturnNotFound()
            {
                Result.Value.Should().BeNull();
            }

            [Fact]
            public void ThenShouldReturnTrue()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }
    }
}
