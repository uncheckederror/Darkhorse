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
        public CadastralAction SelectedAction { get; set; }
        public IEnumerable<Plat> Plats { get; set; }
        public IEnumerable<CadastralStep> Steps { get; set; }
        public IEnumerable<CadastralDoc> Documents { get; set; }
        public IEnumerable<Remark> Warnings { get; set; }
    }
}
