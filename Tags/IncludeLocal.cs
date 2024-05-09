using DotLiquid;
using DotLiquid.Exceptions;
using DotLiquid.Util;
using System.Collections;
using System.Text.RegularExpressions;

namespace CloudLiquid.Tags
{
    public class IncludeLocal : BaseCloudLiquidTag
    {
        #region Private Members

        private static readonly Regex Syntax = R.B(@"({0}+)(\s+(?:with|for)\s+({0}+))?", DotLiquid.Liquid.QuotedFragment);
        private string templateName;
        private string variableName;
        private Dictionary<string, string> attributes;

        #endregion

        #region Public Methods

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            Match syntaxMatch = Syntax.Match(markup);
            if (syntaxMatch.Success)
            {
                templateName = syntaxMatch.Groups[1].Value;
                variableName = syntaxMatch.Groups[3].Value;
                if (string.IsNullOrEmpty(variableName))
                {
                    variableName = null;
                }

                attributes = new Dictionary<string, string>(Template.NamingConvention.StringComparer);

                R.Scan(markup, Liquid.TagAttributes, (key, value) => attributes[key] = value);
            }
            else
            {
                throw new SyntaxException("Syntax Error in 'include' tag - Valid syntax: include [template]");
            }

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            if (templateName == null || attributes == null)
            {
                throw new SyntaxException("Template/Variable/log is Null");
            }

            string shortenedTemplateName = templateName.Substring(1, templateName.Length - 2);
            object variable = context[variableName ?? shortenedTemplateName, variableName != null];
            string variable2 = (string)context[templateName];
            if (string.IsNullOrEmpty(variable2))
            {
                variable2 = shortenedTemplateName;
            }

            var filename = variable2 + ".liquid";

            var inputBlob = File.ReadAllText(Directory.GetCurrentDirectory() + "/liquid/" + filename);

            Template partial = Template.Parse(inputBlob);


            context.Stack(() =>
            {
                foreach (var keyValue in attributes)
                    context[keyValue.Key] = context[keyValue.Value];

                if (variable is IEnumerable enumerable)
                {
                    enumerable.Cast<object>().ToList().ForEach(v =>
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

        #endregion
    }
}