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
    public static class AzureTags
    {

        public static BlobContainerClient FileSystem;
        public static ILogger log;

        public class IncludeAzure : DotLiquid.Block
        {
            private static readonly Regex Syntax = R.B(@"({0}+)(\s+(?:with|for)\s+({0}+))?", Liquid.QuotedFragment);

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
                    R.Scan(markup, Liquid.TagAttributes, (key, value) => _attributes[key] = value);
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
                if (_templateName is null || FileSystem is null || log is null || _attributes is null)
                    throw new SyntaxException("Template/Variable/FileSystem/log is Null");

                string shortenedTemplateName = _templateName.Substring(1, _templateName.Length - 2);
                object variable = context[_variableName ?? shortenedTemplateName, _variableName != null];
                string variable2 = (string)context[_templateName];
                if (variable2 == null || variable2 == "")
                {
                    variable2 = shortenedTemplateName;
                }

                var filename = variable2 + ".liquid";
                log.LogInformation("Liquid Action:Include Azure\n Filename:" + filename + "\n Status: FETCHING");
                var container = FileSystem.GetBlobClient(filename);
                log.LogInformation("Container/Blob Name is liquid-transforms/" + container.Name);
                var az_response = container.Download();
                StreamReader reader = new StreamReader(az_response.Value.Content);
                var inputBlob = reader.ReadToEnd();
                log.LogInformation(inputBlob);
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