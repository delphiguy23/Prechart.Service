using Prechart.Service.Core.TestBase;
using Prechart.Service.Minimumloon.Repositories;
using Prechart.Service.Minimumloon.Services;

namespace Prechart.Service.Minimumloon.Test.Service;

public partial class MinimumloonServiceSpec : WithSubject<MinimumloonService>
{
    public FakeLogger<MinimumloonService> Logger { get; private set; }
    public IMinimumloonRepository Repository { get; private set; }

    public Given TheService => () =>
    {
        Logger = new FakeLogger<MinimumloonService>();
        Repository = An<IMinimumloonRepository>();

        Subject = new MinimumloonService(Logger, Repository);
    };
}
