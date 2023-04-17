using System.Collections.Generic;
using Prechart.Service.Globals.Interfaces.Person;

namespace Prechart.Service.Person.Models;

public class PersonUpsertResults : IPersonUpsertResults
{
    public List<IPersonUpsertResult> Results { get; set; }
}