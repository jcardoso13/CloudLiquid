using Xunit;
using CloudLiquid.Tags;
using DotLiquid;
using Microsoft.Extensions.Logging;
using DotLiquid.FileSystems;


namespace CloudLiquid.Tests
{
    public class IncludeLocalTests
    {
        [Fact]
        public void TestRender_TemplateNotFound()
        {
            Template.RegisterTagFactory(new CloudLiquidTagFactory(typeof(IncludeLocal), "include_local"));
        }
    }
}
