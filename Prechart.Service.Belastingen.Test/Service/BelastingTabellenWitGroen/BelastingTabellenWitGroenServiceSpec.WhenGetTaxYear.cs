using FluentAssertions;
using NSubstitute;
using Prechart.Service.Belastingen.Repositories.BelastingTabellenWitGroen;
using Prechart.Service.Belastingen.Services.BelastingTabellenWitGroen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace Prechart.Service.Belastingen.Test.Service;

public partial class BelastingTabellenWitGroenServiceSpec
{
    public class WhenGetTaxYear : BelastingTabellenWitGroenServiceSpec
    {
        private IFluentResults<List<int>> Result;

        public When GetTaxYear => async () => Result = await Subject.HandleAsync(new BelastingTabellenWitGroenService.GetTaxYear(), CancellationToken.None);

        public class AndGetWoonlandbeginsel : WhenGetTaxYear
        {
            public And FoundGetTaxYear => () => Repository.HandleAsync(Arg.Any<BelastingTabellenWitGroenRepository.GetTaxYear>(), CancellationToken.None).Returns<IFluentResults<List<int>>>(
                ResultsTo.Something(
                    new List<int> { 2022, 2021 }
                    ));

            [Fact]
            public void ThenResultShouldBeSome()
            {
                Result.Value.Should().BeEquivalentTo(new List<int> { 2022, 2021 });
            }
        }

        public class AndGetTaxYearNotFound : WhenGetTaxYear
        {
            public And CaseFound => () => Repository.HandleAsync(Arg.Any<BelastingTabellenWitGroenRepository.GetTaxYear>(), CancellationToken.None).Returns(ResultsTo.NotFound<List<int>>());

            [Fact]
            public void ThenResultShouldBeSome()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }
    }
}
