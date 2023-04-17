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
    public class WhenGetEmailEventType : EmailRepositorySpec
    {
        private EmailRepository.GetEmailEventType emailEventType = new()
        {
            EmailEventType = 0,
        };

        public IFluentResults<EmailEventRecipientModel> Result { get; private set; }

        public class WhenGetEmailEventTypeNull : WhenGetEmailEventType
        {
            public Given GetParamameters => () => { emailEventType = null; };

            public When GetMinimumLoon => async () => Result = await Subject.HandleAsync(emailEventType, CancellationToken.None);

            [Fact]
            public async void ThenDataEmailEventTypeNullFound()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }

        public class WhenGetEmailEventTypeNotFound : WhenGetEmailEventType
        {
            [Fact]
            public async void ThenNoDataFound()
            {
                //Arrange
                var emailEventRecipient = new List<EmailEventRecipientModel>();
                var _mockAsyncCursorEmailEventRecipient = new Mock<IAsyncCursor<EmailEventRecipientModel>>();

                _mockAsyncCursorEmailEventRecipient.Setup(a => a.Current).Returns(emailEventRecipient);
                _mockAsyncCursorEmailEventRecipient.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                _mockAsyncCursorEmailEventRecipient.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockEmailEventRecipientCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<EmailEventRecipientModel>>(),
                        It.IsAny<FindOptions<EmailEventRecipientModel>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_mockAsyncCursorEmailEventRecipient.Object);

                //Act
                var result = await Subject.HandleAsync(new EmailRepository.GetEmailEventType
                {
                    EmailEventType = (int)EmailEventType.CsvDocumentProcess,
                }, CancellationToken.None);


                //Assert
                _mockEmailEventRecipientCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<EmailEventRecipientModel>>(),
                    It.IsAny<FindOptions<EmailEventRecipientModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsNotFound().Should().BeTrue();
            }
        }

        public class AndException : WhenGetEmailEventType
        {
            [Fact]
            public async void ThenException()
            {
                //Arrange
                _mockEmailEventRecipientCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<EmailEventRecipientModel>>(),
                        It.IsAny<FindOptions<EmailEventRecipientModel>>(),
                        It.IsAny<CancellationToken>()))
                    .Callback(() => throw new Exception("exception"));

                //Act
                var result = await Subject.HandleAsync(new EmailRepository.GetEmailEventType
                {
                    EmailEventType = (int)EmailEventType.CsvDocumentProcess,
                }, CancellationToken.None);

                //Assert
                _mockEmailEventRecipientCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<EmailEventRecipientModel>>(),
                    It.IsAny<FindOptions<EmailEventRecipientModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
                result.IsFailure().Should().BeTrue();
            }
        }

        public class WhenGetEmailEventTypeFound : WhenGetEmailEventType
        {
            [Fact]
            public async void ThenDataFound()
            {
                //Arrange
                var emailEventRecipients = new List<EmailEventRecipientModel>
                {
                    new()
                    {
                        Id = "633129fb572a926667ed8c5x".ToObjectId(),
                        EmailEventType = (int) EmailEventType.CsvDocumentProcess,
                        Recipient = "test@test.com",
                        Name = "test noreply",
                        Cc = "test@test.com",
                        Bcc = "test@test.com",
                        IsActive = true,
                        IsDeleted = false,
                    },
                };

                var _mockAsyncCursorEmailEventRecipient = new Mock<IAsyncCursor<EmailEventRecipientModel>>();

                _mockAsyncCursorEmailEventRecipient.Setup(a => a.Current).Returns(emailEventRecipients);
                _mockAsyncCursorEmailEventRecipient.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                _mockAsyncCursorEmailEventRecipient.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockEmailEventRecipientCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<EmailEventRecipientModel>>(),
                        It.IsAny<FindOptions<EmailEventRecipientModel>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_mockAsyncCursorEmailEventRecipient.Object);
                //Act
                var result = await Subject.HandleAsync(new EmailRepository.GetEmailEventType
                {
                    EmailEventType = (int)EmailEventType.CsvDocumentProcess,
                }, CancellationToken.None);

                //Assert
                _mockEmailEventRecipientCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<EmailEventRecipientModel>>(),
                    It.IsAny<FindOptions<EmailEventRecipientModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.Value.EmailEventType = (int)EmailEventType.CsvDocumentProcess;
                result.IsSuccess().Should().BeTrue();
            }
        }
    }
}
