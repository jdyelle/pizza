using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AspirationalPizza.Library.RepoSupport
{
    public class SearchCriteria
    {
        private string _attribute = String.Empty;
        private string _comparison = String.Empty;
        private Dictionary<String, String> _attributesList;
        /// <summary>AND / OR</summary>
        //public string Logic { get; set; }

        public SearchCriteria(Dictionary<String, String> ValidAttributes) 
        { 
            _attributesList = ValidAttributes;
        }

        /// <summary>Field (ex. Party.Name.Fullname) </summary>
        public string Attribute
        {
            get { return _attribute; }
            set
            {
                if (_attributesList.ContainsKey(value)) _attribute = value;
                else throw new ArgumentException("Attribute is not a valid filter item for this object");
            }
        }

        /// <summary>Equals, Greater, Less, Contains </summary>
        public string Comparison
        {
            get { return _comparison; }
            set
            {
                if (_criteriaValues.Contains(value)) _comparison = value;
                else throw new ArgumentException("Comparison is not a valid filter item for object");
            }
        }

        /// <summary>"A" / ["X","Y","Z"] </summary>
        public string Value { get; set; } = String.Empty;

        // Technically this shouldn't be necessary if they're using SearchBase.Comparisons, but a string may have been passed.
        private readonly List<String> _criteriaValues = new List<String>
        {
            "Equal",
            "GreaterThan",
            "GreaterThanOrEquals",
            "LessThan",
            "LessThanOrEquals",
            "Contains",
            "AnyOf",
            "NotIn"
        };

        //Not used yet, but something I picked up along the way.
        public enum Compare
        {
            Or = ExpressionType.Or,
            And = ExpressionType.And,
            Xor = ExpressionType.ExclusiveOr,
            Not = ExpressionType.Not,
            Equal = ExpressionType.Equal,
            Like = ExpressionType.TypeIs + 1,
            NotEqual = ExpressionType.NotEqual,
            OrElse = ExpressionType.OrElse,
            AndAlso = ExpressionType.AndAlso,
            LessThan = ExpressionType.LessThan,
            GreaterThan = ExpressionType.GreaterThan,
            LessThanOrEqual = ExpressionType.LessThanOrEqual,
            GreaterThanOrEqual = ExpressionType.GreaterThanOrEqual
        }
    }
}
