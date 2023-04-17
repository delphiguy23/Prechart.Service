using FluentAssertions;
using Microsoft.Extensions.Logging;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Minimumloon.Models;
using Prechart.Service.Minimumloon.Repositories;
using Xunit;

namespace Prechart.Service.Minimumloon.Test.Repositories;

public partial class MinimumloonRepositorySpec
{
    public class UpsertMinimumLoon : MinimumloonRepositorySpec
    {
        private List<MinimumloonModel> minimumloon;
        public IFluentResults Result { get; private set; }


        public When GetMinimumLoon => async () => Result = await Subject.HandleAsync(new MinimumloonRepository.UpsertMinimumLoon { Minimumloon = minimumloon }, CancellationToken.None);

        public class AndUpsertToTableMinimumLoonNull : UpsertMinimumLoon
        {
            [Fact]
            public void ThenShouldUpsertToTableMinimumLoonNull()
            {
                Result.IsNotFoundOrBadRequest().Should().BeTrue();
            }
        }

        public class AndException : UpsertMinimumLoon
        {
            public And Exception => () => { Subject = new MinimumloonRepository(Logger, Context, Mapper, i => { throw new Exception("Error"); }); };

            [Fact]
            public void ThenShouldReturnFailure()
            {
                Result.IsFailure().Should().BeTrue();
            }


            [Fact]
            public void ThenLoggerException()
            {
                Logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message.StartsWith("Error")).Should().BeTrue();
            }
        }

        public class AndUpsertToTableNothingChanged : UpsertMinimumLoon
        {
            public And Data => () =>
            {
                minimumloon = new List<MinimumloonModel>
                {
                    new()
                    {
                        Id = 0,
                        MinimumloonLeeftijd = 21,
                        MinimumloonPerDag = 1,
                        Jaar = DateTime.Now.Year,
                        MinimumloonRecordActiefTot = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01),
                        MinimumloonRecordActiefVanaf = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 28),
                        MinimumloonRecordActief = true,
                    },
                };

                Context.Minimumloon.Add(new Database.Models.Minimumloon
                {
                    Id = 0,
                    MinimumloonLeeftijd = 21,
                    MinimumloonPerDag = 1,
                    Jaar = DateTime.Now.Year,
                    MinimumloonRecordActiefTot = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01),
                    MinimumloonRecordActiefVanaf = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 28),
                    MinimumloonRecordActief = true,
                });

                Context.SaveChanges();
            };

            [Fact]
            public void ThenShouldUpsertToTableNothingChanged()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }

        public class AndUpsertToTableSuccessMinimumloonLeeftijdUpdateMinimumloonPerDag : UpsertMinimumLoon
        {
            public And Data => () =>
            {
                minimumloon = new List<MinimumloonModel>
                {
                    new()
                    {
                        Id = 0,
                        MinimumloonLeeftijd = 21,
                        MinimumloonPerDag = 33,
                        Jaar = DateTime.Now.Year,
                        MinimumloonRecordActiefTot = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01),
                        MinimumloonRecordActiefVanaf = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 28),
                        MinimumloonRecordActief = true,
                    },
                };

                Context.Minimumloon.Add(new Database.Models.Minimumloon
                {
                    Id = 0,
                    MinimumloonLeeftijd = 21,
                    MinimumloonPerDag = 1,
                    Jaar = DateTime.Now.Year,
                    MinimumloonRecordActiefTot = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01),
                    MinimumloonRecordActiefVanaf = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 28),
                    MinimumloonRecordActief = true,
                });

                Context.SaveChanges();
            };

            [Fact]
            public void ThenShouldUpsertToTableSuccessMinimumloonLeeftijd21()
            {
                Result.IsSuccess().Should().BeTrue();
            }
        }

        public class AndUpsertToTableSuccess : UpsertMinimumLoon
        {
            public And Data => () =>
            {
                minimumloon = new List<MinimumloonModel>
                {
                    new()
                    {
                        Id = 0,
                        MinimumloonLeeftijd = 21,
                        MinimumloonPerDag = 1,
                        Jaar = DateTime.Now.Year,
                        MinimumloonRecordActiefTot = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01),
                        MinimumloonRecordActiefVanaf = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 28),
                        MinimumloonRecordActief = true,
                    },
                };
            };

            [Fact]
            public void ThenShouldUpsertToTableSucces()
            {
                Result.IsSuccess().Should().BeTrue();
            }
        }
    }
}
