using FluentAssertions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Models;
using Prechart.Service.Email.Models;
using Prechart.Service.Email.Repositories;
using Xunit;

namespace Prechart.Service.Email.Test.Repositories;

public partial class EmailRepositorySpec
{
    public class WhenGetPendingEmails : EmailRepositorySpec
    {
        public IFluentResults Result { get; private set; }
        public class WhenGetPendingEmailsTypeNotFound : WhenGetPendingEmails
        {
            [Fact]
            public async void ThenNoDataFound()
            {
                //Arrange
                var emailEventModels = new List<EmailEventModel>();
                var _mockAsyncCursorEmailEvent = new Mock<IAsyncCursor<EmailEventModel>>();

                _mockAsyncCursorEmailEvent.Setup(a => a.Current).Returns(emailEventModels);
                _mockAsyncCursorEmailEvent.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                _mockAsyncCursorEmailEvent.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockEmailEventCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<EmailEventModel>>(),
                        It.IsAny<FindOptions<EmailEventModel>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_mockAsyncCursorEmailEvent.Object);

                //Act
                var result = await Subject.HandleAsync(new EmailRepository.GetPendingEmails
                {
                    PendingEmails = false,
                }, CancellationToken.None);

                //Assert
                _mockEmailEventCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<EmailEventModel>>(),
                    It.IsAny<FindOptions<EmailEventModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsNotFound().Should().BeTrue();
                result.Value.Should().BeNull();
            }
        }

        public class AndException : WhenGetPendingEmails
        {
            [Fact]
            public async void ThenException()
            {
                //Arrange
                _mockEmailEventCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<EmailEventModel>>(),
                        It.IsAny<FindOptions<EmailEventModel>>(),
                        It.IsAny<CancellationToken>()))
                    .Callback(() => throw new Exception("exception"));

                //Act
                var result = await Subject.HandleAsync(new EmailRepository.GetPendingEmails
                {
                    PendingEmails = false,
                }, CancellationToken.None);

                //Act
                _mockEmailEventCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<EmailEventModel>>(),
                    It.IsAny<FindOptions<EmailEventModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
                result.IsFailure().Should().BeTrue();
            }
        }

        public class WhenGetEmailEventTypeFound : WhenGetPendingEmails
        {
            [Fact]
            public async void ThenDataFound()
            {
                var dateCreated = DateTime.Now;

                //Arrange
                var emailEvents = new List<EmailEventModel>
                {
                    new()
                    {
                        Id = "633129fb572a926667ed8d8w".ToObjectId(),
                        EmailEventType = EmailEventType.CsvDocumentProcess,
                        Subject = "Test Subject",
                        Body = "Test Body",
                        EmailFrom = "test@test.com",
                        Created = dateCreated,
                        Sender = "test@test.com",
                        Cc = new List<string> {"test@test.com"},
                        Bcc = new List<string> {"test@test.com"},
                        To = new List<string> {"test@test.com"},
                        ProcessedOn = dateCreated,
                        Attachments= null,
                        Error = null,
                        Sent = true,
                    },
                };
                var _mockAsyncCursorEmailEvent = new Mock<IAsyncCursor<EmailEventModel>>();

                _mockAsyncCursorEmailEvent.Setup(a => a.Current).Returns(emailEvents);
                _mockAsyncCursorEmailEvent.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                _mockAsyncCursorEmailEvent.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockEmailEventCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<EmailEventModel>>(),
                        It.IsAny<FindOptions<EmailEventModel>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_mockAsyncCursorEmailEvent.Object);

                //Act
                var result = await Subject.HandleAsync(new EmailRepository.GetPendingEmails
                {
                    PendingEmails = false,
                }, CancellationToken.None);

                //Assert
                _mockEmailEventCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<EmailEventModel>>(),
                    It.IsAny<FindOptions<EmailEventModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsSuccess().Should().BeTrue();

                result.Value.Should().BeEquivalentTo(emailEvents);
            }
        }
    }
}
