using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prechart.Service.Core.Authorization
{
    public class AreaUserRight
    {
        public string Area { get; set; }
        public IEnumerable<UserRight> Rights { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Area);
            builder.Append('_');
            if (Rights.Contains(UserRight.Read))
            {
                builder.Append('1');
            }

            if (Rights.Contains(UserRight.Update))
            {
                builder.Append('2');
            }

            if (Rights.Contains(UserRight.Create))
            {
                builder.Append('3');
            }

            if (Rights.Contains(UserRight.Delete))
            {
                builder.Append('4');
            }

            return builder.ToString();
        }
    }
}
