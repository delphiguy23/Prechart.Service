using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Minimumloon.Database.Context;
using Prechart.Service.Minimumloon.Models.Mapping;
using Prechart.Service.Minimumloon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prechart.Service.Minimumloon.Test.Repositories;

public partial class MinimumloonRepositorySpec : WithSubject<MinimumloonRepository>
{
    public MinimumloonDBContext Context { get; private set; }
    public FakeLogger<MinimumloonRepository> Logger;
    private IMapper Mapper;

    public Given TheRepository => () =>
    {
        var dbContext = new DbContextOptionsBuilder<MinimumloonDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Logger = An<FakeLogger<MinimumloonRepository>>();

        Context = new MinimumloonDBContext(dbContext);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MinimumloonMapper());
        });

        Mapper = mockMapper.CreateMapper();

        Subject = new MinimumloonRepository(Logger, Context, Mapper, (i) => { Console.Write(i); });
    };
}
