using System;
using System.Threading;
using System.Threading.Tasks;
using FuncatConfiguration.Deserializer.Json;
using FuncatConfiguration.Storage.FileSystem.Tests.TestConfigs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuncatConfiguration.Storage.FileSystem.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public async Task ShouldLoadConfigurationByRelativePath_Async()
        {
            // Arrange
            var configurationManager = await ConfigurationManagerBuilder
                .Create()
                .WithFileSystemStorage("TestEnvironment")
                .WithJsonDeserializer()
                .WithConfigurationType<DatabaseConfiguration>()
                .BuildAsync(CancellationToken.None);

            // Act
            var configuration = await configurationManager
                .GetConfigurationAsync<DatabaseConfiguration>(CancellationToken.None);

            // Assert
            Assert.AreEqual("TestConnectionString1", configuration.ConnectionString1);
            Assert.AreEqual("TestConnectionString2", configuration.ConnectionString2);
        }

        [TestMethod]
        public void ShouldThrowExceptionWhenCannotFindConfigFile_NonAsync()
        {
            // Arrange
            var configurationManager = ConfigurationManagerBuilder
                .Create()
                .WithFileSystemStorage("TestEnvironmentBad")
                .WithJsonDeserializer()
                .WithConfigurationType<DatabaseConfiguration>()
                .Build();

            // Act / Assert
            Assert.ThrowsException<InvalidOperationException>(() => configurationManager
                .GetConfiguration<DatabaseConfiguration>());
        }
    }
}