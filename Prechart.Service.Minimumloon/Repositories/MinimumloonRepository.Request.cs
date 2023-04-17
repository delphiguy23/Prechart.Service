using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prechart.Service.Minimumloon.Models;

namespace Prechart.Service.Minimumloon.Repositories;

public partial class MinimumloonRepository
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
        public int? Leeftijd { get; set; }
    }
}
