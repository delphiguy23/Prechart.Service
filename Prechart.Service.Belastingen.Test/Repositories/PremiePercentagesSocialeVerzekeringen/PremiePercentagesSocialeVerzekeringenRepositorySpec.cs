using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prechart.Service.Belastingen.Database.Context;
using Prechart.Service.Belastingen.Database.Models;
using Prechart.Service.Belastingen.Models.PremiePercentagesSocialeVerzekeringen;
using Prechart.Service.Belastingen.Repositories.PremiePercentagesSocialeVerzekeringen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.TestBase;
using System;
using System.Threading;
using Xunit;

namespace Prechart.Service.Belastingen.Test.Repositories;

public class PremiePercentagesSocialeVerzekeringenRepositorySpec : WithSubject<PremiePercentagesSocialeVerzekeringenRepository>
{
    public ILogger<PremiePercentagesSocialeVerzekeringenRepository> Logger;
    public BelastingenDbContext Context { get; private set; }
    public PremiePercentagesSocialeVerzekeringen PremieData { get; set; }

    public Given TheRepository => () =>
    {
        var dbContext = new DbContextOptionsBuilder<BelastingenDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        Logger = An<ILogger<PremiePercentagesSocialeVerzekeringenRepository>>();

        Context = new BelastingenDbContext(dbContext);

        PremieData = new PremiePercentagesSocialeVerzekeringen
        {
            Id = 1,
            Jaar = 2022,
            PremiePercentageAlgemeneOuderdomsWet = 17.90m,
            PremiePercentageNabestaanden = 0.10m,
            PremiePercentageWetLangdurigeZorg = 9.65m,
            MaximaalSociaalverzekeringPremieloon = 59706.00m,
            PremiePercentageAlgemeenWerkloosheidsFondsLaag = 2.7m,
            PremiePercentageAlgemeenWerkloosheidsFondsHoog = 7.7m,
            PremiePercentageUitvoeringsdfondsvoordeOverheid = 0.68m,
            PremiePercentageWetArbeidsongeschikheidLaag = 5.49m,
            PremiePercentageWetArbeidsongeschikheidHoog = 7.05m,
            PremiePercentageWetKinderopvang = 0.5m,
            PremiePercentageZiektekostenverzekeringWerkgeversbijdrage = 6.75m,
            PremiePercentageZiektekostenverzekeringWerknemersbijdrage = 5.50m,
            MaximaalZiektekostenverzekeringLoon = 59706.00m,
            SocialeVerzekeringenRecordActief = true,
            SocialeVerzekeringenRecordActiefVanaf = new DateTime(2022, 1, 1),
            SocialeVerzekeringenRecordActiefTot = new DateTime(2022, 12, 31),
        };

        Subject = new PremiePercentagesSocialeVerzekeringenRepository(Logger, Context);
    };

    public class WhenGetPremiePercentage : PremiePercentagesSocialeVerzekeringenRepositorySpec
    {
        public IFluentResults<PremieBedragModel> Result { get; private set; }

        public When GetPremie => async () => Result = await Subject.HandleAsync(new PremiePercentagesSocialeVerzekeringenRepository.GetPremiePercentage
        {
            LoonSocialVerzekeringen = 250m,
            LoonZiektekostenVerzekeringsWet = 250m,
            SocialeVerzekeringenDatum = new DateTime(2022, 6, 1),
        }, CancellationToken.None);

        public class AndFound : WhenGetPremiePercentage
        {
            public And Found => () =>
            {
                Context.PremiePercentagesSocialeVerzekeringen.Add(PremieData);
                Context.SaveChanges();
            };

            [Fact]
            public void ThenShouldReturnData()
            {
                Result.Value.Equals
                    (
                    new PremieBedragModel
                    {
                        PremieBedragAlgemeneOuderdomsWet = 44.75m,
                        PremieBedragNabestaanden = 0.25m,
                        PremieBedragWetLangdurigeZorg = 24.13m,
                        PremieBedragSocialeVerzekeringenPremieloon = 250.0m,
                        PremieBedragAlgemeenWerkloosheidsFondsLaag = 6.75m,
                        PremieBedragAlgemeenWerkloosheidsFondsHoog = 19.25m,
                        PremieBedragUitvoeringsFondsvoordeOverheid = 1.70m,
                        PremieBedragWetArbeidsOngeschikheidLaag = 13.73m,
                        PremieBedragWetArbeidsOngeschikheidHoog = 17.63m,
                        PremieBedragWetKinderopvang = 1.25m,
                        PremieBedragZiektekostenVerzekeringsWetWerkgeversbijdrage = 16.88m,
                        PremieBedragZiektekostenVerzekeringsWetWerknemersbijdrage = 13.75m,
                        PremieBedragZiektekostenVerzekeringsWetLoon = 250m,
                    });
            }

            [Fact]
            public void ThenShouldReturnTrue()
            {
                Result.IsSuccess().Should().BeTrue();
            }
        }

        public class AndNotFound : WhenGetPremiePercentage
        {
            [Fact]
            public void ThenShouldReturnData()
            {
                Result.IsNotFound().Should().BeTrue();
            }
        }
    }
}
