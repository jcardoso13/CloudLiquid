using Azure;
using CloudLiquid.Filters;
using DotLiquid;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CloudLiquid.Tests
{
    public class DataFiltersTests
    {
        [Fact]
        public void TestNullifnull()
        {
            Assert.Equal("null", DataFilters.Nullifnull(null, null));
            Assert.Equal("null", DataFilters.Nullifnull(null, String.Empty));
            Assert.NotEqual("null", DataFilters.Nullifnull(null, "TEST"));
        }
        [Theory]
        [InlineData("test", 10, "*", "******test")]
        [InlineData("test", 4, "*", "test")]
        [InlineData("test", 2, "*", "test")]
        [InlineData("", 5, "*", "*****")]
        [InlineData(null, 5, "*", null)]
        public void TestPadleft(string input, int totalWidth, string padChar, string expected)
        {
            string result = DataFilters.Padleft(null, input, totalWidth, padChar);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("test", 10, "*", "test******")]
        [InlineData("test", 4, "*", "test")]
        [InlineData("test", 2, "*", "test")]
        [InlineData("", 5, "*", "*****")]
        [InlineData(null, 5, "*", null)]
        public void TestPadright(string input, int totalWidth, string padChar, string expected)
        {
            string result = DataFilters.Padright(null, input, totalWidth, padChar);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestParseDouble()
        {
            Assert.Equal(128.9, DataFilters.Parsedouble(null, "128,9"));
            Assert.Equal(-128.9, DataFilters.Parsedouble(null, "-128,9"));
            Assert.Equal(0, DataFilters.Parsedouble(null, "0"));
        }

        [Theory]
        [InlineData("test")]
        [InlineData("")]
        public void TestParseDouble_InvalidFormat(string input)
        {
            Assert.Throws<FormatException>(() => DataFilters.Parsedouble(null, input));
        }

        [Fact]
        public void TestDouble_NullInputThrowsArgumentNullException()
        {
            string input = null;
            Assert.Throws<ArgumentNullException>(() => DataFilters.Parsedouble(null, input));
        }
        [Fact]
        public void TestJson_Dictionary() // fazer para int, bool, lista, dicionario/JsonObjects,testar settings nobrackets e testar decimais
        {
            var data = new Dictionary<string, dynamic> { { "key1", "value1" }, { "key2", "value2" } };
            var result = DataFilters.Json(null, data);
            Assert.Equal("{\r\n  \"key1\": \"value1\",\r\n  \"key2\": \"value2\"\r\n}", result);
        }
        [Fact]
        public void TestJson_WithoutBrackets() 
        {
            var data = new Dictionary<string, dynamic> { { "key1", "value1" }, { "key2", "value2" } };
            var result = DataFilters.Json(null, data, "nobrackets");
            Assert.Equal("\r\n  \"key1\": \"value1\",\r\n  \"key2\": \"value2\"\r\n", result);
        }
        [Fact]
        public void TestJson_Int()
        {
            var input = 123;
            string result = DataFilters.Json(null, input);

            Assert.Equal("123", result);
        }
        [Fact]
        public void TestJson_Bool()
        {
            var input = true;
            string result = DataFilters.Json(null, input);

            Assert.Equal("true", result);
        }
        [Fact]
        public void TestJson_List()
        {
            var input = new List<int> { 1, 2, 3 };
            string result = DataFilters.Json(null, input);

            Assert.Equal("[\r\n  1,\r\n  2,\r\n  3\r\n]", result);
        }
        [Fact]
        public void TestJson_Object()
        {
            var input = new { Name = "Maria", Age = 25 };
            string result = DataFilters.Json(null, input);

            Assert.Equal("{\r\n  \"Name\": \"Maria\",\r\n  \"Age\": 25\r\n}", result);
        }
        [Fact]
        public void TestJson_Decimal()
        {
            var input = 123.45;
            string result = DataFilters.Json(null, input);

            Assert.Equal("123.45", result);
        }
        [Fact]
        public void TestJson_DictionaryAndList()
        {
            var data = new Dictionary<string, dynamic>
        {
            { "key1", new List<dynamic> { 1, 2, 3 } },
            { "key2", new List<dynamic> { "a", "b", "c" } }
        };
            string result = DataFilters.Json(null, data);

            Assert.Equal("{\r\n  \"key1\": [\r\n    1,\r\n    2,\r\n    3\r\n  ],\r\n  \"key2\": [\r\n    \"a\",\r\n    \"b\",\r\n    \"c\"\r\n  ]\r\n}", result);
        }

        [Fact]
        public void TestXml_Dictionary() // fazer para int bool, lista, dicionario/JsonObjects e testar decimais
        { 
            var input=new Dictionary<string, dynamic> { { "key1", "value1" }, { "key2", "value2" } };
           
            var result = DataFilters.Xml(null, input);
            
            Assert.Equal("<root type=\"object\">\r\n  <key1 type=\"string\">value1</key1>\r\n  <key2 type=\"string\">value2</key2>\r\n</root>", result);

        }
        [Fact]
        public void TestXml_Int()
        {
            var input = 123;
            string result = DataFilters.Xml(null, input);

            Assert.Equal("<root type=\"number\">123</root>", result);
        }
        [Fact]
        public void TestXml_Bool()
        {
            var input = true;
            string result = DataFilters.Xml(null, input);

            Assert.Equal("<root type=\"boolean\">true</root>", result);
        }
        [Fact]
        public void TestXml_List()
        {
            var input = new List<int> { 1, 2, 3 };
            string result = DataFilters.Xml(null, input);

            Assert.Equal("<root type=\"array\">\r\n  <item type=\"number\">1</item>\r\n  <item type=\"number\">2</item>\r\n  <item type=\"number\">3</item>\r\n</root>", result);
        }
        [Fact]
        public void TestXml_Object()
        {
            var input = new { Name = "Maria", Age = 25 };
            string result = DataFilters.Xml(null, input);

            Assert.Equal("<root type=\"object\">\r\n  <Name type=\"string\">Maria</Name>\r\n  <Age type=\"number\">25</Age>\r\n</root>", result);
        }
        [Fact]
        public void TestXml_Decimal()
        {
            var input = 123.45;
            string result = DataFilters.Xml(null, input);

            Assert.Equal("<root type=\"number\">123.45</root>", result);
        }
        [Fact]
        public void TestXml_DictionaryAndList()
        {
            var data = new Dictionary<string, dynamic>
        {
            { "key1", new List<dynamic> { 1, 2, 3 } },
            { "key2", new List<dynamic> { "a", "b", "c" } }
        };
            string result = DataFilters.Xml(null, data);

            Assert.Equal("<root type=\"object\">\r\n  <key1 type=\"array\">\r\n    <item type=\"number\">1</item>\r\n    <item type=\"number\">2</item>\r\n    <item type=\"number\">3</item>\r\n  </key1>\r\n  <key2 type=\"array\">\r\n    <item type=\"string\">a</item>\r\n    <item type=\"string\">b</item>\r\n    <item type=\"string\">c</item>\r\n  </key2>\r\n</root>", result);
        }

        [Fact]
        public void TestSecret() // when running Functions or in Bash, the runner can set variables 
        {
            Environment.SetEnvironmentVariable("key1", "value1");
            
            Assert.Equal("value1",Environment.GetEnvironmentVariable("key1"));
        }

        [Fact]
        public void TestLiquidContains_DictionaryContainKey()
        {
            var data = new Dictionary<string, dynamic> { { "key1", "value1" }, { "key2", "value2" } };
            string obj = "key1";

            var result = DataFilters.LiquidContains(null, data, obj);

            Assert.True(result);
        }

        [Fact]
        public void TestLiquidContains_DictionaryDoesNotContainKey()
        {
            var data = new Dictionary<string, dynamic> { { "key1", "value1" }, { "key2", "value2" } };
            string obj = "key3";

            var result = DataFilters.LiquidContains(null, data, obj);

            Assert.False(result);
        }

        [Fact]
        public void TestLiquidContains_ListContainString()
        {
            var data = new List<dynamic> { "value1", "value2", "value3" };
            string obj = "value2";

            var result = DataFilters.LiquidContains(null, data, obj);

            Assert.True(result);
        }

        [Fact]
        public void TestLiquidContains_ListDoesNotContainString()
        {
            var data = new List<dynamic> { "value1", "value2", "value3" };
            string obj = "value4";

            var result = DataFilters.LiquidContains(null, data, obj);

            Assert.False(result);
        }

        [Fact]
        public void TestLiquidContains_String()
        {
            string data = "Hello, world!";
            string obj = "world";

            var result = DataFilters.LiquidContains(null, data, obj);

            Assert.True(result);
        }

        [Fact]
        public void TestInt()
        {
            Assert.Equal(42, DataFilters.ConvertToInt(null, "42"));
            Assert.Equal(0, DataFilters.ConvertToInt(null, null)); 
            Assert.Equal(42, DataFilters.ConvertToInt(null, 42.2));
        }

        [Fact]
        public void TestInt_DoubleToInt()
        {
            object data = 42.5;

            int result = DataFilters.ConvertToInt(null, data);
            Assert.Equal(42, result);
        }
        [Fact]
        public void TestInt_InvalidString()
        {
            object data = "abc";
            Assert.Throws<FormatException>(() => DataFilters.ConvertToInt(null, data));
        }
        [Fact]
        public void TestDataType_Hash()
        {
            var data = new Hash();
            var result = DataFilters.DataType(null, data);

            Assert.Equal("Hash", result);
        }
        [Fact]
        public void TestDataType_Dictionary()
        {
            var data = new Dictionary<string, dynamic> { { "Key", "Value" } };
            string result = DataFilters.DataType(null, data);

            Assert.Equal("Hash", result);
        }

        [Fact]
        public void TestDataType_String()
        {
            Assert.Equal("String", DataFilters.DataType(null, "test"));
        }

        [Fact]
        public void TestDataType_Integer()
        {
            Assert.Equal("Integer", DataFilters.DataType(null, 23));
        }


        [Fact]
        public void TestDataType_Boolean()
        {
            Assert.Equal("Boolean", DataFilters.DataType(null, true));
        }

        [Fact]
        public void TestDataType_ReturnNull()
        {
            object data = null;
            string result = DataFilters.DataType(null, data);

            Assert.Null(result);
        }
        [Fact]
        public void TestString_IntegerToString()
        {
            var data = 1234;
            var result = DataFilters.String(null, data);

            Assert.Equal("1234", result);
        }

        [Fact]
        public void TestString_BooleanToString()
        {
            Assert.Equal("True", DataFilters.String(null, true));
        }

        [Fact]
        public void TestString_WithString()
        {
            Assert.Equal("Hello World!", DataFilters.String(null, "Hello World!"));
        }
        [Fact]
        public void TestString_NullToString()
        {
            Assert.Equal("", DataFilters.String(null, null));
        }
        [Fact]
        public void TestIsLoop_ListDynamic()
        {
            var data = new List<dynamic> { 1, 2, 3 };
            bool result = DataFilters.IsLoop(null, data);

            Assert.True(result);
        }

        [Fact]
        public void TestIsLoop_ListInt()
        {
            var data = new List<int> { 1, 2, 3 };
            bool result = DataFilters.IsLoop(null, data);

            Assert.False(result);
        }

        [Fact]
        public void TestIsLoop_WithString()
        {
            var data = "abc";
            bool result = DataFilters.IsLoop(null, data);

            Assert.False(result);
        }

        [Fact]
        public void TestIsLoop_WithNull()
        {
            object data = null;
            bool result = DataFilters.IsLoop(null, data);

            Assert.False(result);
        }

        [Fact]
        public void TestIsLoop_WithHash() 
        {
            var data = new Hash();
            bool result = DataFilters.IsLoop(null, data);

            Assert.False(result);
        }

        [Fact]
        public void TestClearNulls_NullData()
        {
            dynamic data = null;
            var result = DataFilters.ClearNulls(null, data);

            Assert.IsType<Dictionary<string, dynamic>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public void TestClearNulls_NoDictionary()
        {
            dynamic data = "hello world";
            var result = DataFilters.ClearNulls(null, data);

            Assert.IsType<Dictionary<string, dynamic>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public void TestClearNulls_DictionaryContainNulls()
        {
            var data = new Dictionary<string, dynamic>
            {
                { "Key1", "Value1" },
                { "Key2", null },
                { "Key3", "Value3" }
            };
            var result = DataFilters.ClearNulls(null, data);

            Assert.Equal(2, result.Count);
            Assert.True(result.ContainsKey("Key1"));
            Assert.True(result.ContainsKey("Key3"));
            Assert.False(result.ContainsKey("Key2"));
        }
        [Fact]
        public void TestCreateHash_WithoutKey()
        {
            Assert.Empty(DataFilters.CreateHash(null));
        }

        [Fact]
        public void TestCreateHash_WithKey()
        {
            string key = "test";
            var result = DataFilters.CreateHash(null, key);

            Assert.True(result.ContainsKey(key));
            Assert.Null(result[key]);
            Assert.IsType<Dictionary<string, dynamic>>(result);
        }
        [Fact]
        public void TestCreateHash_WithNullKey()
        {
            string key = null;
            var result = DataFilters.CreateHash(null, key);

            Assert.Empty(result);
        }
        [Fact]
        public void TestCreateList_WithoutType()
        {
            Assert.NotNull(DataFilters.CreateList(null));
            Assert.IsType<List<dynamic>>(DataFilters.CreateList(null));
            Assert.Empty(DataFilters.CreateList(null));
        }
        [Fact]
        public void TestCreateList_WithType()
        {
            dynamic type = "abc"; //confirmar o type
            var result = DataFilters.CreateList(null, type);

            Assert.IsType<List<dynamic>>(result);
            Assert.Empty(result);
        }
        [Fact]
        public void TestAddToList_NullInsertAndNullInsertTrue()
        {
            var data = new List<dynamic>() { "item1", "item2" };
            dynamic insert = null;
            bool nullinsert = true;
            bool unique = true;

            var result = DataFilters.AddToList(null, data, insert, unique, nullinsert);

            Assert.Equal(3, result.Count);
            Assert.Contains(insert, result);
        }

        [Fact]
        public void TestAddToList_NullInsertAndNullInsertFalse()
        {
            var data = new List<dynamic>() { "item1", "item2" };
            dynamic insert = null;

            var result = DataFilters.AddToList(null, data, insert);

            Assert.DoesNotContain(null, result);
            Assert.Equal(data, result);
            Assert.Equal(2, result.Count);
        }
        [Fact]
        public void TestAddToList_UniqueFalseAndInsert()
        {
            var data = new List<dynamic>() { "item1", "item2" };
            var insert = "item3";

            var result = DataFilters.AddToList(null, data, insert);
            Assert.Equal(3, result.Count);
            Assert.Contains(insert, result);

        }
        [Fact]
        public void TestAddToList_UniqueTrueAndInsert()
        {
            var data = new List<dynamic>() { "item1", "item2" };
            var insert = "item3";
            bool unique = true;

            var result = DataFilters.AddToList(null, data, insert, unique);

            Assert.Contains(insert, result);
        }

        [Fact]
        public void TestAddToList_UniqueTrueAndInsertAlreadyInList()
        {
            var data = new List<dynamic>() { "item1", "item2" };
            var insert = "item1";
            bool unique = true;

            var result = DataFilters.AddToList(null, data, insert, unique);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void TestRemoveFromList_ElementExist()
        {
            var data = new List<dynamic>() { "item1", "item2", "item3" };
            var key = "item2";

            var result = DataFilters.RemoveFromList(null, data, key);

            Assert.DoesNotContain(key, result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void TestRemoveFromList_ElementDoesNotExist()
        {
            var data = new List<dynamic>() { "item1", "item2", "item3" };
            var key = "item4";

            var result = DataFilters.RemoveFromList(null, data, key);

            Assert.Equal(data, result);
            Assert.Equal(3, result.Count);

        }
        [Fact]
        public void TestGetListFromHash_DataIsNull()
        {
            dynamic data = null;
            string key = "abc";

            var result = DataFilters.GetListFromHash(null, data, key);

            Assert.Null(result);
        }
        [Fact]
        public void TestGetListFromHash_KeyExistInHash()
        {
            var data = new Dictionary<string, dynamic>
        {
            { "key1", new List<dynamic> { 1, 2, 3 } },
            { "key2", new List<dynamic> { "a", "b", "c" } }
        };
            string key = "key2";

            var result = DataFilters.GetListFromHash(null, data, key);

            Assert.IsType<List<dynamic>>(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void TestLog() // confirmar
        {
            dynamic data = "Hello World";
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            DataFilters.Log(null, data);

            var resultado = stringWriter.ToString().Trim();
            Assert.Equal(data, resultado);

        }
        [Fact]
        public void TestRemoveProperty_NullInput()
        {
            dynamic input = null;
            string key = "testKey";

            var result = DataFilters.RemoveProperty(null, input, key);

            Assert.Null(result);
        }
        [Fact]
        public void TestRemoveProperty_EmptyKey()
        {
            dynamic input = new Dictionary<string, object> { { "testKey", "testValue" } };
            string key = "";

            var result = DataFilters.RemoveProperty(null, input, key);

            Assert.Equal(input, result);
        }
        [Fact]
        public void TestRemoveProperty_RemovesPropertyIfExists()
        {
            dynamic input = new Dictionary<string, object> { { "testKey", "testValue" }, { "anotherKey", "anotherValue" } };
            string key = "testKey";

            var result = DataFilters.RemoveProperty(null, input, key);

            Assert.False(result.ContainsKey(key));
            Assert.True(result.ContainsKey("anotherKey"));
            Assert.Equal(1, result.Count);
        }
        [Fact]
        public void TestRemoveProperty_IndexSpecified()
        {
            dynamic input = new List<dynamic> { new Dictionary<string, object> { { "testKey", "testValue" } }, new Dictionary<string, object> { { "anotherKey", "anotherValue" } } };
            string key = "testKey";
            int index = 0;

            var result = DataFilters.RemoveProperty(null, input, key, index);

            Assert.False(result[index].ContainsKey(key));
            Assert.True(result[1].ContainsKey("anotherKey"));
        }
        [Fact]
        public void TestAddProperty_NullInput()
        {
            dynamic input = null;
            string key = "testKey";
            dynamic entry = "testValue";

            var result = DataFilters.AddProperty(null, input, key, entry);

            Assert.Null(result);
        }
        [Fact]
        public void TestAddProperty_EmptyKey()
        {
            dynamic input = new Dictionary<string, object> { { "existingKey", "existingValue" } };
            string key = "";
            dynamic entry = "testValue";

            var result = DataFilters.AddProperty(null, input, key, entry);

            Assert.Equal(input, result);
        }
        [Fact]
        public void TestAddProperty_NewProperty()
        {
            dynamic input = new Dictionary<string, object> { { "existingKey", "existingValue" } };
            string key = "newKey";
            dynamic entry = "newValue";

            var result = DataFilters.AddProperty(null, input, key, entry);

            Assert.Equal("newValue", result[key]);
            Assert.Equal(2, result.Count);
        }
        [Fact]
        public void TestAddProperty_IndexSpecified()
        {
            dynamic input = new List<dynamic>{
                new Dictionary<string, object> { { "existingKey1", "existingValue1" } },
                new Dictionary<string, object> { { "existingKey2", "existingValue2" } }};
            string key = "newKey";
            dynamic entry = "newValue";
            int index = 1;

            var result = DataFilters.AddProperty(null, input, key, entry, index);

            Assert.Equal("newValue", result[index][key]);
        }

        [Fact]
        public void TestSetProperty_NullInput()
        {
            var result = DataFilters.SetProperty(null, null, "key", "entry");
            Assert.Null(result);
        }
        [Fact]
        public void TestSetProperty_EmptyKey()
        {
            dynamic input = new Dictionary<string, object> { { "existingKey", "existingValue" } };
            string key = "";
            dynamic entry = "testValue";

            var result = DataFilters.SetProperty(null, input, key, entry);

            Assert.Equal(input, result);
        }
        [Fact]
        public void TestSetProperty_UpdateProperty()
        {
            dynamic input = new Dictionary<string, object> { { "existingKey", "existingValue" } };
            string key = "existingKey";
            dynamic entry = "newValue";

            var result = DataFilters.SetProperty(null, input, key, entry);

            Assert.Equal("newValue", result[key]);
            Assert.Equal(1, result.Count);
        }
        [Fact]
        public void TestSetProperty_NewProperty() // o setproperty pode adicionar?
        {
            dynamic input = new Dictionary<string, object> { { "existingKey", "existingValue" } };
            string key = "newKey";
            dynamic entry = "newValue";

            var result = DataFilters.SetProperty(null, input, key, entry);

            Assert.Equal("newValue", result[key]);
            Assert.Equal(2, result.Count);
        }
        [Fact]
        public void TestSetProperty_UpdateWithIndex()
        {
            dynamic input = new List<dynamic>{
                new Dictionary<string, object> { { "existingKey1", "existingValue1" } },
                new Dictionary<string, object> { { "existingKey2", "existingValue2" } }};
            string key = "existingKey2";
            dynamic entry = "newValue";
            int index = 1;

            var result = DataFilters.SetProperty(null, input, key, entry, index);

            Assert.Equal("newValue", result[index][key]);
        }

        [Fact]
        public void TestCoalesce_AllNull()
        {
            dynamic[] input = { null, null, null };

            var result = DataFilters.Coalesce(null, input);

            Assert.Null(result);
        }

        [Fact]
        public void TestCoalesce_FirstNull()
        {
            dynamic[] input = { null, "test1", "test2" };

            var result = DataFilters.Coalesce(null, input);
            Assert.Equal("test1", result);
        }
        [Fact]
        public void TestCoalesce()
        {
            dynamic[] input = { "test1", "test2", "test3" };

            var result = DataFilters.Coalesce(null, input);

            Assert.Equal("test1", result);
        }
        [Fact]
        public void TestDecodeBase64_String()
        {
            string encodedString = "SGVsbG8gV29ybGQh"; 
            var result = DataFilters.DecodeBase64(null, encodedString);

            Assert.Equal("Hello World!", result);
        }

        [Fact]
        public void DecodeBase64_InvalidString()
        {
            string encodedString = "InvalidString";
            Assert.Throws<FormatException>(() => DataFilters.DecodeBase64(null, encodedString));
        }

        [Fact]
        public void TestFormatDateTime_DefaultFormat()
        {
            object timestamp = new DateTime(2022, 5, 30, 15, 30, 0);

            var result = DataFilters.FormatDateTime(null, timestamp);

            Assert.Equal("2022-05-30T15:30:00.0000000", result);
        }

        [Fact]
        public void TestFormatDateTime_WithFormat()
        {
            object timestamp = new DateTime(2022, 5, 30, 15, 30, 0);
            //string locale = "en-us";

            var result = DataFilters.FormatDateTime(null, timestamp, "dd/MM/yyyy HH:mm:ss");

            Assert.Equal("30/05/2022 15:30:00", result);
        }

        [Fact]
        public void TestFormatDateTime_NullTimestamp()
        {
            object timestamp = null;
            var result = DataFilters.FormatDateTime(null, timestamp);

            Assert.Null(result);
        }

        [Fact]
        public void TestFormatDateTime_ThrowsExceptionForInvalidTimestamp()
        {
            object timestamp = "invalidTimestamp";
            Assert.Throws<FormatException>(() => DataFilters.FormatDateTime(null, timestamp));
        }

    }
}
        

