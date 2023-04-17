using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;

namespace Prechart.Service.Documents.Upload.Csv.Services
{
    public partial class DocumentsService
    {
        public record ProcessUploadedFile
        {
            public string FileName { get; set; }
            public Stream FileStream { get; set; }
        }

        public record ProcessCsvFile
        {
            public string FileName { get; set; }
            public Stream FileStream { get; set; }
        }

        public record ProcessUploadedCsvFiles
        {
            public List<IFormFile> Files { get; set; }
        }

        public record ProcessUploadedXmlFiles
        {
            public List<IFormFile> Files { get; set; }
            public int XsdYear { get; set; }
        }

        public class UpdatePersonPhoto
        {
            public string Id { get; set; }
            public IFormFile PersonPhoto { get; set; }
        }

        public class DownloadPersonPhoto
        {
            public string Id { get; set; }
        }
    }
}