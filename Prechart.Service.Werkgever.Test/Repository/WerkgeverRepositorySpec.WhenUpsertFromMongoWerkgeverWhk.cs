using FluentAssertions;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Werkgever.Database.Models;
using Prechart.Service.Werkgever.Repository;
using Xunit;

namespace Prechart.Service.Werkgever.Test.Repository;

public partial class WerkgeverRepositorySpec
{
    public class WhenUpsertFromMongoWerkgeverWhk : WerkgeverRepositorySpec
    {
        private static List<MongoWhkPremie> whk;
        private IFluentResults result;

        public When UpsertFromMongoWerkgeverWhk => async () => result = await Subject.HandleAsync(new WerkgeverRepository.UpsertFromMongoWerkgeverWhk
        {
            Werkgever = new MongoWerkgeverModel
            {
                Id = "634cc67ccd68d2512732a337".ToObjectId(),
                Klant = new WerkgeverKlantModel
                {
                    KlantId = "634cc67ccd68d2512732a355",
                    KlantName = "TestKlant",
                },
                Naam = "TestWerkgever",
                Sector = 0,
                FiscaalNummer = "123456789L01",
                LoonheffingenExtentie = "01",
                OmzetbelastingExtentie = "B01",
                WhkPremies = whk,
                DatumActiefVanaf = new DateTime(DateTime.Now.Year, 1, 1),
                DatumActiefTot = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12)),
                DateCreated = new DateTime(DateTime.Now.Year, 1, 1),
                DateLastModified = DateTime.Now,
                Actief = true,
            },
            WerkgeverId = 1,
        }, CancellationToken.None);

        public class AndNoWhk : WhenUpsertFromMongoWerkgeverWhk
        {
            public And Data => () => whk = null;

            [Fact]
            public void ThenNone()
            {
                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }

            [Fact]
            public void ThenLogger()
            {
                _logger
                    .ReceivedLogMessages
                    .FirstOrDefault(log => log.LogLevel == LogLevel.Information &&
                                           log.Message.ToLowerInvariant() == "no whkpremie to upsert.")
                    .Exception.Should()
                    .BeNull();
            }
        }

        public class AndInsertWhk : WhenUpsertFromMongoWerkgeverWhk
        {
            public Given WhkData => () =>
            {
                whk = new List<MongoWhkPremie>
                {
                    new()
                    {
                        Id = "63437753b73780964600b8c3".ToObjectId(),
                        WgaVastWerkgever = 1,
                        WgaVastWerknemer = 1,
                        FlexWerkgever = 1,
                        FlexWerknemer = 1,
                        ZwFlex = 1,
                        ActiefVanaf = new DateTime(DateTime.Now.Year, 1, 1),
                        ActiefTot = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12)),
                        DateCreated = new DateTime(DateTime.Now.Year, 1, 1),
                        DateLastModified = DateTime.Now,
                        Actief = true,
                    },
                };
            };

            [Fact]
            public void ThenSuccess()
            {
                result.IsSuccess().Should().BeTrue();
            }

            [Fact]
            public void ThenUpdated()
            {
                var result = _context.WerkgeverWhkPremies.Any(w => w.WerkgeverWhkMongoId == "63437753b73780964600b8c3" && w.WerkgeverId == 1).Should().BeTrue();
            }
        }

        public class AndUpdateWhk : WhenUpsertFromMongoWerkgeverWhk
        {
            public Given Data => () =>
            {
                _context.WerkgeverWhkPremies.Add(new WerkgeverWhkPremies
                {
                    Id = 1,
                    WerkgeverId = 1,
                    WerkgeverWhkMongoId = "63437753b73780964600b8c3",
                    WgaVastWerkgever = 1,
                    WgaVastWerknemer = 1,
                    FlexWerkgever = 1,
                    FlexWerknemer = 1,
                    ZwFlex = 1,
                    Totaal = 5,
                    ActiefVanaf = new DateTime(DateTime.Now.Year, 1, 1),
                    ActiefTot = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12)),
                    Actief = true,
                });

                _context.SaveChanges();
            };

            public Given WhkData => () =>
            {
                whk = new List<MongoWhkPremie>
                {
                    new()
                    {
                        Id = "63437753b73780964600b8c3".ToObjectId(),
                        WgaVastWerkgever = 5,
                        WgaVastWerknemer = 5,
                        FlexWerkgever = 5,
                        FlexWerknemer = 5,
                        ZwFlex = 5,
                        ActiefVanaf = new DateTime(DateTime.Now.Year, 1, 1),
                        ActiefTot = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12)),
                        DateCreated = new DateTime(DateTime.Now.Year, 1, 1),
                        DateLastModified = DateTime.Now,
                        Actief = true,
                    },
                };
            };

            [Fact]
            public void ThenSuccess()
            {
                result.IsSuccess().Should().BeTrue();
            }

            [Fact]
            public void ThenUpdated()
            {
                var result = _context.WerkgeverWhkPremies.Any(w => w.WerkgeverWhkMongoId == "63437753b73780964600b8c3" && w.WerkgeverId == 1).Should().BeTrue();
            }

            [Fact]
            public void ThenUpdated2()
            {
                var result = _context.WerkgeverWhkPremies.Any(w =>
                    w.WerkgeverWhkMongoId == "63437753b73780964600b8c3" &&
                    w.WerkgeverId == 1 &&
                    w.WgaVastWerkgever == 5 &&
                    w.WgaVastWerknemer == 5 &&
                    w.FlexWerkgever == 5 &&
                    w.FlexWerknemer == 5 &&
                    w.ZwFlex == 5
                ).Should().BeTrue();
            }
        }
    }
}