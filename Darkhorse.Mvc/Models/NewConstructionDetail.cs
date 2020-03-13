using Darkhorse.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Darkhorse.Mvc.Models
{
    public class NewConstructionDetail
    {
        public NewConstruction NewConstruction { get; set; }
        public IEnumerable<Inspection> Inspections { get; set; }
    }
}
