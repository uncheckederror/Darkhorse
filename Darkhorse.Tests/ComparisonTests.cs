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
                Assert.True(o.ToString() == m.ToString());
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
                Assert.True(o.ToString() == m.ToString());
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
                Assert.True(o.ToString() == m.ToString());
            }
        }

        [Fact]
        public async Task ATSHistoryComparison()
        {
            var rpacctid = 2193589;

            var oa = await ATSHistory.GetAsync(rpacctid, oracleDbConnection);
            var ma = await ATSHistory.GetAsync(rpacctid, mssqlDbConnection);
            var oracleAccounts = oa.ToArray();
            var mssqlAccounts = ma.ToArray();

            for (var i = 0; i < oracleAccounts.Length; i++)
            {
                var o = oracleAccounts[i];
                var m = mssqlAccounts[i];

                // Check if objects are equal.
                Assert.True(o.ToString() == m.ToString());
            }
        }

        [Fact]
        public async Task BuildingComparison()
        {
            var rpacctid = 2193589;

            var oa = await Building.GetAsync(rpacctid, oracleDbConnection);
            var ma = await Building.GetAsync(rpacctid, mssqlDbConnection);
            var oracleAccounts = oa.ToArray();
            var mssqlAccounts = ma.ToArray();

            for (var i = 0; i < oracleAccounts.Length; i++)
            {
                var o = oracleAccounts[i];
                var m = mssqlAccounts[i];

                // Check if objects are equal.
                Assert.True(o.ToString() == m.ToString());
            }
        }

        [Fact]
        public async Task ContactComparison()
        {
            var rpacctid = 2193589;

            var oa = await Contact.GetAsync(rpacctid, oracleDbConnection);
            var ma = await Contact.GetAsync(rpacctid, mssqlDbConnection);
            var oracleAccounts = oa.ToArray();
            var mssqlAccounts = ma.ToArray();

            for (var i = 0; i < oracleAccounts.Length; i++)
            {
                var o = oracleAccounts[i];
                var m = mssqlAccounts[i];

                // Check if objects are equal.
                Assert.True(o.ToString() == m.ToString());
            }
        }

        [Fact]
        public async Task CrmContactComparison()
        {
            var rpacctid = 2193589;

            var oa = await CrmContact.GetAsync(rpacctid, oracleDbConnection);
            var ma = await CrmContact.GetAsync(rpacctid, mssqlDbConnection);
            var oracleAccounts = oa.ToArray();
            var mssqlAccounts = ma.ToArray();

            for (var i = 0; i < oracleAccounts.Length; i++)
            {
                var o = oracleAccounts[i];
                var m = mssqlAccounts[i];

                // Check if objects are equal.
                Assert.True(o.ToString() == m.ToString());
            }
        }

        [Fact]
        public async Task LegalDescriptionComparison()
        {
            var rpacctid = 2193589;

            var oa = await LegalDescription.GetAsync(rpacctid, oracleDbConnection);
            var ma = await LegalDescription.GetAsync(rpacctid, mssqlDbConnection);
            var oracleAccounts = oa.ToArray();
            var mssqlAccounts = ma.ToArray();

            for (var i = 0; i < oracleAccounts.Length; i++)
            {
                var o = oracleAccounts[i];
                var m = mssqlAccounts[i];

                // Check if objects are equal.
                Assert.True(o.ToString() == m.ToString());
            }
        }

        [Fact]
        public async Task MobileHomesComparison()
        {
            var rpacctid = 1795343;
            var extension = "R02";
            var oa = await MobileHome.GetAsync(rpacctid, extension, oracleDbConnection);
            var ma = await MobileHome.GetAsync(rpacctid, extension, mssqlDbConnection);
            var oracleAccounts = oa.ToArray();
            var mssqlAccounts = ma.ToArray();

            for (var i = 0; i < oracleAccounts.Length; i++)
            {
                var o = oracleAccounts[i];
                var m = mssqlAccounts[i];

                // Check if objects are equal.
                Assert.True(o.ToString() == m.ToString());
            }
        }

        [Fact]
        public async Task NewConstructionComparison()
        {
            var rpacctid = 1477108;
            var oa = await NewConstruction.GetAsync(rpacctid, oracleDbConnection);
            var ma = await NewConstruction.GetAsync(rpacctid, mssqlDbConnection);
            var oracleAccounts = oa.ToArray();
            var mssqlAccounts = ma.ToArray();

            for (var i = 0; i < oracleAccounts.Length; i++)
            {
                var o = oracleAccounts[i];
                var m = mssqlAccounts[i];

                // Check if objects are equal.
                Assert.True(o.ToString() == m.ToString());
            }
        }

        [Fact]
        public async Task NoticesComparison()
        {
            var rpacctid = 1477108;
            var oa = await Notice.GetAsync(rpacctid, oracleDbConnection);
            var ma = await Notice.GetAsync(rpacctid, mssqlDbConnection);
            var oracleAccounts = oa.ToArray();
            var mssqlAccounts = ma.ToArray();

            for (var i = 0; i < oracleAccounts.Length; i++)
            {
                var o = oracleAccounts[i];
                var m = mssqlAccounts[i];

                // Check if objects are equal.
                Assert.True(o.ToString() == m.ToString());
            }
        }

        [Fact]
        public async Task PlatComparison()
        {
            var accountnumber = "4852-001-001-0008";
            var oa = await Plat.GetNameAsync(accountnumber, oracleDbConnection);
            var ma = await Plat.GetNameAsync(accountnumber, mssqlDbConnection);

            // Check if objects are equal.
            Assert.True(oa.ToString() == ma.ToString());
        }

        [Fact]
        public async Task RealPropertyAccountsFilterGetComparison()
        {
            var accountnumber = "4852";
            var oa = await RealPropertyAccountsFilter.GetAsync(accountnumber, oracleDbConnection);
            var ma = await RealPropertyAccountsFilter.GetAsync(accountnumber, mssqlDbConnection);
            var oracleAccounts = oa.ToArray();
            var mssqlAccounts = ma.ToArray();

            for (var i = 0; i < oracleAccounts.Length; i++)
            {
                var o = oracleAccounts[i];
                var m = mssqlAccounts[i];

                // Check if objects are equal.
                Assert.True(o.ToString() == m.ToString());
            }
        }

        [Fact]
        public async Task RealPropertyAccountsFilterGetByIdComparison()
        {
            var rpacctid = 1477108;
            var oa = await RealPropertyAccountsFilter.GetAsync(rpacctid, oracleDbConnection);
            var ma = await RealPropertyAccountsFilter.GetAsync(rpacctid, mssqlDbConnection);
            var oracleAccounts = oa.ToArray();
            var mssqlAccounts = ma.ToArray();

            for (var i = 0; i < oracleAccounts.Length; i++)
            {
                var o = oracleAccounts[i];
                var m = mssqlAccounts[i];

                // Check if objects are equal.
                Assert.True(o.ToString() == m.ToString());
            }
        }

        [Fact]
        public async Task RealAccountGetByIdComparison()
        {
            var rpacctid = 1477108;
            var oa = await RealAccount.GetAsync(rpacctid, oracleDbConnection);
            var ma = await RealAccount.GetAsync(rpacctid, mssqlDbConnection);

            // Check if objects are equal.
            Assert.True(oa.ToString() == ma.ToString());
        }

        [Fact]
        public async Task RealAccountGetComparison()
        {
            var accountNumber = "5432";
            var oa = await RealAccount.GetAsync(accountNumber, oracleDbConnection);
            var ma = await RealAccount.GetAsync(accountNumber, mssqlDbConnection);
            var oracleAccounts = oa.ToArray();
            var mssqlAccounts = ma.ToArray();

            for (var i = 0; i < oracleAccounts.Length; i++)
            {
                var o = oracleAccounts[i];
                var m = mssqlAccounts[i];

                // Check if objects are equal.
                Assert.True(o.ToString() == m.ToString());
            }
        }
    }
}
