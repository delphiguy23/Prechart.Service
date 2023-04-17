using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;
using Newtonsoft.Json;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Models;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Documents.Upload.Csv.Models;
using Prechart.Service.Documents.Upload.Models;
using Prechart.Service.Globals.Helper;
using Prechart.Service.Globals.Interfaces;
using Prechart.Service.Globals.Interfaces.Batch;
using Prechart.Service.Globals.Interfaces.Belastingen;
using Prechart.Service.Globals.Interfaces.Documents;
using Prechart.Service.Globals.Interfaces.Person;
using Prechart.Service.Globals.Models;
using Prechart.Service.Globals.Models.Batch;
using Prechart.Service.Globals.Models.Email;
using Prechart.Service.Globals.Models.Person;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Documents.Upload.Csv.Services;

public partial class DocumentsService : IDocumentsService
{
    private readonly IBatchHelper _batch;
    private readonly ILogger<DocumentsService> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IRequestClient<ILoonaangifteUploadEvent> _xmlUploadEvent;
    private readonly IRequestClient<IUpdatePersonPhoto> _updatePersonPhoto;
    private readonly IRequestClient<IDownloadPersonPhoto> _downloadPersonPhoto;

    public DocumentsService(ILogger<DocumentsService> logger,
        IBatchHelper batch,
        IRequestClient<ILoonaangifteUploadEvent> xmlUploadEvent,
        IPublishEndpoint publishEndpoint,
        IRequestClient<IUpdatePersonPhoto> updatePersonPhoto,
        IRequestClient<IDownloadPersonPhoto> downloadPersonPhoto
    )
    {
        _logger = logger;
        _batch = batch;
        _xmlUploadEvent = xmlUploadEvent;
        _publishEndpoint = publishEndpoint;
        _updatePersonPhoto = updatePersonPhoto;
        _downloadPersonPhoto = downloadPersonPhoto;
    }

    public async Task<IFluentResults<IInsertToTaxResultMessage>> HandleAsync(ProcessUploadedCsvFiles request, CancellationToken cancellationToken = default)
    {
        var taxResultMessage = new TaxResultMessage();
        var taxResult = new List<IInsertToTaxResult>();
        var csvList = new List<string>();

        try
        {
            var ctr = 1;
            var fileCount = request.Files.Count;

            foreach (var f in request.Files)
            {
                _logger.LogInformation($"Processing file {ctr++} of {fileCount}. Filename: {f.FileName}");
                if (!TaxFilingCsvProcessorHelper.IsCsvValid(f.OpenReadStream()))
                {
                    taxResult.Add(new TaxResult { Filename = f.FileName, IsProcessed = false });
                    taxResultMessage.InsertToTaxResult = taxResult;
                    taxResultMessage.Message = "Unable to process files error occured :  Invalid CSV ";

                    await SendEmailNotification("CSV Upload Failed", taxResultMessage.Message, EmailEventType.CsvDocumentProcess, null);

                    return ResultsTo.BadRequest<IInsertToTaxResultMessage>(taxResultMessage).WithMessage("Unable to process files error occured :  Invalid CSV ");
                }

                var csvRemoveHeader = TaxFilingCsvProcessorHelper.ValidateRemoveHeader(f.OpenReadStream(), "Tabelloon");

                var result =
                    await HandleAsync(new ProcessUploadedFile { FileName = f.FileName, FileStream = csvRemoveHeader });

                taxResult.Add(new TaxResult { Filename = f.FileName, IsProcessed = result?.Value ?? false });
                taxResultMessage.InsertToTaxResult = taxResult;
                csvList.Add(f.FileName);
            }

            taxResultMessage.Message = taxResultMessage.InsertToTaxResult.Any(i => !i.IsProcessed) ? "Not all files are processed succesfully" : "Files are processed succesfully";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            taxResultMessage.InsertToTaxResult.ToList().ForEach(i => i.IsProcessed = false);
            taxResultMessage.Message = $"Unable to process files error occured : {ex.Message} ";

            await SendEmailNotification("CSV Upload Failed", taxResultMessage.Message, EmailEventType.CsvDocumentProcess, null);
        }

        await SendEmailNotification("CSV Upload", taxResultMessage.Message, EmailEventType.CsvDocumentProcess, request.Files);

        return ResultsTo.Something<IInsertToTaxResultMessage>(taxResultMessage);
    }

