using CloudLiquid.ContentFactory;
using DotLiquid;
using System.Globalization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;

namespace CloudLiquid.Filters
{
    #region Public Methods
    public static class DataFilters
    {
        /// <summary>Pads the left side of the input string with the specified character.</summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The input.</param>
        /// <param name="totalWidth">The number of characters in the resulting string.</param>
        /// <param name="padChar">The pad character.</param>
        /// <returns>The input string padded on the left to the specified total width.</returns>
        public static string Padleft(Context context, string input, int totalWidth, string padChar = " ")
        { 
            return input?.PadLeft(totalWidth, padChar[0]);    
        }

        /// <summary>Retrieves the value of an environment variable.</summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The input.</param>
        /// <returns>The value of the specified environment variable </returns>
        public static dynamic Secret(Context context, string input)
        {
            return Environment.GetEnvironmentVariable(input);
        }

        /// <summary> Pads the input string on the right with the specified padding character until the total width is reached</summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The input.</param>
        /// <param name="totalWidth">The number of characters in the resulting string.</param>
        /// <param name="padChar">The pad character.</param>
        /// <returns>The input string padded on the right to the specified total width.</returns>
        public static string Padright(Context context, string input, int totalWidth, string padChar = " ")
        {
            return input?.PadRight(totalWidth, padChar[0]);  
        }

        /// <summary>Returns the input string or "null" if the input is null or empty.</summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The input.</param>
        /// <returns> "null" if the input string is null or empty, otherwise the input string. </returns>
        public static string Nullifnull(Context context, string input)
        {
            return string.IsNullOrEmpty(input) ? "null" : input;
        }

        /// <summary>Parses the input string to a double.</summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The input.</param>
        /// <returns>The parsed double value. </returns>
        public static double Parsedouble(Context context, string input)
        {
            return Double.Parse(input);
        }

        /// <summary>Converts the input to a JSON string.</summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The input.</param>
        /// <param name="settings">Optional settings. If "nobrackets", the outer brackets are removed from the resulting JSON string. </param>
        /// <returns>The JSON string.</returns>
        public static string Json(Context context, dynamic input,string settings=null) 
        {
            string newJ = JsonSerializer.Serialize(input,jsonSettings);
            return settings == "nobrackets" ? newJ.Substring(1,newJ.Length -2): newJ ;
        }

