using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IConstraintsServices
    {
         (double, double, double, double) First(int age, double weight, double height, bool isman, string Lifestyle, int mealnum, string purpose);
         ConstraintsDTO DefineObjectiveFunction(List<FoodItem> foodItems, double targetCalories, double targetProtein, double targetCarbs, double targetFats);
         List<ConstraintsDTO> CreateNutritionalConstraints(double mealCalories, double mealCarbs, double mealFats, double mealProteins, List<FoodItem> foodItems);
        List<FoodItem> ConvertToFoodItem(double[][] simplexResults, List<FoodItem> originalItems);
    }
}
