using AutoMapper;
using MassTransit;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Interfaces.Klant;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Werkgever.Models.Mapping;
using Prechart.Service.Werkgever.Repository;
using Prechart.Service.Werkgever.Service;

namespace Prechart.Service.Werkgever.Test.Service;

public partial class WerkgeverServiceSpec : WithSubject<WerkgeverService>
{
    private IRequestClient<IGetKlant> _client;
    private FakeLogger<WerkgeverService> _logger;
    private IMapper _mapper;
    private IPublishEndpoint _publish;
    private IWerkgeverRepository _repository;
    private List<MongoWerkgeverModel> _werkgevers;

    public Given TheService => () =>
    {
        _logger = new FakeLogger<WerkgeverService>();
        _repository = An<IWerkgeverRepository>();
        _publish = An<IPublishEndpoint>();
        _client = An<IRequestClient<IGetKlant>>();
        _mapper = An<IMapper>();

        var config = new MapperConfiguration(cfg => { cfg.AddProfile(new WerkgeverMapper()); });
        config.AssertConfigurationIsValid();
        _mapper = config.CreateMapper();

        _werkgevers = new List<MongoWerkgeverModel>
        {
            new()
            {
                Id = "633129fb572a926667ed8c5e".ToObjectId(),
                Klant = new WerkgeverKlantModel
                {
                    KlantId = "633129fb572a926667ed8c51",
                    KlantName = "Klant",
                },
                Naam = "TestWerkgever",
                Sector = 0,
                FiscaalNummer = "123456789L01",
                LoonheffingenExtentie = "L01",
                OmzetbelastingExtentie = "B01",
                Actief = true,
            },
        };

        Subject = new WerkgeverService(_logger, _repository, _mapper, _client, _publish);
    };
}
