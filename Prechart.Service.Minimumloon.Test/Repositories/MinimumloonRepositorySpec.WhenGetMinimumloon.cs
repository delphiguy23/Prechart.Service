using FluentAssertions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Minimumloon.Models;
using Prechart.Service.Minimumloon.Repositories;
using Xunit;

namespace Prechart.Service.Minimumloon.Test.Repositories;

public partial class MinimumloonRepositorySpec
{
    public class GetMinimumloon : MinimumloonRepositorySpec
    {
        private MinimumloonRepository.GetMinimumloon minimumLoon = new()
        {
            Datum = new DateTime(DateTime.Now.Year, 1, DateTime.DaysInMonth(DateTime.Now.Year, 1)),
        };

        public IFluentResults<List<Database.Models.Minimumloon>> Result { get; private set; }

        public When GetMinimumLoon => async () => Result = await Subject.HandleAsync(minimumLoon, CancellationToken.None);

        public class AndGetMinimumloonNone : GetMinimumloon
        {
            public Given GetParamameters => () => { minimumLoon.Datum = DateTime.Now; };

            [Fact]
            public void ThenShouldGetMinimumloonNone()
            {
                Result.Value.Count().Should().Be(0);
            }

            [Fact]
            public void ThenShouldGetMinimumloonFoundTrue()
            {
                Result.IsSuccess().Should().BeTrue();
            }
        }

        public class AndGetMinimumloonNull : GetMinimumloon
        {
            public Given GetParamameters => () => { minimumLoon = null; };

            [Fact]
            public void ThenShouldGetMinimumloonNull()
            {
                Result.IsBadRequest().Should().BeTrue();
            }
        }

        public class AndGetMinimumloonSuccess : GetMinimumloon
        {
            public And Data => () =>
            {
                Context.Minimumloon.Add(new Database.Models.Minimumloon
                {
                    Id = 1,
                    MinimumloonLeeftijd = 1,
                    MinimumloonPerDag = 1,
                    MinimumloonRecordActiefVanaf = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01),
                    MinimumloonRecordActiefTot = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 28),
                    MinimumloonRecordActief = true,
                });

                Context.SaveChanges();
            };

            public Given GetParamameters => () => { minimumLoon.Datum = DateTime.Now; };

            [Fact]
            public void ThenShouldGetMinimumloonSuccess()
            {
                Result.Value.Should().BeEquivalentTo(new List<MinimumloonModel>
                {
                    new()
                    {
                        Id = 1,
                        MinimumloonLeeftijd = 1,
                        MinimumloonPerDag = 1,
                        MinimumloonRecordActiefVanaf = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01),
                        MinimumloonRecordActiefTot = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 28),
                        MinimumloonRecordActief = true,
                    },
                });
            }

            [Fact]
            public void ThenShouldGetMinimumloonTrue()
            {
                Result.IsSuccess().Should().BeTrue();
            }
        }
    }
}
