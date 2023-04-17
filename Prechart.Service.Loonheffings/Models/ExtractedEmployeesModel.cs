using System.Collections.Generic;
using Prechart.Service.Globals.Models.Loonheffings.Xsd.Xsd2022;

namespace Prechart.Service.Loonheffings.Models;

public class ExtractedEmployeesModel
{
    public string FileName { get; set; }
    public string TaxNumber { get; set; }
    public string KlantNaam { get; set; }   
    public List<InkomstenverhoudingInitieelTypeNatuurlijkPersoon> Employees { get; set; }
}