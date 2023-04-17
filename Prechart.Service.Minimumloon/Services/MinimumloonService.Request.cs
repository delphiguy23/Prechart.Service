using Prechart.Service.Minimumloon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prechart.Service.Minimumloon.Services;

public partial class MinimumloonService
{
    public record UpsertMinimumLoon
    {
        public List<MinimumloonModel> Minimumloon { get; set; }
    }
    public record GetMinimumloon
    {
        public DateTime Datum { get; set; }
    }

    public record DeleteMinimumloon
    {
        public DateTime Datum { get; set; }
        public int Leeftijd { get; set; }
    }
}
