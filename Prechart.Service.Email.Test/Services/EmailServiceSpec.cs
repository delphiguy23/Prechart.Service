using Hangfire;
using MassTransit;
using Microsoft.Extensions.Options;
using NSubstitute;
using Prechart.Service.Core.Configuration;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Email.Repositories;
using Prechart.Service.Email.Service;

namespace Prechart.Service.Email.Test.Services;

public partial class EmailServiceSpec : WithSubject<EmailService>
{
    private FakeLogger<EmailService> _logger;
    private IPublishEndpoint _publish;
    private IEmailRepository _repository;
    private IBackgroundJobClient _backgroundJobClient;

    public GeneralConfiguration generalConfiguration { get; set; } = new GeneralConfiguration
    {
        MailSettings = new MailConfiguration
        {
            Host = "host",
            Sender = "sender",
            SenderName = "defaultfrom@email",
            Password = "pwd",
            Port = "584",
            Ssl = true,
            UserName = "user",
        }
    };

    private IOptions<GeneralConfiguration> _generalConfiguration;
    public Given TheService => () =>
    {
        _logger = new FakeLogger<EmailService>();
        _repository = An<IEmailRepository>();
        _publish = An<IPublishEndpoint>();
        _backgroundJobClient = An<IBackgroundJobClient>();
        _generalConfiguration = An<IOptions<GeneralConfiguration>>();
        _generalConfiguration.Value.Returns(generalConfiguration);

        Subject = new EmailService(_generalConfiguration, _logger, _repository, _backgroundJobClient);
    };
}
