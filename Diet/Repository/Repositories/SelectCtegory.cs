using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Repository.Entities;



namespace Repository.Repositories
{
    public class SelectCategory : ISelectCategory
    {
        private readonly IContext _foodDbContext;
        
        public SelectCategory(IContext context)
        {
            _foodDbContext = context;
        }
        public List<FoodItem> GetRandomFoodItemsByCategories()
        {
            try
            {

                // שליפת כל הקטגוריות הקיימות במסד הנתונים
                var categories = _foodDbContext.FoodItems
                                        .Select(f => f.Category)
                                        .Distinct()
                                        .ToList();
               
                // בחירת פריט אקראי אחד מכל קטגוריה
                var randomFoodItems = new List<FoodItem>();
                foreach (var category in categories)
                {

                    var foodItem = _foodDbContext.FoodItems

                                        .Where(f => f.Category == category).ToList()
                                        .OrderBy(x => Guid.NewGuid())
                                        .Take(10).ToList();

                    if (foodItem != null )
                    {
                        randomFoodItems.AddRange(foodItem);
                    }
                }

                return randomFoodItems;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetRandomFoodItemsByCategories: {ex.Message}");
                return new List<FoodItem>(); // Return empty list instead of throwing
            }
        }
    
}
}   


