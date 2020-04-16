using DarkHorse.DataAccess;
using System.Collections.Generic;

namespace DarkHorse.Mvc.Models
{
    public class AccountSearchResult
    {
        public IEnumerable<RealAccountSearchResult> Accounts { get; set; }
        public RealAccountSearchResult Query { get; set; }
    }
}
