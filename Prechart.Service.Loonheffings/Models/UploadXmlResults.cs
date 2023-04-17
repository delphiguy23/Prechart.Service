using System.Collections.Generic;
using Prechart.Service.Globals.Interfaces.Documents;

namespace Prechart.Service.Loonheffings.Models;

public class UploadXmlResults : ILoonaangiftesUploadResults
{
    public List<ILoonaangifteUploadResult> Status { get; set; }
}