using Xunit;
using CloudLiquid.Tags;
using DotLiquid;
using Azure.Storage.Blobs;
using DotLiquid.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CloudLiquid.Tests
{
   
    public class AzureTagTests
    {
        [Fact]
        public void TestAzureTag()
        {
            //Template.RegisterTagFactory(new CloudLiquidTagFactory(typeof(AzureTag), "include_azure", logger, blobContainerClient));

            var mockBlobContainerClient = new Mock<BlobContainerClient>();
            Template.RegisterTagFactory(new CloudLiquidTagFactory(typeof(AzureTag), "include_azure", mockBlobContainerClient.Object));

        }
    }
}
