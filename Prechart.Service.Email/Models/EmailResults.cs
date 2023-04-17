using Prechart.Service.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prechart.Service.Email.Models
{
    public class EmailResults
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
    }
}
