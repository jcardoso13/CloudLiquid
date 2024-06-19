using Xunit;
using CloudLiquid.Tags;
using DotLiquid;
namespace CloudLiquid.Tests
{
    public class CaptureXmlTests
    {
        [Fact]
        public void TestRender()
        {
            Template.RegisterTagFactory(new CloudLiquidTagFactory(typeof(CaptureXML), "capture_xml"));

            Helper.AssertTemplateResult(expected: "SUCCESS", template: "{%- capture_xml test -%}<root><testTag>Hello World</testTag></root>{%- endcapture_xml -%}\r\n{%- if test.root.testTag == \"Hello World\" -%}SUCCESS{%- else -%}FAILURE{%- endif -%}");
           // XML aninhado
            Helper.AssertTemplateResult(expected: "SUCCESS", template: "{%- capture_xml test -%}<root><nested><Tag>Value</Tag></nested></root>{%- endcapture_xml -%}\r\n{%- if test.root.nested.Tag == \"Value\" -%}SUCCESS{%- else -%}FAILURE{%- endif -%}");
            // XML com listas
            Helper.AssertTemplateResult(expected: "SUCCESS", template: "{%- capture_xml test -%}<root><items><item>1</item><item>2</item></items></root>{%- endcapture_xml -%}\r\n{%- if test.root.items.item[0] == \"1\" and test.root.items.item[1] == \"2\" -%}SUCCESS{%- else -%}FAILURE{%- endif -%}");

        }
    }
}
