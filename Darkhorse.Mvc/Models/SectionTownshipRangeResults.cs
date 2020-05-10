using DarkHorse.DataAccess;
using System.Collections.Generic;

namespace DarkHorse.Mvc
{
    public class SectionTownshipRangeResults
    {
        public IEnumerable<SectionTownshipRange> SearchResults { get; set; }
        public IEnumerable<SectionTownshipRange> RelatedPlats { get; set; }

    }
}