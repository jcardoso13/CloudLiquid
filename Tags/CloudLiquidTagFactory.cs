using Azure.Storage.Blobs;
using CloudLiquid.Tags;
using DotLiquid;
using Microsoft.Extensions.Logging;

namespace CloudLiquid.Tags
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
        /// <summary>
        /// Initializes a new instance of the CloudLiquidTagFactory class with the specified tag type and name.
        /// </summary>
        /// <param name="tagType">The type of the tag.</param>
        /// <param name="tagName">The name of the tag.</param>
        public CloudLiquidTagFactory(Type tagType, string tagName)
        {
            this.tagType = tagType;
            this.tagName = tagName;
        }

        /// <summary>
        /// Initializes a new instance of the CloudLiquidTagFactory class with the specified tag type, name, and BlobContainerClient.
        /// </summary>
        /// <param name="tagType">The type of the tag.</param>
        /// <param name="tagName">The name of the tag.</param>
        /// <param name="blobContainerClient">The BlobContainerClient instance.</param>
        public CloudLiquidTagFactory(Type tagType, string tagName, BlobContainerClient blobContainerClient) : this(tagType, tagName)
        {
            this.blobContainerClient = blobContainerClient;
        }

        /// <summary>
        /// Initializes a new instance of the CloudLiquidTagFactory class with the specified tag type, name, and logger.
        /// </summary>
        /// <param name="tagType">The type of the tag.</param>
        /// <param name="tagName">The name of the tag.</param>
        /// <param name="logger">The logger instance.</param>
        public CloudLiquidTagFactory(Type tagType, string tagName, ILogger logger) : this(tagType, tagName)
        {
            this.logger = logger;
        }
        /// <summary>
        /// Initializes a new instance of the CloudLiquidTagFactory class with the specified tag type, name, logger, and BlobContainerClient.
        /// </summary>
        /// <param name="tagType">The type of the tag.</param>
        /// <param name="tagName">The name of the tag.</param>
        /// <param name="logger">The logger instance.</param>
        /// <param name="blobContainerClient">The BlobContainerClient instance.</param>
        public CloudLiquidTagFactory(Type tagType, string tagName, ILogger logger, BlobContainerClient blobContainerClient) : this(tagType, tagName, logger)
        {
            this.blobContainerClient = blobContainerClient;
        }

        #endregion

        #region Public Properties

        public string TagName { get { return tagName; } }

        #endregion

        #region Public Methods
        // <summary>
        /// Creates an instance of the specified tag type.
        /// </summary>
        /// <returns>An instance of the tag.</returns>
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