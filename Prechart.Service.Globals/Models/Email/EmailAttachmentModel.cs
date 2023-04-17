using MimeKit;

namespace Prechart.Service.Globals.Models.Email;

public class EmailAttachmentModel
{
    public string Filename { get; set; }
    public byte[] FileData { get; set; }
    public ContentType ContentType { get; set; }
}
