using FluentAssertions;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Werkgever.Repository;
using Xunit;

namespace Prechart.Service.Werkgever.Test.Repository;

public partial class WerkgeverRepositorySpec
{
    public class WhenUpsertFromMongoWerkgever : WerkgeverRepositorySpec
    {
        private bool isActief = true;
        private IFluentResults<int> result;

        public When UpsertFromMongoWerkgever => async () => result = await Subject.HandleAsync(new WerkgeverRepository.UpsertFromMongoWerkgever
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
                WhkPremies = null,
                DatumActiefVanaf = new DateTime(DateTime.Now.Year, 1, 1),
                DatumActiefTot = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12)),
                DateCreated = new DateTime(DateTime.Now.Year, 1, 1),
                DateLastModified = DateTime.Now,
                Actief = isActief,
            },
        }, CancellationToken.None);

        public class AndNotFound : WhenUpsertFromMongoWerkgever
        {
            [Fact]
            public void ThenSome()
            {
                result.IsSuccess().Should().BeTrue();
            }

            public void ThenOne()
            {
                result.Value.Should().Be(1);
            }

            [Fact]
            public void ThenInserted()
            {
                var result = _context.Werkgever.Any(w => w.FiscaalNummer == "123456789L01").Should().BeTrue();
            }
        }

        public class AndFound : WhenUpsertFromMongoWerkgever
        {
            public And IsNotActief => () => isActief = false;

            public Given Data => () =>
            {
                _context.Werkgever.Add(new Database.Models.Werkgever
                {
                    WerkgeverMongoId = "634cc67ccd68d2512732a337",
                    KlantMongoId = "634cc67ccd68d2512732a355",
                    Naam = "TestWerkgever",
                    Sector = 0,
                    FiscaalNummer = "123456789L01",
                    LoonheffingenExtentie = "01",
                    OmzetbelastingExtentie = "B01",
                    DatumActiefVanaf = new DateTime(DateTime.Now.Year, 1, 1),
                    DatumActiefTot = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12)),
                    Actief = true,
                    Id = 1,
                });

                _context.SaveChanges();
            };

            [Fact]
            public void ThenSome()
            {
                result.IsSuccess().Should().BeTrue();
            }

            [Fact]
            public void ThenOne()
            {
                result.Value.Should().Be(1);
            }

            [Fact]
            public void ThenUpdated()
            {
                var result = _context.Werkgever.Any(w => w.FiscaalNummer == "123456789L01" && w.Actief == false).Should().BeTrue();
            }
        }
    }
}
