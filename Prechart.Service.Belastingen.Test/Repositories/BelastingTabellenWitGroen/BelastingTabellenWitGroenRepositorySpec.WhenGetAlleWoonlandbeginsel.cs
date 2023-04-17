using Prechart.Service.Belastingen.Database.Models;
using Prechart.Service.Belastingen.Repositories.BelastingTabellenWitGroen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.TestBase;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace Prechart.Service.Belastingen.Test.Repositories;

public partial class BelastingTabellenWitGroenRepositorySpec
{
    public class GetAlleWoonlandbeginsel : BelastingTabellenWitGroenRepositorySpec
    {
        public IFluentResults<List<Woonlandbeginsel>> Result { get; private set; }
        public When GetCountry => async () => Result = await Subject.HandleAsync(new BelastingTabellenWitGroenRepository.GetWoonlandbeginsel(), CancellationToken.None);

        public class AndFound : GetAlleWoonlandbeginsel
        {
            public And Found => () =>
            {
                Context.Woonlandbeginsel.Add(new Woonlandbeginsel
                {
                    Id = 1,
                    WoonlandbeginselCode = "NL",
                    WoonlandbeginselBenaming = "Netherlands",
                    WoonlandbeginselBelastingCode = 2,
                    Active = true,
                }
                );
                Context.SaveChanges();
            };

            [Fact]
            public void ThenShouldReturnSome()
            {
                Result.ToActionResult().Equals(
                        new List<Woonlandbeginsel>
                        {
                            new()
                            {
                                Id = 1,
                                WoonlandbeginselCode = "NL",
                                WoonlandbeginselBenaming = "Netherlands",
                                WoonlandbeginselBelastingCode = 2,
                                Active = true,
                            },
                        });
            }
        }

        public class AndNotFound : GetAlleWoonlandbeginsel
        {
            [Fact]
            public void ThenShouldReturnNone()
            {
                Result.ToActionResult().Equals(null);
            }
        }
    }
}
