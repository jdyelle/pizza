using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspirationalPizza.Library.RepoSearch
{
    public abstract class SearchBase
    {
        private List<SearchCriteria> FilterRules;

        public SearchBase() { FilterRules = new List<SearchCriteria>(); }
        
        //attribute here should probably be an enum that's overridden by type or something.
        public void AddFilter(string attribute, string criteria, string value)
        {
            FilterRules.Add(new SearchCriteria(Attributes) { Attribute = attribute, Comparison = criteria, Value = value });
        }

        public List<SearchCriteria> GetFilters() { return FilterRules; }

        public abstract Dictionary<string, string> Attributes { get; }
    }
}
