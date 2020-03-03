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
            return View("Accounts");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search([Bind("AccountNumber", "AccountNumberSort", "ProcessNumber", "ActiveIndicator", "Contact", "ContactSort", "ContactType", "StreetNumber", "StreetName", "StreetNameSort", "SectionTownshipRange", "AccountGroup", "QuarterSection", "Tags")] RealAccountSearch query)
        {
            var results = await RpAccts.GetAsync(query.AccountNumber, LISP.ConnectionString);

            return View("Accounts", new AccountVM
            {
                Query = query,
                Accounts = results
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
    }
}
