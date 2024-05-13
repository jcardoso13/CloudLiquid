using DotLiquid;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CloudLiquid.ContentFactory
{
    public class Csv2ContentReader : IContentReader
    {
        public async Task<Hash> ParseRequestAsync(HttpContent content)
        {
            var stream = await content.ReadAsStreamAsync();

            var transformInput = new Dictionary<string, object>();


            List<object[]> csv = new List<object[]>();

            StreamReader sr = new StreamReader(stream);
            var aux = await sr.ReadToEndAsync();

            var aux2 = aux.Split("\n");

            foreach (var str in aux2)
            {
                csv.Add(str.Split(';'));
            }

            transformInput.Add("content", csv.ToArray<object>());

            return Hash.FromDictionary(transformInput);
        }
        public Hash  ParseString(string content)
        {
            var transformInput = new Dictionary<string, object>();
            //por aqui o replace

            List<object[]> csv = new List<object[]>();

            StreamReader sr = new StreamReader(content);

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                csv.Add(line.Split(';'));
            }
            transformInput.Add("content", csv.ToArray<object>());

            return Hash.FromDictionary(transformInput);
        }
    }

    public class CsvContentReader : IContentReader
    {

        public async Task<Hash> ParseRequestAsync(HttpContent content)
        {
            var stream = await content.ReadAsStreamAsync();
            var transformInput = new Dictionary<string, object>();
            //por aqui o replace

            List<object[]> csv = new List<object[]>();

            StreamReader sr = new StreamReader(stream);

            while (!sr.EndOfStream)
            {
                var line = await sr.ReadLineAsync();
                csv.Add(line.Split(','));
            }
            transformInput.Add("content", csv.ToArray<object>());

            return Hash.FromDictionary(transformInput);

        }

        public Hash  ParseString(string content)
        {
            var transformInput = new Dictionary<string, object>();
            //por aqui o replace

            List<object[]> csv = new List<object[]>();

            StreamReader sr = new StreamReader(content);

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                csv.Add(line.Split(','));
            }
            transformInput.Add("content", csv.ToArray<object>());

            return Hash.FromDictionary(transformInput);
        }


    }

    public class TSVContentReader : IContentReader
    {
        public async Task<Hash> ParseRequestAsync(HttpContent content)
        {
            var stream = await content.ReadAsStreamAsync();

            var transformInput = new Dictionary<string, object>();


            List<object[]> tsv = new List<object[]>();

            StreamReader sr = new StreamReader(stream);
            var aux = await sr.ReadToEndAsync();

            var aux2 = aux.Split("\n");

            string sep = "\t";

            foreach (var str in aux2)
            {
                tsv.Add(str.Split(sep.ToCharArray()));
            }

            transformInput.Add("content", tsv.ToArray<object>());

            return Hash.FromDictionary(transformInput);
        }

        public Hash  ParseString(string content)
        {
            var transformInput = new Dictionary<string, object>();


            List<object[]> tsv = new List<object[]>();

            StreamReader sr = new StreamReader(content);
            var aux = sr.ReadToEnd();

            var aux2 = aux.Split("\n");

            string sep = "\t";

            foreach (var str in aux2)
            {
                tsv.Add(str.Split(sep.ToCharArray()));
            }

            transformInput.Add("content", tsv.ToArray<object>());

            return Hash.FromDictionary(transformInput);
        }

    }
}
