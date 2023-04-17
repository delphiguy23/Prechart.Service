using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Prechart.Service.Core.Models;

namespace Prechart.Service.Core.Events;

public interface IEmailEventRecipient
{
    public EmailEventType EmailEventType { get; set; }
}