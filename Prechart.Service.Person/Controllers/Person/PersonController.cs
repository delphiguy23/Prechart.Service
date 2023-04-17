using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;
using Prechart.Service.AuditLog.Utils;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Globals.Helper;
using Prechart.Service.Globals.Models.Person;
using Prechart.Service.Person.Models;
using Prechart.Service.Person.Services.Person;

namespace Prechart.Service.Person.Controllers.Person;

// [Authorize(Roles = "SuperAdmin")]
[ApiController]
[Route("/platform/service/api/person/")]
public class PersonController : ControllerBase
{
    private readonly ILogger<PersonController> _logger;
    private readonly IPersonService _service;

    public PersonController(ILogger<PersonController> logger, IPersonService service)
    {
        _logger = logger;
        _service = service;
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpGet]
    [Route("type/{id}")]
    [LogAuditAction]
    public async Task<ActionResult> Get(int id)
    {
        return (await _service.HandleAsync(new PersonService.GetPersons
        {
            PersonType = (PersonType) id,
        }, CancellationToken.None)).ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpGet]
    [Route("id/{id}")]
    [LogAuditAction]
    public async Task<ActionResult> GetById(string id)
    {
        return (await _service.HandleAsync(new PersonService.GetPersonBy
        {
            Id = id,
        }, CancellationToken.None)).ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpGet]
    [Route("bsn/{id}")]
    [LogAuditAction]
    public async Task<ActionResult> GetByBsn(string id)
    {
        return (await _service.HandleAsync(new PersonService.GetPersonBy
        {
            Bsn = id,
        }, CancellationToken.None)).ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpGet]
    [Route("lastname/{name}")]
    [LogAuditAction]
    public async Task<ActionResult> GetByName(string name)
    {
        return (await _service.HandleAsync(new PersonService.GetPersonBy
        {
            LastName = name,
        }, CancellationToken.None)).ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin, Employee")]
    [HttpGet]
    [Route("username/{username}")]
    [LogAuditAction]
    public async Task<ActionResult> GetByUserName(string username)
    {
        return (await _service.HandleAsync(new PersonService.GetPersonBy
        {
            UserName = username,
        }, CancellationToken.None)).ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpPost]
    [Route("delete/{id}")]
    [LogAuditAction]
    public async Task<ActionResult> DeleteById(string id)
    {
        return (await _service.HandleAsync(new PersonService.DeletePerson
        {
            Id = id,
        }, CancellationToken.None)).ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpGet]
    [Route("werkgever/{taxno}")]
    [LogAuditAction]
    public async Task<ActionResult> GetByWerkgever(string taxno)
    {
        return (await _service.HandleAsync(new PersonService.WerkgeversPersons
        {
            TaxNo = taxno,
            FullHistory = false,
        }, CancellationToken.None)).ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpGet]
    [Route("werkgever/{taxno}/fullhistory")]
    [LogAuditAction]
    public async Task<ActionResult> GetByWerkgeverHistory(string taxno)
    {
        return (await _service.HandleAsync(new PersonService.WerkgeversPersons
        {
            TaxNo = taxno,
            FullHistory = true,
        }, CancellationToken.None)).ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin, Employee")]
    [HttpGet]
    [Route("cumulative/{bsn}")]
    [LogAuditAction]
    public async Task<ActionResult> GetPersonCumulative(string bsn)
    {
        var result = await _service.HandleAsync(new PersonService.GetPersonTaxCumulative
        {
            Bsn = bsn,
        }, CancellationToken.None);

        return result.ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin, Employee")]
    [HttpPost]
    [Route("upsertperson")]
    [LogAuditAction]
    public async Task<ActionResult> UpsertPerson([FromBody] PersonUserModel personUser)
    {
        return (await _service.HandleAsync(new PersonService.UpsertPersonUser
        {
            PersonUser = personUser,
        }, CancellationToken.None)).ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin, Employee")]
    [HttpPost]
    [Route("uploadpersonphoto")]
    [LogAuditAction]
    public async Task<ActionResult> UploadPersonPhoto(string personId, IFormFile personPhoto)
    {
        var streamFile = new MemoryStream();
        var photo = new PersonPhotoModel();
        if (personPhoto is not null)
        {
            var proPhoto = personPhoto;
            await personPhoto.CopyToAsync(streamFile);

            photo.Filename = personPhoto?.FileName ?? null;
            photo.FileData = streamFile?.ToArray() ?? null;
            photo.MediaType = ContentType.Parse(personPhoto?.ContentType ?? null).MediaType.FirstCharacterUpperCase().StringToMediaType();
            photo.MediaSubtype = ContentType.Parse(personPhoto?.ContentType ?? null).MediaSubtype.FirstCharacterUpperCase().StringToMediaSubtype();
        }

        var results = await _service.HandleAsync(new PersonService.UpdatePersonPhoto
        {
            Id = personId.ToObjectId(),
            PersonPhoto = photo,
        });

        return results.ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin, Employee")]
    [HttpPost]
    [Route("downloadpersonphoto")]
    [LogAuditAction]
    public async Task<ActionResult> DownloadPersonPhoto(string personId)
    {
        var results = await _service.HandleAsync(new PersonService.DownloadPersonPhotoViaController
        {
            Id = personId.ToObjectId(),
        });

        if (results.IsNotFoundOrBadRequest() || results.IsFailure())
        {
            return results.ToActionResult();
        }

        return results.Value;
    }

    [Authorize(Roles = "SuperAdmin, Employee, Register, Profile")]
    [HttpGet]
    [Route("getprofile/{id}")]
    [LogAuditAction]
    public async Task<ActionResult> GetProfile(string id)
    {
        var result = await _service.HandleAsync(new PersonService.GetProfile
        {
            Id = id.ToObjectId(),
        }, CancellationToken.None);

        return result.ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpPost]
    [Route("deactivateperson/{id}")]
    [LogAuditAction]
    public async Task<ActionResult> DeactivatePerson(string id)
    {
        return (await _service.HandleAsync(new PersonService.DeactivatePersonUser
        {
            Id = id.ToObjectId(),
        }, CancellationToken.None)).ToActionResult();
    }
}
