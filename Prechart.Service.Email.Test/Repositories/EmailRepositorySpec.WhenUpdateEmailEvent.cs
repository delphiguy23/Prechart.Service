using FluentAssertions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Models;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Email.Models;
using Prechart.Service.Email.Repositories;
using Xunit;

namespace Prechart.Service.Email.Test.Repositories;

public partial class EmailRepositorySpec
{
    public class WhenUpdateEmailEvent : EmailRepositorySpec
    {
        public class WhenUpdateEmailEventNoRecord : WhenUpdateEmailEvent
        {
            [Fact]
            public async void ThenUpdateFailed()
            {
                //Arrange
                var emailEvents = new List<EmailEventModel>();

                var _mockAsyncCursorEmailEvent = new Mock<IAsyncCursor<EmailEventModel>>();

                _mockAsyncCursorEmailEvent.Setup(a => a.Current).Returns(emailEvents);
                _mockAsyncCursorEmailEvent.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                _mockAsyncCursorEmailEvent.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockEmailEventCollection
                    .Setup(op => op.FindOneAndUpdateAsync(
                        It.IsAny<FilterDefinition<EmailEventModel>>(),
                        It.IsAny<UpdateDefinition<EmailEventModel>>(),
                        It.IsAny<FindOneAndUpdateOptions<EmailEventModel, EmailEventModel>>(),
                        It.IsAny<CancellationToken>()));

                //Act
                var result = await Subject.HandleAsync(new EmailRepository.UpdateEmailEvent(), CancellationToken.None);

                //Assert
                _mockEmailEventCollection.Verify(c => c.FindOneAndUpdateAsync(
                    It.IsAny<FilterDefinition<EmailEventModel>>(),
                    It.IsAny<UpdateDefinition<EmailEventModel>>(),
                    It.IsAny<FindOneAndUpdateOptions<EmailEventModel, EmailEventModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }
        }

        public class AndException : WhenUpdateEmailEvent
        {
            [Fact]
            public async void ThenException()
            {
                //Arrange
                _mockEmailEventCollection
                    .Setup(op => op.FindOneAndUpdateAsync(
                        It.IsAny<FilterDefinition<EmailEventModel>>(),
                        It.IsAny<UpdateDefinition<EmailEventModel>>(),
                        It.IsAny<FindOneAndUpdateOptions<EmailEventModel, EmailEventModel>>(),
                        It.IsAny<CancellationToken>()))
                    .Callback(() => throw new Exception("exception"));

                //Act
                var result = await Subject.HandleAsync(new EmailRepository.UpdateEmailEvent
                {
                    Id = "636c94d6f25d84331d8yho9j".ToObjectId(),
                    Error = null,
                    Sent = true,
                }, CancellationToken.None);


                //Assert
                _mockEmailEventCollection.Verify(c => c.FindOneAndUpdateAsync(
                  It.IsAny<FilterDefinition<EmailEventModel>>(),
                  It.IsAny<UpdateDefinition<EmailEventModel>>(),
                  It.IsAny<FindOneAndUpdateOptions<EmailEventModel, EmailEventModel>>(),
                  It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
                result.IsFailure().Should().BeTrue();
            }
        }

        public class WhenUpdateEmailEventSuccess : WhenUpdateEmailEvent
        {

            [Fact]
            public async void ThenUpdateSuccess()
            {
                //Arrange
                var createdDate = DateTime.Now;
                var emailEvents = new List<EmailEventModel>
                {
                    new()
                    {
                        Id = "633129fb572a926667ed8c5g".ToObjectId(),
                        EmailEventType = EmailEventType.CsvDocumentProcess,
                        Subject = "Test Subject",
                        Body = "Test Body",
                        EmailFrom = "test@test.com",
                        Created = createdDate,
                        Sender = "test@test.com",
                        Cc = new List<string> {"test@test.com"},
                        Bcc = new List<string> {"test@test.com"},
                        To = new List<string> {"test@test.com"},
                    },
                };

                var _mockAsyncCursorEmailEvent = new Mock<IAsyncCursor<EmailEventModel>>();

                _mockAsyncCursorEmailEvent.Setup(a => a.Current).Returns(emailEvents);
                _mockAsyncCursorEmailEvent.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                _mockAsyncCursorEmailEvent.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockEmailEventCollection
                    .Setup(op => op.FindOneAndUpdateAsync(
                        It.IsAny<FilterDefinition<EmailEventModel>>(),
                        It.IsAny<UpdateDefinition<EmailEventModel>>(),
                        It.IsAny<FindOneAndUpdateOptions<EmailEventModel, EmailEventModel>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new EmailEventModel
                    {
                        Id = "633129fb572a926667ed8c5g".ToObjectId(),
                        EmailEventType = EmailEventType.CsvDocumentProcess,
                        Subject = "Test Subject",
                        Body = "Test Body",
                        EmailFrom = "test@test.com",
                        Created = createdDate,
                        Sender = "test@test.com",
                        Cc = new List<string> { "test@test.com" },
                        Bcc = new List<string> { "test@test.com" },
                        To = new List<string> { "test@test.com" },
                    });


                //Act
                var result = await Subject.HandleAsync(new EmailRepository.UpdateEmailEvent
                {
                    Id = "633129fb572a926667ed8c5g".ToObjectId(),
                    Error = null,
                    Sent = true,
                }, CancellationToken.None);


                //Assert
                _mockEmailEventCollection.Verify(c => c.FindOneAndUpdateAsync(
                    It.IsAny<FilterDefinition<EmailEventModel>>(),
                    It.IsAny<UpdateDefinition<EmailEventModel>>(),
                    It.IsAny<FindOneAndUpdateOptions<EmailEventModel, EmailEventModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.Should().BeEquivalentTo(ResultsTo.Success(new EmailEventModel
                {
                    Id = "633129fb572a926667ed8c5g".ToObjectId(),
                    EmailEventType = EmailEventType.CsvDocumentProcess,
                    Subject = "Test Subject",
                    Body = "Test Body",
                    EmailFrom = "test@test.com",
                    Created = createdDate,
                    Sender = "test@test.com",
                    Cc = new List<string> { "test@test.com" },
                    Bcc = new List<string> { "test@test.com" },
                    To = new List<string> { "test@test.com" },
                }));
            }
        }
    }
}
