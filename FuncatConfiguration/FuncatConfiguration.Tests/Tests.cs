using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FuncatConfiguration.Tests.Mocks;
using FuncatConfiguration.Tests.TestConfigs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuncatConfiguration.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void Should_build_and_get_configs()
        {
            // Arrange
            var builder = ConfigurationManagerBuilder.Create();
            var manager = builder
                .WithConfigurationType<Config1>(true)
                .WithConfigurationType<Config2>(true)
                .WithDeserializer(new MockDeserializer())
                .WithStorage(new MockStorage(new Dictionary<string, string>
                {
                    { "Config1", "{ \"ConnectionString1\": \"connstr1\", \"SomeInteger1\": \"1\" }" },
                    { "Config2", "{ \"ConnectionString2\": \"connstr2\", \"SomeInteger2\": \"2\" }" },
                }))
                .BuildAsync(CancellationToken.None).Result;

            // Act
            var conf1 = manager.GetConfigurationAsync<Config1>(CancellationToken.None).Result;
            var conf2 = manager.GetConfigurationAsync<Config2>(CancellationToken.None).Result;

            // Assert
            Assert.AreEqual("connstr1", conf1.ConnectionString1);
            Assert.AreEqual(1, conf1.SomeInteger1);

            Assert.AreEqual("connstr2", conf2.ConnectionString2);
            Assert.AreEqual(2, conf2.SomeInteger2);
        }

        [TestMethod]
        public async Task Should_fail_build_cause_of_deserializer_not_set()
        {
            var builder = ConfigurationManagerBuilder.Create();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => builder
                .WithConfigurationType<Config1>(true)
                .WithConfigurationType<Config2>(true)
                .WithStorage(new MockStorage(new Dictionary<string, string>
                {
                    { "Config1", "{ \"ConnectionString1\": \"connstr1\", \"SomeInteger1\": \"1\" }" },
                    { "Config2", "{ \"ConnectionString2\": \"connstr2\", \"SomeInteger2\": \"2\" }" },
                }))
                .BuildAsync(CancellationToken.None));
        }

        [TestMethod]
        public async Task Should_fail_build_cause_of_storage_not_set()
        {
            var builder = ConfigurationManagerBuilder.Create();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => builder
                .WithConfigurationType<Config1>(true)
                .WithConfigurationType<Config2>(true)
                .WithDeserializer(new MockDeserializer())
                .BuildAsync(CancellationToken.None));
        }

        [TestMethod]
        public async Task Should_fail_build_cause_of_zero_config_count()
        {
            var builder = ConfigurationManagerBuilder.Create();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => builder
                .WithDeserializer(new MockDeserializer())
                .WithStorage(new MockStorage(new Dictionary<string, string>
                {
                    { "Config1", "{ \"ConnectionString1\": \"connstr1\", \"SomeInteger1\": \"1\" }" },
                    { "Config2", "{ \"ConnectionString2\": \"connstr2\", \"SomeInteger2\": \"2\" }" },
                }))
                .BuildAsync(CancellationToken.None));
        }
    }
}