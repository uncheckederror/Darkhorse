using DarkHorse.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkHorse.Mvc
{
    public class RealAccountCadastralDetail
    {
        public IEnumerable<CadastralAction> Actions { get; set; }
    }
}
