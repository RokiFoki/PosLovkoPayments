using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDevelopmentHelper
{
    public class MockConfiguration : IConfiguration
    {
        private Dictionary<string, string> dict = new Dictionary<string, string>();

        public string this[string key] { get => dict[key]; set => dict[key] = value; }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return null;
        }

        public IChangeToken GetReloadToken()
        {
            return null;
        }

        public IConfigurationSection GetSection(string key)
        {
            return null;
        }
    }
}
