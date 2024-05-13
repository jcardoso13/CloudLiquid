using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CloudLiquid.ContentFactory
{
    public interface IContentReader
    {
        public Task<Hash> ParseRequestAsync(HttpContent content);
        public Hash       ParseString(string content);
    }
}
