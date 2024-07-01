## CloudLiquid Filters Documentation

Welcome to the documentation for the CloudLiquid filters! This guide will provide you with detailed information on each filter and how to use them in your Liquid templates.

### padleft
**Description:** Pads the left side of the input string with the specified character to ensure the resulting string has a total width as specified.

**JSON Input:**
```json
{
    "input": "Hello",
    "totalWidth": 10,
    "padChar": "*"
}
```

**Liquid:**
```liquid
{{ 'Hello' | padleft: 10, '*' }}
```

**Output:**
```
*****Hello
```

### secret
**Description:** Retrieves the value of an environment variable specified by the input.

**JSON Input:**
```json
{
    "input": "ContainerName"
}
```

**Liquid:**
```liquid
{{ content.input | secret }}
```

**Output:**
```
liquid
```

### padright
**Description:** Pads the right side of the input string with the specified character until the total width specified is reached.

**JSON Input:**
```json
{
    "input": "Hello",
    "totalWidth": 10,
    "padChar": "*"
}
```

**Liquid:**
```liquid
{{ 'Hello' | padright: 10, '*' }}
```

**Output:**
```
Hello*****
```

### nullifnull
**Description:** Returns "null" if the input string is null or empty; otherwise, returns the input string.

**JSON Input:**
```json
{
    "input": ""
}
```

**Liquid:**
```liquid
{{ '' | nullifnull }}
```

**Output:**
```
null
```

### parsedouble
**Description:** Parses the input string to a double.

**JSON Input:**
```json
{
    "input": "123.45"
}
```

**Liquid:**
```liquid
{{ '123.45' | parsedouble }}
```

**Output:**
```
123.45
```

### json
**Description:** Converts the input to a JSON string. Optionally removes the outer brackets if "nobrackets" is specified.

**JSON Input:**
```json
{
    "input": {"name": "John", "age": 30},
    "settings": "nobrackets"
}
```

**Liquid:**
```liquid
{{ {"name": "John", "age": 30} | json: 'nobrackets' }}
```

**Output:**
```
"name":"John","age":30
```

### xml
**Description:** Converts the input object to an XML string.

**JSON Input:**
```json
{
    "input": {"name": "John", "age": 30}
}
```

**Liquid:**
```liquid
{{ {"name": "John", "age": 30} | xml }}
```

**Output:**
```xml
<root>
    <name>John</name>
    <age>30</age>
</root>
```

### liquid_contains
**Description:** Checks if the data contains the specified object.

**JSON Input:**
```json
{
    "data": ["apple", "banana", "cherry"],
    "Obj": "banana"
}
```

**Liquid:**
```liquid
{{ ["apple", "banana", "cherry"] | liquid_contains: 'banana' }}
```

**Output:**
```
true
```

### data_type
**Description:** Determines the data type of the input data and returns its type as a string.

**JSON Input:**
```json
{
    "data": 123
}
```

**Liquid:**
```liquid
{{ 123 | data_type }}
```

**Output:**
```
Integer
```

### convert_to_int
**Description:** Converts the input data to an integer.

**JSON Input:**
```json
{
    "data": "123"
}
```

**Liquid:**
```liquid
{{ "123" | convert_to_int }}
```

**Output:**
```
123
```

### string
**Description:** Converts the specified object to a string.

**JSON Input:**
```json
{
    "data": 123
}
```

**Liquid:**
```liquid
{{ 123 | string }}
```

**Output:**
```
"123"
```

### is_loop
**Description:** Determines whether the specified object is a list.

**JSON Input:**
```json
{
    "data": ["apple", "banana", "cherry"]
}
```

**Liquid:**
```liquid
{{ ["apple", "banana", "cherry"] | is_loop }}
```

**Output:**
```
true
```

### clear_nulls
**Description:** Removes null or empty entries from the specified dictionary.

**JSON Input:**
```json
{
    "data": {"name": "John", "age": null}
}
```

**Liquid:**
```liquid
{{ {"name": "John", "age": null} | clear_nulls }}
```

**Output:**
```json
{
    "name": "John"
}
```

### create_hash
**Description:** Creates a new hash and optionally adds a specified key with a null value.

**JSON Input:**
```json
{
    "key": "exampleKey"
}
```

**Liquid:**
```liquid
{{ '' | create_hash: 'exampleKey' }}
```

