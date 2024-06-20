using Xunit;
using CloudLiquid.Tags;
using DotLiquid;
using DotLiquid.Exceptions;
using Moq;
using DotLiquid.FileSystems;
using Microsoft.Extensions.Primitives;
using System.Globalization;

namespace CloudLiquid.Tests
{
    public class IncludeLocalTests
    { 
        [Fact]
        public void IncludeLocal_InvalidTemplate()
        {
            Template.RegisterTagFactory(new CloudLiquidTagFactory(typeof(IncludeLocal), "include_local"));

            var includeLocal = new IncludeLocal();
            var writer = new StringWriter();

            Assert.Throws<SyntaxException>(() => includeLocal.Render(null, writer));
        }
        [Fact]
        public void IncludeLocal_InvalidMarkup()
        {
            var invalidMarkup = "invalid_markup";
            Template.RegisterTagFactory(new CloudLiquidTagFactory(typeof(IncludeLocal), "include_local"));

            var includeLocal = new IncludeLocal();

            Assert.Throws<SyntaxException>(() => includeLocal.Initialize("include_local", invalidMarkup, null));
        }
        [Fact]
        public void Initialize_ValidMarkup()
        {
            var tagName = "include_local";
            var markup = "\"template_name\"";
            var tokens = new List<string> { "template_name" };
            Template.RegisterTagFactory(new CloudLiquidTagFactory(typeof(IncludeLocal), "include_local"));
            var includeLocal = new IncludeLocal();

            Assert.Throws<SyntaxException>(() => includeLocal.Initialize(tagName, markup, tokens));
        }
    }
}
