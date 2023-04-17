using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Prechart.Service.Belastingen.Models.PremiePercentagesSocialeVerzekeringen;
using Prechart.Service.Belastingen.Repositories.PremiePercentagesSocialeVerzekeringen;
using Prechart.Service.Belastingen.Services.PremiePercentagesSocialeVerzekeringen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.TestBase;
using System;
using System.Threading;
using Xunit;

namespace Prechart.Service.Belastingen.Test.Repositories;

public class PremiePercentagesSocialeVerzekeringenServiceSpec : WithSubject<PremiePercentagesSocialeVerzekeringenService>
{
    public ILogger<PremiePercentagesSocialeVerzekeringenService> Logger;
    public IPremiePercentagesSocialeVerzekeringenRepository Repository { get; set; }

    public PremieBedragModel PremieData { get; set; }

    public Given TheRepository => () =>
    {
        Logger = An<ILogger<PremiePercentagesSocialeVerzekeringenService>>();

        PremieData = new PremieBedragModel
        {
            PremieBedragAlgemeneOuderdomsWet = 44.75m,
            PremieBedragNabestaanden = 0.25m,
            PremieBedragWetLangdurigeZorg = 24.125m,
            PremieBedragSocialeVerzekeringenPremieloon = 250.0m,
            PremieBedragAlgemeenWerkloosheidsFondsLaag = 6.75m,
            PremieBedragAlgemeenWerkloosheidsFondsHoog = 19.25m,
            PremieBedragUitvoeringsFondsvoordeOverheid = 1.70m,
            PremieBedragWetArbeidsOngeschikheidLaag = 13.725m,
            PremieBedragWetArbeidsOngeschikheidHoog = 17.625m,
            PremieBedragWetKinderopvang = 1.25m,
            PremieBedragZiektekostenVerzekeringsWetWerkgeversbijdrage = 16.875m,
            PremieBedragZiektekostenVerzekeringsWetWerknemersbijdrage = 13.75m,
            PremieBedragZiektekostenVerzekeringsWetLoon = 250m,
        };

        Repository = An<IPremiePercentagesSocialeVerzekeringenRepository>();

        Subject = new PremiePercentagesSocialeVerzekeringenService(Repository, Logger);
    };


    public class WhenGetPremiePercentage : PremiePercentagesSocialeVerzekeringenServiceSpec
    {
        public IFluentResults<PremieBedragModel> Result { get; private set; }

        public When GetPremie => async () => Result = await Subject.HandleAsync(new PremiePercentagesSocialeVerzekeringenService.GetPremiePercentage
        {
            LoonSocialVerzekeringen = 250m,
            LoonZiektekostenVerzekeringsWet = 250m,
            SocialeVerzekeringenDatum = new DateTime(2022, 6, 1),
        }, CancellationToken.None);

        public class AndFound : WhenGetPremiePercentage
        {
            public And Found => () => { Repository.HandleAsync(Arg.Any<PremiePercentagesSocialeVerzekeringenRepository.GetPremiePercentage>(), CancellationToken.None).Returns(ResultsTo.Something(PremieData)); };

            [Fact]
            public void ThenShouldReturnData()
            {
                Result.Value.Should().BeEquivalentTo(PremieData);
            }
        }

        public class AndNotFound : WhenGetPremiePercentage
        {
            public And Found => () => { Repository.HandleAsync(Arg.Any<PremiePercentagesSocialeVerzekeringenRepository.GetPremiePercentage>(), CancellationToken.None).Returns(ResultsTo.NotFound<PremieBedragModel>()); };

            [Fact]
            public void ThenShouldReturnData()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }
    }
}
