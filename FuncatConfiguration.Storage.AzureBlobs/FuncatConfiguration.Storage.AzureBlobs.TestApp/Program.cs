namespace FuncatConfiguration.Storage.AzureBlobs.TestApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var manager = ConfigurationManagerBuilder
                .Create()
                .WithAzureBlobsStorage("", "TestConfigs", "Prod")
                .WithDeserializer(new MockDeserializer())
                .WithConfigurationType<TestConfig>()
                .BuildAsync().Result;
        }
    }
}