using Xunit;
using CloudLiquid.Tags;
using DotLiquid;
using Azure.Storage.Blobs;

namespace CloudLiquid.Tests
{
   
    public class AzureTagTests
    {
        [Fact]
        public void TestAzureTag()
        {
            //Template.RegisterTagFactory(new CloudLiquidTagFactory(typeof(AzureTag), "include_azure", logger, blobContainerClient));

        }
    }
}
