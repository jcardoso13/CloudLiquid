using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace CloudLiquid.ObjectModel
{ 
    public class CloudError
    {

        #region Constructors

        public CloudError()
        {
        }

        public CloudError(string error, string function, string action, string exceptionMessage)
        {
            this.Error = error;
            this.Function = function;
            this.Action = action;
            this.ExceptionMessage = exceptionMessage;
        }

        #endregion

        #region Public Properties

        public string Error { get; set; }

        public string Function { get; set; }

        public string Action { get; set; }

        public string ExceptionMessage { get; set; }

        #endregion

        #region Public Methods

        public string FormatMessage(string responseContentType)
        {
            string output = null;
            switch (responseContentType)
            {
                case "application/json":
                    {
                        output = JsonConvert.SerializeObject(this, Formatting.Indented);
                        break;
                    }
                case "application/xml":
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new(this.GetType());
                    {
                        using StringWriter textWriter = new();
                        xmlSerializer.Serialize(textWriter, this);
                        output = textWriter.ToString();
                        break;
                    }
                default:
                    {
                        output = this.ToString();
                        break;
                    }
            }

            return output;
        }

        public override string ToString()
        {
            return $"Action: {this.Action}\nFunction: {this.Function}\nError: {this.Error}\nException: {this.ExceptionMessage}";
        }

        #endregion
    }
}