using FluentAssertions;
using NSubstitute;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Interfaces.Loonheffings;
using Prechart.Service.Globals.Interfaces.Werkgever;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Loonheffings.Models;
using Prechart.Service.Loonheffings.Repositories;
using Prechart.Service.Loonheffings.Service;
using Xunit;

namespace Prechart.Service.Loonheffings.Test.Service;

public partial class LoonheffingsServiceSpec
{
    public class WhenProcessedUploadedXmls : LoonheffingsServiceSpec
    {
        private IFluentResults<List<ProcessXmlResult>> result;
        public When LoonheffingsProcessedResult => async () => result = await Subject.HandleAsync(new LoonheffingsService.ProcessUploadedXmls(), CancellationToken.None);

        public class AndException : WhenProcessedUploadedXmls
        {
            public And GetData => () => _repository.HandleAsync(Arg.Any<LoonheffingsRepository.GetUnProcessUploadedTaxFiling>(), CancellationToken.None)
                .Returns<IFluentResults<List<UnprocessedUploads>>>(e => throw new Exception("exception"));

            [Fact]
            public void ThenShouldReturnFailure()
            {
                result.IsFailure().Should().BeTrue();
            }

            [Fact]
            public void ThenShouldReturnException()
            {
                result.Messages.Any(m => m.Contains("exception")).Should().BeTrue();
            }

            [Fact]
            public void ThenLogger()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
            }
        }

        public class AndDataNotFound : WhenProcessedUploadedXmls
        {
            public And Data => () => _unprocessedUploads.Person = null;

            public And GetData => () => _repository.HandleAsync(Arg.Any<LoonheffingsRepository.GetUnProcessUploadedTaxFiling>(), CancellationToken.None)
                .Returns<IFluentResults<List<UnprocessedUploads>>>(d => ResultsTo.Something(new List<UnprocessedUploads> {_unprocessedUploads}));

            [Fact]
            public void ThenShouldReturnSome()
            {
                result.IsSuccess().Should().BeTrue();
            }
            
            [Fact]
            public async Task ThenHandlerIsCalled()
            {
                await _repository.Received(1).HandleAsync(Arg.Any<LoonheffingsRepository.GetUnProcessUploadedTaxFilingWithoutEmployees>(), CancellationToken.None);
            }

            [Fact]
            public async Task ThenPublishEvent_IXmlToPersons()
            {
                await _publishEndpoint.Received(0).Publish<IXmlToPersons>(Arg.Any<object>());
            }
        }

        public class AndDataFound : WhenProcessedUploadedXmls
        {
            public And GetData => () => _repository.HandleAsync(Arg.Any<LoonheffingsRepository.GetUnProcessUploadedTaxFiling>(), CancellationToken.None)
                .Returns<IFluentResults<List<UnprocessedUploads>>>(d => ResultsTo.Something(new List<UnprocessedUploads> {_unprocessedUploads}));

            [Fact]
            public void ThenShouldReturnSome()
            {
                result.IsSuccess().Should().BeTrue();
            }

            [Fact]
            public void ThenLoggerStart()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("Start")).Should().BeTrue();
            }

            [Fact]
            public void ThenLoggerEnd()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.StartsWith("End")).Should().BeTrue();
            }

            [Fact]
            public async Task ThenHandlerIsCalled()
            {
                await _repository.Received(1).HandleAsync(Arg.Any<LoonheffingsRepository.GetUnProcessUploadedTaxFilingWithoutEmployees>(), CancellationToken.None);
            }

            [Fact]
            public async Task ThenPublishEvent_IMongoWerkgevers()
            {
                await _publishEndpoint.Received(1).Publish<IMongoWerkgevers>(
                    Verify.That<object>(o => o.Should().BeEquivalentTo(new MongoWerkgeversModel
                    {
                        Werkgevers = new List<IMongoWerkgever>
                        {
                            new MongoWerkgeverModel
                            {
                                Klant = new WerkgeverKlantModel
                                {
                                    KlantId = null,
                                    KlantName = "test klant",
                                },
                                Naam = "123456789L01",
                                Sector = 0,
                                FiscaalNummer = "123456789L01",
                                LoonheffingenExtentie = "L01",
                                OmzetbelastingExtentie = "B01",
                                DatumActiefVanaf = new DateTime(2022, 1, 1),
                                DatumActiefTot = new DateTime(2023, 12, 31),
                                Actief = true,
                            },
                        },
                    }, option =>
                        option
                            .Excluding(exclude => exclude.Werkgevers[0].Klant)
                            .Excluding(exclude => exclude.Werkgevers[0].DateCreated)
                            .Excluding(exclude => exclude.Werkgevers[0].DateLastModified))));
            }

            [Fact]
            public async Task ThenPublishEvent_IXmlToPersons()
            {
                await _publishEndpoint.Received(1).Publish<IXmlToPersons>(
                    Verify.That<object>(o => o.Should().BeEquivalentTo(new
                    {
                        Persons = new List<EmployeeOnBoardingModel>
                        {
                            new()
                            {
                                SofiNr = "123456789",
                            },
                        },
                    }, option =>
                        option
                            .Excluding(exclude => exclude.Persons[0].Id)
                            .Excluding(exclude => exclude.Persons[0].Voorletter)
                            .Excluding(exclude => exclude.Persons[0].Voorvoegsel)
                            .Excluding(exclude => exclude.Persons[0].SignificantAchternaam)
                            .Excluding(exclude => exclude.Persons[0].Geboortedatum)
                            .Excluding(exclude => exclude.Persons[0].Nationaliteit)
                            .Excluding(exclude => exclude.Persons[0].Geslacht)
                            .Excluding(exclude => exclude.Persons[0].Werkgever)
                            .Excluding(exclude => exclude.Persons[0].AdresBinnenland)
                            .Excluding(exclude => exclude.Persons[0].AdresBuitenland)
                            .Excluding(exclude => exclude.Persons[0].PersonType)
                            .Excluding(exclude => exclude.Persons[0].Active)
                            .Excluding(exclude => exclude.Persons[0].DateCreated)
                            .Excluding(exclude => exclude.Persons[0].DateLastModified)
                            .Excluding(exclude => exclude.Persons[0].TaxPaymentDetails)
                            .Excluding(exclude => exclude.Persons[0].TaxFileName))));
            }
        }
    }
}
