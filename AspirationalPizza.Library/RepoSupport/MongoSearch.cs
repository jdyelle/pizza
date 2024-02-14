using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspirationalPizza.Library.RepoSupport
{
    internal class MongoSearch<T>
    {
        internal static FilterDefinition<T> FilterBuilder(SearchBase searchObject)
        {
            List<SearchCriteria> CriteriaList = searchObject.GetFilters();
            Dictionary<String, String> Attributes = searchObject.Attributes;
            FilterDefinitionBuilder<T> builder = Builders<T>.Filter;
            FilterDefinition<T> filter = builder.Empty;
            foreach (SearchCriteria _rule in CriteriaList)
            {
                if (Attributes[_rule.Attribute] == "String" || Attributes[_rule.Attribute] == "String[]")
                {
                    FilterDefinition<T> filterDefinition = _rule switch
                    {
                        { Comparison: "Equals" } => builder.Eq(_rule.Attribute, _rule.Value),
                        { Comparison: "Contains" } => builder.StringIn(_rule.Attribute, _rule.Value),
                        { Comparison: "AnyOf" } => builder.AnyIn(_rule.Attribute, _rule.Value),
                        { Comparison: "NotIn" } => builder.Nin(_rule.Attribute, _rule.Value),
                        { Comparison: _ } => throw new ArgumentException("Filter combination doesn't make sense")

                    };
                    filter &= builder.And(filterDefinition);
                }
                else if (Attributes[_rule.Attribute] == "Number" || Attributes[_rule.Attribute] == "DateTime")
                {
                    FilterDefinition<T> filterDefinition = _rule switch
                    {
                        { Comparison: "Equals" } => builder.Eq(_rule.Attribute, _rule.Value),
                        { Comparison: "GreaterThan" } => builder.Gt(_rule.Attribute, _rule.Value),
                        { Comparison: "GreaterThanOrEquals" } => builder.Gte(_rule.Attribute, _rule.Value),
                        { Comparison: "LessThan" } => builder.Lt(_rule.Attribute, _rule.Value),
                        { Comparison: "LessThanOrEquals" } => builder.Lte(_rule.Attribute, _rule.Value),
                        { Comparison: _ } => throw new ArgumentException("Filter combination doesn't make sense")
                    };
                    filter &= builder.And(filterDefinition);
                }
                else throw new Exception("The Criteria was not in the list of accepted values");
            }

            return filter;
        }
    }
}
