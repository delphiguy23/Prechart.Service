using Prechart.Service.Belastingen.Models;
using System.Collections.Generic;
using Prechart.Service.Globals.Models.Belastingen;
using System;
using Prechart.Service.Globals.Interfaces.Belastingen;

namespace Prechart.Service.Belastingen.Repositories.BelastingTabellenWitGroen;

public partial class BelastingTabellenWitGroenRepository 
{
    public record GetWoonlandbeginselCode
    {
        public string WoonlandbeginselCode { get; set; }
    }

    public record GetWoonlandbeginselId
    {
        public int WoonlandbeginselId { get; set; }
    }

    public record GetTypeId
    {
        public string Name { get; set; }
    }

    public record GetWoonlandbeginsel;

    public record GetTypes;

    public record UpsertToTable
    {
        public List<ITaxRecord> TaxTable { get; set; }
        public string TaxType { get; set; }
    }

    public record InsertToTable
    {
        public List<InhoudingModel> TaxTable { get; set; }
        public string TaxType { get; set; }
    }

    public record GetInhoudingGreen
    {
        public decimal InkomenGroen {get; set;}
        public decimal BasisDagen {get; set;}
        public DateTime Geboortedatum { get; set; }
        public bool AhkInd {get; set;}
        public TaxPeriodEnum Loontijdvak {get; set;}        
        public int WoondlandBeginselId {get; set;}
        public int Jaar {get; set;}
    }

    public record GetInhoudingWhite
    {
        public decimal InkomenWit {get; set;}
        public decimal BasisDagen {get; set;}
        public DateTime Geboortedatum { get; set; }
        public bool AhkInd {get; set;}
        public TaxPeriodEnum Loontijdvak {get; set;}        
        public int WoondlandBeginselId {get; set;}
        public int Jaar {get; set;}
    }

        public record GetInhoudingBoth
    {
        public decimal InkomenWit {get; set;}
        public decimal InkomenGroen {get; set;}
        public decimal BasisDagen {get; set;}
        public DateTime Geboortedatum { get; set; }
        public bool AhkInd {get; set;}
        public TaxPeriodEnum Loontijdvak {get; set;}        
        public int WoondlandBeginselId {get; set;}
        public int Jaar {get; set;}
    }

    public record GetTaxYear;

    public record IsAow
    {
        public DateTime Geboortedatum { get; set; }
    }
}