**Output:**
```json
{
    "exampleKey": null
}
```

### create_list
**Description:** Creates a new list with an optional type specification.

**JSON Input:**
```json
{
    "type": "dynamic"
}
```

**Liquid:**
```liquid
{{ '' | create_list: 'dynamic' }}
```

**Output:**
```
[]
```

### add_to_list
**Description:** Adds an item to the specified list with options for unique and null insertion.

**JSON Input:**
```json
{
    "data": ["apple", "banana"],
    "insert": "banana",
    "unique": true,
    "nullInsert": false
}
```
**Liquid:**
```liquid
{% assign list = data | AddToList: 'banana', true, false %}
```

**Output:**
```
["apple", "banana"]
```

### remove_from_list
**Description:** Removes the specified item from the list.

**JSON Input:**
```json
{
    "data": ["apple", "banana", "cherry"],
    "key": "banana"
}
```
**Liquid:**
```liquid
{% assign list = data | RemoveFromList: 'banana' %}
```

**Output:**
```
["apple", "cherry"]
```

### get_list_from_hash
**Description:**  Retrieves a list from a hash based on the specified key.

**JSON Input:**
```json
{
    "data": {"key1": ["item1", "item2"], "key2": ["item3", "item4"]},
    "key": "key1"
}
```
**Liquid:**
```liquid
{% assign list = data | GetListFromHash: 'key1' %}
```

**Output:**
```
["item1", "item2"]
```

### log
**Description:** Writes the specified data to the console as a log entry.

**JSON Input:**
```json
{
    "data": "This is a log message"
}
```
**Liquid:**
```liquid
{{ 'This is a log message' | Log }}
```

**Output:**
```
This is a log message
```

### remove_property
**Description:** Removes a property from the specified object or from an object at the specified index within a list.

**JSON Input:**
```json
{
    "data": [{"key1": "value1", "key2": "value2"}, {"key1": "value3", "key2": "value4"}],
    "key": "key1",
    "index": 0
}
```
**Liquid:**
```liquid
{% assign newData = data | RemoveProperty: 'key1', 0 %}
```

**Output:**
```
[
    {"key2": "value2"},
    {"key1": "value3", "key2": "value4"}
]
```

### add_property
**Description:** Adds a property to an object at the specified index within a list.

**JSON Input:**
```json
{
    "data": [{"key1": "value1"}, {"key1": "value3"}],
    "key": "key2",
    "entry": "value2",
    "index": 1
}
```

**Liquid:**
```liquid
{% assign newData = data | AddProperty: 'key2', 'value2', 1 %}
```

**Output:**
```
[
    {"key1": "value1"},
    {"key1": "value3", "key2": "value2"}
]
```

### set_property
**Description:** Sets the value of a property in an object at the specified index within a list.

**JSON Input:**
```json
{
    "data": [{"key1": "value1"}, {"key1": "value3"}],
    "key": "key1",
    "entry": "newValue",
    "index": 1
}
```

**Liquid:**
```liquid
{% assign newData = data | SetProperty: 'key1', 'newValue', 1 %}
```

**Output:**
```
[
    {"key1": "value1"},
    {"key1": "newValue"}
]
```

### coalesce
**Description:**  Returns the first non-null value from the provided list of dynamic inputs.

**JSON Input:**
```json
{
    "inputs": [null, null, "value", null]
}
```
**Liquid:**
```liquid
{% assign firstNonNull = inputs | Coalesce %}
```

**Output:**
```
"value"
```

### decode_base64
**Description:** Decodes a Base64 encoded string into its original UTF-8 encoded string representation.

**JSON Input:**
```json
{
    "encodedString": "aGVsbG8gd29ybGQ="
}
```
**Liquid:**
```liquid
{% assign decodedString = encodedString | DecodeBase64 %}
```

**Output:**
```
"hello world"
```

### format_date_time
**Description:**Formats a timestamp object into a string representation using the specified format and locale.

**JSON Input:**
```json
{
    "timestamp": "2024-06-01T12:00:00Z",
    "format": "yyyy-MM-dd",
    "locale": "en-us"
}
```
**Liquid:**
```liquid
{% assign formattedDate = timestamp | FormatDateTime: 'yyyy-MM-dd', 'en-us' %}
```

**Output:**
```
"2024-06-01"
```

