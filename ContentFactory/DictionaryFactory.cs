
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CloudLiquid.ContentFactory
{

    public class DictionaryFactory
    {

        public static dynamic RemoveEmptyChildren(dynamic token)
        {
            Dictionary<string,dynamic> dictionary=null;
            List<dynamic> list=null;

            if(token is IDictionary)
            {
                dictionary = (Dictionary<string, dynamic>)token;
                foreach (var kvp in dictionary)
                {
                    if (IsEmpty(kvp.Value))
                    {
                      dictionary.Remove(kvp.Key);
                    }
                    else if (kvp.Value is IDictionary || kvp.Value is IList)
                    {
                        RemoveEmptyChildren(kvp.Value);
                    }
                }
                return dictionary;
            }
            else if(token is IList)
            {
                list = (List<dynamic>)token;
                foreach (var kvp in list)
                {
                    if (IsEmpty(kvp))
                    {
                        list.Remove(kvp);
                    }
                    else if (kvp is IDictionary || kvp is IList)
                    {
                        RemoveEmptyChildren(kvp);
                    }
                }
                return list;
            }
            return null;
        }

        private static bool IsEmpty(object obj)
        {
            if (obj == null) return true;
            if (obj is string str && string.IsNullOrEmpty(str)) return true;
            if (obj is ICollection collection && collection.Count == 0) return true;
            if (obj is IDictionary dict && dict.Count == 0) return true;
            if (obj is IList l && l.Count == 0) return true;
            
            return false;
        }

    }

}