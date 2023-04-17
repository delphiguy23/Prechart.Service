using FluentAssertions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Werkgever.Repository;
using Xunit;

namespace Prechart.Service.Werkgever.Test.Repository;

public partial class WerkgeverRepositorySpec
{
    public class WhenGetSqlWerkgevers : WerkgeverRepositorySpec
    {
        private IFluentResults<List<Database.Models.Werkgever>> result;
        public When GetSqlWerkgevers => async () => result = await Subject.HandleAsync(new WerkgeverRepository.GetSqlWerkgevers(), CancellationToken.None);

        public class AndNotFound : WhenGetSqlWerkgevers
        {
            [Fact]
            public void ThenNone()
            {
                result.Value.Any().Should().BeFalse();
            }
        }

        public class AndFound : WhenGetSqlWerkgevers
        {
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
                result
                    .Value
                    .Any(w => w.Id == 1)
                    .Should().BeTrue();
            }
        }
    }
}