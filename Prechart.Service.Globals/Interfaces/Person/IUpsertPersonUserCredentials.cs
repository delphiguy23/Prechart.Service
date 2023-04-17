namespace Prechart.Service.Globals.Interfaces.Person
{
    public interface IUpsertPersonUserCredentials
    {
        public string PersonId {get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
    }
}
