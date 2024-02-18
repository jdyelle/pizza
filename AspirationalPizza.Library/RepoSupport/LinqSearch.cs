using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AspirationalPizza.Library.RepoSupport
{
    // This is a lambda expression builder that *should* be able to Linq search repositories based on passed criteria objects.
    // TODO: Linq nested object parsing/searching isn't working correctly because of the way we invoke properties - that's going to be a research project.
    internal class LinqSearch<T>
    {
        internal static Func<T, bool> FilterBuilder(SearchBase searchObject)
        {
            List<SearchCriteria> CriteriaList = searchObject.GetFilters();
            Dictionary<String, String> Attributes = searchObject.AttributeMeta;
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "x");
            Expression? filterExpression = null;

            foreach (SearchCriteria criteria in CriteriaList)
            {
                // comparison operator (this will be from SearchCriteria._criteriaValues)
                Expression? comparison = null;

                if (Attributes[criteria.Attribute] == "String")
                {
                    MemberExpression property = Expression.PropertyOrField(parameterExpression, criteria.Attribute);
                    ConstantExpression constant = Expression.Constant(criteria.Value);
                    comparison = criteria switch
                    {
                        { Comparison: "Equal" } => comparison = Expression.Equal(property, constant),
                        { Comparison: "Contains" } => comparison = Expression.Call(property, "Contains", Type.EmptyTypes, constant),
                        { Comparison: "NotIn" } => comparison = Expression.Not(Expression.Call(property, "Contains", Type.EmptyTypes, constant)),
                        { Comparison: _ } => throw new ArgumentException("Filter combination doesn't make sense")
                    };
                }
                else if (Attributes[criteria.Attribute] == "String[]")
                {
                    MemberExpression property = Expression.PropertyOrField(parameterExpression, criteria.Attribute);
                    ConstantExpression constant = Expression.Constant(criteria.Value);
                    comparison = criteria switch
                    {
                        { Comparison: "Contains" } => comparison = Expression.Call(property, "Contains", Type.EmptyTypes, constant),
                        { Comparison: "NotIn" } => comparison = Expression.Not(Expression.Call(property, "Contains", Type.EmptyTypes, constant)),
                        { Comparison: _ } => throw new ArgumentException("Filter combination doesn't make sense")
                    };
                }
                else if (Attributes[criteria.Attribute] == "Number")
                {
                    MemberExpression property = Expression.PropertyOrField(parameterExpression, criteria.Attribute);
                    ConstantExpression constant = Expression.Constant(double.Parse(criteria.Value));
                    comparison = criteria switch
                    {
                        { Comparison: "Equal" } => comparison = Expression.Equal(property, constant),
                        { Comparison: "GreaterThan" } => comparison = Expression.GreaterThan(property, constant),
                        { Comparison: "GreaterThanOrEquals" } => comparison = Expression.GreaterThanOrEqual(property, constant),
                        { Comparison: "LessThan" } => comparison = Expression.LessThan(property, constant),
                        { Comparison: "LessThanOrEquals" } => comparison = Expression.LessThanOrEqual(property, constant),
                        { Comparison: _ } => throw new ArgumentException("Filter combination doesn't make sense")
                    };
                }
                else if (Attributes[criteria.Attribute] == "Date")
                {
                    MemberExpression property = Expression.PropertyOrField(parameterExpression, criteria.Attribute);
                    ConstantExpression constant = Expression.Constant(DateTime.Parse(criteria.Value));
                    comparison = criteria switch
                    {
                        { Comparison: "Equal" } => comparison = Expression.Equal(property, constant),
                        { Comparison: "GreaterThan" } => comparison = Expression.GreaterThan(property, constant),
                        { Comparison: "GreaterThanOrEquals" } => comparison = Expression.GreaterThanOrEqual(property, constant),
                        { Comparison: "LessThan" } => comparison = Expression.LessThan(property, constant),
                        { Comparison: "LessThanOrEquals" } => comparison = Expression.LessThanOrEqual(property, constant),
                        { Comparison: _ } => throw new ArgumentException("Filter combination doesn't make sense")
                    };
                }
                else { throw new ArgumentException("Parameter Type has no logical operator for selected comparison"); }
                if (comparison == null) { throw new Exception("This exception should be caught by lack of argument above"); }

                //Add the current comparison to the filterExpression
                filterExpression = (filterExpression == null) ? comparison : Expression.And(filterExpression, comparison);

            }

            if (filterExpression == null) { return x => true; }

            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameterExpression);
            return lambda.Compile();
        }

        // https://stackoverflow.com/questions/51753165/dynamically-generate-linq-select-with-nested-properties
        public static Expression<Func<TSource, TTarget>> BuildSelector<TSource, TTarget>(string members) =>
        BuildSelector<TSource, TTarget>(members.Split(',').Select(m => m.Trim()));

        public static Expression<Func<TSource, TTarget>> BuildSelector<TSource, TTarget>(IEnumerable<string> members)
        {
            var parameter = Expression.Parameter(typeof(TSource), "e");
            var body = NewObject(typeof(TTarget), parameter, members.Select(m => m.Split('.')));
            return Expression.Lambda<Func<TSource, TTarget>>(body, parameter);
        }

        static Expression NewObject(Type targetType, Expression source, IEnumerable<string[]> memberPaths, int depth = 0)
        {
            var bindings = new List<MemberBinding>();
            var target = Expression.Constant(null, targetType);
            foreach (var memberGroup in memberPaths.GroupBy(path => path[depth]))
            {
                var memberName = memberGroup.Key;
                var targetMember = Expression.PropertyOrField(target, memberName);
                var sourceMember = Expression.PropertyOrField(source, memberName);
                var childMembers = memberGroup.Where(path => depth + 1 < path.Length);
                var targetValue = !childMembers.Any() ? sourceMember :
                    NewObject(targetMember.Type, sourceMember, childMembers, depth + 1);
                bindings.Add(Expression.Bind(targetMember.Member, targetValue));
            }
            return Expression.MemberInit(Expression.New(targetType), bindings);
        }
    }
}
