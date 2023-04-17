using MongoDB.Bson;
using Prechart.Service.Globals.Interfaces.Loonheffings;
using Prechart.Service.Globals.Models.Person;
using Prechart.Service.Person.Models;

namespace Prechart.Service.Person.Services.Person;

public partial class PersonService
{
    public class UpsertPersonUser
    {
        public PersonUserModel PersonUser { get; set; }
    }

    public class UpdatePersonPhoto
    {
        public ObjectId Id { get; set; }
        public PersonPhotoModel PersonPhoto { get; set; }
    }

    public class DownloadPersonPhoto
    {
        public ObjectId Id { get; set; }
    }

    public class DownloadPersonPhotoViaController
    {
        public ObjectId Id { get; set; }
    }

    public class DeactivatePersonUser
    {
        public ObjectId Id { get; set; }
    }

    public record UpsertPersons
    {
        public IXmlToPersons Persons { get; set; }
    }

    public record GetPersonBy
    {
        public string Id { get; set; }
        public string Bsn { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
    }

    public record PersonCredentialCheck
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public record DeletePerson
    {
        public string Id { get; set; }
    }

    public record GetPersons
    {
        public PersonType PersonType { get; set; }
    }

    public record WerkgeversPersons
    {
        public string TaxNo { get; set; }
        public bool FullHistory { get; set; }
    }

    public record GetPersonTaxCumulative
    {
        public string Bsn { get; set; }
    }

    public record UpsertPersonUserCredentials
    {
        public string PersonId { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
    }

    public record ResetPersonUserCredentials
    {
        public string UserName { get; set; }
    }

    public record GetProfile
    {
        public ObjectId Id { get; set; }
    }

    public record CreateTestUser;
}
