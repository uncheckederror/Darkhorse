using DarkHorse.DataAccess;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
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
    public class LISPIntegrationTests
    {
        private readonly ITestOutputHelper output;
        private readonly IConfiguration configuration;
        private readonly IDbConnection oracleDbConnection;

        public LISPIntegrationTests(ITestOutputHelper output)
        {
            this.output = output;

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets("fcb4eb5f-a4e9-49af-9bb0-72b3a44ebda8")
                .Build();

            configuration = config;

            oracleDbConnection = new OracleConnection(configuration.GetConnectionString("LISP"));
        }

        public class TestData : IEnumerable<object[]>
        {
            static readonly string[] _differentPropertyClasses = new string[]
            {
            "112601-1-001-2009",
            "072402-2-107-2007",
            "082402-4-005-1007",
            "362402-2-006-1006",
            "4330-003-003-0004",
            "4381-000-003-0008",
            "312302-3-043-2003",
            "282402-4-055-2000",
            "212401-3-005-2100",
            "9000-010-144-0007",
            "132401-1-029-2007"
            };

            public static IEnumerable<object[]> GetPropertyClassesFromDataGenerator()
            {
                foreach (var account in _differentPropertyClasses)
                {
                    yield return new object[] { account };
                }
            }

            static readonly string[] _weirdAccounts = new string[]
            {
            "4316-033-001-0001",
            "5652-000-001-0009",
            "4158-002-002-0006",
            "4516-001-037-0004",
            "3790-007-009-0004",
            "322301-4-023-2005",
            "032702-2-029-2003",
            "5432-000-065-0009",
            "362301-3-033-2001",
            "282301-3-007-2003",
            "4759-000-005-0000",
            "4538-010-004-0206",
            "3771-001-004-0005",
            "152501-3-004-2009",
            "142301-1-052-2007",
            "4232-000-003-0009",
            "172302-2-017-2005",
            "012602-3-036-2005",
            "9000-001-265-0009",
            "192501-3-006-2003",
            "322501-3-023-1007"
            };

            public static IEnumerable<object[]> GetWeirdAccountsFromDataGenerator()
            {
                foreach (var account in _weirdAccounts)
                {
                    yield return new object[] { account };
                }
            }

            static readonly int[] _differentZones = new int[]
            {
            2575694,
            2344315,
            2636041,
            2311363,
            2640779,
            2641231,
            2637825,
            2506525,
            2324234,
            2611986,
            2636843,
            2395325,
            2605152,
            2524395,
            2632149,
            2632776,
            2605376,
            2310803,
            2637825,
            2330090,
            2507192,
            2105898,
            2444198,
            1173509,
            2636686,
            2638948,
            2627438,
            2632834,
            2630150,
            2631083,
            2615011,
            2638732,
            2640894,
            2641017,
            2640951,
            2605970,
            2187383,
            2539203,
            2636173,
            2582765,
            2587764,
            1987759,
            2612281,
            2640654,
            2631240,
            2639839,
            2639862
            };

            public static IEnumerable<object[]> GetZonesFromDataGenerator()
            {
                foreach (var account in _differentZones)
                {
                    yield return new object[] { account };
                }
            }

            static readonly int[] _differentDesignDistricts = new int[]
            {
            1238930,
            1238278,
            1608827,
            1238591,
            1404474,
            1200930,
            1238161,
            1581826,
            1245562,
            2529196,
            1405703,
            1222918,
            1239607,
            1240670
            };

            public static IEnumerable<object[]> GetDesignDistrictsFromDataGenerator()
            {
                foreach (var account in _differentDesignDistricts)
                {
                    yield return new object[] { account };
                }
            }

            static readonly int[] _newConstructionIds = new int[]
            {
                206387,
                205544,
                205533,
                205528,
                204842,
                204542,
                204356,
                204340,
                204338,
                203802
            };

            public static IEnumerable<object[]> GetNewConstructionsFromDataGenerator()
            {
                foreach (var account in _newConstructionIds)
                {
                    yield return new object[] { account };
                }
            }


            public IEnumerator<object[]> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [Theory]
        [MemberData(nameof(TestData.GetZonesFromDataGenerator), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.GetDesignDistrictsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task RealPropertyAccountById(int rpAcctId)
        {
            var accounts = await RealPropertyAccount.GetAsync(rpAcctId, oracleDbConnection);

            foreach (var account in accounts)
            {
                Assert.NotNull(account);
                Assert.Equal(rpAcctId, account.RP_ACCT_ID);
                output.WriteLine(account.ToString());
            }
        }

        [Theory]
        [MemberData(nameof(TestData.GetPropertyClassesFromDataGenerator), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.GetWeirdAccountsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task RealPropertyAccountByNumber(string accountNumber)
        {
            var accounts = await RealPropertyAccount.GetAsync(accountNumber, oracleDbConnection);

            foreach (var account in accounts)
            {
                Assert.NotNull(account);
                Assert.Equal(accountNumber, account.ACCT_NO);
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
        public async Task RealPropertyAccountByPatternASas()
        {
            var results = await CalculatePrepaymentAmounts.GetStoredProcAsync("RP", "5432-000-061-0003", "April", oracleDbConnection);

            Assert.True(results.SignUpDue > 0M);
            Assert.True(results.MontlyDue > 0M);
        }

        [Theory]
        [MemberData(nameof(TestData.GetZonesFromDataGenerator), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.GetDesignDistrictsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task AccountTagsGet(int rpAcctId)
        {
            var results = await AccountTag.GetAsync(rpAcctId, oracleDbConnection);
            foreach (var result in results)
            {
                Assert.NotNull(results);
                Assert.False(string.IsNullOrWhiteSpace(result.TAG_CODE));
                output.WriteLine(result.ToString());
            }
        }


        [Theory]
        [MemberData(nameof(TestData.GetZonesFromDataGenerator), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.GetDesignDistrictsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task AccountTagsGetCode(int rpAcctId)
        {
            var results = await AccountTag.GetCodeAsync(rpAcctId, oracleDbConnection);
            foreach (var result in results)
            {
                Assert.NotNull(results);
                Assert.False(string.IsNullOrWhiteSpace(result.TAG_CODE));
                output.WriteLine(result.ToString());
            }
        }

        [Theory]
        [MemberData(nameof(TestData.GetNewConstructionsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task InspectionsGet(int newConstructionId)
        {
            var results = await Inspection.GetAsync(newConstructionId, oracleDbConnection);

            foreach (var result in results)
            {
                Assert.NotNull(result);
                Assert.True(result.NEW_CONSTRUCTION_ID > 0);
            }
        }

        [Theory]
        [MemberData(nameof(TestData.GetZonesFromDataGenerator), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.GetDesignDistrictsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task ATSHistoryGet(int rpAcctId)
        {
            var results = await ATSHistory.GetAsync(rpAcctId, oracleDbConnection);
            Assert.NotNull(results);
            foreach (var result in results)
            {
                Assert.NotNull(result);
                Assert.True(result.ATS_HIST_ID > 0);
                output.WriteLine(result.ToString());
            }
        }

        [Theory]
        [MemberData(nameof(TestData.GetZonesFromDataGenerator), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.GetDesignDistrictsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task BuildingGet(int rpAcctId)
        {
            var results = await Building.GetAsync(rpAcctId, oracleDbConnection);
            Assert.NotNull(results);
            foreach (var result in results)
            {
                Assert.NotNull(result);
                Assert.True(result.LRSN > 0);
                output.WriteLine(result.ToString());
            }
        }

        [Theory]
        [MemberData(nameof(TestData.GetZonesFromDataGenerator), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.GetDesignDistrictsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task ContactGet(int rpAcctId)
        {
            var results = await Contact.GetAsync(rpAcctId, oracleDbConnection);
            Assert.NotNull(results);
            foreach (var result in results)
            {
                Assert.NotNull(result);
                Assert.False(string.IsNullOrWhiteSpace(result.NAME));
                output.WriteLine(result.ToString());
            }
        }

        [Theory]
        [MemberData(nameof(TestData.GetZonesFromDataGenerator), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.GetDesignDistrictsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task CrmContactGet(int rpAcctId)
        {
            var results = await CrmContact.GetAsync(rpAcctId, oracleDbConnection);
            Assert.NotNull(results);
            foreach (var result in results)
            {
                Assert.NotNull(result);
                Assert.False(string.IsNullOrWhiteSpace(result.CRM_FIRSTNAME));
                output.WriteLine(result.ToString());
            }
        }

        [Theory]
        [MemberData(nameof(TestData.GetZonesFromDataGenerator), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.GetDesignDistrictsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task LegalDescriptionGet(int rpAcctId)
        {
            var results = await LegalDescription.GetAsync(rpAcctId, oracleDbConnection);
            Assert.NotNull(results);
            foreach (var result in results)
            {
                Assert.NotNull(result);
                Assert.False(string.IsNullOrWhiteSpace(result.LEGAL_TEXT));
                output.WriteLine(result.ToString());
            }
        }

        // TODO: Figure out how to write a test for Mobile Homes.

        [Theory]
        [MemberData(nameof(TestData.GetZonesFromDataGenerator), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.GetDesignDistrictsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task NewConstructionGet(int rpAcctId)
        {
            var results = await NewConstruction.GetAsync(rpAcctId, oracleDbConnection);
            Assert.NotNull(results);
            foreach (var result in results)
            {
                Assert.NotNull(result);
                Assert.False(string.IsNullOrWhiteSpace(result.JURISDICTION));
                output.WriteLine(result.ToString());
            }
        }

        [Theory]
        [MemberData(nameof(TestData.GetZonesFromDataGenerator), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.GetDesignDistrictsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task NoticeGet(int rpAcctId)
        {
            var results = await Notice.GetAsync(rpAcctId, oracleDbConnection);
            Assert.NotNull(results);
            foreach (var result in results)
            {
                Assert.NotNull(result);
                Assert.False(string.IsNullOrWhiteSpace(result.NAME));
                output.WriteLine(result.ToString());
            }
        }

        [Theory]
        [MemberData(nameof(TestData.GetPropertyClassesFromDataGenerator), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.GetWeirdAccountsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task PlatGet(string accountNumber)
        {
            var result = await Plat.GetNameAsync(accountNumber, oracleDbConnection);

            var check = int.TryParse(accountNumber.Substring(0, 2), out int plat);

            if (plat < 37 && check)
            {
                // These accounts are not in a Plat
                Assert.True(string.IsNullOrWhiteSpace(result.PLAT_NAME));
            }
            if (plat >= 37 && check)
            {
                Assert.NotNull(result);
                Assert.False(string.IsNullOrWhiteSpace(result.PLAT_NAME));
                output.WriteLine(result.ToString());
            }
        }

        [Theory]
        [MemberData(nameof(TestData.GetPropertyClassesFromDataGenerator), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.GetWeirdAccountsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task RealPropertyAccountsFilterGet(string accountNumber)
        {
            var results = await RealPropertyAccountsFilter.GetAsync(accountNumber, oracleDbConnection);

            Assert.NotNull(results);
            foreach (var result in results)
            {
                Assert.NotNull(result);
                Assert.False(string.IsNullOrWhiteSpace(result.CONTACT_NAME));
                output.WriteLine(result.ToString());
            }
        }

        [Theory]
        [MemberData(nameof(TestData.GetZonesFromDataGenerator), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.GetDesignDistrictsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task RealPropertyAccountsFilterGetById(int rpAcctId)
        {
            var results = await RealPropertyAccountsFilter.GetAsync(rpAcctId, oracleDbConnection);

            Assert.NotNull(results);
            foreach (var result in results)
            {
                Assert.NotNull(result);
                Assert.False(string.IsNullOrWhiteSpace(result.CONTACT_NAME));
                output.WriteLine(result.ToString());
            }
        }

        [Theory]
        [MemberData(nameof(TestData.GetZonesFromDataGenerator), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.GetDesignDistrictsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task RealAccountGetById(int rpAcctId)
        {
            var result = await RealAccount.GetAsync(rpAcctId, oracleDbConnection);

            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result.ACCT_NO));
            output.WriteLine(result.ToString());
        }

        [Theory]
        [MemberData(nameof(TestData.GetPropertyClassesFromDataGenerator), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.GetWeirdAccountsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task RealPropertyGet(string accountNumber)
        {
            var results = await RealAccount.GetAsync(accountNumber, oracleDbConnection);

            Assert.NotNull(results);
            foreach (var result in results)
            {
                Assert.NotNull(result);
                Assert.False(string.IsNullOrWhiteSpace(result.ACCT_NO));
                output.WriteLine(result.ToString());
            }
        }

        [Theory]
        [MemberData(nameof(TestData.GetZonesFromDataGenerator), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.GetDesignDistrictsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task RemarkGet(int rpAcctId)
        {
            var results = await Remark.GetAsync(rpAcctId, oracleDbConnection);

            Assert.NotNull(results);
            foreach (var result in results)
            {
                Assert.NotNull(result);
                Assert.False(string.IsNullOrWhiteSpace(result.REMARKS));
                output.WriteLine(result.ToString());
            }
        }

        [Theory]
        [MemberData(nameof(TestData.GetZonesFromDataGenerator), MemberType = typeof(TestData))]
        [MemberData(nameof(TestData.GetDesignDistrictsFromDataGenerator), MemberType = typeof(TestData))]
        public async Task RealPropertyAccountGroupGetNumber(int rpAcctId)
        {
            var result = await RealPropertyAccountGroup.GetNumberAsync(rpAcctId, oracleDbConnection);

            Assert.NotNull(result);
            // Some accounts don't have group numbers.
            //Assert.False(result.GROUP_NO > 0);
            output.WriteLine(result.ToString());

        }
    }
}
