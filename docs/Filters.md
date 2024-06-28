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
    "input": "PATH"
}
```

**Liquid:**
```liquid
{{ 'PATH' | secret }}
```

**Output:**
```
/usr/local/bin:/usr/bin:/bin:/usr/sbin:/sbin
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

