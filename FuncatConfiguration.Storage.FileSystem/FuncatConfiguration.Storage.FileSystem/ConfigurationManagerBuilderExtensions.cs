namespace FuncatConfiguration.Storage.FileSystem
{
    public static class ConfigurationManagerBuilderExtensions
    {
        /// <summary>
        /// Use file system storage for configuration files
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="folder">Path to folder with confgiuration files</param>
        /// <param name="isRelative">Is path to folder relative or absolute</param>
        /// <returns></returns>
        public static ConfigurationManagerBuilder WithFileSystemStorage(this ConfigurationManagerBuilder builder, string folder, bool isRelative = true)
        {
            return builder.WithStorage(new FileSystemStorage(folder, isRelative));
        }
    }
}