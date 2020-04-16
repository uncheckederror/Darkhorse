using DarkHorse.DataAccess;
using System.Collections.Generic;

namespace DarkHorse.Mvc.Models
{
    public class ChangeHistoryDetail
    {
        public RealPropertyAccount Account { get; set; }
        public IEnumerable<ATSHistory> Histories { get; set; }
        public IEnumerable<Remark> Remarks { get; set; }
    }
}
