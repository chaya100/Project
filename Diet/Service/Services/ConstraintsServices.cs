using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Repository.Interface;
using DTO;
using Service.Interface;

namespace Service.Services
{
    public class ConstraintsServices : IConstraintsServices
    {
        private readonly IContext _context;

        public ConstraintsServices(IContext context)
        {
            _context = context;
        }

        public (double, double, double, double) First(int age, double weight, double height, bool isman, string Lifestyle, int mealnum, string purpose)
        {
            double bmrHarrisBenedict;
            if (isman)
                bmrHarrisBenedict = 66 + (weight * 13.8) + (5 * height) - (6.8 * age);
            else
                bmrHarrisBenedict = 655 + (weight * 9.6) + (1.8 * height) - (4.7 * age);

            double ActivityLevel = Lifestyle switch
            {
                "יושבני" => 1.2,
                "קל" => 1.375,
                "בינוני" => 1.55,
                "פעיל" => 1.725,
                "מאוד פעיל" => 1.9,
                _ => 0
            };

            double tdee = bmrHarrisBenedict * ActivityLevel;
            tdee = tdee * 1.1;
            double dailyCalories = purpose switch
            {
                "עליה" => tdee + tdee * 0.5,
                "ירידה" => tdee - tdee * 0.5,
                "שמירה" => tdee,
                _ => 0
            };

            double DailyProteins = 0.8 * weight;
            double DailyCarbs = dailyCalories * 0.55 / 4;
            double DailyFats = dailyCalories * 0.27 / 9;

            return (DailyCarbs, DailyFats, DailyProteins, dailyCalories);
        }

        public ConstraintsDTO DefineObjectiveFunction(List<FoodItem> foodItems, double targetCalories, double targetProtein, double targetCarbs, double targetFats)
        {
            var coefficients = foodItems.Select(f =>
                Math.Abs((f.Calories ?? 0)) * 2.0+
                Math.Abs((f.Protein ?? 0)) * 1.5 +
                Math.Abs((f.Carbs ?? 0)) * 1.0 +
                Math.Abs((f.Fat ?? 0)) * 1.0
            ).ToList();


            return new ConstraintsDTO
            {
                Coefficients = coefficients,
                Limit = 0,
                type = ConstraintType.Equal
            };
        }

        public List<ConstraintsDTO> CreateNutritionalConstraints(double mealCalories, double mealCarbs, double mealFats, double mealProteins, List<FoodItem> foodItems)
        {
            var calorieMargin = 0.05;
            var proteinMargin = 0.005;
            var carbMargin = 0.005;
            var fatMargin = 0.005;

            var constraints = new List<ConstraintsDTO>();

            constraints.Add(new ConstraintsDTO
            {
                Coefficients = foodItems.Select(f => f.Calories ?? 0).ToList(),
                Limit = mealCalories * (1 + calorieMargin),
                SlackVariable = "S1",
                type = ConstraintType.LessThanOrEqual
            });

            constraints.Add(new ConstraintsDTO
            {
                Coefficients = foodItems.Select(f => f.Calories ?? 0).ToList(),
                Limit = mealCalories * (1 - calorieMargin),
                SlackVariable = "S2",
                type = ConstraintType.GreaterThanOrEqual
            });

            constraints.Add(new ConstraintsDTO
            {
                Coefficients = foodItems.Select(f => f.Protein ?? 0).ToList(),
                Limit = mealProteins * (1 + proteinMargin),
                SlackVariable = "S3",
                type = ConstraintType.LessThanOrEqual
            });

            constraints.Add(new ConstraintsDTO
            {
                Coefficients = foodItems.Select(f => f.Protein ?? 0).ToList(),
                Limit = mealProteins * (1 - proteinMargin),
                SlackVariable = "S4",
                type = ConstraintType.GreaterThanOrEqual
            });

            constraints.Add(new ConstraintsDTO
            {
                Coefficients = foodItems.Select(f => f.Carbs ?? 0).ToList(),
                Limit = mealCarbs * (1 + carbMargin),
                SlackVariable = "S5",
                type = ConstraintType.LessThanOrEqual
            });

            constraints.Add(new ConstraintsDTO
            {
                Coefficients = foodItems.Select(f => f.Carbs ?? 0).ToList(),
                Limit = mealCarbs * (1 - carbMargin),
                SlackVariable = "S6",
                type = ConstraintType.GreaterThanOrEqual
            });

            constraints.Add(new ConstraintsDTO
            {
                Coefficients = foodItems.Select(f => f.Fat ?? 0).ToList(),
                Limit = mealFats * (1 + fatMargin),
                SlackVariable = "S7",
                type = ConstraintType.LessThanOrEqual
            });

            constraints.Add(new ConstraintsDTO
            {
                Coefficients = foodItems.Select(f => f.Fat ?? 0).ToList(),
                Limit = mealFats * (1 - fatMargin),
                SlackVariable = "S8",
                type = ConstraintType.GreaterThanOrEqual
            });

            // מגבלה חדשה למנוע שילוב של חלב ובשר באותה ארוחה
            constraints.Add(new ConstraintsDTO
            {
                Coefficients = foodItems.Select(f => (f.Category == "Dairy produc7s" || f.Category == "meat") ? 1.0 : 0.0).ToList(),
                Limit = 1,
                SlackVariable = "S10",
                type = ConstraintType.Equal
            });

            constraints.Add(new ConstraintsDTO
            {
                Coefficients = Enumerable.Repeat(1.0, foodItems.Count).ToList(),
                Limit = Math.Max(3.0, foodItems.Count / 2.0),
                SlackVariable = "S9",
                type = ConstraintType.LessThanOrEqual
            });

            return constraints;
        }

        public List<FoodItem> ConvertToFoodItem(double[][] simplexResults, List<FoodItem> originalItems)
        {
            if (simplexResults == null || simplexResults.Length == 0 || simplexResults[0] == null)
                throw new ArgumentException("Simplex results cannot be null or empty");

            var result = new List<FoodItem>();

            for (int i = 0; i < simplexResults[0].Length; i++)
            {
                double value = simplexResults[0][i];
                if (value > 0.01)
                {
                    var original = originalItems[i];

                    result.Add(new FoodItem
                    {
                        Food = original.Food,
                        Measure = original.Measure,
                        Category = original.Category,
                        Calories = (original.Calories ?? 0) * value,
                        Protein = (original.Protein ?? 0) * value,
                        Carbs = (original.Carbs ?? 0) * value,
                        Fat = (original.Fat ?? 0) * value
                    });
                }
            }

            return result;
        }
    }
}