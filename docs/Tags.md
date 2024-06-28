## CloudLiquid Tags Documentation

Welcome to the documentation for the CloudLiquid Tags! This guide will provide you with detailed information on each tag and how to use them in your Liquid templates.


### AzureTag Documentation

The `AzureTag` is designed to include templates from Azure Blob Storage into your Liquid templates. It initializes with either a `BlobContainerClient` or both a `BlobContainerClient` and a `ILogger` to facilitate interactions with Azure Blob Storage and optional logging.

#### Example Usage

Assuming you have a template stored in Azure Blob Storage named `greeting.liquid` with the content:

```liquid
Hello, {{ content.user.name }}!
```

And your JSON input is:

```json
{
  "user": {
    "name": "John Doe"
  }
}
```

You would use the `AzureTag` in your main Liquid template like so:

```liquid
{% include_azure '"greeting" %}
```

The output would be:

```
Hello, John Doe!
```

### IncludeLocal Documentation

The `IncludeLocal` tag allows you to include local Liquid templates within another Liquid template. It supports passing variables to the included templates.

#### Example Usage

Given a local template named `signature`:

```liquid
Best regards,
{{ content.user.signature }}
```

And your JSON input is:

```json
{
  "user": {
    "signature": "John Doe"
  }
}
```

You can include this local template in your main Liquid template as follows:

```liquid
{% include_local 'signature' %}
```

The output would be:

```
Best regards,
John Doe
```

### CaptureXML Documentation

The `CaptureXML` tag captures the content within its block, parses it as XML, converts it to JSON, and assigns it to a variable. This is useful for converting XML content to a JSON object within a Liquid template.

#### Example Usage

Given the following XML wrapped in a `CaptureXML` tag:

```liquid
{% capture_xml userJson %}
<user>
  <name>John Doe</name>
  <email>john.doe@example.com</email>
</user>
{% endcapture_xml %}
```

The captured XML is converted to JSON and assigned to `userJson`. The equivalent JSON representation would be:

```json
{
  "user": {
    "name": "John Doe",
    "email": "john.doe@example.com"
  }
}
```

You can then use `userJson` in your Liquid template to access the converted JSON data.

### CaptureJSON Documentation

The `CaptureJSON` tag captures the content within its block, parses it as JSON, and assigns it to a variable. This allows for capturing and manipulating JSON data directly within a Liquid template.

#### Example Usage

If you have JSON content that you want to capture and assign to a variable:

```liquid
{% capture_json userinfo %}
{
  "name": "John Doe",
  "email": "john.doe@example.com"
}
{% endcapture_json %}
```

The JSON content is captured and assigned to `userInfo`. You can then access `userInfo` in your Liquid template, for example:

```liquid
Hello, {{ userInfo.name }}!
```

The output would be:

```
Hello, John Doe!
```

Each of these tags enhances the functionality of Liquid templates, allowing for dynamic content inclusion, format conversion, and data manipulation directly within the template.