using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ISelectCategoryService iSelectCategoryService;
        private readonly IConstraintsServices iConstraintsServices;

        public UserController(ISelectCategoryService iSelectCategoryService, IConstraintsServices iConstraintsServices)
        {
            this.iSelectCategoryService = iSelectCategoryService;
            this.iConstraintsServices = iConstraintsServices;
        }

        [HttpGet("CalculateSimplex/{age}/{weight}/{height}/{isman}/{Lifestyle}/{purpose}")]
        public IActionResult CalculateSimplex(int age, double weight, double height, bool isman, string Lifestyle, string purpose)
        {
            try
            {
                List<object> foodItemsWithMealNumbers = new List<object>();

                var (dailyCarbs, dailyFats, dailyProteins, dailyCalories) = iConstraintsServices.First(age, weight, height, isman, Lifestyle, 0, purpose);

                var mealGoals = new List<(double Calories, double Proteins, double Carbs, double Fats)>
                {
                    (dailyCalories * 0.25, dailyProteins * 0.25, dailyCarbs * 0.25, dailyFats * 0.25),
                    (dailyCalories * 0.48, dailyProteins * 0.48, dailyCarbs * 0.48, dailyFats * 0.48),
                    (dailyCalories * 0.25, dailyProteins * 0.25, dailyCarbs * 0.25, dailyFats * 0.25),
                    (dailyCalories * 0.02, dailyProteins * 0.02, dailyCarbs * 0.02, dailyFats * 0.02)
                };

                // מאכלים שנבחרו כבר לארוחות קודמות
                HashSet<string> usedFoodNames = new HashSet<string>();

                for (int mealIndex = 0; mealIndex < mealGoals.Count; mealIndex++)
                {
                    var (mealCalories, mealProteins, mealCarbs, mealFats) = mealGoals[mealIndex];

                    // הבאת כל המאכלים והסרת המאכלים שכבר נבחרו
                    var allFoodItems = iSelectCategoryService.GetRandomFoodItems();
                    var randomFoodItems = allFoodItems
                        .Where(f => !usedFoodNames.Contains(f.Food))
                        .ToList();

                    if (randomFoodItems == null || randomFoodItems.Count < 5)
                    {
                        return BadRequest($"לא נמצאו מספיק פריטי מזון לארוחה {mealIndex + 1}");
                    }

                    var constraints = iConstraintsServices.CreateNutritionalConstraints(mealCalories, mealCarbs, mealFats, mealProteins, randomFoodItems);

                    var objectiveFunction = iConstraintsServices.DefineObjectiveFunction(
                        randomFoodItems,
                        mealCalories,
                        mealProteins,
                        mealCarbs,
                        mealFats
                    );

                    var simplexModel = new Simplex
                    {
                        Constraints = constraints,
                        Variables = randomFoodItems.Select(f => f.Food).ToList(),
                        ObjectiveFunction = objectiveFunction
                    };

                    try
                    {
                        var results = simplexModel.RunSimplex(simplexModel);

                        var selectedFoodItems = iConstraintsServices.ConvertToFoodItem(new double[][] { results }, randomFoodItems);

                        for (int i = 0; i < results.Length && i < randomFoodItems.Count; i++)
                        {
                            if (results[i] > 0.01)
                            {
                                var originalItem = randomFoodItems[i];

                                foodItemsWithMealNumbers.Add(new
                                {
                                    MealNumber = mealIndex + 1,
                                    Food = originalItem.Food,
                                    Measure = originalItem.Measure,
                                    Quantity = Math.Round(results[i], 1),
                                    Calories = originalItem.Calories,
                                    Proteins = originalItem.Protein,
                                    Carbs = originalItem.Carbs,
                                    Fats = originalItem.Fat,
                                    Category = originalItem.Category
                                });

                                // הוספת המאכל לרשימת מאכלים שכבר נבחרו
                                usedFoodNames.Add(originalItem.Food);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in simplex for meal {mealIndex + 1}: {ex.Message}");
                    }
                }

                return Ok(foodItemsWithMealNumbers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה כללית: {ex.Message}");
            }
        }
    }
}