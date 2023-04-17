using FluentAssertions;
using Microsoft.Extensions.Logging;
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
    public class GetAddToEmailArchive : EmailServiceSpec
    {
        public List<EmailAttachmentModel> attachments;
        public List<string> bcc;
        public string body;
        public List<string> cc;
        public DateTime created;
        public EmailEventType emailEventType;
        public string error;

        private IFluentResults<EmailEventModel> Result;
        public DateTime? sent;
        public string subject;
        public List<string> to;

        public When WGetAddToEmailArchive => async () => Result = await Subject.HandleAsync(new EmailService.AddToEmailArchive
        {
            Subject = subject,
            Body = body,
            To = to,
            Cc = cc,
            Bcc = bcc,
            Attachments = attachments,
            EmailEventType = emailEventType,
            Sent = sent,
            Created = created,
            Error = error,
        }, CancellationToken.None);

        public class WhenAddToEmailArchiveEmailRecipientNull : GetAddToEmailArchive
        {
            public And GetEmailEventRecipientModel => () => _repository.HandleAsync(Arg.Any<EmailRepository.GetEmailEventType>(), CancellationToken.None).Returns<IFluentResults<EmailEventRecipientModel>>(e =>
            ResultsTo.NotFound(new EmailEventRecipientModel()));

            public And GetInsertEmailEvent => () => _repository.HandleAsync(Arg.Any<EmailRepository.InsertEmailEvent>(), CancellationToken.None).Returns<IFluentResults<EmailEventModel>>(e =>
            ResultsTo.Something(
                new EmailEventModel
                {
                    Sender = "testsender@test.com",
                })
            );

            [Fact]
            public void ThenResultShouldBeEmailMessage()
            {
                Result.Messages.Should().BeEquivalentTo("Email Recipient is null");
            }

            [Fact]
            public void ThenResultShouldBeEmailRecipientNull()
            {
                Result.Value.Should().BeEquivalentTo(new EmailEventModel());
            }
        }

        public class WhenGetEmailEventTypeFoundException : GetAddToEmailArchive
        {
            public And Data => () =>
            {
                subject = "Test Subject";
                body = "Test Body";
                to = new List<string>();
                cc = new List<string>();
                bcc = new List<string>();
                attachments = new List<EmailAttachmentModel>();
                emailEventType = new EmailEventType();
                created = DateTime.Now;
            };


            public And GetEmailRecipient => () => _repository.HandleAsync(Arg.Any<EmailRepository.GetEmailEventType>(), CancellationToken.None).Returns<IFluentResults<EmailEventRecipientModel>>(e => throw new Exception("GetEmailEventType Error"));

            public And GetInsertEmailEvent => () => _repository.HandleAsync(Arg.Any<EmailRepository.InsertEmailEvent>(), CancellationToken.None).Returns<IFluentResults<EmailEventModel>>(e =>
            ResultsTo.Something(
                new EmailEventModel
                {
                    Sender = "testsender@test.com",
                })
            );

            [Fact]
            public void ThenLoggerException()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message.StartsWith("GetEmailEventType Error")).Should().BeTrue();
            }

            [Fact]
            public void ThenResultShouldBeEmailRecipientIsException()
            {
                Result.IsFailure().Should().BeTrue();
            }
        }
    }
}
