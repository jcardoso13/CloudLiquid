using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using System.Text.Json;
using System.Dynamic;
using System.IO;
using System.Text.Json.Serialization;

namespace CloudLiquid.ContentFactory
{
    public class JsonContentReader : IContentReader
    {
        public JsonContentReader()
        {

        }

        public async Task<Hash> ParseRequestAsync(HttpContent content)
        {
            string requestBody = await content.ReadAsStringAsync();

            var transformInput = new Dictionary<string, object>();

            var requestJson = JsonSerializer.Deserialize<Dictionary<string, object>>(requestBody, new JsonSerializerOptions
            {
                Converters = {new DictionaryStringObjectJsonConverter()}
            });
            //var requestJson = JsonConvert.DeserializeObject<IDictionary<string, object>>(requestBody, new DictionaryConverter());

            // Wrap the JSON input in another content node to provide compatibility with Logic Apps Liquid transformations
            transformInput.Add("content", requestJson);

            return Hash.FromDictionary(transformInput);
        }

		public Hash ParseString(string content)
		{
			var transformInput = new Dictionary<string, object>();
            var requestJson = JsonSerializer.Deserialize<Dictionary<string, object>>(content, new JsonSerializerOptions
            {
                Converters = {new DictionaryStringObjectJsonConverter()}
            });
            transformInput.Add("content", requestJson);
            return Hash.FromDictionary(transformInput);
		}


    }
}
