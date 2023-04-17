using System.Collections.Generic;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Models.Person;
using Prechart.Service.Person.Models;
using PersonModel = Prechart.Service.Person.Models.PersonModel;

namespace Prechart.Service.Person.Repositories.Person;

public interface IPersonRepository :
    IHandlerAsync<PersonRepository.GetPersons, IFluentResults<List<PersonModel>>>,
    IHandlerAsync<PersonRepository.GetPersonById, IFluentResults<PersonModel>>,
    IHandlerAsync<PersonRepository.GetPersonByBsn, IFluentResults<PersonModel>>,
    IHandlerAsync<PersonRepository.GetPersonByName, IFluentResults<PersonModel>>,
    IHandlerAsync<PersonRepository.GetPersonByUserName, IFluentResults<PersonModel>>,
    IHandlerAsync<PersonRepository.GetPersonByCredential, IFluentResults<PersonModel>>,
    IHandlerAsync<PersonRepository.DeletePerson, IFluentResults<PersonModel>>,
    IHandlerAsync<PersonRepository.UpsertPersons, IFluentResults<PersonUpsertResults>>,
    IHandlerAsync<PersonRepository.UpsertPersonUser, IFluentResults<PersonUserModel>>,
    IHandlerAsync<PersonRepository.GetProfile, IFluentResults<PersonUserModel>>,
    IHandlerAsync<PersonRepository.DeactivatePersonUser, IFluentResults<PersonUserModel>>,
    IHandlerAsync<PersonRepository.WerkgeversPersons, IFluentResults<List<PersonModel>>>,
    IHandlerAsync<PersonRepository.GetPersonTaxCumulative, IFluentResults<PersonTaxCumulative>>,
    IHandlerAsync<PersonRepository.UpsertPersonUserCredentials, IFluentResults<bool>>,
    IHandlerAsync<PersonRepository.UpdatePersonPhoto, IFluentResults<bool>>,
    IHandlerAsync<PersonRepository.DownloadPersonPhoto, IFluentResults<PersonPhotoModel>>
{
}