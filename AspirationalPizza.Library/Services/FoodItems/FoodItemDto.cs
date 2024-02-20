using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspirationalPizza.Library.Services.FoodItems
{
    public class FoodItemDto
    {   
        public String? FoodItemId { get; set; }
        public String? ItemName { get; set; } = null; 
        public String ItemType { get; set; } = String.Empty;

        // Pizza Specific
        public List<String> LeftToppings { get; set; } = new List<String>();
        public List<String> RightToppings { get; set; } = new List<String>();
        public String CrustStyle { get; set; } = String.Empty;

        //General
        public String SauceType { get; set; } = String.Empty;
        public String SauceVolume { get; set; } = String.Empty;
        public String? CookingInstructions { get; set; } = null;

    }
}
