using FluentAssertions;
using NSubstitute;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Werkgever.Repository;
using Prechart.Service.Werkgever.Service;
using Xunit;

namespace Prechart.Service.Werkgever.Test.Service;

public partial class WerkgeverServiceSpec
{
    public class WhenUpsertMongoWhk : WerkgeverServiceSpec
    {
        private IFluentResults<bool> result;

        public When UpsertMongoWhk => async () => result = await Subject.HandleAsync(new WerkgeverService.UpsertMongoWhk
        {
            WerkgeverId = "633129fb572a926667ed8c5e".ToObjectId(),
            Id = "633129fb572a926667ed8c51".ToObjectId(),
            WgaVastWerkgever = 0,
            WgaVastWerknemer = 0,
            FlexWerkgever = 0,
            FlexWerknemer = 0,
            ZwFlex = 0,
            ActiefVanaf = new DateTime(DateTime.Now.Year, 1, 1),
            ActiefTot = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12)),
            SqlId = 1,
            Actief = true,
        }, CancellationToken.None);

        public class AndException : WhenUpsertMongoWhk
        {
            public And Exception => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.UpsertMongoWhk>(), CancellationToken.None)
                .Returns<IFluentResults<bool>>(e => throw new Exception("exception"));

            [Fact]
            public void ThenShouldReturnSuccess()
            {
                result.IsFailure().Should().BeTrue();
            }

            [Fact]
            public void ThenLoggerError()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
            }
        }

        public class AndInserted : WhenUpsertMongoWhk
        {
            public And Exception => () => _repository.HandleAsync(Arg.Any<WerkgeverRepository.UpsertMongoWhk>(), CancellationToken.None)
                .Returns<IFluentResults<bool>>(e => ResultsTo.Success(true));

            [Fact]
            public void ThenShouldReturnSome()
            {
                result.IsSuccess().Should().BeTrue();
            }
        }
    }
}
