using Darkhorse.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Darkhorse.Mvc.Models
{
    public class AccountVM
    {
        public IEnumerable<RpAccts> Accounts { get; set; }
        public RealAccountSearch Query { get; set; }
    }
}
