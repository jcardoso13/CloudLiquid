using CloudLiquid.ContentFactory;
using DotLiquid;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text;
using System.Xml;

namespace CloudLiquid.Filters
{
    #region Public Methods
    public static class DataFilters
    {
        public static string Padleft(Context context, string input, int totalWidth, string padChar = " ")
        {
            return input.PadLeft(totalWidth, padChar[0]);
        }

        public static dynamic Secret(Context context, string input)
        {
            return Environment.GetEnvironmentVariable(input);
        }

        public static string Padright(Context context, string input, int totalWidth, string padChar = " ")
        {
            return input.PadRight(totalWidth, padChar[0]);
        }

        public static string Nullifnull(Context context, string input)
        {
            return string.IsNullOrEmpty(input) ? "null" : input;
        }

        public static double Parsedouble(Context context, string input)
        {
            return Double.Parse(input);
        }

        public static string Json(Context context, dynamic input)
        {
            return JsonConvert.SerializeObject(input, jsonSettings);
        }

        public static string Xml(Context context, dynamic input)
        {
            string jsonString = JsonConvert.SerializeObject(input, jsonSettings);
            XmlDocument doc = JsonConvert.DeserializeXmlNode(jsonString);
            var XmlString = doc.OuterXml;
            return XmlString;
        }

        public static string Json_nohtml(Context context, dynamic input)
        {
            return JsonConvert.SerializeObject(input, jsonNoHtmlSettings);
        }

