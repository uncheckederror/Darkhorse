using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Darkhorse.Mvc.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Darkhorse.DataAccess;

namespace Darkhorse.Mvc.Models
{
    public class RealController : Controller
    {
        private readonly ILogger<RealController> _logger;
        private readonly IConfiguration _configuration;

        public RealController(ILogger<RealController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IDbConnection LISP
        {
            get
            {
                return new OracleConnection(_configuration.GetConnectionString("LISP"));
            }
        }

        public async Task<IActionResult> Index()
        {
            return View("Search");
        }

        public async Task<IActionResult> Account(string rpAcctId)
        {
            var checkRealAccountId = int.TryParse(rpAcctId, out int realAccountId);
            if (!checkRealAccountId)
            {
                return View("Search");
            }

            // Top panel data
            var results = await RealPropertyAccount.GetAsync(realAccountId, LISP.ConnectionString);
            var account = results.FirstOrDefault();
            var search = await RealPropertyAccountsFilter.GetAsync(account.ACCT_NO, LISP.ConnectionString);
            var searchAccount = search.FirstOrDefault();

            // Tabbed data
            var contacts = await Contacts.GetAsync(searchAccount.RP_ACCT_OWNER_ID, LISP.ConnectionString);
            var legal = await LegalDiscription.GetAsync(searchAccount.RP_ACCT_ID, LISP.ConnectionString);

            // Create an empty plat, in case there's not one for this account.
            var plat = new Plat();
            var checkPlat = int.TryParse(searchAccount.ACCT_NO.Substring(0, 2), out int platValue);
            if (checkPlat && platValue >= 37)
            {
                // Set the real plat here if it exists.
                plat = await Plat.GetNameAsync(searchAccount.ACCT_NO, LISP.ConnectionString);
            }

            var situses = await RealPropertySiteAddress.GetAsync(searchAccount.RP_ACCT_ID, LISP.ConnectionString);

            // TODO: Rewrite this section as a single query.
            var newConstruction = await NewConstruction.GetAsync(searchAccount.RP_ACCT_ID, LISP.ConnectionString);
            var ncPairs = new List<NewConstructionDetail>();
            foreach (var nc in newConstruction)
            {
                var inspections = await Inspection.GetAsync(nc.NEW_CONSTRUCTION_ID, LISP.ConnectionString);
                ncPairs.Add(new NewConstructionDetail
                {
                    NewConstruction = nc,
                    Inspections = inspections
                });
            }



            return View("Account", new RealAccountDetail
            {
                Account = account,
                Contacts = contacts,
                LegalDiscriptions = legal,
                Plat = plat,
                SiteAddresses = situses,
                Inspections = ncPairs
            });
        }

        public async Task<IActionResult> Search([Bind("AccountNumber", "AccountNumberSort", "ProcessNumber", "ActiveIndicator", "Contact", "ContactSort", "ContactType", "StreetNumber", "StreetName", "StreetNameSort", "SectionTownshipRange", "AccountGroup", "QuarterSection", "Tags")] RealAccountSearchResult query)
        {
            var accounts = new List<RealAccountSearchResult>();
            IEnumerable<RealPropertyAccountsFilter> results;
            if (query?.ProcessNumber > 100000)
            {
                results = await RealPropertyAccountsFilter.GetAsync(query.ProcessNumber, LISP.ConnectionString);
            }
            else
            {
                results = await RealPropertyAccountsFilter.GetAsync(query.AccountNumber, LISP.ConnectionString);
            }

            foreach (var result in results)
            {
                var realAccountYear = await RealPropertyAccountYears.GetRealAccountFiltersAsync(result.RP_ACCT_OWNER_ID, DateTime.Now.AddYears(1), LISP.ConnectionString);
                var tags = await AccountTags.GetAsync(result.RP_ACCT_OWNER_ID, LISP.ConnectionString);
                var accountGroup = await RealPropertyAccountGroups.GetAsync(result.RP_ACCT_OWNER_ID, LISP.ConnectionString);
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

            return View("Search", new AccountSearchResults
            {
                Query = accounts.FirstOrDefault(),
                Accounts = accounts
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
