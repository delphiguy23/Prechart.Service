using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Interfaces.Person;
using Prechart.Service.Globals.Models.Person;
using Prechart.Service.Person.Models;
using PersonModel = Prechart.Service.Person.Models.PersonModel;

namespace Prechart.Service.Person.Services.Person;

public interface IPersonService :
    IHandlerAsync<PersonService.GetPersonBy, IFluentResults<PersonModel>>,
    IHandlerAsync<PersonService.UpsertPersons, IFluentResults<IPersonUpsertResults>>,
    IHandlerAsync<PersonService.UpsertPersonUser, IFluentResults<PersonUserModel>>,
    IHandlerAsync<PersonService.DeactivatePersonUser, IFluentResults<PersonUserModel>>,
    IHandlerAsync<PersonService.DeletePerson, IFluentResults<PersonModel>>,
    IHandlerAsync<PersonService.GetPersons, IFluentResults<List<PersonModel>>>,
    IHandlerAsync<PersonService.WerkgeversPersons, IFluentResults<List<PersonModel>>>,
    IHandlerAsync<PersonService.GetProfile, IFluentResults<PersonUserModel>>,
    IHandlerAsync<PersonService.GetPersonTaxCumulative, IFluentResults<PersonTaxCumulative>>,
    IHandlerAsync<PersonService.PersonCredentialCheck, IFluentResults<string>>,
    IHandlerAsync<PersonService.UpsertPersonUserCredentials, IFluentResults<bool>>,
    IHandlerAsync<PersonService.ResetPersonUserCredentials, IFluentResults<bool>>,
    IHandlerAsync<PersonService.UpdatePersonPhoto, IFluentResults<bool>>,
    IHandlerAsync<PersonService.DownloadPersonPhoto, IFluentResults<PersonPhotoModel>>,
    IHandlerAsync<PersonService.DownloadPersonPhotoViaController, IFluentResults<FileContentResult>>,
    IHandlerAsync<PersonService.CreateTestUser, IFluentResults<PersonModel>>

{
}
