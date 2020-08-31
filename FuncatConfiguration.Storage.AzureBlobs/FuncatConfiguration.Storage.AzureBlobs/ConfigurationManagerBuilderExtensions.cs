namespace FuncatConfiguration.Storage.AzureBlobs
{
    public static class ConfigurationManagerBuilderExtensions
    {
        /// <summary>
        /// Use azure blob storage for configuration files
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionString">Connection string to azure storage account</param>
        /// <param name="containerName">Configuration files container name</param>
        /// <param name="relativePathInContainer">Relative path to folder in container that contains configuration files</param>
        /// <returns>Builder</returns>
        public static ConfigurationManagerBuilder WithAzureBlobsStorage(this ConfigurationManagerBuilder builder, string connectionString, string containerName, string relativePathInContainer)
        {
            return builder.WithStorage(new AzureBlobStorage(connectionString, containerName, relativePathInContainer));
        }

        /// <summary>
        /// Use azure file share storage for configuration files
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionString">Connection string to azure storage account</param>
        /// <param name="containerName">Configuration files container name</param>
        /// <param name="relativePathInContainer">Relative path to folder in container that contains configuration files</param>
        /// <returns>Builder</returns>
        public static ConfigurationManagerBuilder WithAzureFileShareStorage(this ConfigurationManagerBuilder builder, string connectionString, string shareName, string relativePathInShare)
        {
            return builder.WithStorage(new AzureFileShareStorage(connectionString, shareName, relativePathInShare));
        }
    }
}