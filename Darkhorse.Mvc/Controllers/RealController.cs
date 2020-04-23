using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

using DarkHorse.DataAccess;
using DarkHorse.Mvc.Models;

namespace DarkHorse.Mvc.Controllers
{
    public class RealController : Controller
    {
        private readonly ILogger<RealController> _logger;
        private readonly IConfiguration _configuration;
        private readonly bool _useOracleDB;
        private readonly string _dbConnectionString;

        public RealController(ILogger<RealController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _useOracleDB = _configuration["DatabaseSource"].Equals("LISP");
            _dbConnectionString = _useOracleDB
                ? _configuration.GetConnectionString("LISP")
                : _configuration.GetConnectionString("LISPROD");
        }

        private IDbConnection DbConnection => _useOracleDB
            ? new OracleConnection(_dbConnectionString) as IDbConnection
            : new SqlConnection(_dbConnectionString) as IDbConnection;

        public IActionResult Index()
        {
            return View("Search");
        }

        [Route("Real/Account/{rpAcctId}")]
        public async Task<IActionResult> Account(string rpAcctId, string page)
        {
            using var dbConnection = DbConnection;

            var checkRealAccountId = int.TryParse(rpAcctId, out int realAccountId);
            if (!checkRealAccountId)
            {
                return View("Search");
            }

            // Top panel data
            var results = await RealPropertyAccount.GetAsync(realAccountId, dbConnection);
            var account = results.FirstOrDefault();
            var search = await RealPropertyAccountsFilter.GetAsync(account.ACCT_NO, dbConnection);
            var searchAccount = search.FirstOrDefault();

            // Tabbed data
            var contacts = await Contact.GetAsync(searchAccount.RP_ACCT_OWNER_ID, dbConnection);
            var legal = await LegalDescription.GetAsync(searchAccount.RP_ACCT_ID, dbConnection);

            // Create an empty plat, in case there's not one for this account.
            var plat = new Plat();
            var checkPlat = int.TryParse(searchAccount.ACCT_NO.Substring(0, 2), out int platValue);
            if (checkPlat && platValue >= 37)
            {
                // Set the real plat here if it exists.
                plat = await Plat.GetNameAsync(searchAccount.ACCT_NO, dbConnection);
            }

            var situses = await RealPropertySiteAddress.GetAsync(searchAccount.RP_ACCT_ID, dbConnection);

            // TODO: Rewrite this section as a single query.
            var newConstruction = await NewConstruction.GetAsync(searchAccount.RP_ACCT_ID, dbConnection);
            var ncPairs = new List<NewConstructionDetail>();
            foreach (var nc in newConstruction)
            {
                var inspections = await Inspection.GetAsync(nc.NEW_CONSTRUCTION_ID, dbConnection);
                ncPairs.Add(new NewConstructionDetail
                {
                    NewConstruction = nc,
                    Inspections = inspections
                });
            }

            var accountGroup = await RealPropertyAccountGroup.GetAsync(searchAccount.RP_ACCT_OWNER_ID, dbConnection);
            var notices = await Notice.GetAsync(searchAccount.RP_ACCT_OWNER_ID, dbConnection);
            var sales = await SalesAccount.GetAsync(searchAccount.RP_ACCT_ID, dbConnection);
            var tags = await AccountTag.GetAsync(searchAccount.RP_ACCT_OWNER_ID, dbConnection);
            var crmContacts = await CrmContact.GetAsync(searchAccount.RP_ACCT_ID, dbConnection);
            var buildings = await Building.GetAsync(searchAccount.RP_ACCT_ID, dbConnection);

            // TODO: Display the mobile homes on the Buildings tab.
            //var mobileHomes = new List<MobileHome>();
            //foreach (var building in buildings)
            //{
            //    var mobileHome = await MobileHome.GetAsync(searchAccount.RP_ACCT_ID, building.BLDGNO, _dbConnection.ConnectionString);
            //    if (!string.IsNullOrWhiteSpace(mobileHome.MH_MAKE))
            //    {

            //    }
            //}

            return View("Account", new RealAccountDetail
            {
                Account = account,
                Contacts = contacts,
                LegalDescriptions = legal,
                Plat = plat,
                SiteAddresses = situses,
                Inspections = ncPairs,
                AccountGroups = accountGroup,
                // TODO: Fix this SQL query so that it doesn't return duplicates and we can remove this inefficent hack.
                Notices = notices.GroupBy(x => x.NOTICE_ID).Select(y => y.FirstOrDefault()),
                Sales = sales,
                Tags = tags,
                CrmContacts = crmContacts,
                Buildings = buildings
            });
        }

