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
        /// <summary>
        /// Renders the tag, capturing its content, parsing it as XML, and converting it to JSON.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="result">The text writer to render to.</param>
        /// <exception>Thrown when the captured content cannot be parsed as JSON.</exception>
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