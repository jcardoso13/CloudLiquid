using Xunit;
using CloudLiquid.Tags;
using DotLiquid;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Moq;
using DotLiquid.Exceptions;
using DotLiquid.NamingConventions;

namespace CloudLiquid.Tests
{
    public class AzureTagTests
    {
        [Fact]
        public void AzureTag_TemplateNotFound()
        {
            var mockLogger = new Mock<ILogger<AzureTag>>();
            var mockBlobContainerClient = new Mock<BlobContainerClient>();
            Template.RegisterTagFactory(new CloudLiquidTagFactory(typeof(AzureTag), "include_azure", mockLogger.Object, mockBlobContainerClient.Object));

            var azureTag = new AzureTag(mockLogger.Object, mockBlobContainerClient.Object);

            using var writer = new StringWriter();
            Assert.Throws<SyntaxException>(() => azureTag.Render(null, writer));
        }
        [Fact]
        public void AzureTag_InitializeWithLoggerAndBlob()
        {
            var mockLogger = new Mock<ILogger<AzureTag>>();
            var mockBlobContainerClient = new Mock<BlobContainerClient>();
            Template.RegisterTagFactory(new CloudLiquidTagFactory(typeof(AzureTag), "include_azure", mockLogger.Object, mockBlobContainerClient.Object));

            var azureTag = new AzureTag(mockLogger.Object, mockBlobContainerClient.Object);

            Assert.NotNull(azureTag);
        }
        [Fact]
        public void AzureTag_InvalidMarkupSyntax()
        {
            var mockLogger = new Mock<ILogger<AzureTag>>();
            var mockBlobContainerClient = new Mock<BlobContainerClient>();
            Template.RegisterTagFactory(new CloudLiquidTagFactory(typeof(AzureTag), "include_azure", mockLogger.Object, mockBlobContainerClient.Object));

            var azureTag = new AzureTag(mockLogger.Object, mockBlobContainerClient.Object);

            Assert.Throws<SyntaxException>(() => azureTag.Initialize("include_azure", "invalid syntax", new List<string>()));
        }
        
    }
}
