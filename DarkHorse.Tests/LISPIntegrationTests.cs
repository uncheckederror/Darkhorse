using DarkHorse.DataAccess;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DarkHorse.Tests
{
    public class LISPIntegrationTests
    {
        private readonly ITestOutputHelper output;
        private readonly IConfiguration configuration;
        private readonly IDbConnection dbConnection;

        public LISPIntegrationTests(ITestOutputHelper output)
        {
            this.output = output;

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets("fcb4eb5f-a4e9-49af-9bb0-72b3a44ebda8")
                .Build();

            configuration = config;

            dbConnection = new OracleConnection(configuration.GetConnectionString("LISP"));
        }

        [Fact]
        public async Task RealPropertyAccountById()
        {
            var acctId = 2361145;
            var accounts = await RealPropertyAccount.GetAsync(acctId, dbConnection);

            foreach (var account in accounts)
            {
                Assert.NotNull(account);
                Assert.Equal(acctId, account.RP_ACCT_ID);
                output.WriteLine(account.ToString());
            }
        }

        [Fact]
        public async Task RealPropertyAccountByNumber()
        {
            var acctNo = "092202-1-060-2004";
            var accounts = await RealPropertyAccount.GetAsync(acctNo, dbConnection);

            foreach (var account in accounts)
            {
                Assert.NotNull(account);
                Assert.Equal(acctNo, account.ACCT_NO);
                output.WriteLine(account.ToString());
            }
        }

        [Fact]
        public async Task RealPropertyAccountByPattern()
        {
            var pattern = "092202-1-06";
            var results = await RealPropertyAccount.GetAsync(new Regex(pattern), dbConnection);
            var accounts = results.ToList();

            Assert.True(accounts.Any());

            accounts.ForEach(a => output.WriteLine(a.ToString()));
        }
    }
}
