using CloudLiquid.ContentFactory;
using DotLiquid;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Text.Json;

namespace CloudLiquid.Tags
{
    public class CaptureXML : BaseCloudLiquidTag
    {
        #region Public Methods

        public override void Render(Context context, TextWriter result)
        {
            using TextWriter textWriter = new StringWriter(result.FormatProvider);
            base.Render(context, textWriter);
            string contents = textWriter.ToString();
            XElement xmlDocumentWithoutNs = XmlContentReader.RemoveAllNamespaces(XElement.Parse(contents));
            var xDoc = new XDocument(xmlDocumentWithoutNs);
            var json = JsonConvert.SerializeXNode(xDoc).Replace("\"@", "\"_");
            // Convert the XML converted JSON to an object tree of primitive types
            var requestJson = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string,dynamic>>(json,new JsonSerializerOptions{
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true,
                    AllowTrailingCommas = true,
                    Converters = {new DictionaryStringObjectJsonConverter()}
                });
            context.Scopes.Last()[this.To] = requestJson;
        }

        #endregion
    }
}