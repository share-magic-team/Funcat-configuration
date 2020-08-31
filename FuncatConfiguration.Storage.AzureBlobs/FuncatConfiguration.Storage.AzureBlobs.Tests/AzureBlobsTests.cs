using System.IO;
using System.Threading;
using FuncatConfiguration.Deserializer.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuncatConfiguration.Storage.AzureBlobs.Tests
{
    [TestClass]
    public class AzureBlobsTests
    {
        [TestMethod]
        public void ShouldLoadConfigsFromBlobAndFolder_prod()
        {
            // Arrange
            var connStr = GetAzureBlobConnectionString();

            var manager = ConfigurationManagerBuilder
                .Create()
                .WithAzureBlobsStorage(connStr, "testconfigs", "prod")
                .WithJsonDeserializer()
                .WithConfigurationType<SqlConnectionConfig>()
                .WithConfigurationType<SomeExternalServiceConnectionConfig>()
                .BuildAsync(CancellationToken.None).Result;

            // Act
            var sqlConnectionConfig = manager.GetConfigurationAsync<SqlConnectionConfig>(CancellationToken.None).Result;
            var someExternalServiceConnectionConfig = manager.GetConfigurationAsync<SomeExternalServiceConnectionConfig>(CancellationToken.None).Result;

            // Assert
            Assert.IsNotNull(sqlConnectionConfig);
            Assert.IsNotNull(someExternalServiceConnectionConfig);
            Assert.AreEqual("TestConnectionString1", sqlConnectionConfig.ConnectionString);
            Assert.AreEqual("testurl1", someExternalServiceConnectionConfig.Url);
            Assert.AreEqual("login1", someExternalServiceConnectionConfig.BasicAuthLogin);
            Assert.AreEqual("pass1", someExternalServiceConnectionConfig.BasicAuthPassword);
        }

        [TestMethod]
        public void ShouldLoadConfigsFromBlobAndFolder_Prod()
        {
            // Arrange
            var connStr = GetAzureBlobConnectionString();

            var manager = ConfigurationManagerBuilder
                .Create()
                .WithAzureBlobsStorage(connStr, "testconfigs", "Prod")
                .WithJsonDeserializer()
                .WithConfigurationType<SqlConnectionConfig>()
                .WithConfigurationType<SomeExternalServiceConnectionConfig>()
                .BuildAsync(CancellationToken.None).Result;

            // Act
            var sqlConnectionConfig = manager.GetConfigurationAsync<SqlConnectionConfig>(CancellationToken.None).Result;
            var someExternalServiceConnectionConfig = manager.GetConfigurationAsync<SomeExternalServiceConnectionConfig>(CancellationToken.None).Result;

            // Assert
            Assert.IsNotNull(sqlConnectionConfig);
            Assert.IsNotNull(someExternalServiceConnectionConfig);
            Assert.AreEqual("TestConnectionString", sqlConnectionConfig.ConnectionString);
            Assert.AreEqual("testurl", someExternalServiceConnectionConfig.Url);
            Assert.AreEqual("login", someExternalServiceConnectionConfig.BasicAuthLogin);
            Assert.AreEqual("pass", someExternalServiceConnectionConfig.BasicAuthPassword);
        }

        [TestMethod]
        public void ShouldLoadConfigsFromBlobAndNoFolder()
        {
            // Arrange
            var connStr = GetAzureBlobConnectionString();

            var manager = ConfigurationManagerBuilder
                .Create()
                .WithAzureBlobsStorage(connStr, "testconfigs")
                .WithJsonDeserializer()
                .WithConfigurationType<SqlConnectionConfig>()
                .WithConfigurationType<SomeExternalServiceConnectionConfig>()
                .BuildAsync(CancellationToken.None).Result;

            // Act
            var sqlConnectionConfig = manager.GetConfigurationAsync<SqlConnectionConfig>(CancellationToken.None).Result;
            var someExternalServiceConnectionConfig = manager.GetConfigurationAsync<SomeExternalServiceConnectionConfig>(CancellationToken.None).Result;

            // Assert
            Assert.IsNotNull(sqlConnectionConfig);
            Assert.IsNotNull(someExternalServiceConnectionConfig);
            Assert.AreEqual("TestConnectionString2", sqlConnectionConfig.ConnectionString);
            Assert.AreEqual("testurl2", someExternalServiceConnectionConfig.Url);
            Assert.AreEqual("login2", someExternalServiceConnectionConfig.BasicAuthLogin);
            Assert.AreEqual("pass2", someExternalServiceConnectionConfig.BasicAuthPassword);
        }

        private string GetAzureBlobConnectionString()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                    path: "application.json",
                    optional: false,
                    reloadOnChange: true)
                .Build();

            return configuration.GetConnectionString("SettingsStorage");
        }
    }
}