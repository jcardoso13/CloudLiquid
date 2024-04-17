using DotLiquid;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CloudLiquid
{
    public static class CloudLiquid
    {

        public static BlobContainerClient FileSystem;
        public static ILogger log;
        public static string sqlConnection;

        public static string output;

        public static string action;

        public static string message;

        internal static Dictionary<string,string> _dict = new Dictionary<string, string>{
            {"CloudLiquid_Start","Error Registering Tags and Templates"},
            {"Parsing_Liquid","Error parsing Liquid"},
            {"Rendering_Output","Error rendering output using DotLiquid Engine"},
            {"Checking_Output_For_Errors","InnerException found in the Output from Liquid Engine"}
        };

        static CloudLiquid()
        {
            // Register Filters
            Template.RegisterTag<DataTags.IncludeLocal>("include_local");
            Template.RegisterTag<AzureTags.IncludeAzure>("include_azure");
            Template.RegisterTag<DataTags.CaptureJSON>("capturejson");
            Template.RegisterTag<DataTags.CaptureJSON>("capture_json");
            Template.RegisterTag<DataTags.CaptureXML>("capture_xml");
            Template.RegisterFilter(typeof(DataFilters));
        }

        public static string Run(Hash input, string file)
        {
            action="CloudLiquid_Start";
            log.LogInformation("Starting CloudLiquidRuntime");
            // Set AzureTags Parameters
            AzureTags.FileSystem = FileSystem;
            AzureTags.log = log;
            try
            {
                action="Parsing_Liquid";
                Template liquid = Template.Parse(file);
                action="Rendering_Output";
                output = liquid.Render(input);
                if (liquid.Errors != null && liquid.Errors.Count > 0)
                {
                    log.LogInformation("Errors Found:");
                    string concat_message = "";
                    int count = liquid.Errors.Count > 5 ? 5 : liquid.Errors.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (liquid.Errors[i].InnerException != null)
                        {
                            action="Checking_Output_For_Errors";
                            throw new Exception(liquid.Errors[i].Message);
                        }
                        else
                        {
                            concat_message += "Warning rendering Liquid liquid:" + liquid.Errors[i].Message + "\n";
                        }
                    }
                    log.LogInformation("Found "+liquid.Errors.Count+" errors but only "+count+" shown");
                    log.LogInformation(concat_message);
                }
            }
            catch (Exception ex)
            {
                try { log.LogError(ex.Message, ex); } catch { }
                message = _dict[action];
                throw new Exception(ex.Message);
            }
            return output;
        }
    }
}