        /// <summary>Converts the input object to an XML string.</summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The input.</param>
        /// <returns> The XML string.</returns>
        public static string Xml(Context context, dynamic input)
        {
            string jsonString = JsonSerializer.Serialize(input, jsonSettings);
            var doc = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(
            Encoding.ASCII.GetBytes(jsonString), new XmlDictionaryReaderQuotas()));
            var XmlString = doc.ToString();
            return XmlString;
        }
        /// <summary>Checks if the data contains the specified object.</summary>
        /// <param name="context">The context.</param>
        /// <param name="data">The data.</param>
        /// <param name="Obj">The object.</param>
        /// <returns>True if the data contains the object, false otherwise. </returns>
        public static bool LiquidContains(Context context, object data, string Obj)
        {
            if (data is Hash || data is Dictionary<string, dynamic>)
            {
                Hash ndata;
                if (data is Dictionary<string, dynamic> dictionary)
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
                        if (l is Dictionary<string, dynamic> dictionary)
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

        /// <summary>Returns the data type of the input data.</summary>
        /// <param name="context">The context.</param>
        /// <param name="data">The data to check.</param>
        /// <returns>Determines the data type of the specified object and returns its type as a string.</returns>
        public static string DataType(Context context, object data)
        {
            switch(data)
            {
                case Hash hash: return "Hash";
                case Dictionary<string,dynamic> dic: return "Hash";
                case string str: return "String";
                case int num: return "Integer";
                case bool b: return "Boolean";
                case double d: return "Double"; 
                default: return null;
            }
        }

        /// <summary>Converts the input data to an integer.</summary>
        /// <param name="context">The context.</param>
        /// <param name="data">The data to convert.</param>
        /// <returns>The converted integer.</returns>
        public static int ConvertToInt(Context context, object data)
        {
            return Convert.ToInt32(data);
        }
        /// <summary>Converts the specified object to a string. </summary>
        /// <param name="context">The context.</param>
        /// <param name="data">The data to be converted to a string.</param>
        /// <returns>The string representation of the specified object. </returns>
        public static string String(Context context, object data)
        {
            return Convert.ToString(data);
        }
        /// <summary>Determines whether the specified object is a list.</summary>
        /// <param name="context">The context.</param>
        /// <param name="data">The data to be checked.</param>
        /// <returns> True if the specified context is loop, false otherwise.</returns>
        public static Boolean IsLoop(Context context, Object data)
        {
            return data is List<dynamic>;
        }

        /// <summary> Removes null or empty entries from the specified dictionary.</summary>
        /// <param name="context">The context.</param>
        /// <param name="data">The dictionary to be processed.</param>
        /// <returns>A new dictionary with null or empty entries removed if the input is a dictionary; 
        /// otherwise, returns an empty dictionary if the input is null or not a dictionary.</returns>
        public static Dictionary<string, dynamic> ClearNulls(Context context, dynamic data)
        {
            if (data == null) return [];
            if (data is IDictionary<string, dynamic>)
                return (Dictionary<string, dynamic>)DictionaryFactory.RemoveEmptyChildren((IDictionary<string, dynamic>)data);

            return [];
        }

        /// <summary> Creates a new hash and optionally adds a specified key with a null value. </summary>
        /// <param name="context">The context.</param>
        /// <param name="key">An optional key to be added to the dictionary with a null value.</param>
        /// <returns>A new dictionary with the specified key and null value if the key is provided; 
        /// otherwise, an empty dictionary.</returns>
        public static Dictionary<string, dynamic> CreateHash(Context context, string key = null)
        {
            var transformInput = new Dictionary<string, dynamic>();

            if (key != null)
            {
                transformInput.Add(key, null);
            }

            return transformInput;
        }

        /// <summary>Creates a new list with an optional type specification.</summary>
        /// <param name="context">The context.</param>
        /// <param name="type">An optional parameter specifying the type of elements in the list.</param>
        /// <returns>A new empty list of dynamic elements.</returns>
        public static List<dynamic> CreateList(Context context, dynamic type = null)
        {
            var newA = new List<dynamic>();
            return newA;
        }
        /// <summary>Adds an item to the specified list with options for unique and null insertion.</summary>
        /// <param name="context">The context.</param>
        /// <param name="data">The list to which the item will be added.</param>
        /// <param name="insert">TThe item to be added to the list.</param>
        /// <param name="unique">A boolean indicating whether the item should only be added if it is not already in the list.</param>
        /// <param name="nullInsert">A boolean indicating whether null values are allow to be inserted.</param>
        /// <returns>The updated list with the item added if conditions are met.</returns>
        public static List<dynamic> AddToList(Context context, List<dynamic> data, dynamic insert, bool unique = false, bool nullInsert = false)
        {
            if (nullInsert || insert != null)
            {
                if (unique == false || (unique && !data.Contains(insert)))
                {
                    data.Add(insert);
                }
            }
            return data;
        }

        /// <summary>Removes the specified item from the list. </summary>
        /// <param name="context">The context.</param>
        /// <param name="data">The list from which the item will be removed.</param>
        /// <param name="key">The item to be removed from the list.</param>
        /// <returns>The updated list with the item removed.</returns>
        public static List<dynamic> RemoveFromList(Context context, List<dynamic> data, dynamic key)
        {
            data.Remove(key);
            return data;
        }
        /// <summary>Retrieves a list from a hash based on the specified key.</summary>
        /// <param name="context">The context.</param>
        /// <param name="data">The hash from which the list will be retrieved.</param>
        /// <param name="key">The key used to access the list within the hash.</param>
        /// <returns> The list associated with the specified key in the hash or null 
        /// if the data parameter is null.</returns>
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

        /// <summary>Writes the specified data to the console as a log entry.</summary>
        /// <param name="context">The context.</param>
        /// <param name="data">The data to be logged.</param>
        /// <returns>Always returns null. </returns>
        public static string Log(Context context, dynamic data)
        {
            Console.WriteLine(data);
            return null;
        }

        /// <summary>Removes a property from the specified object or from an object at the specified index within a list. </summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The object or list from which the property will be removed.</param>
        /// <param name="key">The key of the property to be removed.</param>
        /// <param name="index">The optional index of the object within a list. Default value is -1.</param>
        /// <returns>If the input object is null, returns null or if the key is null or empty, returns the input object.
        /// If index is -1 returns a new object with the specified property removed.
        /// If index is not -1 returns a new list with the property removed from the object at the specified index.</returns>
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
                    return data;
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
        /// <summary>Adds a property to the specified object or to an object at the specified index within a list. </summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The object or list to which the property will be added.</param>
        /// <param name="key">The key of the property to be added</param>
        /// <param name="entry">The value of the property to be added.</param>
        /// <param name="index">The optional index of the object within a list. Default value is -1. </param>
        /// <returns> If the input object is null, returns null or if the key is null or empty, returns the input object.
        /// If index is -1 returns a new object with the specified property added.
        /// If index is not -1 returns a new list with the property added to the object at the specified index.</returns>
        public static dynamic AddProperty(Context context, dynamic input, string key, dynamic entry, int index = -1) //mudar addproperty para dar erro quando já existe
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

                if (!newData.ContainsKey(key))
                {
                    newData[key] = entry;
                }
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
                if (!(newData[index].ContainsKey(key)))
                {
                    newData[index][key] = entry;
                }

                return newData;
            }
        }

