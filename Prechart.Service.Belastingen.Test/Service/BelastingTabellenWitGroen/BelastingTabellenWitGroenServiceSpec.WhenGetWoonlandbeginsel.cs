using FluentAssertions;
using NSubstitute;
using Prechart.Service.Belastingen.Database.Models;
using Prechart.Service.Belastingen.Repositories.BelastingTabellenWitGroen;
using Prechart.Service.Belastingen.Services.BelastingTabellenWitGroen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace Prechart.Service.Belastingen.Test.Service;

public partial class BelastingTabellenWitGroenServiceSpec
{
    public class WhenGetWoonlandbeginsel : BelastingTabellenWitGroenServiceSpec
    {
        private IFluentResults<List<Woonlandbeginsel>> Result;

        public When GetAlleWoonlandbeginsel => async () => Result = await Subject.HandleAsync(new BelastingTabellenWitGroenService.GetAlleWoonlandbeginsel(), CancellationToken.None);

        public class AndGetWoonlandbeginsel : WhenGetWoonlandbeginsel
        {
            public And FoundGetWoonlandbeginsel => () => Repository.HandleAsync(Arg.Any<BelastingTabellenWitGroenRepository.GetWoonlandbeginsel>(), CancellationToken.None).Returns(
                ResultsTo.Something(
                    new List<Woonlandbeginsel>
                    {
                        new ()
                        {
                            Id = 1,
                            WoonlandbeginselCode = "NL",
                            WoonlandbeginselBenaming = "Netherlands",
                            WoonlandbeginselBelastingCode = 2,
                            Active = true,
                        },
                    })
                );

            [Fact]
            public void ThenResultShouldBeSome()
            {
                Result.Value.Should().BeEquivalentTo(
                    new List<Woonlandbeginsel>
                    {
                        new()
                        {
                            Id = 1,
                            WoonlandbeginselCode = "NL",
                            WoonlandbeginselBenaming = "Netherlands",
                            WoonlandbeginselBelastingCode = 2,
                            Active = true,
                        },
                    }
                );
            }
        }

        public class AndGetWoonlandbeginselNotFound : WhenGetWoonlandbeginsel
        {
            public And CaseFound => () => Repository.HandleAsync(Arg.Any<BelastingTabellenWitGroenRepository.GetWoonlandbeginsel>(), CancellationToken.None).Returns(
                ResultsTo.Something(
                    new List<Woonlandbeginsel>()));

            [Fact]
            public void ThenResultShouldBeSome()
            {
                Result.Value.Should().BeEmpty();
            }
        }
    }
}
