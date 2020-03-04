using Darkhorse.DataAccess;
using System.Collections.Generic;

namespace Darkhorse.Mvc.Models
{
    public class Account
    {
        public IEnumerable<RealAccountSearch> Accounts { get; set; }
        public RealAccountSearch Query { get; set; }
    }
}
