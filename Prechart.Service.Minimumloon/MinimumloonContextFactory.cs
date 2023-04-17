using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Prechart.Service.Minimumloon.Database.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prechart.Service.Minimumloon;

public class MinimumloonContextFactory : IDesignTimeDbContextFactory<MinimumloonDBContext>
{
    public Prechart.Service.Minimumloon.Database.Context.MinimumloonDBContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MinimumloonDBContext>();
        optionsBuilder.UseSqlServer("Server=Tiamzon-pc;Database=Svc-Prechart;user id=sa;password=Calender365;");

        return new MinimumloonDBContext(optionsBuilder.Options);
    }
}
