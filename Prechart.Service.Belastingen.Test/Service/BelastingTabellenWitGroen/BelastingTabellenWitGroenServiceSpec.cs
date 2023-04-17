using Prechart.Service.Belastingen.Database.Models;
using Prechart.Service.Belastingen.Repositories.BelastingTabellenWitGroen;
using Prechart.Service.Belastingen.Services.BelastingTabellenWitGroen;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Helper;
using System.Collections.Generic;

namespace Prechart.Service.Belastingen.Test.Service;

public partial class BelastingTabellenWitGroenServiceSpec : WithSubject<BelastingTabellenWitGroenService>
{
    public FakeLogger<BelastingTabellenWitGroenService> Logger { get; private set; }
    public IBelastingTabellenWitGroenRepository Repository { get; private set; }
    public IBatchHelper BatchHelper { get; private set; }

    public List<Woonlandbeginsel> Woonlandbeginsel { get; set; }
    public Given TheService => () =>
    {
        Woonlandbeginsel = new List<Woonlandbeginsel>
                      {
                          new Woonlandbeginsel
                          {
                              Active = true,
                              WoonlandbeginselBelastingCode = 2,
                              WoonlandbeginselBenaming = "België",
                              WoonlandbeginselCode = "BE",
                          },
                          new Woonlandbeginsel
                          {
                              Active = true,
                              WoonlandbeginselBelastingCode = 2,
                              WoonlandbeginselBenaming = "Nederland",
                              WoonlandbeginselCode = "NL",
                          },
                          new Woonlandbeginsel
                          {
                              Active = true,
                              WoonlandbeginselBelastingCode = 2,
                              WoonlandbeginselBenaming = "Landenkring",
                              WoonlandbeginselCode = "LK",
                          },
                          new Woonlandbeginsel
                          {
                              Active = true,
                              WoonlandbeginselBelastingCode = 2,
                              WoonlandbeginselBenaming = "Derde Landen",
                              WoonlandbeginselCode = "DL",
                          },
                          new Woonlandbeginsel
                          {
                              Active = true,
                              WoonlandbeginselBelastingCode = 2,
                              WoonlandbeginselBenaming = "Suriname/Aruba",
                              WoonlandbeginselCode = "SA",
                          },
                      };

        Logger = new FakeLogger<BelastingTabellenWitGroenService>();
        Repository = An<IBelastingTabellenWitGroenRepository>();
        BatchHelper = An<IBatchHelper>();


        Subject = new BelastingTabellenWitGroenService(Logger, Repository, BatchHelper);
    };
}
