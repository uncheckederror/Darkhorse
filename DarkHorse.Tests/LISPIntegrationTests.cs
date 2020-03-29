using DarkHorse.DataAccess;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;
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
        private readonly IDbConnection oracleDbConnection;
        private readonly IDbConnection mssqlDbConnection;

        public LISPIntegrationTests(ITestOutputHelper output)
        {
            this.output = output;

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets("fcb4eb5f-a4e9-49af-9bb0-72b3a44ebda8")
                .Build();

            configuration = config;

            oracleDbConnection = new OracleConnection(configuration.GetConnectionString("LISP"));
            mssqlDbConnection = new SqlConnection(configuration.GetConnectionString("LISPROD"));
        }

        [Fact]
        public async Task RealPropertyAccountById()
        {
            var acctId = 2361145;
            var accounts = await RealPropertyAccount.GetAsync(acctId, oracleDbConnection);

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
            var accounts = await RealPropertyAccount.GetAsync(acctNo, oracleDbConnection);

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
            var results = await RealPropertyAccount.GetAsync(new Regex(pattern), oracleDbConnection);
            var accounts = results.ToList();

            Assert.True(accounts.Any());

            accounts.ForEach(a => output.WriteLine(a.ToString()));
        }

        [Fact]
        public async Task AccountTagsGetById()
        {
            var accountWithTags = 2193589;
            var results = await AccountTag.GetAsync(accountWithTags, oracleDbConnection);
            foreach (var result in results)
            {
                Assert.NotNull(results);
                Assert.False(string.IsNullOrWhiteSpace(result.TAG_CODE));
                output.WriteLine(result.ToString());
            }
        }

        [Fact]
        public async Task InspectionsGetById()
        {
            var newConstructionId = 10319;

            var results = await Inspection.GetAsync(newConstructionId, oracleDbConnection);

            foreach (var result in results)
            {
                Assert.NotNull(result);
                Assert.True(result.NEW_CONSTRUCTION_ID > 0);
            }
        }
    }
}
