using Azure.Storage.Blobs;
using CloudLiquid.Filters;
using CloudLiquid.ObjectModel;
using CloudLiquid.Tags;
using Scriban;
using Scriban.Runtime;
using Microsoft.Extensions.Logging;
using System.Text;

namespace CloudLiquid.Core
{
    public class ScribanProcessor(ILogger logger, BlobContainerClient blobContainerClient)
    {
        #region Private Members

        private static readonly Dictionary<string, string> errorDictionary = new()
        {
            {"CloudLiquid_Start","Error Registering Tags and Templates"},
            {"Parsing_Liquid","Error parsing Liquid"},
            {"Rendering_Output","Error rendering output using DotLiquid Engine"},
            {"Checking_Output_For_Errors","InnerException found in the Output from Liquid Engine"}
        };

        private readonly BlobContainerClient blobContainerClient = blobContainerClient;
        private readonly ILogger logger = logger;

        private string action;
        private string errorMessage;

        #endregion

        #region Public Methods

        public void InitializeFunctions()
        {
            ScriptObject script= new ScriptObject();
            var context = new CloudContext();
            script.Import(typeof(DataFilters));
            context.PushGlobal(script);
        }

        public RunResult Run(ScriptObject input, string file)
        {
            RunResult result = new() { Success = true };

            action = "CloudLiquid_Start";

            logger.LogInformation("Starting CloudLiquidRuntime");

            string output = null;

            try
            {
                action = "Parsing_Liquid";

                Template scriban = Template.Parse(file);

                action = "Rendering_Output";

                var context = new CloudContext();
                context.PushGlobal(input);

                output = scriban.Render(context);

                if (scriban.HasErrors)
                {
                    logger.LogInformation("Errors Found:");
                    foreach(var error in scriban.Messages)
                    {
                        logger.LogWarning(error.ToString());
                    }
                    logger.LogWarning(context.GetWarningMessage());
                }
            }
            catch (Exception ex)
            {
                try 
                { 
                    logger.LogError(ex.Message, ex); 
                } catch { }

                errorMessage = $"{errorDictionary[action]}: {ex.Message}";
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