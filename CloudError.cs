using System.Net.Http;
using System.Net;
using CloudLiquid.ContentFactory;
using Newtonsoft.Json;

namespace CloudLiquid
{    
    public class CloudError
	{
		public string Error = null;
		public DateTime TimeStamp; //DateTime.Now
		public string Function = null;
		public string Action = null;

		public HttpStatusCode Code;

		public string Exception=null;
		
		public CloudError()
		{
		}

		public CloudError(string _error, string _function,string _action, HttpStatusCode _code,string _exception)
		{
            Error=_error;
            TimeStamp= DateTime.Now;
            Function=_function;
            Action=_action;
            Code=_code;
            Exception=_exception;
		}

        public string FormatMessage(string responseContentType)
        {
            string output=null;
            switch(responseContentType)
			{
				case "application/json": output= JsonConvert.SerializeObject(this,Formatting.Indented);
				break;
				case "application/xml": System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(this.GetType());
				using(StringWriter textWriter = new StringWriter())
				{
					x.Serialize(textWriter, this);
					output=textWriter.ToString();
				}
				break;
				default: output = this.ToString();
				break;
			}
            return output;
        }

        public override string ToString()
        {
            return "Action:"+Action+"\nFunction:"+Function+"\nError:"+Error+"\nCode:"+ Code.ToString()+"\nException:"+Exception;
        }
	}
}