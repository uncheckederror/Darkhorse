using System;
using System.Collections.Generic;
using System.Text;

namespace DarkHorse.DataAccess
{
    public class PLAT_SECTIONS : BaseTableClass
    {
        #region Fields

        public int PLAT_SECTION_ID { get; set; }
        public int PLAT_ID { get; set; }
        public int SEC_TWN_RNG_ID { get; set; }

        #endregion
    }
}
