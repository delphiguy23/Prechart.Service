using FluentAssertions;
using MongoDB.Bson;
using NSubstitute;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Models;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Email.Models;
using Prechart.Service.Email.Repositories;
using Prechart.Service.Email.Service;
using Prechart.Service.Globals.Models.Email;
using Xunit;

namespace Prechart.Service.Email.Test.Services;

public partial class EmailServiceSpec
{
    public class WhenSendPendingEmails : EmailServiceSpec
    {
        private IFluentResults<List<EmailEventModel>> Result;
        public When SendPendingEmails => async () => Result = await Subject.HandleAsync(new EmailService.SendPendingEmails(), CancellationToken.None);

        public class WhenSendPendingEmailRecipientNull : WhenSendPendingEmails
        {
            public And GetEmailRecipient => () => _repository.HandleAsync(Arg.Any<EmailRepository.GetPendingEmails>(), CancellationToken.None).Returns<IFluentResults<List<EmailEventModel>>>(e =>
           ResultsTo.NotFound(new List<EmailEventModel>()));

            [Fact]
            public void ThenResultShouldBeEmailRecipientNull()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }

        public class WhenSendPendingEmailsUsernameNull : WhenSendPendingEmails
        {
            public And GetHostName => () => _generalConfiguration.Value.MailSettings.Host = null;
            public And GetEmailRecipient => () => _repository.HandleAsync(Arg.Any<EmailRepository.GetPendingEmails>(), CancellationToken.None).Returns<IFluentResults<List<EmailEventModel>>>(ResultsTo.Something(new List<EmailEventModel>()));

            [Fact]
            public void ThenResultShouldBeEmailUsernameNull()
            {
                Result.IsBadRequest().Should().BeTrue();
            }
        }

        public class WhenSendPendingEmailsNotFpund : WhenSendPendingEmails
        {
            public And GetPendingEmails => () => _repository.HandleAsync(Arg.Any<EmailRepository.GetPendingEmails>(), CancellationToken.None).Returns<IFluentResults<List<EmailEventModel>>>(ResultsTo.NotFound<List<EmailEventModel>>());

            public And UpdateEmailEvent => () => _repository.HandleAsync(Arg.Any<EmailRepository.UpdateEmailEvent>(), CancellationToken.None).Returns(ResultsTo.NotFound(new EmailEventModel()));

            [Fact]
            public void ThenResultShouldBeSuccess()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }

        public class WhenSendPendingEmailsSuccess : WhenSendPendingEmails
        {
            public And GetPendingEmails => () => _repository.HandleAsync(Arg.Any<EmailRepository.GetPendingEmails>(), CancellationToken.None).Returns<IFluentResults<List<EmailEventModel>>>(
                ResultsTo.Something(
                    new List<EmailEventModel>
                        {
                            new()
                            {
                                Id = new ObjectId(),
                                Attachments = new List<EmailAttachmentModel>(),
                                EmailEventType = EmailEventType.CsvDocumentProcess,
                                Subject = "Test Subject",
                                Body = "Test Body",
                                Created = DateTime.Now,
                                To = new List<string> {"testsender@test.com"},
                            },
                        }
                    ));

            public And UpdateEmailEvent => () => _repository.HandleAsync(Arg.Any<EmailRepository.UpdateEmailEvent>(), CancellationToken.None).Returns(ResultsTo.Success(new EmailEventModel
            {
                Id = new ObjectId(),
                Attachments = new List<EmailAttachmentModel>(),
                EmailEventType = EmailEventType.CsvDocumentProcess,
                Subject = "Test Subject",
                Body = "Test Body",
                Created = DateTime.Now,
                To = new List<string> { "testsender@test.com" },
            }));

            [Fact]
            public void ThenResultShouldBeSuccess()
            {
                Result.IsSuccess().Should().BeTrue();
            }
        }
    }
}
