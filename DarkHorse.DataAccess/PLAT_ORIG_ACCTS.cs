using System;
using System.Collections.Generic;
using System.Text;

namespace DarkHorse.DataAccess
{
    public class PLAT_ORIG_ACCTS : BaseTableClass
    {
        #region Fields

        public int PLAT_ORIG_ACCT_ID { get; set; }
        public int PLAT_ID { get; set; }
        public string ACCT_NO { get; set; }

        #endregion
    }
}
