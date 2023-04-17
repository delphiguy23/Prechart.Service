using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Prechart.Service.Belastingen.Consumers;
using Prechart.Service.Belastingen.Database.Models;
using Prechart.Service.Belastingen.Models;
using Prechart.Service.Belastingen.Services.BelastingTabellenWitGroen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.Service;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Interfaces.Belastingen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Prechart.Service.Belastingen.Test.Consumer;

public class InsertToTaxTableConsumerSpec : WithSubject<InsertToTaxTableConsumer>
{
    public FakeLogger<InsertToTaxTableConsumer> Logger { get; private set; }
    public IBelastingTabellenWitGroenService Service { get; private set; }
    public IInsertToTaxTableEvent InsertToTaxTableEventEntity { get; private set; }
    public ConsumeContext<IInsertToTaxTableEvent> InsertToTaxTableEventContext { get; private set; }
    public IResult<BerekenInhoudingModel> Result { get; private set; }

    public Given TheService => () =>
    {
        Logger = new FakeLogger<InsertToTaxTableConsumer>();
        Service = An<IBelastingTabellenWitGroenService>();
        InsertToTaxTableEventEntity = An<IInsertToTaxTableEvent>();
        InsertToTaxTableEventEntity.TaxType.Returns("wit");
        InsertToTaxTableEventEntity.TaxTable.Returns(new List<ITaxRecord>
        {
            new InhoudingModel
            {
                TypeId = 1,
                CountryId = 1,
            },
            new InhoudingModel
            {
                TypeId = 1,
                CountryId = 1,
            },
            new InhoudingModel
            {
                TypeId = 1,
                CountryId = 1,
            },
        });
        InsertToTaxTableEventContext = An<ConsumeContext<IInsertToTaxTableEvent>>();
        InsertToTaxTableEventContext.Message.Returns(InsertToTaxTableEventEntity);

        Subject = new InsertToTaxTableConsumer(Logger, Service);
    };

    public class WhenConsuming : InsertToTaxTableConsumerSpec
    {
        public When Consuming => async () => await Subject.Consume(InsertToTaxTableEventContext);

        public class AndUpsertedToTable : WhenConsuming
        {
            [Fact]
            public void ThenShouldRecieveSome()
            {
                Service.Received(1).HandleAsync(Verify.That<BelastingTabellenWitGroenService.UpsertToTable>(i => i.Should()
                    .BeEquivalentTo(InsertToTaxTableEventEntity)), CancellationToken.None);
            }

            public class AndException : WhenConsuming
            {
                public And Exception => () => Service.HandleAsync(Arg.Any<BelastingTabellenWitGroenService.UpsertToTable>(), CancellationToken.None).Returns<IFluentResults<bool>>(e => throw new Exception("Error"));

                [Fact]
                public void ThenLogger()
                {
                    Logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error).Should().BeTrue();
                }
            }

            public class AndFoundNull : WhenConsuming
            {
                public And GetData => () => Service.HandleAsync(Arg.Any<BelastingTabellenWitGroenService.UpsertToTable>(), CancellationToken.None).Returns<IFluentResults<bool>>(e => ResultsTo.Success(false));

                [Fact]
                public void ThenLogger()
                {
                    Logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information).Should().BeTrue();
                }
            }

            public class AndFoundFalse : WhenConsuming
            {
                private readonly IInsertToTaxTable tax = new InsertToTaxTable
                {
                    IsProcessed = false,
                };

                public And GetData => () => Service.HandleAsync(Arg.Any<BelastingTabellenWitGroenService.UpsertToTable>(), CancellationToken.None).Returns<IFluentResults<bool>>(e => ResultsTo.Success(false));


                [Fact]
                public void ThenShouldAndFoundSuccess()
                {
                    Subject.Should().BeAssignableTo<InsertToTaxTableConsumer>().Which.Should().Equals(tax);
                }
            }

            public class AndFoundTrue : WhenConsuming
            {
                private readonly IInsertToTaxTable tax = new InsertToTaxTable
                {
                    IsProcessed = true,
                };

                public And GetData => () => Service.HandleAsync(Arg.Any<BelastingTabellenWitGroenService.UpsertToTable>(), CancellationToken.None).Returns<IFluentResults<bool>>(e => ResultsTo.Success(false));

                [Fact]
                public void ThenShouldAndFoundSuccess()
                {
                    Subject.Should().BeAssignableTo<InsertToTaxTableConsumer>().Which.Should().Equals(tax);
                }
            }
        }
    }
}
