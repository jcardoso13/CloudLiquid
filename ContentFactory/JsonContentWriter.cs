using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace CloudLiquid.ContentFactory
{
    public class JsonContentWriter : IContentWriter
    {
        string _contentType;

        public JsonContentWriter(string contentType)
        {
            _contentType = contentType;
        }

        public StringContent CreateResponse(string output)
        {
            JsonSerializerOptions jsonSerializerSettings = new JsonSerializerOptions{
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true,
                    Converters = {new DictionaryStringObjectJsonConverter()}
                };

            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(output,jsonSerializerSettings);
            var jsonString = JsonSerializer.Serialize(jsonObject,jsonSerializerSettings);

            return new StringContent(jsonString, Encoding.UTF8, _contentType);
        }
    }
}
