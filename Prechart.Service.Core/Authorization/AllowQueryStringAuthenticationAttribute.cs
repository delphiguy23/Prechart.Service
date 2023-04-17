using System;

namespace Prechart.Service.Core.Authorization
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowQueryStringAuthenticationAttribute : Attribute
    {
    }
}
