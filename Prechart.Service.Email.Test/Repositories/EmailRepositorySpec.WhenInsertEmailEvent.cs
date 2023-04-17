using FluentAssertions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Models;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Email.Models;
using Prechart.Service.Email.Repositories;
using Xunit;

namespace Prechart.Service.Email.Test.Repositories;

public partial class EmailRepositorySpec
{
    public class WhenInsertEmailEvent : EmailRepositorySpec
    {
        private readonly EmailRepository.InsertEmailEvent insertEmailEvent = new();

        public IFluentResults<EmailEventModel> Result { get; private set; }

        public class WhenGetEmailEventTypeNull : WhenInsertEmailEvent
        {
            public Given GetParamameters => () => { insertEmailEvent.EmailEvent = null; };

            public When GetInsertEmailEvent => async () => Result = await Subject.HandleAsync(insertEmailEvent, CancellationToken.None);

            [Fact]
            public async void ThenDataInsertEmailEventNullFound()
            {
                Result.IsNotFoundOrBadRequest().Should().BeTrue();
            }
        }

        public class AndException : WhenInsertEmailEvent
        {
            [Fact]
            public async void ThenException()
            {
                //Arrange
                _mockEmailEventCollection
                    .Setup(op => op.InsertOneAsync(
                        It.IsAny<EmailEventModel>(),
                        It.IsAny<InsertOneOptions>(),
                        It.IsAny<CancellationToken>()))
                    .Callback(() => throw new Exception("exception"));

                // Act
                var result = await Subject.HandleAsync(new EmailRepository.InsertEmailEvent
                {
                    EmailEvent = new EmailEventModel
                    {
                        Id = "147c94d6f88d84331d7d6c27".ToObjectId(),
                        EmailEventType = EmailEventType.Normal,
                    },
                }, CancellationToken.None);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
                result.IsFailure().Should().BeTrue();
            }
        }

        public class AndInserted : WhenInsertEmailEvent
        {
            [Fact]
            public async void ThenInserted()
            {
                //Arrange
                _mockEmailEventCollection
                    .Setup(op => op.InsertOneAsync(
                        It.IsAny<EmailEventModel>(),
                        It.IsAny<InsertOneOptions>(),
                        It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult((object)null));

                //Act
                var result = await Subject.HandleAsync(new EmailRepository.InsertEmailEvent
                {
                    EmailEvent = new EmailEventModel
                    {
                        Id = "987c94d6f88d84331d7d6c77".ToObjectId(),
                        EmailEventType = EmailEventType.Normal,
                    },
                }, CancellationToken.None);

                result.Value.EmailEventType.Should().BeEquivalentTo(EmailEventType.Normal);

                result.IsSuccess().Should().BeTrue();
            }
        }
    }
}
