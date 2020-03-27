using DarkHorse.DataAccess;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DarkHorse.Tests
{
    public class LISPRODIntegrationTests
    {
        private readonly ITestOutputHelper output;
        private readonly IConfiguration configuration;
        private readonly IDbConnection dbConnection;

        public LISPRODIntegrationTests(ITestOutputHelper output)
        {
            this.output = output;

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets("fcb4eb5f-a4e9-49af-9bb0-72b3a44ebda8")
                .Build();

            configuration = config;

            dbConnection = new SqlConnection(configuration.GetConnectionString("LISPROD"));
        }

        [Fact]
        public async Task RealPropertyAccountById()
        {
            int acctId = 2361145;
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
            int rpAcctId = 2361145;
            var accounts = await RealPropertyAccount.GetAsync(rpAcctId, dbConnection);

            foreach (var account in accounts)
            {
                Assert.NotNull(account);
                Assert.Equal(rpAcctId, account.RP_ACCT_ID);
                output.WriteLine(account.ToString());
            }
        }

        [Fact]
        public async Task RealPropertyAccountByPattern()
        {
            int rpAcctId = 2361145;
            var accounts = await RealPropertyAccount.GetAsync(rpAcctId, dbConnection);

            foreach (var account in accounts)
            {
                Assert.NotNull(account);
                Assert.Equal(rpAcctId, account.RP_ACCT_ID);
                output.WriteLine(account.ToString());
            }
        }
    }
}
