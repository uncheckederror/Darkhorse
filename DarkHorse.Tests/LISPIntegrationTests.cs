using DarkHorse.DataAccess;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DarkHorse.Tests
{
    public class LISPIntegrationTests
    {
        private readonly ITestOutputHelper output;
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public LISPIntegrationTests(ITestOutputHelper output)
        {
            this.output = output;
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets("fcb4eb5f-a4e9-49af-9bb0-72b3a44ebda8")
                .Build();
            configuration = config;
            connectionString = configuration.GetConnectionString("LISP");
        }

        [Fact]
        public async Task RealPropertyAccountGetAsync()
        {
            int rpAcctId = 2361145;
            var accounts = await RealPropertyAccount.GetAsync(rpAcctId, configuration.GetConnectionString("LISP"));
            foreach (var account in accounts)
            {
                Assert.NotNull(account);
                Assert.Equal(rpAcctId, account.RP_ACCT_ID);
                output.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(account));
            }
        }
    }
}
