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

        /// <summary>
        /// Initializes a new instance with the specified logger.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public AzureTag(ILogger logger) : base(logger) { }

        /// <summary>
        /// Initializes a new instance with the specified BlobContainerClient.
        /// </summary>
        /// <param name="blobContainerClient">The BlobContainerClient.</param>
        public AzureTag(BlobContainerClient blobContainerClient) : base()
        {
            this.InitializeBlobContainerClient(blobContainerClient);
        }

        ///<summary>
        /// Initializes a new instance with the specified logger and BlobContainerClient.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="blobContainerClient">The BlobContainerClient.</param>
        public AzureTag(ILogger logger, BlobContainerClient blobContainerClient) : base(logger) 
        { 
            this.InitializeBlobContainerClient(blobContainerClient);
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Initializes the BlobContainerClient.
        /// </summary>
        /// <param name="blobContainerClient">The BlobContainerClient.</param>
        public void InitializeBlobContainerClient(BlobContainerClient blobContainerClient) 
        { 
            this.blobContainerClient = blobContainerClient; 
        }

        /// <summary>
        /// Initializes the tag with the specified tagName, markup, and tokens.
        /// </summary>
        /// <param name="tagName">The name of the tag.</param>
        /// <param name="markup">The markup for the tag.</param>
        /// <param name="tokens">The list of tokens.</param>
        /// <exception>Thrown when the markup syntax is incorrect.</exception>
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

        /// <summary>
        /// Renders the tag to the specified result using the provided context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="result">The result to render to.</param>
        /// <exception>Thrown when the template, variable, file system, or logger is null.</exception>
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

            var blobClient = blobContainerClient.GetBlobClient(filename);

            this.Logger.LogInformation($"Container/Blob Name is liquid-transforms/{blobClient.Name}");

            var azResponse = blobClient.Download();

            StreamReader reader = new(azResponse.Value.Content);

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