using FluentAssertions;
using MongoDB.Driver;
using Moq;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Globals.Models.Loonheffings;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Werkgever.Repository;
using Xunit;

namespace Prechart.Service.Werkgever.Test.Repository;

public partial class WerkgeverRepositorySpec
{
    public class WhenGetCollectieve : WerkgeverRepositorySpec
    {
        public class AndException : WhenGetCollectieve
        {
            [Fact]
            public async void ThenException()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                        It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                        It.IsAny<CancellationToken>()))
                    .Callback(() => throw new Exception("exception"));

                //Act
                var result = await Subject.HandleAsync(new WerkgeverRepository.GetCollectieve
                {
                    TaxNo = "123456789L01",
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
                result.IsFailure().Should().BeTrue();
                result.Messages.Any(m => m.Contains("exception")).Should().BeTrue();
            }
        }

        public class AndParamValue : WhenGetCollectieve
        {
            [Fact]
            public async void ThenNone()
            {
                //Arrange
                List<MongoWerkgeverModel> mongoWerkgeverlsNull = null;
                var mockAsyncCursorMongoWerkgeverModel = new Mock<IAsyncCursor<MongoWerkgeverModel>>();

                mockAsyncCursorMongoWerkgeverModel.Setup(a => a.Current).Returns(mongoWerkgeverlsNull);
                mockAsyncCursorMongoWerkgeverModel.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                mockAsyncCursorMongoWerkgeverModel.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                        It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(mockAsyncCursorMongoWerkgeverModel.Object);

                //Act
                var result = await Subject.HandleAsync(new WerkgeverRepository.GetCollectieve
                {
                    TaxNo = string.Empty,
                }, CancellationToken.None);

                //Assert
                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }
        }

