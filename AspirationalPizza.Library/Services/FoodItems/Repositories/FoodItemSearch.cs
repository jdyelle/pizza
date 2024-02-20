using System;
using System.Collections.Generic;
using AspirationalPizza.Library.RepoSupport;

namespace AspirationalPizza.Library.Services.FoodItems.Repositories
{
    public class FoodItemSearch : SearchBase
    {
        public FoodItemSearch() : base() { }

        /// <summary> Pass this to a new SearchCriteria as valid attributes and types to build a filter.
        /// These are type specific filter validations. </summary>
        public override Dictionary<string, string> AttributeMeta
        {
            get
            {
                return new Dictionary<string, string>
                    {
                        { "FoodItemId", "String" },
                        { "Name", "String" },
                        { "Description", "String" },
                        { "Ingredients", "String[]" },
                        { "Price", "Number" },
                        { "Category", "String" }
                    };
            }
        }

        new public record Attributes : SearchBase.Attributes
        {
            public const string FoodItemId = "FoodItemId";
            public const string Name = "Name";
            public const string Description = "Description";
            public const string Ingredients = "Ingredients";
            public const string Price = "Price";
            public const string Category = "Category";
        }
    }
}
