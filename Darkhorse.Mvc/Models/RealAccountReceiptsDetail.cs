using DarkHorse.DataAccess;

using System.Collections.Generic;

namespace DarkHorse.Mvc
{
    public class RealAccountReceiptsDetail
    {
        public RealPropertyAccount Account { get; set; }
        public RealPropertyAccountsFilter AccountsFilter { get; set; }
        public ReceiptRefund SelectedReceiptRefund { get; set; }
        public IEnumerable<ReceiptRefund> ReceiptRefunds { get; set; }
    }
}
