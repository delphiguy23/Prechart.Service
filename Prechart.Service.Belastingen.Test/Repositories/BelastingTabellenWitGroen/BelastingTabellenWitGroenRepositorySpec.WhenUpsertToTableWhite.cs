using FluentAssertions;
using Prechart.Service.Belastingen.Database.Models;
using Prechart.Service.Belastingen.Models;
using Prechart.Service.Belastingen.Repositories.BelastingTabellenWitGroen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Interfaces.Belastingen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Prechart.Service.Belastingen.Test.Repositories;

public partial class BelastingTabellenWitGroenRepositorySpec
{
    public class UpsertToTableWhite : BelastingTabellenWitGroenRepositorySpec
    {
        private List<ITaxRecord> taxRecords;
        public IFluentResults<int> Result { get; private set; }
        public When GetTax => async () => Result = await Subject.HandleAsync(new BelastingTabellenWitGroenRepository.UpsertToTable { TaxType = "wit", TaxTable = taxRecords }, CancellationToken.None);

        public class AndUpsertToTableSuccess : UpsertToTableWhite
        {
            public And Data => () =>
            {
                taxRecords = new List<ITaxRecord>
                {
                    new InhoudingModel {TypeId = 1, CountryId = 1, Year = 2022},
                };

                Context.White.Add(new White
                {
                    TypeId = 1,
                    CountryId = 1,
                    Year = 2022,
                });

                Context.SaveChanges();
            };


            [Fact]
            public void ThenShouldUpsertToTableSuccess()
            {
                Result.IsSuccess().Should().BeTrue();
            }


            [Fact]
            public void ThenRecordInsertedToWhite()
            {
                var result = Context.White.Any().Should().Be(true);
            }

            [Fact]
            public void ThenOldRecordIsDeletedToWhite()
            {
                var result = Context.White.Any(c => c.Id == 1).Should().BeFalse();
            }
        }

        public class AndException : UpsertToTableWhite
        {
            public And Exception => () => { Subject = new BelastingTabellenWitGroenRepository(Logger, Context, Mapper, i => { throw new Exception("Error"); }); };

            [Fact]
            public void ThenShouldReturnFailure()
            {
                Result.IsFailure().Should().BeTrue();
            }
        }
    }
}
