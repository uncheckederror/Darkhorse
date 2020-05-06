using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DarkHorse.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace DarkHorse.Mvc.Controllers
{
    public class CommonController : Controller
    {
        private readonly ILogger<CommonController> _logger;
        private readonly IConfiguration _configuration;
        private readonly bool _useOracleDB;
        private readonly string _dbConnectionString;

        public CommonController(ILogger<CommonController> logger, IConfiguration configuration)
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
            return View();
        }

        [Route("Common/Contact/{contactId}")]
        public async Task<IActionResult> GetContact(int contactId)
        {
            using var dbConnection = DbConnection;

            var contacts = await Contact.GetByIdAsync(contactId, dbConnection);
            var numbers = await ContactNumber.GetAsync(contactId, dbConnection);

            return View("Contact", new CommonContactDetail
            {
                Contacts = contacts,
                SelectedContact = contacts.FirstOrDefault(),
                PhoneNumbers = numbers
            });
        }

        [Route("Common/ContactRelatedAccounts/{contactId}")]
        public async Task<IActionResult> GetContactRelatedAccounts(int contactId)
        {
            using var dbConnection = DbConnection;

            var relatedContacts = await ContactAccount.GetAsync(contactId, dbConnection);

            return View("RelatedAccounts", relatedContacts);
        }
    }
}