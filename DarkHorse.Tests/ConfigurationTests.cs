using DarkHorse.DataAccess;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DarkHorse.Tests
{
    public class ConfigurationTests
    {
        private readonly ITestOutputHelper output;

        public ConfigurationTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void IsTheConfigurationFileValid()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets("fcb4eb5f-a4e9-49af-9bb0-72b3a44ebda8")
                .Build();

            foreach (var con in config.AsEnumerable())
            {
                output.WriteLine(con.ToString());
            }
        }
    }
}
