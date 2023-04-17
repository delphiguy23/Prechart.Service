using FluentAssertions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Minimumloon.Repositories;
using Xunit;

namespace Prechart.Service.Minimumloon.Test.Repositories;

public partial class MinimumloonRepositorySpec
{
    public class DeleteMinimumloon : MinimumloonRepositorySpec
    {
        private DateTime getDatum;
        private int getLeeftijd;
        public IFluentResults Result { get; private set; }

        public When GetMinimumLoon => async () => Result = await Subject.HandleAsync(new MinimumloonRepository.DeleteMinimumloon { Datum = getDatum, Leeftijd = getLeeftijd }, CancellationToken.None);

        public class AndDeleteMinimumloonNull : DeleteMinimumloon
        {
            [Fact]
            public void ThenShouldDeleteMinimumloonNull()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }

        public class AndDeleteMinimumloonSuccess : DeleteMinimumloon
        {
            public Given GetParamameters => () =>
            {
                getDatum = DateTime.Now;
                getLeeftijd = 21;
            };

            public And Data => () =>
            {
                Context.Minimumloon.Add(new Database.Models.Minimumloon
                {
                    Id = 1,
                    Jaar = DateTime.Now.Year,
                    MinimumloonLeeftijd = 21,
                    MinimumloonPerDag = 1,
                    MinimumloonRecordActiefVanaf = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01),
                    MinimumloonRecordActiefTot = DateTime.Now,
                    MinimumloonRecordActief = true,
                });

                Context.SaveChanges();
            };

            [Fact]
            public void ThenShouldDeleteMinimumloonSuccess()
            {
                Result.IsSuccess().Should().BeTrue();
            }
        }
    }
}
