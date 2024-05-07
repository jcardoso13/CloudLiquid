using Azure.Storage.Blobs;
using CloudLiquid.ObjectModel;
using CloudLiquid.Tags;
using DotLiquid;
using Microsoft.Extensions.Logging;
using System.Text;

namespace CloudLiquid
{
    public class CloudLiquid
    {
        #region Private Members

        private static readonly Dictionary<string, string> _dict = new Dictionary<string, string>{
            {"CloudLiquid_Start","Error Registering Tags and Templates"},
            {"Parsing_Liquid","Error parsing Liquid"},
            {"Rendering_Output","Error rendering output using DotLiquid Engine"},
            {"Checking_Output_For_Errors","InnerException found in the Output from Liquid Engine"}
        };

        private readonly BlobContainerClient blobContainerClient;
        private readonly ILogger logger;

        private string action;
        private string errorMessage;

        #endregion

        #region Constructors

        public CloudLiquid() { }


        public CloudLiquid(ILogger logger) : this()
        {
            this.logger = logger;
        }

        public CloudLiquid(BlobContainerClient blobContainerClient) : this()
        {
            this.blobContainerClient = blobContainerClient;
        }

        public CloudLiquid(ILogger logger, BlobContainerClient blobContainerClient) : this()
        {
            this.logger = logger;
            this.blobContainerClient = blobContainerClient;
        }

        #endregion

        #region Public Methods

        public void InitializeTagsAndFilters()
        {
            Template.RegisterTagFactory(new CloudLiquidTagFactory(typeof(IncludeLocal), "include_local", logger));
            Template.RegisterTagFactory(new CloudLiquidTagFactory(typeof(AzureTag), "include_azure", logger, blobContainerClient));
            Template.RegisterTagFactory(new CloudLiquidTagFactory(typeof(CaptureJSON), "capture_json", logger));
            Template.RegisterTagFactory(new CloudLiquidTagFactory(typeof(CaptureXML), "capture_xml", logger));
            Template.RegisterFilter(typeof(DataFilters));
        }

        public RunResult Run(Hash input, string file)
        {
            RunResult result = new() { Success = true };

            action = "CloudLiquid_Start";

            logger.LogInformation("Starting CloudLiquidRuntime");

            string output = null;

            try
            {
                action = "Parsing_Liquid";

                Template liquid = Template.Parse(file);

                action = "Rendering_Output";

                output = liquid.Render(input);

                if (liquid.Errors?.Count > 0)
                {
                    logger.LogInformation("Errors Found:");

                    StringBuilder sbMessage = new();

                    int count = liquid.Errors.Count > 5 ? 5 : liquid.Errors.Count;

                    for (int i = 0; i < count; i++)
                    {
                        if (liquid.Errors[i].InnerException != null)
                        {
                            action = "Checking_Output_For_Errors";
                            throw new Exception(liquid.Errors[i].Message);
                        }
                        else
                        {
                            sbMessage.AppendLine($"Warning rendering Liquid liquid: {liquid.Errors[i].Message}");
                        }
                    }

                    logger.LogWarning($"Found {liquid.Errors.Count} errors but only {count} shown.");

                    logger.LogWarning(sbMessage.ToString());
                }
            }
            catch (Exception ex)
            {
                try { logger.LogError(ex.Message, ex); } catch { }
                errorMessage = _dict[action];
                result.Success = false;
                result.ErrorMessage = errorMessage;
                result.ErrorAction = action;
            }

            result.Output = output;

            return result;
        }

        #endregion
    }
}