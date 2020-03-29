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
    public class ComparisonTests
    {
        private readonly ITestOutputHelper output;
        private readonly IConfiguration configuration;
        private readonly IDbConnection oracleDbConnection;
        private readonly IDbConnection mssqlDbConnection;

        public ComparisonTests(ITestOutputHelper output)
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

        [Fact]
        public async Task InspectionsComparison()
        {
            var newConstructionId = 10319;

            var oa = await Inspection.GetAsync(newConstructionId, oracleDbConnection);
            var ma = await Inspection.GetAsync(newConstructionId, mssqlDbConnection);
            var oracleAccounts = oa.ToArray();
            var mssqlAccounts = ma.ToArray();

            for (var i = 0; i < oracleAccounts.Length; i++)
            {
                var o = oracleAccounts[i];
                var m = mssqlAccounts[i];

                // Check if objects are equal.
                Assert.True(o.ASSESSOR_INSPECTION_ID == m.ASSESSOR_INSPECTION_ID);
                Assert.True(o.CREATED_BY == m.CREATED_BY);
                Assert.True(o.CREATED_DT == m.CREATED_DT);
                Assert.True(o.INSPECTION_DT == m.INSPECTION_DT);
                Assert.True(o.INSPECTOR == m.INSPECTOR);
                Assert.True(o.MODIFIED_BY == m.MODIFIED_BY);
                Assert.True(o.MODIFIED_DT == m.MODIFIED_DT);
                Assert.True(o.NC_STATUS == m.NC_STATUS);
                Assert.True(o.NC_VALUE == m.NC_VALUE);
                Assert.True(o.NEW_CONSTRUCTION_ID == m.NEW_CONSTRUCTION_ID);
                Assert.True(o.POSTED_DT == m.POSTED_DT);
                Assert.True(o.POSTED_FLAG == m.POSTED_FLAG);
            }
        }
    }
}