        [Route("Real/Search")]
        public async Task<IActionResult> Search([Bind("AccountNumber", "AccountNumberSort", "ProcessNumber", "ActiveIndicator", "Contact", "ContactSort", "ContactType", "StreetNumber", "StreetName", "StreetNameSort", "SectionTownshipRange", "AccountGroup", "QuarterSection", "Tags")] RealAccountSearchResult query)
        {
            using var dbConnection = DbConnection;

            var accounts = new List<RealAccountSearchResult>();
            IEnumerable<RealPropertyAccountsFilter> results;
            if (query?.ProcessNumber > 100000)
            {
                results = await RealPropertyAccountsFilter.GetAsync(query.ProcessNumber, dbConnection);
            }
            else if (!string.IsNullOrWhiteSpace(query?.AccountNumber))
            {
                results = await RealPropertyAccountsFilter.GetAsync(query.AccountNumber, dbConnection);
            }
            else
            {
                return View("Search");
            }

            foreach (var result in results)
            {
                var realAccountYear = await RealPropertyAccountYear.GetRealAccountFiltersAsync(result.RP_ACCT_OWNER_ID, DateTime.Now.AddYears(1), dbConnection);
                var tags = await AccountTag.GetCodeAsync(result.RP_ACCT_OWNER_ID, dbConnection);
                var accountGroup = await RealPropertyAccountGroup.GetNumberAsync(result.RP_ACCT_OWNER_ID, dbConnection);
                string outTags = string.Empty;
                foreach (var tag in tags)
                {
                    outTags += $"{tag?.TAG_CODE}, ";
                }
                var checkStreetNumber = int.TryParse(result?.STREET_NO, out var streetNumber);
                accounts.Add(new RealAccountSearchResult
                {
                    AccountNumber = result?.ACCT_NO,
                    ProcessNumber = result.RP_ACCT_ID,
                    ActiveIndicatior = GetActiveIndicatorFromAccountStatus(result),
                    Contact = result?.CONTACT_NAME,
                    ContactType = result?.CONTACT_TYPE,
                    StreetNumber = checkStreetNumber ? streetNumber : 0,
                    StreetName = result?.STREET_NAME,
                    StreetAddress = result?.STREET_ADDR,
                    SectionTownshipRange = result?.SEC_TWN_RNG,
                    AccountGroup = accountGroup.GROUP_NO,
                    QuarterSection = result?.QUARTER_SECTION,
                    Tags = outTags,
                    PropertyClass = realAccountYear.PROPERTY_CLASS,
                    ParcelAcreage = realAccountYear.PARCEL_ACREAGE,
                    TaxCode = realAccountYear.TAX_CODE
                });
            }

            if (accounts.Count == 1)
            {
                // TODO: Figure out how to use the Redirect method so the URL doesn't look messed up.
                return await Account(accounts.FirstOrDefault().ProcessNumber.ToString(), string.Empty);
            }
            else
            {
                return View("Search", new AccountSearchResult
                {
                    Query = accounts.FirstOrDefault(),
                    Accounts = accounts
                });
            }
        }

        [Route("Real/ChangeHistory/{rpAcctId}")]
        public async Task<IActionResult> ChangeHistory(string rpAcctId)
        {
            using var dbConnection = DbConnection;

            var checkRealAccountId = int.TryParse(rpAcctId, out int realAccountId);
            if (!checkRealAccountId)
            {
                return View("Search");
            }

            // Top panel data
            var results = await RealPropertyAccount.GetAsync(realAccountId, dbConnection);
            var account = results.FirstOrDefault();
            var search = await RealPropertyAccountsFilter.GetAsync(account.ACCT_NO, dbConnection);
            var searchAccount = search.FirstOrDefault();

            // Change history specific data.
            var history = await ATSHistory.GetAsync(searchAccount.RP_ACCT_ID, dbConnection);
            var remarks = await Remark.GetAsync(searchAccount.RP_ACCT_ID, dbConnection);
            return View("ChangeHistory", new ChangeHistoryDetail
            {
                Account = account,
                Histories = history,
                Remarks = remarks
            });
        }

        [Route("Real/TaxYears/{rpAcctId}")]
        public async Task<IActionResult> TaxYears(string rpAcctId, int year)
        {
            using var dbConnection = DbConnection;

            var checkRealAccountId = int.TryParse(rpAcctId, out int realAccountId);
            if (!checkRealAccountId)
            {
                return View("Search");
            }

            // Top panel data
            var results = await RealPropertyAccount.GetAsync(realAccountId, dbConnection);
            var account = results.FirstOrDefault();
            var search = await RealPropertyAccountsFilter.GetAsync(account.ACCT_NO, dbConnection);
            var searchAccount = search.FirstOrDefault();

            var taxYears = await RealPropertyAccountYear.GetAsync(searchAccount.RP_ACCT_OWNER_ID, dbConnection);
            // Get the current tax year by default.
            var taxYear = taxYears.Where(x => x.TAX_YR == DateTime.Now.Year).FirstOrDefault();
            if (year > 1970)
            {
                taxYear = taxYears.Where(x => x.TAX_YR == year).FirstOrDefault();
            }

            var senior = new SeniorCitizenRate();
            // Check for Senior Citizen status
            if (taxYear.TAX_STATUS == "S")
            {
                senior = await SeniorCitizenRate.GetAsync(taxYear.RP_ACCT_YR_ID, dbConnection);
            }

            var otherAssessments = await OtherAssessment.GetAsync(taxYear.RP_ACCT_YR_ID, taxYear.TAX_YR, dbConnection);
            var exemptions = await RealPropertyExemptions.GetAsync(searchAccount.RP_ACCT_OWNER_ID, taxYear.RP_ACCT_YR_ID, dbConnection);

            return View("TaxYears", new RealAccountTaxYearsDetail
            {
                Account = account,
                TaxYears = taxYears,
                TaxYear = taxYear,
                SeniorCitizen = senior,
                OtherAssessment = otherAssessments,
                RealPropertyExemptions = exemptions
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static string GetActiveIndicatorFromAccountStatus(RealPropertyAccountsFilter account)
        {
            switch (account?.ACCT_STATUS)
            {
                case 'A':
                    return "Active";
                // This might not be the right char.
                case 'R':
                    return "Reference";
                case 'D':
                    return "Inactive";
                default:
                    return string.Empty;
            }
        }
    }
}
