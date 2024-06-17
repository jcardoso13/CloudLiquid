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
            // Helper.AssertTemplateResult(expected: "SUCCESS", template: "{%- capture_xml test -%}<root><TEST_TAG>HELLO_WORLD</TEST_TAG></root>{%- endcapture_xml -%}\r\n{%- if test.TEST_TAG == \"HELLO_WORLD\" -%}SUCCESS{%- else -%}FAILURE{%- endif -%}");

        }
    }
}
