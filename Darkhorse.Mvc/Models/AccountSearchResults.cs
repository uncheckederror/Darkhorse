using Darkhorse.DataAccess;
using System.Collections.Generic;

namespace Darkhorse.Mvc.Models
{
    public class AccountSearchResults
    {
        public IEnumerable<RealAccountSearchResult> Accounts { get; set; }
        public RealAccountSearchResult Query { get; set; }
    }
}
