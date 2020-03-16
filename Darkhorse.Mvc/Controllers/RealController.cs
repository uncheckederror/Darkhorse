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

        public async Task<IActionResult> Account(string rpAcctId, string page)
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
            var contacts = await Contact.GetAsync(searchAccount.RP_ACCT_OWNER_ID, LISP.ConnectionString);
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

            var accountGroup = await RealPropertyAccountGroup.GetAsync(searchAccount.RP_ACCT_OWNER_ID, LISP.ConnectionString);
            var notices = await Notice.GetAsync(searchAccount.RP_ACCT_OWNER_ID, LISP.ConnectionString);
            var sales = await SalesAccount.GetAsync(searchAccount.RP_ACCT_ID, LISP.ConnectionString);
            var tags = await AccountTags.GetAsync(searchAccount.RP_ACCT_OWNER_ID, LISP.ConnectionString);
            var crmContacts = await CrmContact.GetAsync(searchAccount.RP_ACCT_ID, LISP.ConnectionString);
            var buildings = await Building.GetAsync(searchAccount.RP_ACCT_ID, LISP.ConnectionString);
            // TODO: Display the mobile homes on the Buildings tab.
            //var mobileHomes = new List<MobileHome>();
            //foreach (var building in buildings)
            //{
            //    var mobileHome = await MobileHome.GetAsync(searchAccount.RP_ACCT_ID, building.BLDGNO, LISP.ConnectionString);
            //    if (!string.IsNullOrWhiteSpace(mobileHome.MH_MAKE))
            //    {

            //    }
            //}

            if (string.IsNullOrWhiteSpace(page))
            {
                return View("Account", new RealAccountDetail
                {
                    Account = account,
                    Contacts = contacts,
                    LegalDiscriptions = legal,
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
            else
            {
                switch (page)
                {
                    case "ChangeHistory":
                        var history = await ATSHistory.GetAsync(searchAccount.RP_ACCT_ID, LISP.ConnectionString);
                        var remarks = await Remark.GetAsync(searchAccount.RP_ACCT_ID, LISP.ConnectionString);
                        return View("ChangeHistory", new ChangeHistoryDetail
                        {
                            Account = account,
                            Histories = history,
                            Remarks = remarks
                        });
                }

                return View("Account", new RealAccountDetail
                {
                    Account = account,
                    Contacts = contacts,
                    LegalDiscriptions = legal,
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
                var tags = await AccountTags.GetCodeAsync(result.RP_ACCT_OWNER_ID, LISP.ConnectionString);
                var accountGroup = await RealPropertyAccountGroup.GetNumberAsync(result.RP_ACCT_OWNER_ID, LISP.ConnectionString);
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
