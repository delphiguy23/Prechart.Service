using System;

namespace Prechart.Service.Belastingen.Database.Models
{
    public class AOW
    {
        public int Id { get; set; }
        public DateTime GeborenNa { get; set; }
        public DateTime GeborenVoor { get; set; }
        public int? GerechtigdIn { get; set; }
        public int? ExtraMaandenNa65 { get; set; }

    }
}