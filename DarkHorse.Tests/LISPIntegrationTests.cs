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
        public async Task RealPropertyAccountGetComparison()
        {
            var acctId = 2361145;

            var oa = await RealPropertyAccount.GetAsync(acctId, oracleDbConnection);
            var ma = await RealPropertyAccount.GetAsync(acctId, mssqlDbConnection);
            var oracleAccounts = oa.ToArray();
            var mssqlAccounts = ma.ToArray();

            for (var i = 0; i < oracleAccounts.Length; i++)
            {
                var o = oracleAccounts[i];
                var m = mssqlAccounts[i];

                // Check if objects are equal.
                Assert.True(m.ACCT_NO == o.ACCT_NO);
                Assert.True(m.CREATED_BY == o.CREATED_BY);
                Assert.True(m.CREATED_DT == o.CREATED_DT);
                Assert.True(m.INACTIVE_DT == m.INACTIVE_DT);
                Assert.True(m.MAP_NO == o.MAP_NO);
                Assert.True(m.MODIFIED_BY == o.MODIFIED_BY);
                Assert.True(m.MODIFIED_DT == o.MODIFIED_DT);
                Assert.True(m.NEIGHBORHOOD_CODE == o.NEIGHBORHOOD_CODE);
                Assert.True(m.PP_AS_RP_FLAG == m.PP_AS_RP_FLAG);
                Assert.True(m.QUARTER_SECTION == o.QUARTER_SECTION);
                Assert.True(m.REFERENCE_DT == o.REFERENCE_DT);
                Assert.True(m.RP_ACCT_ID == o.RP_ACCT_ID);
                Assert.True(m.SEC_TWN_RNG == o.SEC_TWN_RNG);
                Assert.True(m.WORK_GROUP == o.WORK_GROUP);

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

        // Beacuse the MS-SQL path hasn't been implemented this test will fail.
        [Fact]
        public async Task AccountTagsGetByIdComparison()
        {
            var accountWithTags = 2193589;

            var oa = await AccountTag.GetAsync(accountWithTags, oracleDbConnection);
            var ma = await AccountTag.GetAsync(accountWithTags, mssqlDbConnection);
            var oracleAccounts = oa.ToArray();
            var mssqlAccounts = ma.ToArray();

            for (var i = 0; i < oracleAccounts.Length; i++)
            {
                var o = oracleAccounts[i];
                var m = mssqlAccounts[i];

                // Check if objects are equal.
                Assert.True(m.ALERT_FLAG == o.ALERT_FLAG);
                Assert.True(m.BEGIN_DT == o.BEGIN_DT);
                Assert.True(m.CREATED_BY == o.CREATED_BY);
                Assert.True(m.CREATED_DT == o.CREATED_DT);
                Assert.True(m.DESCRIPTION == o.DESCRIPTION);
                Assert.True(m.END_DT == o.END_DT);
                Assert.True(m.LOCK_ACCT_FLAG == o.LOCK_ACCT_FLAG);
                Assert.True(m.MODIFIED_BY == o.MODIFIED_BY);
                Assert.True(m.MODIFIED_DT == o.MODIFIED_DT);
                Assert.True(m.NO_STATEMENT_FLAG == o.NO_STATEMENT_FLAG);
                Assert.True(m.PROGRAM_GEN_FLAG == o.PROGRAM_GEN_FLAG);
                Assert.True(m.QUE_STATEMENT_FLAG == o.QUE_STATEMENT_FLAG);
                Assert.True(m.REMOVED_BY == o.REMOVED_BY);
                Assert.True(m.SYSTEM_TAG_FLAG == o.SYSTEM_TAG_FLAG);
                Assert.True(m.TAG_CODE == o.TAG_CODE);
                Assert.True(m.TEMP_FLAG == o.TEMP_FLAG);
                Assert.True(m.TRANSFER_FLAG == o.TRANSFER_FLAG);
            }
        }
    }
}
