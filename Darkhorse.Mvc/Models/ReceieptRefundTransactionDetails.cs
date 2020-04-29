using DarkHorse.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkHorse.Mvc
{
    public class ReceieptRefundTransactionDetails
    {
        public ReceiptRefund ReceiptRefund { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}
