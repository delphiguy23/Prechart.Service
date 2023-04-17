namespace Prechart.Service.Core.TestBase
{
    public delegate void Given();
    public delegate void Teardown();

#pragma warning disable CA1716 // Identifiers should not match keywords
    public delegate void And();
    public delegate void When();
#pragma warning restore CA1716 // Identifiers should not match keywords
}
