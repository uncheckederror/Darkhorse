using DarkHorse.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkHorse.Mvc
{
    public class RealAccountReceiptsDetail
    {
        public RealPropertyAccount Account { get; set; }
        public RealPropertyAccountsFilter AccountsFilter { get; set; }
        public IEnumerable<ReceiptRefund> ReceiptRefunds { get; set; }
    }
}
