


using System.Collections;
using System.Text.RegularExpressions;
using DotLiquid;
using DotLiquid.Exceptions;
using DotLiquid.Util;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using CloudLiquid.ContentFactory;
using Newtonsoft.Json;
using System.Xml.Linq;
using DotLiquid.FileSystems;

namespace CloudLiquid
{


    public static class DataTags
    {
        public class CaptureJSON : DotLiquid.Block
        {

            internal static Regex VariableSegmentRegex => LazyVariableSegmentRegex.Value;
            private static readonly Lazy<Regex> LazyVariableSegmentRegex = new Lazy<Regex>(() => R.B(R.Q(@"\A\s*(?<Variable>{0}+)\s*\Z"), DotLiquid.Liquid.VariableSegment), LazyThreadSafetyMode.ExecutionAndPublication);


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
            private static readonly Lazy<Regex> LazyVariableSegmentRegex = new Lazy<Regex>(() => R.B(R.Q(@"\A\s*(?<Variable>{0}+)\s*\Z"), DotLiquid.Liquid.VariableSegment), LazyThreadSafetyMode.ExecutionAndPublication);


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

                public class IncludeLocal : DotLiquid.Block
        {
            private static readonly Regex Syntax = R.B(@"({0}+)(\s+(?:with|for)\s+({0}+))?", DotLiquid.Liquid.QuotedFragment);

            private string _templateName, _variableName;
            private Dictionary<string, string> _attributes;

            public override void Initialize(string tagName, string markup, List<string> tokens)
            {
                Match syntaxMatch = Syntax.Match(markup);
                if (syntaxMatch.Success)
                {
                    _templateName = syntaxMatch.Groups[1].Value;
                    _variableName = syntaxMatch.Groups[3].Value;
                    if (_variableName == string.Empty)
                        _variableName = null;
                    _attributes = new Dictionary<string, string>(Template.NamingConvention.StringComparer);
                    R.Scan(markup, DotLiquid.Liquid.TagAttributes, (key, value) => _attributes[key] = value);
                }
                else
                    throw new SyntaxException("Syntax Error in 'include' tag - Valid syntax: include [template]");

                base.Initialize(tagName, markup, tokens);
            }

            protected override void Parse(List<string> tokens)
            {
            }

            public override void Render(Context context, TextWriter result)
            {
                if (_templateName is null || _attributes is null)
                    throw new SyntaxException("Template/Variable/log is Null");

                string shortenedTemplateName = _templateName.Substring(1, _templateName.Length - 2);
                object variable = context[_variableName ?? shortenedTemplateName, _variableName != null];
                string variable2 = (string)context[_templateName];
                if (variable2 == null || variable2 == "")
                {
                    variable2 = shortenedTemplateName;
                }

                var filename = variable2 + ".liquid";
                var inputBlob = File.ReadAllText(System.IO.Directory.GetCurrentDirectory()+"/liquid/"+filename);
                Template partial = Template.Parse(inputBlob);


                context.Stack(() =>
                {
                    foreach (var keyValue in _attributes)
                        context[keyValue.Key] = context[keyValue.Value];

                    if (variable is IEnumerable)
                    {
                        ((IEnumerable)variable).Cast<object>().ToList().ForEach(v =>
                        {
                            context[shortenedTemplateName] = v;
                            partial.Render(result, RenderParameters.FromContext(context, result.FormatProvider));
                        });
                        return;
                    }

                    context[shortenedTemplateName] = variable;
                    partial.Render(result, RenderParameters.FromContext(context, result.FormatProvider));
                });
            }
        }
        
    }
}