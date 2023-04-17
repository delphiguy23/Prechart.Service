using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace Prechart.Service.Core.TestBase
{
    public class HttpContentJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var t = JToken.FromObject(value);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                var o = (JObject)t;

                var httpContent = value as HttpContent;
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
                var data = httpContent.ReadAsStringAsync().Result;
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits

                o.AddFirst(new JProperty("Data", data));

                o.WriteTo(writer);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType) => typeof(HttpContent).IsAssignableFrom(objectType);
    }
}
