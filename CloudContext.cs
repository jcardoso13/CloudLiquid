using Azure.Storage.Blobs;
using CloudLiquid.Filters;
using CloudLiquid.ObjectModel;
using CloudLiquid.Tags;
using Scriban;
using Scriban.Runtime;
using Microsoft.Extensions.Logging;
using System.Text;
using Scriban.Functions;
using Scriban.Helpers;
using Scriban.Parsing;
using Scriban.Syntax;

namespace CloudLiquid.Core
{
    public class CloudContext : TemplateContext
    {

        private StringBuilder warningMessage = new();

        public string GetWarningMessage()
        {
            return warningMessage.ToString();
        }
        public void CheckVariableFound(ScriptVariable variable, bool found)
        {
            //ScriptVariable.Arguments is a special "magic" variable which is not always present so ignore this
            if (StrictVariables && !found && variable != ScriptVariable.Arguments)
            {
                warningMessage.AppendLine($"The variable '{variable}' was not found in the current context.");
            }
        }
    }
}