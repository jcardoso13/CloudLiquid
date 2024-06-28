# Cloud Liquid Framework

CloudLiquid is a versatile framework designed to facilitate the parsing of data from HTTP content or strings containing JSON, CSV, and XML data into objects that other projects can utilize. Originally developed to work with CloudLiquid and LiquidConsole or Transform-Data-Functions, it plays a crucial role in transforming payloads into suitable formats for various templating engines. For DotLiquid, it converts payloads into Hash objects (Dictionary<string, object>), and for Scriban, it transforms payloads into ScriptObjects.

## Features
- Data Parsing: Easily parse JSON, CSV, and XML data into Hash objects.
- Integration with Templating Engines: Expanding DotLiquid for transforming payloads.
- Flexible Content Factory: Utilize the ContentFactory module for advanced data handling and transformations.

## Getting Started
To get started with CloudLiquid, clone the repository to your local machine:
```
git clone https://github.com/jcardoso13/CloudLiquid.git
```
Ensure you have the necessary dependencies installed and follow the setup instructions detailed in the AzureCloudLiquid README.


## Using Liquid Templates with CloudLiquid

CloudLiquid enhances the use of Liquid templates, allowing for easy integration and manipulation of data in formats like JSON, CSV, and XML. Here's how to get started with Liquid templates in CloudLiquid:

### Basic Example

To render a simple greeting message using a Liquid template:

1. **Create a Liquid Template**

```liquid
Hello, {{ user.name }}!
```

2. **Parse and Render the Template in CloudLiquid**

Assuming you have a JSON string `{"name": "John Doe"}`, you can parse this JSON and render the template as follows:


```csharp
using DotLiquid;
using Microsoft.Extensions.Logging;
using CloudLiquid.Azure.Exceptions;
using CloudLiquid.Core;
using CloudLiquid.ObjectModel;

// Parse the JSON string into a Hash object
var userData = "{\"name\": \"John Doe\"}";



using var loggerFactory = LoggerFactory.Create(builder =>
{
  builder.AddConsole();
}); 

// Create a logger instance for the Program class
var logger = loggerFactory.CreateLogger<Program>();

// Create a new instance of the LiquidProcessor class
var liquidProcessor = new LiquidProcessor(logger, null);

// Get a content reader for JSON data
var contentReader = ContentFactory.GetContentReader("application/json");

// Parse the user data string and run the liquid processor with the parsed data
RunResult result = liquidProcessor.Run(contentReader.ParseString(userData), "Hello, {{ user.name }}!");


Console.WriteLine(result.Output); // Outputs: Hello, John Doe!
```

### Advanced Usage

CloudLiquid also supports more complex scenarios, such as iterating over collections, using conditionals, and incorporating custom filters or tags that you define. Here's an example of iterating over a list of items in a Liquid template:

```liquid
{%- if content.name != null -%}
{{ content | clear_nulls | json }}
{%- endif -%}
```
Input of the Liquid Template:
```json
{
    "name": "John Doe",
    "age": null,
    "email": "johndoe@example.com",
    "address": null,
    "isEmployed": true,
    "hobbies": ["reading", "running", "cooking"],
    "favoriteColor": "blue",
    "height": 175.5,
    "weight": 70.2
}
```


And the corresponding C# code to render this template:

```csharp
using DotLiquid;
using Microsoft.Extensions.Logging;
using CloudLiquid.Azure.Exceptions;
using CloudLiquid.Core;
using CloudLiquid.ObjectModel;

// JSON Input
var userData = "{\r\n    \"name\": \"John Doe\",\r\n    \"age\": null,\r\n    \"email\": \"johndoe@example.com\",\r\n    \"address\": null,\r\n    \"isEmployed\": true,\r\n    \"hobbies\": [\"reading\", \"running\", \"cooking\"],\r\n    \"favoriteColor\": \"blue\",\r\n    \"height\": 175.5,\r\n    \"weight\": 70.2\r\n}";



using var loggerFactory = LoggerFactory.Create(builder =>
{
  builder.AddConsole();
}); 

// Create a logger instance for the Program class
var logger = loggerFactory.CreateLogger<Program>();
var json = "{\r\n{% for item in content %}\r\n  {{ item.key | json }}: {{ item.value | json }}\r\n{% endfor %}\r\n}";
// Create a new instance of the LiquidProcessor class
var liquidProcessor = new LiquidProcessor(logger, null);

// Get a content reader for JSON data
var contentReader = ContentFactory.GetContentReader("application/json");

// Parse the user data string and run the liquid processor with the parsed data
RunResult result = liquidProcessor.Run(contentReader.ParseString(userData), json);


Console.WriteLine(result.Output);
```

This will generate the following output:

```json
{
    "name": "John Doe",
    "email": "johndoe@example.com",
    "isEmployed": true,
    "hobbies": ["reading", "running", "cooking"],
    "favoriteColor": "blue",
    "height": 175.5,
    "weight": 70.2
}
```

## Filter and Tags Documentation

The documentation for CloudLiquid filters can be found in the [Filters](./docs/Filters.md) file. This comprehensive guide provides detailed information on each filter available in CloudLiquid, including their usage, parameters, and examples. Whether you need to manipulate strings, format dates, or manipulate JSON Objects and Lists, the filter documentation will help you leverage the full power of CloudLiquid's filtering capabilities. Make sure to refer to this documentation whenever you need to apply filters to your CloudLiquid templates.



## Contributing
Contributions to CloudLiquid are welcome! If you have any questions, feature requests, or issues, please add an issue on GitHub or contact the repository owner, João Pedro Cardoso (jpcardoso@outlook.pt).


## License
CloudLiquid is licensed under the GPLv3 License. Feel free to use, modify, and distribute the code as per the license agreement.


## More Information

This README is generated as part of the CloudLiquid framework documentation. For the most up-to-date information, please visit the [GitHub repository](https://github.com/jcardoso13/CloudLiquid).

Any Questions or Feature requests, please add an Issue or contact Repo Owner João Pedro Cardoso (jpcardoso@outlook.pt).