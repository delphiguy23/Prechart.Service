using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Prechart.Service.Core.Models;
using Prechart.Service.Core.Utils;
using Prechart.Service.Globals.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prechart.Service.Email.Models;

public class SmtpModel
{
    public string Sender { get; set; }
    public string Email { get; set; }
    public string Host { get; set; }
    public string Username { get; set; }
    public int Port { get; set; }
    public string Password { get; set; }
}
