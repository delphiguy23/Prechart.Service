using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Prechart.Service.Globals.Models.Person;
using Prechart.Service.Person.Models;
using PersonModel = Prechart.Service.Person.Models.PersonModel;

namespace Prechart.Service.Person.Repositories.Person;

public partial class PersonRepository
{
    public record GetPersons
    {
        public PersonType PersonType { get; set; }
    }

    public record GetPersonById
    {
        public string Id { get; set; }
    }

    public record GetPersonByBsn
    {
        public string Bsn { get; set; }
    }

    public record GetPersonByName
    {
        public string LastName { get; set; }
    }

    public record GetPersonByUserName
    {
        public string UserName { get; set; }
    }

    public record UpsertPersons
    {
        public List<PersonModel> Persons { get; set; }
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

    public record UpsertPersonUserCredentials
    {
        public string PersonId { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
    }

    public record UpsertPersonUser
    {
        public PersonUserModel PersonUser { get; set; }
    }

    public class DeactivatePersonUser
    {
        public ObjectId Id { get; set; }
    }

    public record DeletePerson
    {
        public string Id { get; set; }
    }

    public record WerkgeversPersons
    {
        public string TaxNo { get; set; }
    }

    public record GetPersonsByBsn
    {
        public List<string> Bsn { get; set; }
    }

    public record GetPersonTaxCumulative
    {
        public string Bsn { get; set; }
    }

    public record GetPersonByCredential
    {
        public string LastBsn { get; set; }
        public string Personnummer { get; set; }
        public DateTime Birthdate { get; set; }
    }

    public record GetProfile
    {
        public ObjectId Id { get; set; }
    }
}
