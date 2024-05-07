using Newtonsoft.Json;
using System.Net;

namespace CloudLiquid
{
    public class CloudError
	{
        #region Private Members

        private readonly string error = null;
		private readonly string function = null;
		private readonly string action = null;
		private readonly HttpStatusCode httpStatusCode;
        private readonly string exceptionMessage = null;

        #endregion

        #region Constructors

        public CloudError()
		{
		}

        public CloudError(string error, string function, string action, HttpStatusCode httpStatusCode, string exceptionMessage)
		{
            this.error = error;
            this.function = function;
            this.action = action;
            this.httpStatusCode = httpStatusCode;
            this.exceptionMessage = exceptionMessage;
		}

        #endregion

        #region Public Methods

        public string FormatMessage(string responseContentType)
        {
            string output = null;
            switch(responseContentType)
			{
				case "application/json":
                    {
                        output = JsonConvert.SerializeObject(this, Formatting.Indented);
                        break;
                    }
				case "application/xml": System.Xml.Serialization.XmlSerializer xmlSerializer = new(this.GetType());
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
            return $"Action: {action}\nFunction: {function}\nError: {error}\nHttp Status Code: {httpStatusCode}\nException: {exceptionMessage}";
        }

        #endregion
    }
}