using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Prechart.Service.Belastingen.Consumers;
using Prechart.Service.Belastingen.Database.Models;
using Prechart.Service.Belastingen.Services.BelastingTabellenWitGroen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Interfaces.Belastingen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Prechart.Service.Belastingen.Test.Consumer;

public class GetWoonlandbeginselConsumerSpec : WithSubject<GetWoonlandbeginselConsumer>
{
    public FakeLogger<GetWoonlandbeginselConsumer> Logger { get; private set; }
    public IBelastingTabellenWitGroenService Service { get; private set; }
    public IGetWoonlandbeginselEvent IGetWoonlandbeginselEventEntity { get; private set; }
    public ConsumeContext<IGetWoonlandbeginselEvent> IGetWoonlandbeginselEventContext { get; private set; }
    public IWoonlandbeginsel Result { get; private set; }

    public Given TheService => () =>
    {
        Logger = new FakeLogger<GetWoonlandbeginselConsumer>();
        Service = An<IBelastingTabellenWitGroenService>();
        IGetWoonlandbeginselEventEntity = An<IGetWoonlandbeginselEvent>();

        IGetWoonlandbeginselEventContext = An<ConsumeContext<IGetWoonlandbeginselEvent>>();
        IGetWoonlandbeginselEventContext.Message.Returns(IGetWoonlandbeginselEventEntity);

        Subject = new GetWoonlandbeginselConsumer(Logger, Service);
    };

    public class WhenConsuming : GetWoonlandbeginselConsumerSpec
    {
        public When Consuming => async () => await Subject.Consume(IGetWoonlandbeginselEventContext);

        public class AndUpsertedToTable : WhenConsuming
        {
            [Fact]
            public void ThenShouldReturnSome()
            {
                Service.HandleAsync(Verify.That<BelastingTabellenWitGroenService.GetAlleWoonlandbeginsel>(i => i.Should().BeEquivalentTo(IGetWoonlandbeginselEventEntity)), CancellationToken.None);
            }

            public class AndException : WhenConsuming
            {
                public And Exception => () => Service.HandleAsync(Arg.Any<BelastingTabellenWitGroenService.GetAlleWoonlandbeginsel>(), CancellationToken.None).Returns<IFluentResults<List<Woonlandbeginsel>>>(e => throw new Exception("Error"));

                [Fact]
                public void ThenLogger()
                {
                    Logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error).Should().BeTrue();
                }
            }

            public class AndNotFound : WhenConsuming
            {
                /*
                 public And GetData => () => _repository.HandleAsync(Arg.Any<KlantRepository.GetKlants>(), CancellationToken.None)
                .Returns<IFluentResults<List<KlantModel>>>(e => ResultsTo.NotFound<List<KlantModel>>());
                 */

                public And GetData => () => Service.HandleAsync(Arg.Any<BelastingTabellenWitGroenService.GetAlleWoonlandbeginsel>(), CancellationToken.None).Returns<IFluentResults<List<Woonlandbeginsel>>>(e => ResultsTo.NotFound<List<Woonlandbeginsel>>());
                ////public And GetData => () => Service.HandleAsync(Arg.Any<BelastingTabellenWitGroenService.GetAlleWoonlandbeginsel>(), CancellationToken.None).Returns(
                ////    d => new List<Woonlandbeginsel>
                ////    {
                ////        new(),
                ////    }.Some());

                [Fact]
                public void ThenShouldAndFoundSuccess()
                {
                    Subject.Should().BeAssignableTo<GetWoonlandbeginselConsumer>().Which.Should().Equals(new Woonlandbeginsel());
                }
            }

            public class AndFound : WhenConsuming
            {

                public And GetData => () => Service.HandleAsync(Arg.Any<BelastingTabellenWitGroenService.GetAlleWoonlandbeginsel>(), CancellationToken.None).Returns<IFluentResults<List<Woonlandbeginsel>>>(e => ResultsTo.Something(
                   new List<Woonlandbeginsel>
                   {
                        new Woonlandbeginsel
                        {
                            Id = 1,
                            WoonlandbeginselCode = "NL",
                            WoonlandbeginselBenaming = "Netherlands",
                            WoonlandbeginselBelastingCode = 2,
                            Active = true,
                        },
                   }
                    ));

                ////public And GetData => () => Service.HandleAsync(Arg.Any<BelastingTabellenWitGroenService.GetAlleWoonlandbeginsel>(), CancellationToken.None).Returns(d => new List<Woonlandbeginsel>
                ////{
                ////    new()
                ////    {
                ////        Id = 1,
                ////        WoonlandbeginselCode = "NL",
                ////        WoonlandbeginselBenaming = "Netherlands",
                ////        WoonlandbeginselBelastingCode = 2,
                ////        Active = true,
                ////    },
                ////}.Some());

                [Fact]
                public void ThenShouldAndFoundSuccess()
                {
                    Subject.Should().BeAssignableTo<GetWoonlandbeginselConsumer>().Which.Should().Equals(new List<IWoonlandbeginsel>
                    {
                        new Woonlandbeginsel
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
        }
    }
}
