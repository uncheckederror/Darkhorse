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
using System.Globalization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ServiceReference;
using OracleReports;

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
        public async Task<IActionResult> Account(string rpAcctId)
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
            var search = await RealPropertyAccountsFilter.GetByAccountNumberAsync(account.ACCT_NO, dbConnection);
            var searchAccount = search.FirstOrDefault();
            var owner = await RealAccountOwner.GetAsync(searchAccount.RP_ACCT_OWNER_ID, dbConnection);

            // Tabbed data
            var contacts = await Contact.GetAsync(searchAccount.RP_ACCT_OWNER_ID, dbConnection);
            var legal = await LegalDescription.GetAsync(searchAccount.RP_ACCT_ID, dbConnection);
            var zone = await RealAccountParcel.GetAsync(searchAccount.RP_ACCT_ID, dbConnection);

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

            var accountGroup = await RealPropertyAccountGroup.GetByRpAcctOwnerIdAsync(searchAccount.RP_ACCT_OWNER_ID, dbConnection);
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
                TaxService = owner,
                Contacts = contacts,
                LegalDescriptions = legal,
                Parcels = zone,
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
                results = await RealPropertyAccountsFilter.GetByRpAcctIdAsync(query.ProcessNumber, dbConnection);
            }
            else if (!string.IsNullOrWhiteSpace(query?.AccountNumber))
            {
                results = await RealPropertyAccountsFilter.GetByAccountNumberAsync(query.AccountNumber, dbConnection);
            }
            else if (!string.IsNullOrWhiteSpace(query?.SectionTownshipRange) && !string.IsNullOrWhiteSpace(query?.QuarterSection))
            {
                results = await RealPropertyAccountsFilter.GetBySTRAsync(query.SectionTownshipRange, query.QuarterSection, dbConnection);
            }
            else if (!string.IsNullOrWhiteSpace(query?.Contact))
            {
                results = await RealPropertyAccountsFilter.GetByNameAsync(query.Contact, dbConnection);
            }
            else if (query?.StreetNumber > 0 || !string.IsNullOrWhiteSpace(query?.StreetName) || query?.StreetNumber > 0 && !string.IsNullOrWhiteSpace(query?.StreetName.ToString()))
            {
                results = await RealPropertyAccountsFilter.GetByAddressAsync(query.StreetNumber.ToString(), query.StreetName, dbConnection);
            }
            else if (!string.IsNullOrWhiteSpace(query?.Tags))
            {
                var tags = query?.Tags.Replace(" ", "").Split(',').ToList();
                results = await RealPropertyAccountsFilter.GetByTagAsync(tags, dbConnection);
            }
            else if (query?.AccountGroup != null && query?.AccountGroup > 0)
            {
                results = await RealPropertyAccountsFilter.GetByAccountGroupAsync(query.AccountGroup, dbConnection);
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
                    Tags = outTags.Length > 0 ? outTags.Substring(0, outTags.Length - 2) : outTags,
                    PropertyClass = realAccountYear.PROPERTY_CLASS,
                    ParcelAcreage = realAccountYear.PARCEL_ACREAGE,
                    TaxCode = realAccountYear.TAX_CODE
                });
            }
            // TODO: Find a way to do this that doesn't maintain the original URL.
            //if (accounts.Count == 1)
            //{
            //    // TODO: Figure out how to use the Redirect method so the URL doesn't look messed up.
            //    return await Account(accounts.FirstOrDefault().ProcessNumber.ToString(), string.Empty);
            //}
            //else
            //{
            return View("Search", new AccountSearchResult
            {
                Query = accounts.FirstOrDefault(),
                Accounts = accounts
            });
            //}
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
            var search = await RealPropertyAccountsFilter.GetByAccountNumberAsync(account.ACCT_NO, dbConnection);
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


        [Route("Real/TaxService/{rpAcctId}")]
        public async Task<IActionResult> RealAccountTaxService(string rpAcctId)
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
            var search = await RealPropertyAccountsFilter.GetByAccountNumberAsync(account.ACCT_NO, dbConnection);
            var searchAccount = search.FirstOrDefault();
            var owner = await RealAccountOwner.GetAsync(searchAccount.RP_ACCT_OWNER_ID, dbConnection);

            // Change history specific data.
            var service = await TaxService.GetAsync(owner?.TAX_SERVICE_ID, dbConnection);
            return View("TaxService", new TaxServiceDetail
            {
                Account = account,
                TaxService = service
            });
        }

        [Route("Real/AccountHistory/{rpAcctId}")]
        public async Task<IActionResult> RealAccountAccountHistory(string rpAcctId, string contactId)
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
            var search = await RealPropertyAccountsFilter.GetByAccountNumberAsync(account.ACCT_NO, dbConnection);
            var searchAccount = search.FirstOrDefault();
            var owners = await RealAccountOwner.GetByRpAcctIdAsync(account.RP_ACCT_ID, dbConnection);
            var selectedOwner = owners.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(contactId))
            {
                selectedOwner = owners.Where(x => x.CONTACT_ID.ToString() == contactId).FirstOrDefault();
            }

            var contacts = await Contact.GetRealContactsByIdAsync(selectedOwner.CONTACT_ID, dbConnection);
            var accountGroups = await RealPropertyAccountGroup.GetByRpAcctOwnerIdAsync(selectedOwner.RP_ACCT_OWNER_ID, dbConnection);
            var notices = await Notice.GetAsync(selectedOwner.RP_ACCT_OWNER_ID, dbConnection);
            var tags = await AccountTag.GetAsync(searchAccount.RP_ACCT_OWNER_ID, dbConnection);

            return View("AccountHistory", new RealAccountHistoryDetail
            {
                Account = account,
                AccountsFilter = searchAccount,
                Owners = owners,
                SelectedOwner = selectedOwner,
                Contacts = contacts,
                AccountGroups = accountGroups,
                Notices = notices,
                AccountTags = tags
            });
        }

        [Route("Real/AccountGroup/{acctGroupId}")]
        public async Task<IActionResult> RealPropertyAccountGroups(string acctGroupId)
        {
            using var dbConnection = DbConnection;

            var checkRealAccountId = int.TryParse(acctGroupId, out int realPropertyAccountGroupId);
            if (!checkRealAccountId)
            {
                return View("Search");
            }

            var accountGroups = await RealPropertyAccountGroup.GetAsync(realPropertyAccountGroupId, dbConnection);
            var selectedAccountGroup = accountGroups.FirstOrDefault();

            return View("AccountGroups", new RealAccountGroupsDetail
            {
                AccountGroups = accountGroups,
                SelectedAccountGroup = selectedAccountGroup
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
            var search = await RealPropertyAccountsFilter.GetByAccountNumberAsync(account.ACCT_NO, dbConnection);
            var searchAccount = search.FirstOrDefault();

            var taxYears = await RealPropertyAccountYear.GetAsync(searchAccount.RP_ACCT_ID, dbConnection);
            // Get the current tax year by default.
            var taxYear = taxYears.Where(x => x.TAX_YR == DateTime.Now.Year).FirstOrDefault();
            if (year > 1970)
            {
                taxYear = taxYears.Where(x => x.TAX_YR == year).FirstOrDefault();
            }

            if (taxYear == null)
            {
                var mostRecent = taxYears.Max(y => y.TAX_YR);
                taxYear = taxYears.Where(x => x.TAX_YR == mostRecent).FirstOrDefault();
            }

            foreach (var y in taxYears)
            {
                var taxRate = await TaxCodeRateYear.GetAsync(y.TAX_STATUS, y.TAX_CODE, y.TAX_YR, dbConnection);
                y.TaxRate = taxRate.tax_rate ?? 0M;
            }

            var senior = new SeniorCitizenRate();
            // Check for Senior Citizen status
            if (taxYear.TAX_STATUS == "S")
            {
                senior = await SeniorCitizenRate.GetAsync(taxYear.RP_ACCT_YR_ID, dbConnection);
            }

            var otherAssessments = await OtherAssessment.GetAsync(taxYear.RP_ACCT_YR_ID, taxYear.TAX_YR, dbConnection);
            var exemptions = await RealPropertyExemptions.GetAsync(searchAccount.RP_ACCT_OWNER_ID, taxYear.RP_ACCT_YR_ID, dbConnection);
            // Example account 1685254
            var stormwaterType = await SSWMAssessment.GetTypeAsync(taxYear.SSWM_ASMT_ID ?? 0, dbConnection);
            var ffpAssessmentId = await FFPAssessment.GetRateAsync(taxYear.FFP_ASMT_ID ?? 0, dbConnection);
            var noxWeed = await NoxiousWeedAssessment.GetTypeAsync(taxYear.NOX_WEED_ASMT_ID, dbConnection);
            var adjustments = await RealPropertyAdjustment.GetAsync(taxYear.RP_ACCT_YR_ID, dbConnection);
            var prepayment = await RealPropertyPrepayment.GetAsync(taxYear.RP_ACCT_YR_ID, dbConnection);
            var payments = await RealPropertyYearPayment.GetAsync(taxYear.RP_ACCT_YR_ID, dbConnection);

            return View("TaxYears", new RealAccountTaxYearsDetail
            {
                Account = account,
                AccountsFilter = searchAccount,
                TaxYears = taxYears,
                TaxYear = taxYear,
                SeniorCitizen = senior,
                OtherAssessments = otherAssessments,
                RealPropertyExemptions = exemptions,
                StormwaterManagement = stormwaterType,
                FFPRate = ffpAssessmentId,
                NoxiousWeedAssessment = noxWeed,
                RealPropertyAdjustments = adjustments,
                RealPropertyPrepayments = prepayment,
                RealPropertyYearPayment = payments
            });
        }

        [Route("Real/Prorate/{rpAcctId}")]
        public async Task<IActionResult> StartProrateTaxYear(string rpAcctId, int year)
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
            var search = await RealPropertyAccountsFilter.GetByAccountNumberAsync(account.ACCT_NO, dbConnection);
            var searchAccount = search.FirstOrDefault();

            var taxYears = await RealPropertyAccountYear.GetAsync(searchAccount.RP_ACCT_ID, dbConnection);

            var taxYear = taxYears.Where(x => x.TAX_YR == year).FirstOrDefault();

            return View("Prorate", new RealAccountProrate
            {
                RpAcctId = account.RP_ACCT_ID,
                RpAcctYrId = taxYear.RP_ACCT_YR_ID,
                TaxYear = taxYear.TAX_YR
            });
        }

        [HttpPost]
        [Route("Real/Prorate/{rpAcctId}")]
        public async Task<IActionResult> CreateProrateTaxYear([Bind("RpAcctId", "RpAcctYrId", "TaxYear", "StartDate", "Reason")] RealAccountProrate prorate)
        {
            using var dbConnection = DbConnection;

            // Top panel data
            var results = await RealPropertyAccount.GetAsync(prorate.RpAcctId, dbConnection);
            var account = results.FirstOrDefault();
            var search = await RealPropertyAccountsFilter.GetByAccountNumberAsync(account.ACCT_NO, dbConnection);
            var searchAccount = search.FirstOrDefault();

            var taxYears = await RealPropertyAccountYear.GetAsync(searchAccount.RP_ACCT_ID, dbConnection);

            var taxYear = taxYears.Where(x => x.TAX_YR == prorate.TaxYear).FirstOrDefault();

            // TODO: Actually save this prorate data to the database at somepoint.

            return View("Prorate", new RealAccountProrate
            {
                RpAcctId = account.RP_ACCT_ID,
                RpAcctYrId = taxYear.RP_ACCT_YR_ID,
                TaxYear = taxYear.TAX_YR,
                Reason = prorate.Reason,
                StartDate = prorate.StartDate
            });
        }

        [Route("Real/SSWM/Rates")]
        public async Task<IActionResult> SSWMRates()
        {
            using var dbConnection = DbConnection;

            var assessments = await SSWMAssessment.GetAllAsync(dbConnection);

            return View("SSWM", assessments);
        }

        [Route("Real/FFP/Rates")]
        public async Task<IActionResult> FFPRates()
        {
            using var dbConnection = DbConnection;

            var assessments = await FFPAssessment.GetAllAsync(dbConnection);

            return View("FFP", assessments);
        }

        [Route("Real/NoxWeed/Rates")]
        public async Task<IActionResult> NoxWeeRates()
        {
            using var dbConnection = DbConnection;

            var assessments = await NoxiousWeedAssessment.GetAllAsync(dbConnection);

            return View("NoxWeed", assessments);
        }

        [Route("Real/Other/Rates")]
        public async Task<IActionResult> OtherRates()
        {
            using var dbConnection = DbConnection;

            var assessments = await OtherAssessmentTypes.GetAllAsync(dbConnection);

            return View("Other", assessments);
        }

        [Route("Real/Payments/{rpAcctId}")]
        public async Task<IActionResult> RealPropertyPayments(string rpAcctId)
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
            var search = await RealPropertyAccountsFilter.GetByAccountNumberAsync(account.ACCT_NO, dbConnection);
            var searchAccount = search.FirstOrDefault();

            var taxYears = await RealPropertyAccountYear.GetAsync(searchAccount.RP_ACCT_ID, dbConnection);


            // TODO: Verify this works and figure out where to get the Cashier record from.
            //var cashier = await Cashier.GetAsync("COUNTER1", dbConnection);

            return View("Payments", new RealAccountPaymentsDetail
            {
                Account = account,
                AccountsFilter = searchAccount
            });
        }

        [Route("Real/Receipts/{rpAcctId}")]
        public async Task<IActionResult> RealPropertyAccountReceipts(string rpAcctId, int? receipt)
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
            var search = await RealPropertyAccountsFilter.GetByAccountNumberAsync(account.ACCT_NO, dbConnection);
            var searchAccount = search.FirstOrDefault();
            var recieptRefunds = await ReceiptRefund.GetAsync(searchAccount.RP_ACCT_OWNER_ID, dbConnection);
            var selected = recieptRefunds.FirstOrDefault();

            if (receipt != null)
            {
                selected = recieptRefunds.Where(x => x.RECEIPT_ID == receipt).FirstOrDefault();
            }

            return View("Receipts", new RealAccountReceiptsDetail
            {
                Account = account,
                AccountsFilter = searchAccount,
                SelectedReceiptRefund = selected,
                ReceiptRefunds = recieptRefunds
            });
        }

        [Route("Real/Receipt/{receiptId}")]
        public async Task<IActionResult> RealPropertyAccountReceipt(int receiptId)
        {
            using var dbConnection = DbConnection;

            var transactions = await Transaction.GetAsync(receiptId, dbConnection);
            foreach (var transaction in transactions)
            {
                var fund = GetCollectionFundDescriptionFromId(transaction.COLLECT_FUND);
                transaction.FundDescription = fund;
            }

            var receipt = await ReceiptRefund.GetSingleReceiptAsync(receiptId, dbConnection);

            return View("Transactions", new ReceieptRefundTransactionDetails
            {
                ReceiptRefund = receipt,
                Transactions = transactions
            });
        }

        [Route("Real/ReceiptBatch/{receiptBatchId}")]
        public async Task<IActionResult> RealPropertyAccountReceiptBatchPayments(int receiptBatchId)
        {
            using var dbConnection = DbConnection;

            var payments = await PaymentReceipt.GetAsync(receiptBatchId, dbConnection);

            return View("ReceiptBatchPayments", payments);
        }

        [Route("Real/EnterPrepayment/{rpAcctId}")]
        public async Task<IActionResult> RealPropertyEnterPrepayment(string rpAcctId)
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

            return View("CalculatePrepayment", new CalculatePrepaymentDetail
            {
                RealPropertyAccountId = account.RP_ACCT_ID
            });
        }

        [Route("Real/CalcPrepayment/{rpAcctId}")]
        public async Task<IActionResult> RealPropertyCalculatePrepayment(string rpAcctId, [Bind("Month")] CalculatePrepaymentDetail Result)
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
            var search = await RealPropertyAccountsFilter.GetByAccountNumberAsync(account.ACCT_NO, dbConnection);
            var searchAccount = search.FirstOrDefault();

            // This is the C# version
            //var calculated = await CalculatePrepaymentAmounts.GetAsync(account.ACCT_NO, dbConnection);
            //calculated.Calculate(Result.Month);

            // This is the Stored Proc
            var calculated = await CalculatePrepaymentAmounts.GetStoredProcAsync("RP", account.ACCT_NO, CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Result.Month), dbConnection);

            Result.TotalDue = calculated.SignUpDue;
            Result.MonthlyFee = 2.00M;
            Result.MonthlyPayment = calculated.MontlyDue;
            Result.RealPropertyAccountId = account.RP_ACCT_ID;

            return View("CalculatePrepayment", Result);
        }

        [Route("Real/Contact/{contactId}")]
        public async Task<IActionResult> RealPropertyAccountContact(int contactId)
        {
            using var dbConnection = DbConnection;

            var contacts = await Contact.GetRealContactsByIdAsync(contactId, dbConnection);
            var numbers = await ContactNumber.GetAsync(contactId, dbConnection);

            return View("Contact", new RealAccountContactDetail
            {
                Contacts = contacts,
                SelectedContact = contacts.FirstOrDefault(),
                PhoneNumbers = numbers
            });
        }

        [Route("Real/SectionTownshipRange")]
        public async Task<IActionResult> SectionTownshipRangeLookup(string str)
        {
            using var dbConnection = DbConnection;

            var sectionTownShipRanges = await SectionTownshipRange.GetAsync(str, dbConnection);
            if (sectionTownShipRanges.Count() == 1)
            {
                var relatedPlats = await SectionTownshipRange.GetRelatedPlatsAsync(sectionTownShipRanges.FirstOrDefault().SEC_TWN_RNG_ID, dbConnection);

                return View("SectionTownshipRange", new SectionTownshipRangeResults
                {
                    SearchResults = sectionTownShipRanges,
                    RelatedPlats = relatedPlats
                });
            }
            else
            {
                return View("SectionTownshipRange", new SectionTownshipRangeResults
                {
                    SearchResults = sectionTownShipRanges
                });
            }
        }

        [Route("Real/Cadastral/")]
        public async Task<IActionResult> RealPropertyCadastral(int? an)
        {
            using var dbConnection = DbConnection;

            // Remap to a more descriptive name, while keeping the URL simple.
            var actionNumber = an ?? 0;

            var curretTaxYear = new DateTime(2020, 1, 1);

            if (an > 1999)
            {
                var actions = await CadastralAction.GetByIdAsync(curretTaxYear, actionNumber, dbConnection);

                return View("Cadastral", new RealAccountCadastralDetail
                {
                    Actions = actions
                });
            }
            else
            {
                var actions = await CadastralAction.GetAllAsync(curretTaxYear, dbConnection);

                return View("Cadastral", new RealAccountCadastralDetail
                {
                    Actions = actions
                });
            }
        }

        [Route("Real/Cadastral/New")]
        public async Task<IActionResult> CreateNewRealPropertyCadastral()
        {
            using var dbConnection = DbConnection;

            var plats = await Plat.GetAllAsync(dbConnection);

            return View("NewCadastral", new RealAccountCadastralDetail
            {
                Actions = new List<CadastralAction>(),
                SelectedAction = new CadastralAction(),
                Plats = plats
            });
        }

        [Route("Real/Cadastral/{cadastralActionNumber}")]
        public async Task<IActionResult> RealPropertyCadastralAction(int cadastralActionNumber)
        {
            using var dbConnection = DbConnection;

            var action = await CadastralAction.GetByActionNumberAsync(cadastralActionNumber, dbConnection);
            if (action.PLAT_ID != null)
            {
                // This doesn't make sense, because I've already checked for the null before I use the nullcoalesing operator here.
                var plat = await Plat.GetByIdAsync(action.PLAT_ID ?? 0, dbConnection);
                action.PLAT_NAME = plat.PLAT_NAME;
                action.PLAT_NO = plat.PLAT_NO;
            }

            var workgroup = await CadastralAction.GetWorkGroupByIdAsync(action.CADASTRAL_ACTN_ID, dbConnection);
            var steps = await CadastralStep.GetAsync(action.CADASTRAL_ACTN_ID, dbConnection);
            var docs = await CadastralDoc.GetAsync(action.CADASTRAL_ACTN_ID, dbConnection);
            var warnings = await Remark.GetWarningsAsync(action.CADASTRAL_ACTN_ID, dbConnection);

            return View("NewCadastral", new RealAccountCadastralDetail
            {
                Actions = workgroup,
                SelectedAction = action,
                Steps = steps,
                Documents = docs,
                Warnings = warnings
            });
        }

        [Route("Real/Nonprofit")]
        public async Task<IActionResult> RealPropertyNonprofit(int nonProfitAccountId)
        {
            using var dbConnection = DbConnection;

            if (nonProfitAccountId > 99)
            {
                var selectedAccount = await NonprofitAccount.GetAsync(nonProfitAccountId, dbConnection);
                var account = selectedAccount.FirstOrDefault();

                if (selectedAccount.Count() == 0)
                {
                    return View("Nonprofit");
                }

                var years = await NonprofitAccount.GetAccountYearAsync(account.NON_PROFIT_ACCT_ID, dbConnection);

                foreach (var year in years)
                {
                    var flag = await NonprofitAccount.GetAppliedExemptionAsync(year.NON_PROFIT_ACCT_YR_ID, dbConnection);
                    year.NON_PROFIT_FLAG = flag.NON_PROFIT_FLAG;
                }

                return View("Nonprofit", new RealAccountNonprofitDetail
                {
                    SelectedAccount = account,
                    AccountYears = years
                });
            }
            else
            {
                var accounts = await NonprofitAccount.GetAllAsync(dbConnection);
                var account = accounts.FirstOrDefault();

                var selectedAccount = await NonprofitAccount.GetAsync(account.NON_PROFIT_ID, dbConnection);
                account = selectedAccount.FirstOrDefault();

                var years = await NonprofitAccount.GetAccountYearAsync(account.NON_PROFIT_ACCT_ID, dbConnection);
                foreach (var year in years)
                {
                    var flag = await NonprofitAccount.GetAppliedExemptionAsync(year.NON_PROFIT_ACCT_YR_ID, dbConnection);
                    year.NON_PROFIT_FLAG = flag.NON_PROFIT_FLAG;
                }

                return View("Nonprofit", new RealAccountNonprofitDetail
                {
                    SelectedAccount = account,
                    AccountYears = years
                });
            }
        }

        [Route("Real/Reports")]
        public async Task<IActionResult> RealPropertyReportsByReportName(string reportName)
        {
            if (string.IsNullOrWhiteSpace(reportName))
            {
                return View("RealReports");
            }

            using var dbConnection = DbConnection;

            var client = new RWWebServiceClient();
            client.ClientCredentials.UserName.UserName = _configuration.GetConnectionString("ReportsUsername");
            client.ClientCredentials.UserName.Password = _configuration.GetConnectionString("ReportsPassword");
            var serverName = _configuration.GetConnectionString("ReportsServer");
            // The format is username/password/database
            var credentialsString = $"{_configuration.GetConnectionString("ReportsUsername")}/{_configuration.GetConnectionString("ReportsPassword")}@";
            if (serverName.Contains("test"))
            {
                // The test reports on the test reports server are complied to run against the LISS database.
                credentialsString += "liss";
            }
            var job = new Job();
            var parameters = new List<FormParameter>();
            switch (reportName)
            {
                case "rckpendingrpt":
                    job = await Job.StartJobAsync(client, serverName, reportName, credentialsString, parameters);
                    break;
                case "LISRPARDELQACCTS":
                    job = await Job.StartJobAsync(client, serverName, reportName, credentialsString, parameters);
                    break;
                case "LISRPARLTROFDMD":
                    parameters = new List<FormParameter>
                                    {
                                        new FormParameter
                                        {
                                            Name = "PF_foreclose_month_day",
                                            Value = "1201"
                                        },
                                        new FormParameter
                                        {
                                            Name = "PF_rp_acct_id",
                                            Value = "2385011"
                                        }
                                    };

                    job = await Job.StartJobAsync(client, serverName, reportName, credentialsString, parameters);
                    break;
            }

            if (job.JobId == 0)
            {
                return View("RealReports", $"Report {reportName} failed to start. Please contact support.");
            }

            bool jobFinished = false;
            var responseStatus = await client.getJobInfoAsync(job.JobId, serverName, string.Empty);
            while (!jobFinished)
            {
                await Task.Delay(1000);
                job = Job.ParseFromStatusResponse(responseStatus);
                if (job.StatusCode == 4)
                    if (job.StatusCode == 4)
                    {
                        // Sucess
                        jobFinished = true;
                    }
                if (job.StatusCode == 5)
                {
                    // Failure
                    jobFinished = true;
                }
                if (job.StatusCode == 2)
                {
                    jobFinished = false;
                }
                responseStatus = await client.getJobInfoAsync(job.JobId, serverName, string.Empty);
            }

            return Redirect($"http://kclis3.co.kitsap.local:9002/reports/rwservlet/getjobid{job.JobId}?server=reportservertest");
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

        public static string GetCollectionFundDescriptionFromId(string collectionFundId)
        {
            switch (collectionFundId)
            {
                case "R99901":
                    return "Real Property Taxes Paid";
                case "R99902":
                    return "Real Property Interest Paid";
                case "R99903":
                    return "Real Property Penalty Paid";
                case "R99905":
                    return "Real Property Advance Payment";
                case "R99906":
                    return "Real Property Prepayment";
                case "P99901":
                    return "Personal Property Taxes Paid";
                case "P99902":
                    return "Personal Property Interest Paid";
                case "P99903":
                    return "Personal Property Penalty Paid";
                case "P99904":
                    return "Late Filing Penalty Paid";
                // This bug is verbatim from the PL/SQL source code.
                //case "P99905":
                //    return "Personal Property Advance Payment";
                case "P99905":
                    return "Personal Property Prepayment";
                case "P00901":
                    return "PP Cert Othr Cnty Paid";
                case "P00902":
                    return "PP Cert Othr Interest Paid";
                case "F99901":
                    return "FFP Paid";
                case "A00801":
                    return "Noxious Weed Asmt Paid";
                case "A00802":
                    return "Noxious Weed Interest Paid";
                default:
                    break;
            }

            // TODO: Maybe Regex here?
            // This won't work because Contains doesn't support wildcard chars.
            //if (collectionFundId.Contains("A%01"))
            //{
            //    return "SSWM/Abatement Paid";
            //}
            //if (collectionFundId.Contains("A%02"))
            //{
            //    return "SSWM/Abatement Interest Paid";
            //}
            //if (collectionFundId.Contains("L%01"))
            //{
            //    return "LID Taxes Paid";
            //}
            //if (collectionFundId.Contains("L%02"))
            //{
            //    return "LID Taxes Paid";
            //}
            //if (collectionFundId.Contains("L%03"))
            //{
            //    return "LID Penalty Paid";
            //}

            if (collectionFundId.Contains("A") && collectionFundId.Contains("01"))
            {
                return "SSWM/Abatement Paid";
            }
            if (collectionFundId.Contains("A") && collectionFundId.Contains("02"))
            {
                return "SSWM/Abatement Interest Paid";
            }
            if (collectionFundId.Contains("L") && collectionFundId.Contains("01"))
            {
                return "LID Taxes Paid";
            }
            if (collectionFundId.Contains("L") && collectionFundId.Contains("02"))
            {
                return "LID Taxes Paid";
            }
            if (collectionFundId.Contains("L") && collectionFundId.Contains("03"))
            {
                return "LID Penalty Paid";
            }

            switch (collectionFundId)
            {
                case "D99901":
                    return "Collection Costs";
                case "N99901":
                    return "NSF Cost";
                case "E99901":
                    return "Prepayment Fee";
                default:
                    break;
            }

            // If none of these switch statements and if statements have returned yet, then give back nothing.
            return string.Empty;
        }
    }
}
