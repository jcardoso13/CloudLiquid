using Xunit;
using CloudLiquid.Tags;
using DotLiquid;

namespace CloudLiquid.Tests
{
    public class CaptureJsonTests
    {
        [Fact]
        public void TestCaptureJson()
        {
            Template.RegisterTagFactory(new CloudLiquidTagFactory(typeof(CaptureJSON), "capture_json"));
            Helper.AssertTemplateResult(expected: "SUCCESS", template: "{%- capture_json test -%}{\"TEST_TAG\":\"HELLO_WORLD\"}{%- endcapture_json -%}\r\n{%- if test.TEST_TAG == \"HELLO_WORLD\" -%}SUCCESS{%- else -%}FAILURE{%- endif -%}");
            Helper.AssertTemplateResult(expected: "FAILURE", template: "{%- capture_json test -%}{\"TEST_TAG\":\"HELLO\"}{%- endcapture_json -%}\r\n{%- if test.TEST_TAG == \"HELLO_WORLD\" -%}SUCCESS{%- else -%}FAILURE{%- endif -%}");
            Helper.AssertTemplateResult(expected: "FAILURE", template: "{%- capture_json test -%}{\"ANOTHER_TAG\":\"HELLO\"}{%- endcapture_json -%}\r\n{%- if test.TEST_TAG == \"HELLO_WORLD\" -%}SUCCESS{%- else -%}FAILURE{%- endif -%}");
            // JSON vazio
            Helper.AssertTemplateResult(expected: "FAILURE", template: "{%- capture_json test -%}{}{%- endcapture_json -%}\r\n{%- if test.TEST_TAG == \"HELLO_WORLD\" -%}SUCCESS{%- else -%}FAILURE{%- endif -%}");
            //boolean
            Helper.AssertTemplateResult(expected: "SUCCESS", template: "{%- capture_json test -%}{\"test1\":true,\"test2\":false}{%- endcapture_json -%}\r\n{%- if test.test1 == true and test.test2 == false -%}SUCCESS{%- else -%}FAILURE{%- endif -%}");
        }
    }
}
