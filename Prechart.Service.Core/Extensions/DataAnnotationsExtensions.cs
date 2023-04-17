using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Prechart.Service.Core.Extensions
{
    public static class DataAnnotationsExtensions
    {
        public static bool TryValidate(this object theObject, out ICollection<ValidationResult> results)
        {
            var context = new ValidationContext(theObject, serviceProvider: null, items: null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(theObject, context, results, validateAllProperties: true);
        }
    }
}
