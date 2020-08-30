using Newtonsoft.Json;

namespace FuncatConfiguration.Deserializer.Json
{
    public static class ConfigurationManagerBuilderExtensions
    {
        /// <summary>
        /// Use json deserializer for configuration data
        /// </summary>
        /// <param name="builder">Builder</param>
        /// <param name="settings">Deserializer settings</param>
        /// <returns>Builder</returns>
        public static ConfigurationManagerBuilder WithJsonDeserializer(this ConfigurationManagerBuilder builder, JsonSerializerSettings settings = null)
        {
            return builder.WithDeserializer(new JsonDeserializer(settings));
        }
    }
}