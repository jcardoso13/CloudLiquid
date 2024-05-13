using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CloudLiquid.ContentFactory
{
    public class XmlContentWriter : IContentWriter
    {
        string _contentType;

        public XmlContentWriter(string contentType)
        {
            _contentType = contentType;
        }

        public StringContent CreateResponse(string output)
        {

            var xDoc = XDocument.Parse(output);
            var XmlString = xDoc.ToString();

            return new StringContent(XmlString, Encoding.UTF8, _contentType);
        }
    }
}
