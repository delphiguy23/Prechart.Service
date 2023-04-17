using FluentAssertions;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
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
    public class WhenUpsertEmailEventRecipient : EmailRepositorySpec
    {
        private readonly EmailRepository.UpsertEmailEventRecipient upsertedEmailEvent = new();
        private ObjectId upsertId;

        public IFluentResults Result { get; private set; }

        public class WhenGetEmailEventTypeNull : WhenUpsertEmailEventRecipient
        {
            public Given GetParamameters => () => { upsertedEmailEvent.EmailEvent = null; };

            public When GetInsertEmailEvent => async () => Result = await Subject.HandleAsync(upsertedEmailEvent, CancellationToken.None);

            [Fact]
            public async void ThenDataInsertEmailEventNullFound()
            {
                Result.IsNotFoundOrBadRequest().Should().BeTrue();
            }
        }

        public class AndException : WhenUpsertEmailEventRecipient
        {
            [Fact]
            public async void ThenException()
            {
                _mockEmailEventRecipientCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<EmailEventRecipientModel>>(),
                        It.IsAny<FindOptions<EmailEventRecipientModel>>(),
                        It.IsAny<CancellationToken>()))
                    .Callback(() => throw new Exception("exception"));

                //Act
                var result = await Subject.HandleAsync(new EmailRepository.UpsertEmailEventRecipient
                {
                    EmailEvent = new EmailEventRecipientModel
                    {
                        Id = new ObjectId(),
                        Name = "Test",
                        Recipient = "test@test.com",
                        EmailEventType = (int)EmailEventType.Normal,
                        Cc = "test@test.com",
                        Bcc = "test@test.com",
                        IsActive = true,
                        IsDeleted = false,
                    },
                }, CancellationToken.None);

                //Assert
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
                result.IsFailure().Should().BeTrue();
            }
        }
        public class AndUpsertedInvalidRecipient : WhenUpsertEmailEventRecipient
        {
            [Fact]
            public async void ThenUpsertedInvalidRecipient()
            {
                //Arrange
                _mockEmailEventRecipientCollection
                    .Setup(op => op.InsertOneAsync(
                        It.IsAny<EmailEventRecipientModel>(),
                        It.IsAny<InsertOneOptions>(),
                        It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult((object)null));

                //Act
                var result = await Subject.HandleAsync(new EmailRepository.UpsertEmailEventRecipient
                {
                    EmailEvent = new EmailEventRecipientModel
                    {
                        Id = "636c94d6f25d84331d7d6t45".ToObjectId(),
                        Name = "Test",
                        Recipient = "test@test.com;invalid",
                        EmailEventType = (int)EmailEventType.Normal,
                        Cc = "test@test.com",
                        Bcc = "test@test.com",
                        IsActive = true,
                        IsDeleted = false,
                    },
                }, CancellationToken.None);

                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }
        }
        public class AndInserted : WhenUpsertEmailEventRecipient
        {
            [Fact]
            public async void ThenInserted()
            {
                //Arrange
                List<EmailEventRecipientModel> emailEventRecipientNull = new();

                var mockAsyncCursoEventRecipientModel = new Mock<IAsyncCursor<EmailEventRecipientModel>>();

                mockAsyncCursoEventRecipientModel.Setup(a => a.Current).Returns(emailEventRecipientNull);
                mockAsyncCursoEventRecipientModel.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                mockAsyncCursoEventRecipientModel.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockEmailEventRecipientCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<EmailEventRecipientModel>>(),
                        It.IsAny<FindOptions<EmailEventRecipientModel>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(mockAsyncCursoEventRecipientModel.Object);

                _mockEmailEventRecipientCollection
                    .Setup(op => op.InsertOneAsync(
                        It.IsAny<EmailEventRecipientModel>(),
                        It.IsAny<InsertOneOptions>(),
                        It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult((object)null));

                //Act
                var result = await Subject.HandleAsync(new EmailRepository.UpsertEmailEventRecipient
                {
                    EmailEvent = new EmailEventRecipientModel
                    {
                        Id = "636c94d6f25d84331d7d6t45".ToObjectId(),
                        Name = "Test",
                        Recipient = "test@test.com",
                        EmailEventType = (int)EmailEventType.Normal,
                        Cc = "test@test.com",
                        Bcc = "test@test.com",
                        IsActive = true,
                        IsDeleted = false,
                    },
                }, CancellationToken.None);

                result.Messages.Should().BeEquivalentTo("Records Added");
                result.IsSuccess().Should().BeTrue();
            }
        }

        public class AndUpdated : WhenUpsertEmailEventRecipient
        {
            [Fact]
            public async void ThenUpdated()
            {
                //Arrange
                List<EmailEventRecipientModel> emailEventRecipient =
                    new List<EmailEventRecipientModel>
                {
                   new EmailEventRecipientModel
                    {
                        Id = "636c94d6f25d84331d7d6t45".ToObjectId(),
                        Name = "Test",
                        Recipient = "test@test.com",
                        EmailEventType = (int)EmailEventType.Normal,
                        Cc = "test@test.com",
                        Bcc = "test@test.com",
                        IsActive = true,
                        IsDeleted = false,
                    }
                };

                var mockAsyncCursoEventRecipientModel = new Mock<IAsyncCursor<EmailEventRecipientModel>>();

                mockAsyncCursoEventRecipientModel.Setup(a => a.Current).Returns(emailEventRecipient);
                mockAsyncCursoEventRecipientModel.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                mockAsyncCursoEventRecipientModel.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockEmailEventRecipientCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<EmailEventRecipientModel>>(),
                        It.IsAny<FindOptions<EmailEventRecipientModel>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(mockAsyncCursoEventRecipientModel.Object);

                _mockEmailEventRecipientCollection
                    .Setup(op => op.InsertOneAsync(
                        It.IsAny<EmailEventRecipientModel>(),
                        It.IsAny<InsertOneOptions>(),
                        It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult((object)null));

                //Act
                var result = await Subject.HandleAsync(new EmailRepository.UpsertEmailEventRecipient
                {
                    EmailEvent = new EmailEventRecipientModel
                    {
                        Id = "636c94d6f25d84331d7d6t45".ToObjectId(),
                        Name = "Test",
                        Recipient = "test@test.com",
                        EmailEventType = (int)EmailEventType.Normal,
                        Cc = "test@test.com",
                        Bcc = "test@test.com",
                        IsActive = true,
                        IsDeleted = false,
                    },
                }, CancellationToken.None);

                result.Messages.Should().BeEquivalentTo("Records Updated");
                result.IsSuccess().Should().BeTrue();
            }
        }
    }
}
