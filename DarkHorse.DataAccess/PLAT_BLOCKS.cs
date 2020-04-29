using System;
using System.Collections.Generic;
using System.Text;

namespace DarkHorse.DataAccess
{
    public class PLAT_BLOCKS : BaseTableClass
    {
        #region Fields

        public int PLAT_BLOCK_ID { get; set; }
        public int PLAT_ID { get; set; }
        public string BLOCK_NO { get; set; }
        public int NO_LOTS { get; set; }

        #endregion
    }
}