        //example: "Approval_Stage": {{content.table.Approval_Stage | look_up: content.input.Approval_Stage }}
        // Search for the Object (secondObject) in the input JSON (data)
        public static object LookUp(Context context, dynamic data, string secondObject)
        {
            try
            {
                if (data[secondObject] != null)
                {
                    return data[secondObject];
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool LiquidContains(Context context, object data, string Obj)
        {
            if (data is Hash || data is Dictionary<string, object>)
            {
                Hash ndata;
                if (data is Dictionary<string, object> dictionary)
                {
                    ndata = Hash.FromDictionary(dictionary);
                }
                else
                {
                    ndata = (Hash)data;
                }

                return ndata.Contains(Obj);
            }
            else if (data is List<dynamic> ldata)
            {
                foreach (var l in ldata)
                {
                    if (l is String)
                    {
                        if (l.CompareTo(Obj) == 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        Hash ndata;
                        if (l is Dictionary<string, object> dictionary)
                        {
                            ndata = Hash.FromDictionary(dictionary);
                        }
                        else
                        {
                            ndata = (Hash)data;
                        }

                        Hash outp = Hash.FromDictionary(ndata);

                        if (outp.Contains(Obj))
                        {
                            return true;
                        }
                    }
                }
            }
            else if (data is string ndata)
            {
                return ndata.Contains(Obj);
            }
            else
            {
                return false;
            }

            return false;
        }

        public static string DataType(Context context, object data)
        {
            if (data is Hash || data is Dictionary<string, object>)
            {
                return "Hash";
            }
            else if (data is List<dynamic>)
            {
                return "List";
            }
            else if (data is String)
            {
                return "String";
            }
            else if (data is int)
            {
                return "Integer";
            }
            else if (data is bool)
            {
                return "Boolean";
            }
            return "null";
        }

        public static int Int(Context context, object data)
        {
            return Convert.ToInt32(data);
        }
        public static string String(Context context, object data)
        {
            return Convert.ToString(data);
        }
        public static Boolean IsLoop(Context context, Object data)
        {
            return (data is List<Object>);
        }

        public static Hash ClearNulls(Context context, dynamic data)
        {
            if (data == null)
            {
                return [];
            }

            var result = JsonConvert.SerializeObject(data,
                                                    new JsonSerializerSettings()
                                                    {
                                                        NullValueHandling = NullValueHandling.Ignore
                                                    });

            JToken jvar = JToken.Parse(result);

            var nresult = Convert.ToString(JsonContentReader.RemoveEmptyChildren(jvar));

            var requestJson = JsonConvert.DeserializeObject<IDictionary<string, object>>(nresult, new DictionaryConverter());

            return Hash.FromDictionary(requestJson);
        }

        public static Hash CreateHash(Context context, string key = null)
        {

            var transformInput = new Dictionary<string, object>();

            if (key != null)
            {
                transformInput.Add(key, null);
            }

            return Hash.FromDictionary(transformInput);
        }

        public static List<dynamic> CreateList(Context context, dynamic type = null)
        {
            var newA = new List<dynamic>();
            return newA;
        }
        public static List<dynamic> AddToList(Context context, List<dynamic> data, dynamic insert, bool unique = false, bool nullInsert = false)
        {
            if (insert != null || nullInsert == true)
            {
                if (unique && !data.Contains(insert))
                {
                    data.Add(insert);
                }
                else
                {
                    data.Add(insert);
                }
            }

            return data;
        }

        public static List<dynamic> RemoveFromList(Context context, List<dynamic> data, dynamic key)
        {
            if (data.Contains(key))
            {
                data.Remove(key);
            }

            return data;
        }
        public static List<dynamic> GetListFromHash(Context context, dynamic data, string key)
        {
            if (data == null)
            {
                return null;
            }

            List<dynamic> output = [];

            var d = data[key];
            output = (List<dynamic>)d;

            return output;
        }

        public static string Log(Context context, dynamic data)
        {
            Console.WriteLine(data);
            return null;
        }

        public static dynamic RemoveProperty(Context context, dynamic input, string key, int index = -1)
        {
            if (index == -1)
            {
                var data = input;
                if (data == null)
                {
                    return null;
                }

                if (string.IsNullOrEmpty(key))
                {
                    return Hash.FromDictionary(data);
                }

                Hash newData = [];

                newData = Hash.FromDictionary(data);

                if (newData.ContainsKey(key))
                {
                    newData.Remove(key);
                }

                return newData;
            }
            else
            {
                var data = input[index];

                if (data == null)
                {
                    return null;
                }

                if (string.IsNullOrEmpty(key))
                {
                    return Hash.FromDictionary(input);
                }

                List<dynamic> newData = [];

                newData = (List<dynamic>)input;

                if (newData[index].ContainsKey(key))
                {
                    newData[index].Remove(key);
                }

                return newData;

            }
        }
        public static dynamic AddProperty(Context context, dynamic input, string key, dynamic entry, int index = -1)
        {
            if (index == -1)
            {
                var data = input;
                if (data == null)
                {
                    return null;
                }

                if (string.IsNullOrEmpty(key))
                {
                    return Hash.FromDictionary(data);
                }

                Hash newData = [];

                newData = Hash.FromDictionary(data);

                newData[key] = entry;

                return Hash.FromDictionary(newData);
            }
            else
            {
                if (input == null)
                {
                    return null;
                }

                var data = input[index];

                if (string.IsNullOrEmpty(key))
                {
                    return Hash.FromDictionary(input);
                }

                List<dynamic> newData = [];

                newData = (List<dynamic>)input;

                newData[index][key] = entry;

                return newData;
            }
        }

        public static dynamic SetProperty(Context context, dynamic input, string key, dynamic entry, int index = -1)
        {
            if (index == -1)
            {
                var data = input;

                if (data == null)
                {
                    return null;
                }

                if (string.IsNullOrEmpty(key))
                {
                    return Hash.FromDictionary(data);
                }

                Hash newData = [];

                newData = Hash.FromDictionary(data);

                newData[key] = entry;

                return newData;
            }
            else
            {

                if (input == null)
                {
                    return null;
                }

                var data = input[index];

                if (string.IsNullOrEmpty(key))
                {
                    return Hash.FromDictionary(input);
                }

                List<dynamic> newData = [];

                newData = (List<dynamic>)input;

                newData[index][key] = entry;

                return newData;
            }
        }

        public static string JsonCurObject(Context context, dynamic input)
        {
            string newJ = JsonConvert.SerializeObject(input, jsonSettings);
            return newJ.Substring(1, newJ.Length - 2);
        }

        public static dynamic Coalesce(Context context, params dynamic[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] != null)
                {
                    return input[i];
                }
            }
            return null;
        }

        public static dynamic DecodeBase64(Context context, string encodedString)
        {
            byte[] data = Convert.FromBase64String(encodedString);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        public static string FormatDateTime(Context context, object timestamp, string format = "yyyy-MM-ddTHH:mm:ss.fffffffK", string locale = "en-us")
        {
            if (timestamp == null)
            {
                return null;
            }

            DateTime newDate = new();
            newDate = DateTime.Parse(timestamp.ToString());
            return newDate.ToString(format, new CultureInfo(locale));
        }

        #endregion

    #region Private Methods

    private static JsonSerializerSettings jsonSettings = new JsonSerializerSettings
    {
        StringEscapeHandling = StringEscapeHandling.Default
    };

    private static JsonSerializerSettings jsonNoHtmlSettings = new JsonSerializerSettings
    {
        StringEscapeHandling = StringEscapeHandling.EscapeHtml
    };

    #endregion
    }
}