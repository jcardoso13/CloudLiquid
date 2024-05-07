using CloudLiquid.ContentFactory;
using DotLiquid;
using Newtonsoft.Json;
using System.Xml.Linq;

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
            var requestJson = JsonConvert.DeserializeObject<IDictionary<string, object>>(json, new DictionaryConverter());
            context.Scopes.Last()[this.To] = Hash.FromDictionary(requestJson);
        }

        #endregion
    }
}