        public class AndNotFound : WhenGetCollectieve
        {
            [Fact]
            public async void ThenNone()
            {
                //Arrange
                List<MongoWerkgeverModel> mongoWerkgeverlsNull = new();
                var mockAsyncCursorMongoWerkgeverModel = new Mock<IAsyncCursor<MongoWerkgeverModel>>();

                mockAsyncCursorMongoWerkgeverModel.Setup(a => a.Current).Returns(mongoWerkgeverlsNull);
                mockAsyncCursorMongoWerkgeverModel.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                mockAsyncCursorMongoWerkgeverModel.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                        It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(mockAsyncCursorMongoWerkgeverModel.Object);

                //Act
                var result = await Subject.HandleAsync(new WerkgeverRepository.GetCollectieve
                {
                    TaxNo = "123456789L01",
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }
        }

        public class AndDataFoundWithNoCollectieve : WhenGetCollectieve
        {
            [Fact]
            public async void ThenSome()
            {
                //Arrange
                List<MongoWerkgeverModel> mongoWerkgeverlsNotNull = new()
                {
                    new MongoWerkgeverModel
                    {
                        Id = "633129fb572a926667ed8c5e".ToObjectId(),
                        FiscaalNummer = "123456789L01",
                        DateCreated = new DateTime(2022, 1, 1),
                        Actief = true,
                        WhkPremies = new List<MongoWhkPremie>
                        {
                            new()
                            {
                                Id = "63437781b73780964600ba4d".ToObjectId(),
                                WgaVastWerkgever = 0,
                                WgaVastWerknemer = 0,
                                FlexWerkgever = 0,
                                FlexWerknemer = 0,
                                ZwFlex = 0,
                                ActiefVanaf = new DateTime(DateTime.Now.Year, 1, 1),
                                ActiefTot = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12)),
                                DateCreated = new DateTime(DateTime.Now.Year, 2, 1),
                                Actief = true,
                            },
                        },
                    },
                };

                var mockAsyncCursorMongoWerkgeverModel = new Mock<IAsyncCursor<MongoWerkgeverModel>>();

                mockAsyncCursorMongoWerkgeverModel.Setup(a => a.Current).Returns(mongoWerkgeverlsNotNull);
                mockAsyncCursorMongoWerkgeverModel.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                mockAsyncCursorMongoWerkgeverModel.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                        It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(mockAsyncCursorMongoWerkgeverModel.Object);

                //Act
                var result = await Subject.HandleAsync(new WerkgeverRepository.GetCollectieve
                {
                    TaxNo = "123456789L01",
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }
        }

        public class AndDataFound : WhenGetCollectieve
        {
            [Fact]
            public async void ThenSome()
            {
                //Arrange
                List<MongoWerkgeverModel> mongoWerkgeverlsNotNull = new()
                {
                    new MongoWerkgeverModel
                    {
                        Id = "633129fb572a926667ed8c5e".ToObjectId(),
                        FiscaalNummer = "123456789L01",
                        DateCreated = new DateTime(2022, 1, 1),
                        Actief = true,
                        WhkPremies = new List<MongoWhkPremie>
                        {
                            new()
                            {
                                Id = "63437781b73780964600ba4d".ToObjectId(),
                                WgaVastWerkgever = 0,
                                WgaVastWerknemer = 0,
                                FlexWerkgever = 0,
                                FlexWerknemer = 0,
                                ZwFlex = 0,
                                ActiefVanaf = new DateTime(DateTime.Now.Year, 1, 1),
                                ActiefTot = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12)),
                                DateCreated = new DateTime(DateTime.Now.Year, 2, 1),
                                Actief = true,
                            },
                        },
                        Collectieve = new List<CollectieveAangifteModel>
                        {
                            new()
                            {
                                TaxNo = "123456789L01",
                                Periode = $"{DateTime.Now.Year}-{DateTime.Now.Month:D2}",
                                ProcessedDate = DateTime.Now,
                                CollectieveType = CollectieveType.Normaal,
                                TotLnLbPh = 0,
                                TotLnSv = 0,
                                TotPrlnAofAnwLg = 0,
                                TotPrlnAofAnwHg = 0,
                                TotPrlnAofAnwUit = 0,
                                TotPrlnAwfAnwLg = 0,
                                TotPrlnAwfAnwHg = 0,
                                TotPrlnAwfAnwHz = 0,
                                PrLnUfo = 0,
                                IngLbPh = 0,
                                EhPubUitk = 0,
                                EhGebrAuto = 0,
                                EhVut = 0,
                                EhOvsFrfWrkkstrg = 0,
                                AvZeev = 0,
                                VrlAvso = 0,
                                TotPrAofLg = 0,
                                TotPrAofHg = 0,
                                TotPrAofUit = 0,
                                TotOpslWko = 0,
                                TotPrGediffWhk = 0,
                                TotPrAwfLg = 0,
                                TotPrAwfHg = 0,
                                TotPrAwfHz = 0,
                                PrUfo = 0,
                                IngBijdrZvw = 0,
                                TotWghZvw = 0,
                                TotTeBet = 0,
                                TotGen = 0,
                                SaldoCorrectiesVoorgaandTijdvak = new SaldoCorrectiesVoorgaandTijdvakModel[]
                                {
                                },
                            },
                        },
                    },
                };

                var mockAsyncCursorMongoWerkgeverModel = new Mock<IAsyncCursor<MongoWerkgeverModel>>();

                mockAsyncCursorMongoWerkgeverModel.Setup(a => a.Current).Returns(mongoWerkgeverlsNotNull);
                mockAsyncCursorMongoWerkgeverModel.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
                mockAsyncCursorMongoWerkgeverModel.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

                _mockCollection
                    .Setup(op => op.FindAsync(
                        It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                        It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(mockAsyncCursorMongoWerkgeverModel.Object);

                //Act
                var result = await Subject.HandleAsync(new WerkgeverRepository.GetCollectieve
                {
                    TaxNo = "123456789L01",
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.FindAsync(
                    It.IsAny<FilterDefinition<MongoWerkgeverModel>>(),
                    It.IsAny<FindOptions<MongoWerkgeverModel>>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                result.IsSuccess().Should().BeTrue();

                result
                    .Value
                    .Any(c => c.TaxNo == "123456789L01" &&
                              c.Periode == $"{DateTime.Now.Year}-{DateTime.Now.Month:D2}")
                    .Should()
                    .BeTrue();
            }
        }
    }
}