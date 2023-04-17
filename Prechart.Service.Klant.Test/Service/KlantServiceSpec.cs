using MassTransit;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Models.Klant;
using Prechart.Service.Klant.Repository;
using Prechart.Service.Klant.Service;

namespace Prechart.Service.Klant.Test.Service;

public partial class KlantServiceSpec : WithSubject<KlantService>
{
    private List<KlantModel> _klantEmpty;
    private List<KlantModel> _klantModels;
    private FakeLogger<KlantService> _logger;
    private IPublishEndpoint _publish;
    private IKlantRepository _repository;

    public Given TheService => () =>
    {
        _logger = new FakeLogger<KlantService>();
        _repository = An<IKlantRepository>();
        _publish = An<IPublishEndpoint>();

        _klantModels = new List<KlantModel>
        {
            new()
            {
                Id = "633129fb572a926667ed8c5d".ToObjectId(),
                KlantNaam = "Test Klant",
                Werkgevers = new List<string> {"123456789L01"},
                ContactPersons = null,
                Active = true,
            },
        };

        _klantEmpty = new List<KlantModel>();

        Subject = new KlantService(_logger, _repository, _publish);
    };
}
