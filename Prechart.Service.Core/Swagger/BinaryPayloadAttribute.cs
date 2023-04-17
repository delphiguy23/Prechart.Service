using System;

namespace Prechart.Service.Core.Swagger
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BinaryPayloadAttribute : Attribute
    {
        public BinaryPayloadAttribute(string contentType) => ContentType = contentType;

        public string ContentType { get; }
    }
}
