


using System.Collections;
using System.Text.RegularExpressions;
using DotLiquid;
using DotLiquid.Exceptions;
using DotLiquid.Util;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using TransformData.ContentFactory;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace CloudLiquid
{


    public static class DataTags
    {
        public class CaptureJSON : DotLiquid.Block
        {

            internal static Regex VariableSegmentRegex => LazyVariableSegmentRegex.Value;
            private static readonly Lazy<Regex> LazyVariableSegmentRegex = new Lazy<Regex>(() => R.B(R.Q(@"\A\s*(?<Variable>{0}+)\s*\Z"), Liquid.VariableSegment), LazyThreadSafetyMode.ExecutionAndPublication);


            private string _to;

            public override void Initialize(string tagName, string markup, List<string> tokens)
            {
                Match syntaxMatch = VariableSegmentRegex.Match(markup);
                if (syntaxMatch.Success)
                    _to = syntaxMatch.Groups["Variable"].Value;
                else
                    throw new SyntaxException("JSONVarTagSyntaxException");

                base.Initialize(tagName, markup, tokens);

            }

            public override void Render(Context context, TextWriter result)
            {
                using (TextWriter temp = new StringWriter(result.FormatProvider))
                {
                    base.Render(context, temp);
                    string tempaux = temp.ToString();
                    var requestJson = JsonConvert.DeserializeObject<IDictionary<string, object>>(tempaux, new DictionaryConverter());
                    context.Scopes.Last()[_to] = Hash.FromDictionary(requestJson);
                    //context.Scopes.Last()[_to] = temp.ToString();
                }
            }
        }

        public class CaptureXML : DotLiquid.Block
        {

            internal static Regex VariableSegmentRegex => LazyVariableSegmentRegex.Value;
            private static readonly Lazy<Regex> LazyVariableSegmentRegex = new Lazy<Regex>(() => R.B(R.Q(@"\A\s*(?<Variable>{0}+)\s*\Z"), Liquid.VariableSegment), LazyThreadSafetyMode.ExecutionAndPublication);


            private string _to;

            public override void Initialize(string tagName, string markup, List<string> tokens)
            {
                Match syntaxMatch = VariableSegmentRegex.Match(markup);
                if (syntaxMatch.Success)
                    _to = syntaxMatch.Groups["Variable"].Value;
                else
                    throw new SyntaxException("JSONVarTagSyntaxException");

                base.Initialize(tagName, markup, tokens);

            }

            public override void Render(Context context, TextWriter result)
            {
                using (TextWriter temp = new StringWriter(result.FormatProvider))
                {
                    base.Render(context, temp);
                    string tempaux = temp.ToString();
                    //var xDoc = XDocument.Parse(requestBody);
                    XElement xmlDocumentWithoutNs = XmlContentReader.RemoveAllNamespaces(XElement.Parse(tempaux));
                    var xDoc = new XDocument(xmlDocumentWithoutNs);
                    var json = JsonConvert.SerializeXNode(xDoc).Replace("\"@", "\"_");
                    // Convert the XML converted JSON to an object tree of primitive types
                    var requestJson = JsonConvert.DeserializeObject<IDictionary<string, object>>(json, new DictionaryConverter());
                    context.Scopes.Last()[_to] = Hash.FromDictionary(requestJson);
                    //context.Scopes.Last()[_to] = temp.ToString();
                }
            }
        }
    }
}