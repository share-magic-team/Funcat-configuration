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
        public async Task ShouldLoadConfigurationByRelativePath()
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
        public async Task ShouldThrowExceptionWhenCannotFindConfigFile()
        {
            // Arrange
            var configurationManager = await ConfigurationManagerBuilder
                .Create()
                .WithFileSystemStorage("TestEnvironmentBad")
                .WithJsonDeserializer()
                .WithConfigurationType<DatabaseConfiguration>()
                .BuildAsync(CancellationToken.None);

            // Act / Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => configurationManager
                .GetConfigurationAsync<DatabaseConfiguration>(CancellationToken.None));
        }
    }
}