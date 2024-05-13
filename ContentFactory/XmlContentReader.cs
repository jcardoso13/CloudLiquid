using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DotLiquid;
using Newtonsoft.Json;
using System.Text.Json;

namespace CloudLiquid.ContentFactory
{
    public class XmlContentReader : IContentReader
    {
        public XmlContentReader()
        {

        }
        public static XElement RemoveAllNamespaces(XElement e)
        {
            return new XElement(e.Name.LocalName,
              (from n in e.Nodes()
               select ((n is XElement) ? RemoveAllNamespaces(n as XElement) : n)),
                  (e.HasAttributes) ?
                    (from a in e.Attributes()
                     where (!a.IsNamespaceDeclaration)
                     select new XAttribute(a.Name.LocalName, a.Value)) : null);
        }
        public async Task<Hash> ParseRequestAsync(HttpContent content)
        {
            string requestBody = await content.ReadAsStringAsync();
            
            var transformInput = new Dictionary<string, object>();

            //var xDoc = XDocument.Parse(requestBody);
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(requestBody));
            var xDoc = new XDocument(xmlDocumentWithoutNs);
            var json = JsonConvert.SerializeXNode(xDoc).Replace("\"@","\"_");
            // Convert the XML converted JSON to an object tree of primitive types
            var requestJson = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string,object>>(json,new JsonSerializerOptions{
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true,
                    Converters = {new DictionaryStringObjectJsonConverter()}
                });
            // Wrap the JSON input in another content node to provide compatibility with Logic Apps Liquid transformations
            transformInput.Add("content", requestJson);

            return Hash.FromDictionary(transformInput);
        }

        public Hash  ParseString(string content)
        {
            var transformInput = new Dictionary<string, object>();
            //var xDoc = XDocument.Parse(requestBody);
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(content));
            var xDoc = new XDocument(xmlDocumentWithoutNs);
            var json = JsonConvert.SerializeXNode(xDoc).Replace("\"@","\"_");
            // Convert the XML converted JSON to an object tree of primitive types
            var requestJson = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string,object>>(json,new JsonSerializerOptions{
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true,
                    Converters = {new DictionaryStringObjectJsonConverter()}
                });

            // Wrap the JSON input in another content node to provide compatibility with Logic Apps Liquid transformations
            transformInput.Add("content", requestJson);

            return Hash.FromDictionary(transformInput);
        }

    }
}
