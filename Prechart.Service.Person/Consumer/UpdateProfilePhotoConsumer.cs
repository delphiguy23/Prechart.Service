using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Globals.Interfaces.Person;
using Prechart.Service.Globals.Models;
using Prechart.Service.Person.Services.Person;

namespace Prechart.Service.Person.Consumer;

public class UpdatePersonPhotoConsumer : IConsumer<IUpdatePersonPhoto>
{
    private readonly ILogger<UpdatePersonPhotoConsumer> _logger;
    private readonly IPersonService _service;

    public UpdatePersonPhotoConsumer(ILogger<UpdatePersonPhotoConsumer> logger, IPersonService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IUpdatePersonPhoto> context)
    {
        _logger.LogInformation($"{nameof(IUpdatePersonPhoto)} event with payload {context.Message}");

        try
        {
            var result = await _service.HandleAsync(new PersonService.UpdatePersonPhoto
            {
                Id = context.Message.Id.ToObjectId(),
                PersonPhoto = context.Message.PersonPhoto,
            });

            if (result is null && result.IsNotFoundOrBadRequest())
            {
                await context.RespondAsync(new NotFound());
            }

            await context.RespondAsync<IUpdatePersonPhotoResults>(new {Result = result?.Value});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Consume));
        }
    }
}