## CloudLiquid Filters Documentation

Welcome to the documentation for the CloudLiquid filters! This guide will provide you with detailed information on each filter and how to use them in your Liquid templates.

### Padleft
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
{{ 'Hello' | Padleft: 10, '*' }}
```

**Output:**
```
*****Hello
```

### Secret
**Description:** Retrieves the value of an environment variable specified by the input.

**JSON Input:**
```json
{
    "input": "PATH"
}
```

**Liquid:**
```liquid
{{ 'PATH' | Secret }}
```

**Output:**
```
/usr/local/bin:/usr/bin:/bin:/usr/sbin:/sbin
```

### Padright
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
{{ 'Hello' | Padright: 10, '*' }}
```

**Output:**
```
Hello*****
```

### Nullifnull
**Description:** Returns "null" if the input string is null or empty; otherwise, returns the input string.

**JSON Input:**
```json
{
    "input": ""
}
```

**Liquid:**
```liquid
{{ '' | Nullifnull }}
```

**Output:**
```
null
```

### Parsedouble
**Description:** Parses the input string to a double.

**JSON Input:**
```json
{
    "input": "123.45"
}
```

**Liquid:**
```liquid
{{ '123.45' | Parsedouble }}
```

**Output:**
```
123.45
```

### Json
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
{{ {"name": "John", "age": 30} | Json: 'nobrackets' }}
```

**Output:**
```
"name":"John","age":30
```

### Xml
**Description:** Converts the input object to an XML string.

**JSON Input:**
```json
{
    "input": {"name": "John", "age": 30}
}
```

**Liquid:**
```liquid
{{ {"name": "John", "age": 30} | Xml }}
```

**Output:**
```xml
<root>
    <name>John</name>
    <age>30</age>
</root>
```

### LiquidContains
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
{{ ["apple", "banana", "cherry"] | LiquidContains: 'banana' }}
```

**Output:**
```
true
```

### DataType
**Description:** Determines the data type of the input data and returns its type as a string.

**JSON Input:**
```json
{
    "data": 123
}
```

**Liquid:**
```liquid
{{ 123 | DataType }}
```

**Output:**
```
Integer
```

### ConvertToInt
**Description:** Converts the input data to an integer.

**JSON Input:**
```json
{
    "data": "123"
}
```

**Liquid:**
```liquid
{{ "123" | ConvertToInt }}
```

**Output:**
```
123
```

### String
**Description:** Converts the specified object to a string.

**JSON Input:**
```json
{
    "data": 123
}
```

**Liquid:**
```liquid
{{ 123 | String }}
```

**Output:**
```
"123"
```

### IsLoop
**Description:** Determines whether the specified object is a list.

**JSON Input:**
```json
{
    "data": ["apple", "banana", "cherry"]
}
```

**Liquid:**
```liquid
{{ ["apple", "banana", "cherry"] | IsLoop }}
```

**Output:**
```
true
```

### ClearNulls
**Description:** Removes null or empty entries from the specified dictionary.

**JSON Input:**
```json
{
    "data": {"name": "John", "age": null}
}
```

**Liquid:**
```liquid
{{ {"name": "John", "age": null} | ClearNulls }}
```

**Output:**
```json
{
    "name": "John"
}
```

### CreateHash
**Description:** Creates a new hash and optionally adds a specified key with a null value.

**JSON Input:**
```json
{
    "key": "exampleKey"
}
```

**Liquid:**
```liquid
{{ '' | CreateHash: 'exampleKey' }}
```

**Output:**
```json
{
    "exampleKey": null
}
```

### CreateList
**Description:** Creates a new list with an optional type specification.

**JSON Input:**
```json
{
    "type": "dynamic"
}
```

**Liquid:**
```liquid
{{ '' | CreateList: 'dynamic' }}
```

**Output:**
```
[]
```
