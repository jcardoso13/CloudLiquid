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
            // number
            Helper.AssertTemplateResult(expected: "SUCCESS", template: "{%- capture_json test -%}{\"number\":1234, \"float\":12.34}{%- endcapture_json -%}\r\n{%- if test.number == 1234 and test.float == 12.34 -%}SUCCESS{%- else -%}FAILURE{%- endif -%}");
            // arrays
            Helper.AssertTemplateResult(expected: "SUCCESS", template: "{%- capture_json test -%}{\"array\":[1,2,3,4]}{%- endcapture_json -%}\r\n{%- if test.array[0] == 1 and test.array[3] == 4 -%}SUCCESS{%- else -%}FAILURE{%- endif -%}");
            // objetos aninhados
            Helper.AssertTemplateResult(expected: "SUCCESS", template: "{%- capture_json test -%}{\"nested\":{\"test\":\"value\"}}{%- endcapture_json -%}\r\n{%- if test.nested.test == \"value\" -%}SUCCESS{%- else -%}FAILURE{%- endif -%}");

        }
    }
}
