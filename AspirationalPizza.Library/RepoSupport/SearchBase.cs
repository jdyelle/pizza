using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspirationalPizza.Library.RepoSupport
{
    public abstract class SearchBase
    {
        private List<SearchCriteria> FilterRules;

        public SearchBase() { FilterRules = new List<SearchCriteria>(); }
        
        //attribute here should probably be an enum that's overridden by type or something.
        public void AddFilter(string attribute, string criteria, string value)
        {
            FilterRules.Add(new SearchCriteria(AttributeMeta) { Attribute = attribute, Comparison = criteria, Value = value });
        }

        public record Comparisons
        {
            public const string Equal = "Equal";
            public const string GreaterThan = "GreaterThan";
            public const string GreaterThanOrEquals = "GreaterThanOrEquals";
            public const string LessThan = "LessThan";
            public const string LessThanOrEquals = "LessThanOrEquals";
            public const string Contains = "Contains";
            public const string AnyOf = "AnyOf";
            public const string NotIn = "NotIn";
        }

        public List<SearchCriteria> GetFilters() { return FilterRules; }

        public abstract Dictionary<string, string> AttributeMeta { get; }
        public abstract record Attributes;
    }
}
