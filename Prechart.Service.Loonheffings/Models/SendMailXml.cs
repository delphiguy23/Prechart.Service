using Prechart.Service.Globals.Interfaces.Documents;
using System.Collections.Generic;

namespace Prechart.Service.Loonheffings.Models;

public class SendMailXml
{
    public string FileName { get; set; }
    public byte[] Stream { get; set; }
    public string Error { get; set; }
    public bool IsValid { get; set; }
}
