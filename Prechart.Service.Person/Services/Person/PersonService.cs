using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Globals.Interfaces.Loonheffings;
using Prechart.Service.Globals.Interfaces.Person;
using Prechart.Service.Globals.Models;
using Prechart.Service.Globals.Models.Loonheffings;
using Prechart.Service.Globals.Models.Person;
using Prechart.Service.Person.Models;
using Prechart.Service.Person.Repositories.Person;
using static Prechart.Service.Person.Repositories.Person.PersonRepository;
using PersonModel = Prechart.Service.Person.Models.PersonModel;

namespace Prechart.Service.Person.Services.Person;

public partial class PersonService : IPersonService
{
    private readonly ILogger<PersonService> _logger;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IPersonRepository _repository;

    public PersonService(ILogger<PersonService> logger, IPersonRepository repository, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<IFluentResults<IPersonUpsertResults>> HandleAsync(UpsertPersons request, CancellationToken cancellationToken = default)
    {
        var persons = _mapper.Map<List<PersonModel>>(request.Persons.Persons);

        var result = await _repository.HandleAsync(new PersonRepository.UpsertPersons {Persons = persons});

        if (result.Status == FluentResultsStatus.NotFound)
        {
            return ResultsTo.NotFound<IPersonUpsertResults>().FromResults(result);
        }

        var filenames = result.Value.Results.Select(r => r.TaxFileName).Distinct().ToList();

        if (filenames.Any())
        {
            var forProcess = filenames.Select(p =>
                new ProcessXmlResult
                {
                    FileName = p,
                    Processed = true,
                    ProcessErrors = string.Empty,
                    EmployeesInserted = result.Value.Results.Where(r => r.Operation == DataOperation.Insert && r.TaxFileName == p).DistinctBy(r => r.Bsn).Count(),
                    EmployeesUpdated = result.Value.Results.Where(r => r.Operation == DataOperation.Update && r.TaxFileName == p).DistinctBy(r => r.Bsn).Count(),
                }
            ).ToList();

            foreach (var p in forProcess)
            {
                await _publishEndpoint.Publish<ILoonaangifteProcessResult>(p);
            }
        }

        return ResultsTo.Something<IPersonUpsertResults>(result.Value);
    }


    public async Task<IFluentResults<PersonUserModel>> HandleAsync(UpsertPersonUser request, CancellationToken cancellationToken = default)
    {
        return ResultsTo.Something(await _repository.HandleAsync(new PersonRepository.UpsertPersonUser {PersonUser = request.PersonUser}));
    }

    public async Task<IFluentResults<bool>> HandleAsync(UpdatePersonPhoto request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (request.PersonPhoto.FileData is null)
            {
                return ResultsTo.BadRequest<bool>().WithMessage("Upload a photo");
            }

            var result = await _repository.HandleAsync(new PersonRepository.UpdatePersonPhoto {Id = request.Id, PersonPhoto = request.PersonPhoto});

            return ResultsTo.Something(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults<PersonPhotoModel>> HandleAsync(DownloadPersonPhoto request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.HandleAsync(new PersonRepository.DownloadPersonPhoto {Id = request.Id}, cancellationToken);

            if (result.IsNotFoundOrBadRequest() || result.IsFailure() || result.Value.FileData is null)
            {
                return ResultsTo.Failure<PersonPhotoModel>();
            }

            return ResultsTo.Something(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<PersonPhotoModel>().FromException(e);
        }
    }

    public async Task<IFluentResults<FileContentResult>> HandleAsync(DownloadPersonPhotoViaController request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await HandleAsync(new DownloadPersonPhoto {Id = request.Id});

            if (result.IsNotFoundOrBadRequest() || result.IsFailure() || result.Value.FileData is null)
            {
                return ResultsTo.Failure<FileContentResult>().WithMessage("Person  or photo does not exist");
            }

            return ResultsTo.Success(new FileContentResult(result.Value.FileData, $"{result.Value.MediaType}/{result.Value.MediaSubtype}"));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<FileContentResult>().FromException(e);
        }
    }

    public async Task<IFluentResults<PersonUserModel>> HandleAsync(DeactivatePersonUser request, CancellationToken cancellationToken = default)
    {
        return ResultsTo.Something(await _repository.HandleAsync(new PersonRepository.DeactivatePersonUser {Id = request.Id}));
    }

    public async Task<IFluentResults<PersonUserModel>> HandleAsync(GetProfile request, CancellationToken cancellationToken = default)
    {
        return ResultsTo.Something(await _repository.HandleAsync(new PersonRepository.GetProfile {Id = request.Id}));
    }

    public async Task<IFluentResults<List<PersonModel>>> HandleAsync(GetPersons request, CancellationToken cancellationToken = default)
    {
        var persons = await _repository.HandleAsync(new PersonRepository.GetPersons {PersonType = request.PersonType});

        if (persons.Status == FluentResultsStatus.NotFound)
        {
            return ResultsTo.NotFound<List<PersonModel>>().FromResults(persons);
        }

        foreach (var person in persons.Value)
        {
            var details = GetLatestProcessedForEachPeriode(person.TaxPaymentDetails);
            person.TaxPaymentDetails = details;
        }

        return ResultsTo.Success<List<PersonModel>>().FromResults(persons);
    }

    public async Task<IFluentResults<PersonModel>> HandleAsync(GetPersonBy request, CancellationToken cancellationToken = default)
    {
        IFluentResults<PersonModel> person = null;

        if (!string.IsNullOrWhiteSpace(request.Id))
        {
            person = await _repository.HandleAsync(new GetPersonById
            {
                Id = request.Id,
            });
        }

        if (!string.IsNullOrWhiteSpace(request.Bsn))
        {
            person = await _repository.HandleAsync(new GetPersonByBsn
            {
                Bsn = request.Bsn,
            });
        }

        if (!string.IsNullOrWhiteSpace(request.LastName))
        {
            person = await _repository.HandleAsync(new GetPersonByName
            {
                LastName = request.LastName,
            });
        }

        if (!string.IsNullOrWhiteSpace(request.UserName))
        {
            person = await _repository.HandleAsync(new GetPersonByUserName
            {
                UserName = request.UserName,
            });
        }

        if (!person.IsSuccess() || person.IsNotFoundOrBadRequest())
        {
            return ResultsTo.Something(person).WithMessage("Person not found");
        }

        person.Value.TaxPaymentDetails = GetLatestProcessedForEachPeriode(person.Value.TaxPaymentDetails);

        return ResultsTo.Success<PersonModel>().FromResults(person);
    }

    public async Task<IFluentResults<string>> HandleAsync(PersonCredentialCheck request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return ResultsTo.BadRequest<string>().WithMessage("Invalid arguments for UserName and Credential.");
            }

            var usernameLength = request.Username.Length;
            var lastBsnIndex = usernameLength - 4;

            if (lastBsnIndex < 0)
            {
                return ResultsTo.BadRequest<string>().WithMessage("Invalid arguments for UserName.");
            }

            var personnummer = request.Username.Substring(0, lastBsnIndex);
            var lastBsn = request.Username.Substring(lastBsnIndex);

            if (!DateTime.TryParseExact(request.Password, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                return ResultsTo.BadRequest<string>().WithMessage("Invalid arguments for UserName and Credential.");
            }

            var person = await _repository.HandleAsync(new GetPersonByCredential
            {
                LastBsn = lastBsn,
                Personnummer = personnummer,
                Birthdate = parsedDate,
            });

            if (person.Status == FluentResultsStatus.NotFound)
            {
                return ResultsTo.NotFound<string>().FromResults(person);
            }

            if (!string.IsNullOrWhiteSpace(person.Value.UserName) && !string.IsNullOrWhiteSpace(person.Value.EmailAddress))
            {
                return ResultsTo.NotFound<string>().FromResults(person);
            }

            return ResultsTo.Success(person.Value.Id.ToString());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<string>().FromException(e);
        }
    }

    public async Task<IFluentResults<PersonModel>> HandleAsync(DeletePerson request, CancellationToken cancellationToken = default)
    {
        return ResultsTo.Something(await _repository.HandleAsync(
            new PersonRepository.DeletePerson
            {
                Id = request.Id,
            }));
    }

    public async Task<IFluentResults<List<PersonModel>>> HandleAsync(WerkgeversPersons request, CancellationToken cancellationToken = default)
    {
        var isFullHistory = request?.FullHistory ?? false;

        if (isFullHistory)
        {
            return ResultsTo.Something(await _repository.HandleAsync(new PersonRepository.WerkgeversPersons {TaxNo = request.TaxNo}));
        }

        var activeHistory = await _repository.HandleAsync(new PersonRepository.WerkgeversPersons {TaxNo = request.TaxNo});

        if (activeHistory is null)
        {
            return ResultsTo.NotFound<List<PersonModel>>().FromResults(activeHistory);
        }

        foreach (var history in activeHistory.Value)
        {
            history.TaxPaymentDetails = GetLatestProcessedForEachPeriode(history.TaxPaymentDetails);
        }

        return ResultsTo.Something(activeHistory.Value.OrderBy(x => x.SignificantAchternaam).ToList());
    }

    public async Task<IFluentResults<PersonTaxCumulative>> HandleAsync(GetPersonTaxCumulative request, CancellationToken cancellationToken = default)
    {
        return ResultsTo.Something(await _repository.HandleAsync(new PersonRepository.GetPersonTaxCumulative {Bsn = request.Bsn}));
    }

    public async Task<IFluentResults<bool>> HandleAsync(UpsertPersonUserCredentials request, CancellationToken cancellationToken = default)
    {
        var person = await _repository.HandleAsync(new GetPersonById {Id = request.PersonId});

        if (person.Status == FluentResultsStatus.NotFound || person.Status == FluentResultsStatus.Failure)
        {
            return ResultsTo.Something(false);
        }

        return ResultsTo.Something(await _repository.HandleAsync(new PersonRepository.UpsertPersonUserCredentials
        {
            PersonId = request.PersonId,
            UserName = request.UserName,
            EmailAddress = request.EmailAddress,
        }));
    }

    public async Task<IFluentResults<bool>> HandleAsync(ResetPersonUserCredentials request, CancellationToken cancellationToken = default)
    {
        var person = await _repository.HandleAsync(new GetPersonByUserName {UserName = request.UserName});

        if (person.Status == FluentResultsStatus.NotFound || person.Status == FluentResultsStatus.Failure)
        {
            return ResultsTo.Failure<bool>().WithMessage("Person does not exist").FromResults(person);
        }

        return ResultsTo.Something(await _repository.HandleAsync(new PersonRepository.UpsertPersonUserCredentials
        {
            PersonId = person.Value.Id.ToString(),
            UserName = string.Empty,
            EmailAddress = string.Empty,
        }));
    }

    public async Task<IFluentResults<PersonModel>> HandleAsync(CreateTestUser request, CancellationToken cancellationToken = default)
    {
        if (await _repository.HandleAsync(new GetPersonByBsn {Bsn = "999999999"}) is { } _results && _results.IsSuccess() && _results.Value is not null)
        {
            return ResultsTo.Something(_results);
        }

        if (await HandleAsync(new GetPersons {PersonType = PersonType.Employee}) is { } results && results.IsSuccess())
        {
            var testPersonUser = GenerateTestUser(results.Value.FirstOrDefault());

            if (testPersonUser is not null &&
                await _repository.HandleAsync(new PersonRepository.UpsertPersons {Persons = new List<PersonModel> {testPersonUser}}) is { } result &&
                result.IsSuccess())
            {
                return ResultsTo.Something(testPersonUser);
            }
        }

        return ResultsTo.NotFound<PersonModel>();
    }

    private static List<TaxPaymentDetails> GetLatestProcessedForEachPeriode(List<TaxPaymentDetails> data)
    {
        var taxNumbers = data.Select(d => d.TaxNo).Distinct().ToList();
        var taxPeriodes = data.Select(d => d.TaxPeriode).Distinct().ToList();

        var result = new List<TaxPaymentDetails>();

        foreach (var taxNumber in taxNumbers)
        {
            foreach (var taxPeriode in taxPeriodes)
            {
                var entry = data.Where(d => d.TaxNo == taxNumber && d.TaxPeriode == taxPeriode).OrderByDescending(d => d.TaxFileProcessDate).FirstOrDefault();

                if (entry is null)
                {
                    continue;
                }

                result.Add(entry);
            }
        }

        return result.OrderByDescending(o => o.TaxPeriode).ToList();
    }

    private static PersonModel GenerateTestUser(PersonModel? request)
    {
        if (request is null)
        {
            return null;
        }

        var data = request;

        data.SofiNr = "999999999";
        data.EmailAddress = "test@user.com";
        data.UserName = "TEST@USER.COM";
        data.SignificantAchternaam = "Test";
        data.Voorletter = "T";
        data.Id = ObjectId.Empty;

        if (data?.Werkgever is not null)
        {
            data.Werkgever.ForEach(w =>
            {
                w.Klant = "Test Klant";
                w.LoonheffingsNr = "999999999L09";
            });
        }

        if (data?.TaxPaymentDetails is not null)
        {
            data.TaxPaymentDetails.ForEach(t => t.TaxNo = "999999999L09");
        }

        if (data?.PersonDaywages is not null)
        {
            data.PersonDaywages.ForEach(pd =>
            {
                pd.TaxNo = "999999999L09";
                pd.TaxDetails.ForEach(td => td.TaxNo = "999999999L09");
            });
        }

        return data;
    }
}
