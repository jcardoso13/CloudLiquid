# ContentFactory Module

The ContentFactory module is a core component of the CloudLiquid framework, designed to facilitate the parsing and transformation of data from various formats such as JSON, CSV, and XML into objects. This module is highly versatile and can be integrated with different templating engines like DotLiquid and Scriban, making it an essential tool for data handling and transformations.

## Features

- **Data Parsing**: Supports parsing data from JSON, CSV, and XML formats into objects.
- **Integration with Templating Engines**: Seamlessly works with DotLiquid and Scriban to transform payloads into suitable formats.
- **Flexible and Extensible**: Designed to be easily extended for additional data formats and use cases.

## Getting Started

To use the ContentFactory module in your projects, ensure that you have the CloudLiquid framework installed. You can clone the CloudLiquid repository and include the ContentFactory module in your project:

```
git clone https://github.com/jcardoso13/CloudLiquid.git
```
Refer to the CloudLiquid framework's [README.md](README.md) for detailed setup instructions and dependencies.

## Usage

The ContentFactory module provides a set of classes and interfaces to work with different content types. Here's a quick overview of how to use it:

### Reading Content

To read content, use the appropriate `IContentReader` implementation for your data format. For example, to read JSON data:

```csharp
var jsonReader = new CloudLiquid.ContentFactory.JsonContentReader();
var myObject = jsonReader.ParseString(jsonString);
```

### Writing Content

Similarly, to write content, use the corresponding IContentWriter implementation:

```csharp

var jsonWriter = new CloudLiquid.ContentFactory.JsonContentWriter();
var jsonString = jsonWriter.CreateResponse(myObject);
```

For more detailed examples and advanced features, refer to the specific documentation within the ContentFactory module.



### Contributing

Contributions to the CloudLiquid framework, including the ContentFactory module, are welcome. Please refer to the main CloudLiquid README.md for contribution guidelines.


### License
The ContentFactory module, as part of the CloudLiquid framework, is licensed under the GNU General Public License v3.0. See the LICENSE file for more details.


### More Information


For the most up-to-date information on the ContentFactory module and the CloudLiquid framework, please visit the GitHub repository.

This documentation is part of the CloudLiquid framework. For further improvements and detailed documentation, keep an eye on the GitHub repository.

This README provides an overview of the ContentFactory module, its features, usage examples, and contribution guidelines, tailored to the information available in your workspace.