using System;

namespace FuncatConfiguration
{
    internal static class Utils
    {
        public static string TransformConfigurationTypeName(string configurationName)
        {
            const string configuration = "configuration";
            if (configurationName.Equals(configuration, StringComparison.InvariantCultureIgnoreCase))
                return configurationName;
            if (configurationName.EndsWith(configuration, StringComparison.InvariantCultureIgnoreCase))
                return configurationName.Substring(0, configurationName.Length - configuration.Length);
            return configurationName;
        }
    }
}