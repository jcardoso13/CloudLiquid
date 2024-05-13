using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudLiquid.ContentFactory
{
    public static class ContentFactory
    {
        public static IContentReader GetContentReader(string contentType)
        {
            switch (contentType)
            {
                case "application/xml":
                case "text/xml":
                    return new XmlContentReader();
                case "text/csv":
                    return new CsvContentReader();
                case "text/csv2":
                    return new Csv2ContentReader();
				case "text/tab-separated-values":
					return new TSVContentReader();
                default:
                    return new JsonContentReader();
            }
        }

        public static IContentWriter GetContentWriter(string contentType)
        {
            switch (contentType)
            {
                case "application/xml":
                case "text/xml":
                    return new XmlContentWriter(contentType);
                case "application/json":
                default:
                    return new JsonContentWriter(contentType);
                case "text/plain":
                    return new BasicContentWriter(contentType);
            }
        }
    }
}
