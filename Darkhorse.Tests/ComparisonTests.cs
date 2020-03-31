using DarkHorse.DataAccess;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
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

        private int compareLists<T>(T[] oracleAccounts, T[] mssqlAccounts) where T : BaseTableClass
        {
            var mismatches = 0;

            if (oracleAccounts.Length != mssqlAccounts.Length)
            {
                throw new ArgumentOutOfRangeException("The arrays of Oracle and MSSQL ojects are not the same length. This comparison should be done in the calling method before it gets here.");
            }

            for (var i = 0; i < oracleAccounts.Length; i++)
            {
                mismatches += compareObject(oracleAccounts[i], mssqlAccounts[i]);
            }

            return mismatches;
        }

        private int compareObject<T>(T oracleResult, T mssqlResult) where T : BaseTableClass
        {
            var mismatch = oracleResult.CompareTo(mssqlResult);

            if (mismatch != 0)
            {
                output.WriteLine($"[Oracle] {oracleResult}");
                output.WriteLine($"[MSSQL] {mssqlResult}");
            }

            return mismatch;
        }

        [Fact]
        public async Task AccountTagsGetByIdComparison()
        {
            var accountWithTags = 2193589;

            var oracleAccounts = (await AccountTag.GetAsync(accountWithTags, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await AccountTag.GetAsync(accountWithTags, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task InspectionsComparison()
        {
            var newConstructionId = 10319;

            var oracleAccounts = (await Inspection.GetAsync(newConstructionId, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await Inspection.GetAsync(newConstructionId, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task ATSHistoryComparison()
        {
            var rpacctid = 2193589;

            var oracleAccounts = (await ATSHistory.GetAsync(rpacctid, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await ATSHistory.GetAsync(rpacctid, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task BuildingComparison()
        {
            var rpacctid = 2193589;

            var oracleAccounts = (await Building.GetAsync(rpacctid, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await Building.GetAsync(rpacctid, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task ContactComparison()
        {
            var rpacctid = 2193589;

            var oracleAccounts = (await Contact.GetAsync(rpacctid, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await Contact.GetAsync(rpacctid, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task CrmContactComparison()
        {
            var rpacctid = 2193589;

            var oracleAccounts = (await CrmContact.GetAsync(rpacctid, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await CrmContact.GetAsync(rpacctid, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task LegalDescriptionComparison()
        {
            var rpacctid = 2193589;

            var oracleAccounts = (await LegalDescription.GetAsync(rpacctid, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await LegalDescription.GetAsync(rpacctid, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task MobileHomesComparison()
        {
            var rpacctid = 1795343;
            var extension = "R02";

            var oracleAccounts = (await MobileHome.GetAsync(rpacctid, extension, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await MobileHome.GetAsync(rpacctid, extension, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task NewConstructionComparison()
        {
            var rpacctid = 1477108;

            var oracleAccounts = (await NewConstruction.GetAsync(rpacctid, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await NewConstruction.GetAsync(rpacctid, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task NoticesComparison()
        {
            var rpacctid = 1477108;

            var oracleAccounts = (await Notice.GetAsync(rpacctid, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await Notice.GetAsync(rpacctid, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task PlatComparison()
        {
            var accountnumber = "4852-001-001-0008";

            var oa = await Plat.GetNameAsync(accountnumber, oracleDbConnection);
            var ma = await Plat.GetNameAsync(accountnumber, mssqlDbConnection);

            Assert.Equal(0, compareObject(oa, ma));
        }

        [Fact]
        public async Task RealPropertyAccountsFilterGetComparison()
        {
            var accountnumber = "4852";

            var oracleAccounts = (await RealPropertyAccountsFilter.GetAsync(accountnumber, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await RealPropertyAccountsFilter.GetAsync(accountnumber, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task RealPropertyAccountsFilterGetByIdComparison()
        {
            var rpacctid = 1477108;

            var oracleAccounts = (await RealPropertyAccountsFilter.GetAsync(rpacctid, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await RealPropertyAccountsFilter.GetAsync(rpacctid, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task RealAccountGetByIdComparison()
        {
            var rpacctid = 1477108;

            var oa = await RealAccount.GetAsync(rpacctid, oracleDbConnection);
            var ma = await RealAccount.GetAsync(rpacctid, mssqlDbConnection);

            Assert.Equal(0, compareObject(oa, ma));
        }

        [Fact]
        public async Task RealAccountGetComparison()
        {
            var accountNumber = "5432";

            var oracleAccounts = (await RealAccount.GetAsync(accountNumber, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await RealAccount.GetAsync(accountNumber, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task RemarksGetComparison()
        {
            var rpacctid = 1477108;

            var oracleAccounts = (await Remark.GetAsync(rpacctid, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await Remark.GetAsync(rpacctid, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task RealPropertyAccountGroupGetNumberComparison()
        {
            var rpacctid = 1477108;

            var oracleAccounts = (await RealPropertyAccountGroup.GetAsync(rpacctid, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await RealPropertyAccountGroup.GetAsync(rpacctid, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task RealPropertyAccountGroupGetComparison()
        {
            var rpacctid = 1477108;

            var oracleAccounts = (await RealPropertyAccountGroup.GetAsync(rpacctid, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await RealPropertyAccountGroup.GetAsync(rpacctid, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task RealPropertyAccountYearGetComparison()
        {
            var rpacctid = 1477108;

            var oracleAccounts = (await RealPropertyAccountYear.GetAsync(rpacctid, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await RealPropertyAccountYear.GetAsync(rpacctid, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task RealPropertyAccountYearFilterComparison()
        {
            var rpacctid = 1477108;
            var year = new DateTime(2, 2, 2018);

            var oa = await RealPropertyAccountYear.GetRealAccountFiltersAsync(rpacctid, year, oracleDbConnection);
            var ma = await RealPropertyAccountYear.GetRealAccountFiltersAsync(rpacctid, year, mssqlDbConnection);

            Assert.Equal(0, compareObject(oa, ma));
        }

        [Fact]
        public async Task RealPropertyAccountGetComparison()
        {
            var acctId = 2361145;

            var oracleAccounts = (await RealPropertyAccount.GetAsync(acctId, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await RealPropertyAccount.GetAsync(acctId, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task RealPropertyAccountGetByAccountNumberComparison()
        {
            var accountNumber = "4852-001-001-0008";

            var oracleAccounts = (await RealPropertyAccount.GetAsync(accountNumber, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await RealPropertyAccount.GetAsync(accountNumber, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task RealPropertyAccountGetByPatternComparison()
        {
            var pattern = "^092202-1-06[0-9]-2[0-9]{3}$";

            var oracleAccounts = (await RealPropertyAccount.GetAsync(new Regex(pattern), oracleDbConnection)).ToArray();
            var mssqlAccounts = (await RealPropertyAccount.GetAsync(new Regex(pattern), mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task RealPropertyContactGetComparison()
        {
            var contactId = 1000488;

            var oracleAccounts = (await RealPropertyContact.GetAsync(contactId, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await RealPropertyContact.GetAsync(contactId, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task RealPropertySiteAddressGetComparison()
        {
            var rpAcctId = 2361145;

            var oracleAccounts = (await RealPropertyContact.GetAsync(rpAcctId, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await RealPropertyContact.GetAsync(rpAcctId, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }

        [Fact]
        public async Task SalesGetComparison()
        {
            var rpAcctId = 2361145;

            var oracleAccounts = (await RealPropertyContact.GetAsync(rpAcctId, oracleDbConnection)).ToArray();
            var mssqlAccounts = (await RealPropertyContact.GetAsync(rpAcctId, mssqlDbConnection)).ToArray();

            Assert.Equal(oracleAccounts.Length, mssqlAccounts.Length);
            Assert.Equal(0, compareLists(oracleAccounts, mssqlAccounts));
        }
    }
}
