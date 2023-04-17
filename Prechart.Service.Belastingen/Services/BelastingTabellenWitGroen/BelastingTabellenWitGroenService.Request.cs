using System;
using System.Collections.Generic;
using Prechart.Service.Globals.Interfaces.Belastingen;
using Prechart.Service.Globals.Models.Batch;
using Prechart.Service.Globals.Models.Belastingen;

namespace Prechart.Service.Belastingen.Services.BelastingTabellenWitGroen;

public partial class BelastingTabellenWitGroenService
{
    public record GetInhouding
    {
        public decimal InkomenWit { get; set; }
        public decimal InkomenGroen { get; set; }
        public decimal BasisDagen { get; set; }
        public DateTime Geboortedatum { get; set; }
        public bool AlgemeneHeffingsKortingIndicator { get; set; }
        public TaxPeriodEnum Loontijdvak { get; set; }
        public int WoondlandBeginselId { get; set; }
        public DateTime ProcesDatum { get; set; }
    }

    public record UpsertToTable
    {
        public List<ITaxRecord> TaxTable { get; set; }
        public string TaxType { get; set; }
    }

    public record ProcessPendingCsv
    {
        public BatchProcess BatchRecords { get; set; }
    }

    public record GetAlleWoonlandbeginsel;

    public record GetTaxYear;
}
