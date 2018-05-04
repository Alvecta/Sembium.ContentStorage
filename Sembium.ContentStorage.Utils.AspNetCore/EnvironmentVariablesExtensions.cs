using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Sembium.ContentStorage.Utils.AspNetCore
{
    public static class EnvironmentVariablesExtensions
    {
        public static IConfigurationBuilder AddEnvironmentVariablesAsSettings(this IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Add(new EnvironmentVariablesAsSettingsConfigurationSource());
            return configurationBuilder;
        }

        private class EnvironmentVariablesAsSettingsConfigurationSource : IConfigurationSource
        {
            public IConfigurationProvider Build(IConfigurationBuilder builder)
            {
                return new EnvironmentVariablesAsSettingsConfigurationProvider();
            }
        }

        private class EnvironmentVariablesAsSettingsConfigurationProvider : ConfigurationProvider
        {
            public override void Load()
            {
                foreach (DictionaryEntry ev in Environment.GetEnvironmentVariables())
                {
                    var key = ev.Key.ToString();
                    var value = ev.Value.ToString();

                    if (key.Contains("__"))
                    {
                        var translatedKey = key.Replace("___", ".").Replace("__", ":");
                        Data[translatedKey] = value;
                    }
                }
            }
        }
    }
}
