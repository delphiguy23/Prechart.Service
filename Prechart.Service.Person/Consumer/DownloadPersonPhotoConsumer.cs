using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Globals.Interfaces.Person;
using Prechart.Service.Globals.Models;
using Prechart.Service.Person.Models;
using Prechart.Service.Person.Services.Person;
using System;
using System.Threading.Tasks;

namespace Prechart.Service.Person.Consumer;

public class DownloadPersonPhotoConsumer : IConsumer<IDownloadPersonPhoto>
{
    private readonly ILogger<DownloadPersonPhotoConsumer> _logger;
    private readonly IPersonService _service;

    public DownloadPersonPhotoConsumer(ILogger<DownloadPersonPhotoConsumer> logger, IPersonService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IDownloadPersonPhoto> context)
    {
        _logger.LogInformation($"{nameof(DownloadPersonPhotoConsumer)} event with payload {context.Message}");

        try
        {
            var result = await _service.HandleAsync(new PersonService.DownloadPersonPhoto
            {
                Id = context.Message.Id.ToObjectId(),
            });

            var downloadModel = new PersonDownloadModel();

            if (result.IsNotFoundOrBadRequest() || result.IsFailure())
            {
                await context.RespondAsync(new NotFound());
            }
            else
            {
                downloadModel.FileData = result?.Value.FileData is null ? null : result.Value.FileData;
                downloadModel.Filename = result?.Value.Filename is null ? null : result.Value.Filename;
                downloadModel.MediaSubtype = result?.Value.MediaSubtype is null ? 0 : result.Value.MediaSubtype;
                downloadModel.MediaType = result?.Value.MediaType is null ? 0 : result.Value.MediaType;
            }

            await context.RespondAsync(downloadModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Consume));
        }
    }
}