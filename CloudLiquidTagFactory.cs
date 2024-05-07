using Azure.Storage.Blobs;
using CloudLiquid.Tags;
using DotLiquid;
using Microsoft.Extensions.Logging;

namespace CloudLiquid
{
    public class CloudLiquidTagFactory : ITagFactory
    {
        #region Private Members

        private readonly BlobContainerClient blobContainerClient;
        private readonly ILogger logger;
        private readonly Type tagType;
        private readonly string tagName;

        #endregion

        #region Constructors

        public CloudLiquidTagFactory(Type tagType, string tagName)
        {
            this.tagType = tagType;
            this.tagName = tagName;
        }

        public CloudLiquidTagFactory(Type tagType, string tagName, BlobContainerClient blobContainerClient) : this(tagType, tagName)
        {
            this.blobContainerClient = blobContainerClient;
        }

        public CloudLiquidTagFactory(Type tagType, string tagName, ILogger logger) : this(tagType, tagName)
        {
            this.logger = logger;
        }

        public CloudLiquidTagFactory(Type tagType, string tagName, ILogger logger, BlobContainerClient blobContainerClient) : this(tagType, tagName, logger)
        {
            this.blobContainerClient = blobContainerClient;
        }

        #endregion

        #region Public Properties

        public string TagName { get { return tagName; } }

        #endregion

        #region Public Methods

        public Tag Create() 
        { 
            var tag = (Tag)Activator.CreateInstance(tagType);

            if (tag is BaseCloudLiquidTag baseCloudLiquidTag && this.logger != null)
            {
                baseCloudLiquidTag.InitializeLogger(logger);
            }

            if (tag is AzureTag azureTag && this.blobContainerClient != null)
            {
                azureTag.InitializeBlobContainerClient(blobContainerClient);
            }

            return tag;
        }

        #endregion
    }
}