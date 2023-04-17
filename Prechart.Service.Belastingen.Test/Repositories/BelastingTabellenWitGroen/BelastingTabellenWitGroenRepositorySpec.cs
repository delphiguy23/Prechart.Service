using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prechart.Service.Belastingen.Database.Context;
using Prechart.Service.Belastingen.Models.Mapping;
using Prechart.Service.Belastingen.Repositories.BelastingTabellenWitGroen;
using Prechart.Service.Core.TestBase;
using System;

namespace Prechart.Service.Belastingen.Test.Repositories;

public partial class BelastingTabellenWitGroenRepositorySpec : WithSubject<BelastingTabellenWitGroenRepository>
{
    public BelastingenDbContext Context { get; private set; }
    public ILogger<BelastingTabellenWitGroenRepository> Logger;
    private IMapper Mapper;

    public Given TheRepository => () =>
    {
        var dbContext = new DbContextOptionsBuilder<BelastingenDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Logger = An<ILogger<BelastingTabellenWitGroenRepository>>();

        Context = new BelastingenDbContext(dbContext);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new BelastingenMapper());
        });

        Mapper = mockMapper.CreateMapper();

        Subject = new BelastingTabellenWitGroenRepository(Logger, Context, Mapper, (i) => { Console.Write(i); });
    };

}
