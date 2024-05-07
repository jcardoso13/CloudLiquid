using Azure.Storage.Blobs;
using DotLiquid;
using DotLiquid.Exceptions;
using DotLiquid.Util;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Text.RegularExpressions;

namespace CloudLiquid.Tags
{
    // This class defines a Tag that includes a template from Azure Blob Storage
    public class AzureTag : BaseCloudLiquidTag
    {
        #region Private Members

        private static readonly Regex Syntax = R.B(@"({0}+)(\s+(?:with|for)\s+({0}+))?", DotLiquid.Liquid.QuotedFragment);
        private string templateName, variableName;
        private Dictionary<string, string> attributes;
        private BlobContainerClient blobContainerClient;

        #endregion

        #region Constructors

        public AzureTag() : base() { }

        public AzureTag(ILogger logger) : base(logger) { }

        public AzureTag(BlobContainerClient blobContainerClient) : base()
        {
            this.InitializeBlobContainerClient(blobContainerClient);
        }

        public AzureTag(ILogger logger, BlobContainerClient blobContainerClient) : base(logger) 
        { 
            this.InitializeBlobContainerClient(blobContainerClient);
        }

        #endregion

        #region Public Methods

        public void InitializeBlobContainerClient(BlobContainerClient blobContainerClient) 
        { 
            this.blobContainerClient = blobContainerClient; 
        }

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            Match syntaxMatch = Syntax.Match(markup);

            if (syntaxMatch.Success)
            {
                templateName = syntaxMatch.Groups[1].Value;
                variableName = syntaxMatch.Groups[3].Value;

                if (variableName == string.Empty)
                {
                    variableName = null;
                }

                attributes = new Dictionary<string, string>(Template.NamingConvention.StringComparer);
                R.Scan(markup, DotLiquid.Liquid.TagAttributes, (key, value) => attributes[key] = value);
            }
            else
            {
                throw new SyntaxException("Syntax Error in 'include' tag - Valid syntax: include [template]");
            }

            base.Initialize(tagName, markup, tokens);
        }

        protected override void Parse(List<string> tokens)
        {
        }

        public override void Render(Context context, TextWriter result)
        {
            if (this.templateName == null || blobContainerClient == null || this.Logger == null || attributes == null)
            {
                throw new SyntaxException("Template/Variable/FileSystem/log is Null");
            }

            string shortenedTemplateName = this.templateName.Substring(1, this.templateName.Length - 2);

            object variable = context[variableName ?? shortenedTemplateName, variableName != null];

            string templateName = (string)context[this.templateName];

            if (string.IsNullOrEmpty(templateName))
            {
                templateName = shortenedTemplateName;
            }

            var filename = $"{templateName}.liquid";

            this.Logger.LogInformation($"Liquid Action:Include Azure\n Filename: {filename}\n Status: FETCHING");

            var container = blobContainerClient.GetBlobClient(filename);

            this.Logger.LogInformation($"Container/Blob Name is liquid-transforms/{container.Name}");

            var azResponse = container.Download();

            StreamReader reader = new StreamReader(azResponse.Value.Content);

            var inputBlob = reader.ReadToEnd();

            this.Logger.LogInformation(inputBlob);

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