    public async Task<IFluentResults<ILoonaangiftesUploadResults>> HandleAsync(ProcessUploadedXmlFiles request, CancellationToken cancellationToken = default)
    {
        var xmlStream = new List<IXmlStream>();

        foreach (var file in request.Files)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                xmlStream.Add(new XmlStreamModel { FileName = file.FileName, Stream = stream.ToArray() });
            }
        }

        var uploadEvent = new LoonaangifteUploadEvent
        {
            XsdYear = request.XsdYear,
            Files = xmlStream,
        };

        var xmlUploadsStatus = await _xmlUploadEvent.GetResponse<ILoonaangiftesUploadResults, NotFound>(uploadEvent, timeout: RequestTimeout.After(m: 5));

        await Task.Delay(5 * 1000);
        await _publishEndpoint.Publish<IBatchTriggerNow>(new { BatchName = "XmlUpload" });

        if (xmlUploadsStatus.Is(out Response<ILoonaangiftesUploadResults> response))
        {
            return ResultsTo.Success(response.Message);
        }

        return ResultsTo.NotFound<ILoonaangiftesUploadResults>();
    }
    public async Task<IFluentResults<bool>> HandleAsync(UpdatePersonPhoto request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.PersonPhoto is null)
            {
                return ResultsTo.BadRequest<bool>().WithMessage("Photo not uploaded");
            }

            var proPhoto = request.PersonPhoto;
            var streamFile = new MemoryStream();
            await request.PersonPhoto.CopyToAsync(streamFile);

            var PersonPhoto = new PersonPhotoModel
            {
                Filename = proPhoto.FileName,
                FileData = streamFile.ToArray() ?? null,
                MediaType = ContentType.Parse(proPhoto.ContentType ?? string.Empty).MediaType.FirstCharacterUpperCase().StringToMediaType(),
                MediaSubtype = ContentType.Parse(proPhoto.ContentType ?? string.Empty).MediaSubtype.FirstCharacterUpperCase().StringToMediaSubtype(),
            };

            var response = await _updatePersonPhoto.GetResponse<IUpdatePersonPhotoResults, NotFound>(new
            {
                Id = request.Id,
                PersonPhoto = PersonPhoto,
            });

            if ((!response.Is(out Response<IUpdatePersonPhotoResults> personPhotoUploadResponse)) || personPhotoUploadResponse.Message.Result == false)
            {
                return ResultsTo.Failure<bool>("Failed to Upload Images");
            }

            return ResultsTo.Success(personPhotoUploadResponse.Message.Result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return ResultsTo.Failure<bool>(ex.Message);
        }
    }

    public async Task<IFluentResults<FileContentResult>> HandleAsync(DownloadPersonPhoto request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _downloadPersonPhoto.GetResponse<IDownloadPersonPhotoResults, NotFound>(new
            {
                Id = request.Id,
            });

            if (!response.Is(out Response<IDownloadPersonPhotoResults> personPhotoDownloadResponse))
            {
                return ResultsTo.Failure<FileContentResult>().WithMessage("Person or photo does not exist");
            }
            else
            {
                return ResultsTo.Success(new FileContentResult(personPhotoDownloadResponse.Message.FileData, $"{personPhotoDownloadResponse.Message.MediaType}/{personPhotoDownloadResponse.Message.MediaSubtype}"));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return ResultsTo.Failure<FileContentResult>().FromException(ex);
        }
    }

    private async Task<IFluentResults<bool>> HandleAsync(ProcessUploadedFile request)
    {
        try
        {
            var fileExt = Path.GetExtension(request.FileName);

            switch (fileExt.ToLower())
            {
                case ".csv":
                    await _batch.CreateBatchRecord(new BatchProcess
                    {
                        BatchName = "CsvUpload",
                        TotalTask = 1,
                        CompletedTask = 0,
                        Status = BatchProcessStatus.ReadyForProcessing,
                        PublishedOn = DateTime.Now,
                        ScheduledOn = DateTime.Now,
                        Payload = JsonConvert.SerializeObject(new CsvBatchRecord
                        {
                            FileName = request.FileName,
                            CsvFile = await request.FileStream.ToByteArray(),
                        }),
                    });

                    return ResultsTo.Success<bool>(true);
                default: return ResultsTo.Success<bool>(false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return ResultsTo.Failure<bool>(false).FromException(ex);
        }
    }

    private async Task SendEmailNotification(string subject, string body, EmailEventType emailEventType, List<IFormFile>? attachments)
    {
        try
        {
            var emailbody = body;
            List<EmailAttachmentModel> files = new();

            if (attachments is not null)
            {
                emailbody = EmailHelper.EmailBodyCreator(body, "Filename").ToString().Replace("{__emailbody__}", CreateSendCsvUploadEmailBody(attachments));

                foreach (var attachment in attachments)
                {
                    var streamFile = new MemoryStream();
                    await attachment.CopyToAsync(streamFile);
                    files.Add(new EmailAttachmentModel { Filename = attachment.FileName, FileData = streamFile?.ToArray() ?? null, ContentType = ContentType.Parse(attachment.ContentType) });
                }
            }

            await _publishEndpoint.Publish<IEmailEvent>(
                new EmailEventModel
                {
                    Subject = subject,
                    Body = emailbody,
                    EmailEventType = emailEventType,
                    Attachments = files,
                });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    private string CreateSendCsvUploadEmailBody(List<IFormFile> attachments)
    {
        var body = new StringBuilder();
        var formatProvider = CultureInfo.InvariantCulture;

        try
        {
            if (attachments is null || !attachments.Any())
            {
                return body.ToString();
            }

            foreach (var c in attachments)
            {
                body.AppendLine(@"<TR>");
                body.AppendLine(formatProvider, $@"<td>{c.FileName}</td>");
                body.AppendLine(@"</tr>");
            }
        }
        catch (Exception ex)
        {
            body.Clear();
            _logger.LogError(ex, "Error Occurred during the creation of email body");
        }

        return body.ToString();
    }
}
