using FluentAssertions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Werkgever.Database.Models;
using Prechart.Service.Werkgever.Repository;
using Xunit;

namespace Prechart.Service.Werkgever.Test.Repository;

public partial class WerkgeverRepositorySpec
{
    public class WhenGetSqlWerkgeversWhkPremies : WerkgeverRepositorySpec
    {
        private IFluentResults<List<WerkgeverWhkPremies>> result;

        public When GetSqlWerkgeversWhkPremies => async () => result = await Subject.HandleAsync(new WerkgeverRepository.GetSqlWerkgeversWhkPremies
        {
            WerkgeverId = 1,
        }, CancellationToken.None);

        public class AndNotFound : WhenGetSqlWerkgeversWhkPremies
        {
            [Fact]
            public void ThenNone()
            {
                result.Value.Any().Should().BeFalse();
            }
        }

        public class AndFound : WhenGetSqlWerkgeversWhkPremies
        {
            public Given Data => () =>
            {
                _context.WerkgeverWhkPremies.Add(new WerkgeverWhkPremies
                {
                    Id = 1,
                    WerkgeverId = 1,
                    Actief = true,
                });

                _context.SaveChanges();
            };

            [Fact]
            public void ThenSome()
            {
                result
                    .Value
                    .Any(w => w.Id == 1 && w.WerkgeverId == 1)
                    .Should().BeTrue();
            }
        }
    }
}