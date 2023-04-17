using FluentAssertions;
using MongoDB.Driver;
using NSubstitute;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Loonheffings.Repositories;
using Prechart.Service.Loonheffings.Service;
using Xunit;

namespace Prechart.Service.Loonheffings.Test.Service;

public partial class LoonheffingsServiceSpec
{
    public class WhenLoonheffingsProcessedResult : LoonheffingsServiceSpec
    {
        private IFluentResults<UpdateResult> result;

        public When LoonheffingsProcessedResult => async () => result = await Subject.HandleAsync(new LoonheffingsService.LoonheffingsProcessedResult
        {
            FileName = "test.xml",
            Processed = true,
            ProcessErrors = string.Empty,
            EmployeesInserted = 1,
            EmployeesUpdated = 1,
        }, CancellationToken.None);

        public class AndException : WhenLoonheffingsProcessedResult
        {
            public And GetData => () => _repository.HandleAsync(Arg.Any<LoonheffingsRepository.UpdateProcessedXml>(), CancellationToken.None)
                .Returns<IFluentResults<UpdateResult>>(e => throw new Exception("exception"));

            [Fact]
            public void ThenShouldReturnException()
            {
                result.IsFailure().Should().BeTrue();
            }

            [Fact]
            public void ThenLogger()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
            }
        }

        public class AndDataFound : WhenLoonheffingsProcessedResult
        {
            public And GetData => () => _repository.HandleAsync(Arg.Any<LoonheffingsRepository.UpdateProcessedXml>(), CancellationToken.None)
                .Returns<IFluentResults<UpdateResult>>(d => ResultsTo.Success(_updateResultFound));

            [Fact]
            public void ThenShouldReturnSome()
            {
                result.IsSuccess().Should().BeTrue();
            }

            [Fact]
            public void ThenShouldReturnUpdateResult()
            {
                result.Value.Should().BeAssignableTo<UpdateResult>();
            }

            [Fact]
            public void ThenLogger()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
            }

            [Fact]
            public async Task ThenHandlerIsCalled()
            {
                await _repository.Received(1).HandleAsync(Verify.That<LoonheffingsRepository.UpdateProcessedXml>(i => i.Should()
                    .BeEquivalentTo(new LoonheffingsRepository.UpdateProcessedXml
                    {
                        FileName = "test.xml",
                        EmployeesInserted = 1,
                        EmployeesUpdated = 1,
                        Errors = string.Empty,
                    })), CancellationToken.None);
            }
        }

        public class AndDataNotFound : WhenLoonheffingsProcessedResult
        {
            public And GetData => () => _repository.HandleAsync(Arg.Any<LoonheffingsRepository.UpdateProcessedXml>(), CancellationToken.None)
                .Returns<IFluentResults<UpdateResult>>(d => ResultsTo.Something(_updateResultNotFound));

            [Fact]
            public void ThenShouldReturnNone()
            {
                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }

            [Fact]
            public void ThenLogger()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
            }

            [Fact]
            public async Task ThenHandlerIsCalled()
            {
                await _repository.Received(1).HandleAsync(Verify.That<LoonheffingsRepository.UpdateProcessedXml>(i => i.Should()
                    .BeEquivalentTo(new LoonheffingsRepository.UpdateProcessedXml
                    {
                        FileName = "test.xml",
                        EmployeesInserted = 1,
                        EmployeesUpdated = 1,
                        Errors = string.Empty,
                    })), CancellationToken.None);
            }
        }
    }
}
