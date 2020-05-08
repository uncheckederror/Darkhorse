using DarkHorse.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkHorse.Mvc
{
    public class RealAccountGroupsDetail
    {
        public IEnumerable<RealPropertyAccountGroup> AccountGroups { get; set; }
        public RealPropertyAccountGroup SelectedAccountGroup { get; set; }
    }
}