        /// <summary>Sets the value of a property in the specified object or in an object at the specified index within a list. </summary>
        /// <param name="context">The context.</param>
        /// <param name="input">The object or list in which the property value will be set.</param>
        /// <param name="key">The key of the property to be set. </param>
        /// <param name="entry">The value to set for the property.</param>
        /// <param name="index">The optional index of the object within a list. Default value is -1.</param>
        /// <returns> If the input object is null, returns null or if the key is null or empty, returns the input object.
        /// If index is -1 returns a object with the property value set.
        /// If index is not -1 returns a list with the property value set to the object at the specified index.</returns>
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
                    return data;
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
                    return input;
                }

                List<dynamic> newData = [];

                newData = (List<dynamic>)input;

                newData[index][key] = entry;

                return newData;
            }
        }
        /// <summary>Returns the first non-null value from the provided list of dynamic inputs. </summary>
        /// <param name="context">The context.</param>
        /// <param name="input">A list of dynamic inputs from which to select the first non-null value.</param>
        /// <returns> The first non-null value from the provided list of dynamic inputs, or null if all inputs are null.</returns>
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

        /// <summary>Decodes a Base64 encoded string into its original UTF-8 encoded string representation. </summary>
        /// <param name="context">The context.</param>
        /// <param name="encodedString">The encoded string.</param>
        /// <returns>The original UTF-8 encoded string represented by the Base64 encoded string.</returns>
        public static dynamic DecodeBase64(Context context, string encodedString)
        {
            byte[] data = Convert.FromBase64String(encodedString);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        /// <summary>Formats a timestamp object into a string representation using the specified format and locale. </summary>
        /// <param name="context">The context.</param>
        /// <param name="timestamp">The timestamp object to be formatted.</param>
        /// <param name="format">The format string specifying the format. Default value is "yyyy-MM-ddTHH:mm:ss.fffffffK".</param>
        /// <param name="locale">The locale.</param>
        /// <returns>A string representation of the format timestamp object according to the specified format and locale.
        /// Returns null if the timestamp object is null.</returns>
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

        private static JsonSerializerOptions jsonSettings = new JsonSerializerOptions{
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
        AllowTrailingCommas = true,
        Converters = {new DictionaryStringObjectJsonConverter()}
    };

    private static JsonSerializerOptions jsonnohtmlSettings = new JsonSerializerOptions{
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Default,
        WriteIndented = true,
        AllowTrailingCommas = true,
        Converters = {new DictionaryStringObjectJsonConverter()}
    };

    #endregion
    }
}