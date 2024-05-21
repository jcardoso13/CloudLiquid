using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace CloudLiquid.ContentFactory
{
    public class JsonContentWriter : IContentWriter
    {
        string _contentType;

        public JsonContentWriter(string contentType)
        {
            _contentType = contentType;
        }

        public StringContent CreateResponse(string output)
        {
            return new StringContent(output, Encoding.UTF8, _contentType);
        }
    }
}
