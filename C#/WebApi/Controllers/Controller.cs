//using Microsoft.AspNetCore.Mvc;
//using Service.Interface;
//using Mock;
//using Repository.Interface;
//using Repository.Entities;
//using DTO;
//using Service.Services;
//using System.Linq;
//using System.Runtime.ExceptionServices;
//using System.Threading.Tasks;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace WebApi.Controllers
//{
//    [Route("api/controller")]
//    [ApiController]
//    public class Controller : ControllerBase
//    {
//        private readonly ISelectCategoryService iSelectCategoryService;
//        private readonly IConstraintsServices iConstraintsServices;

//        public Controller(ISelectCategoryService iSelectCategoryService, IConstraintsServices iConstraintsServices)
//        {
//            this.iSelectCategoryService = iSelectCategoryService;
//            this.iConstraintsServices = iConstraintsServices;

//        }
//        [HttpGet("CalculateSimplex/{age}/{weight}/{height}/{isman}/{Lifestyle}/{purpose}")]
//        public IActionResult CalculateSimplex(int age, double weight, double height, bool isman, string Lifestyle, string purpose)
//        {
//            // רשימה שתכיל את כל המאכלים עם מזהי הארוחות
//            List<object> foodItemsWithMealNumbers = new List<object>();

//            // חישוב הערכים היומיים (קלוריות, חלבונים, פחמימות ושומנים)
//            var (dailyCarbs, dailyFats, dailyProteins, dailyCalories) = iConstraintsServices.First(age, weight, height, isman, Lifestyle, 3, purpose);

//            // חלוקת הערכים היומיים ל-4 ארוחות
//            var mealGoals = new List<(double Calories, double Proteins, double Carbs, double Fats)>
//    {
//        (dailyCalories * 0.25, dailyProteins * 0.25, dailyCarbs * 0.25, dailyFats * 0.25), // ארוחת בוקר
//        (dailyCalories * 0.35, dailyProteins * 0.35, dailyCarbs * 0.35, dailyFats * 0.35), // ארוחת צהריים
//        (dailyCalories * 0.30, dailyProteins * 0.30, dailyCarbs * 0.30, dailyFats * 0.30), // ארוחת ערב
//        (dailyCalories * 0.10, dailyProteins * 0.10, dailyCarbs * 0.10, dailyFats * 0.10)  // חטיף
//    };

//            // לולאה ליצירת תפריט לכל ארוחה
//            for (int mealIndex = 0; mealIndex < mealGoals.Count; mealIndex++)
//            {
//                var (mealCalories, mealProteins, mealCarbs, mealFats) = mealGoals[mealIndex];

//                // הגרלה אקראית של מאכלים
//                var randomFoodItems = iSelectCategoryService.GetRandomFoodItems();

//                // יצירת אילוצים עבור הארוחה
//                var constraints = iConstraintsServices.CreateNutritionalConstraints(mealCalories, mealCarbs, mealFats, mealProteins, randomFoodItems);

//                // הגדרת פונקציית המטרה
//                var objectiveFunction = iConstraintsServices.DefineObjectiveFunction(randomFoodItems);

//                // יצירת מודל הסימפלקס
//                var simplexModel = new Simplex
//                {
//                    Constraints = constraints,
//                    Variables = randomFoodItems.Select(f => f.Food).ToList(),
//                    ObjectiveFunction = objectiveFunction
//                };

//                // הרצת אלגוריתם הסימפלקס
//                var results = simplexModel.RunSimplex(simplexModel);

//                // המרת התוצאות לרשימת מאכלים
//                var selectedFoodItems = iConstraintsServices.ConvertToFoodItem(new double[][] { results });

//                // הוספת המאכלים עם מזהה הארוחה לרשימה
//                foodItemsWithMealNumbers.AddRange(selectedFoodItems.Select(f => new
//                {
//                    MealNumber = mealIndex + 1, // מזהה הארוחה (1-based index)
//                    Food = f.Food,
//                    Measure = f.Measure,
//                    Calories = f.Calories,
//                    Proteins = f.Protein,
//                    Carbs = f.Carbs,
//                    Fats = f.Fat
//                }));
//            }

//            // החזרת התוצאה
//            return Ok(foodItemsWithMealNumbers);
//        }

//    }

//        }
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Mock;
using Repository.Interface;
using Repository.Entities;
using DTO;
using Service.Services;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Controller : ControllerBase
    {
        private readonly ISelectCategoryService iSelectCategoryService;
        private readonly IConstraintsServices iConstraintsServices;

        public Controller(ISelectCategoryService iSelectCategoryService, IConstraintsServices iConstraintsServices)
        {
            this.iSelectCategoryService = iSelectCategoryService;
            this.iConstraintsServices = iConstraintsServices;
        }

        // תיקון: הסרת סלאשים רצופים בנתיב
        [HttpGet("CalculateSimplex/{age}/{weight}/{height}/{isman}/{Lifestyle}/{purpose}")]
        public IActionResult CalculateSimplex(int age, double weight, double height, bool isman, string Lifestyle, string purpose)
        {
            try
            {
                // רשימה שתכיל את כל המאכלים עם מזהי הארוחות
                List<object> foodItemsWithMealNumbers = new List<object>();

                // חישוב הערכים היומיים (קלוריות, חלבונים, פחמימות ושומנים)
                var (dailyCarbs, dailyFats, dailyProteins, dailyCalories) = iConstraintsServices.First(age, weight, height, isman, Lifestyle, 0, purpose);

                // חלוקת הערכים היומיים ל-4 ארוחות
                var mealGoals = new List<(double Calories, double Proteins, double Carbs, double Fats)>
                {
                    (dailyCalories * 0.25, dailyProteins * 0.25, dailyCarbs * 0.25, dailyFats * 0.25), // ארוחת בוקר
                    (dailyCalories * 0.35, dailyProteins * 0.35, dailyCarbs * 0.35, dailyFats * 0.35), // ארוחת צהריים
                    (dailyCalories * 0.30, dailyProteins * 0.30, dailyCarbs * 0.30, dailyFats * 0.30), // ארוחת ערב
                    (dailyCalories * 0.10, dailyProteins * 0.10, dailyCarbs * 0.10, dailyFats * 0.10)  // חטיף
                };

                // לולאה ליצירת תפריט לכל ארוחה
                for (int mealIndex = 0; mealIndex < mealGoals.Count; mealIndex++)
                {
                    var (mealCalories, mealProteins, mealCarbs, mealFats) = mealGoals[mealIndex];

                    // הגרלה אקראית של מאכלים מתאימים לארוחה
                    var randomFoodItems = iSelectCategoryService.GetRandomFoodItems();

                    // וידוא שיש מספיק פריטי מזון לסימפלקס
                    if (randomFoodItems == null || randomFoodItems.Count < 5)
                    {
                        return BadRequest($"לא נמצאו מספיק פריטי מזון לארוחה {mealIndex + 1}");
                    }

                    // יצירת אילוצים עבור הארוחה
                    var constraints = iConstraintsServices.CreateNutritionalConstraints(mealCalories, mealCarbs, mealFats, mealProteins, randomFoodItems);

                    // הגדרת פונקציית המטרה
                    var objectiveFunction = iConstraintsServices.DefineObjectiveFunction(randomFoodItems);

                    // יצירת מודל הסימפלקס
                    var simplexModel = new Simplex
                    {
                        Constraints = constraints,
                        Variables = randomFoodItems.Select(f => f.Food).ToList(),
                        ObjectiveFunction = objectiveFunction
                    };

                    try
                    {
                        // הרצת אלגוריתם הסימפלקס
                        var results = simplexModel.RunSimplex(simplexModel);

                        // המרת התוצאות לרשימת מאכלים
                        var selectedFoodItems = iConstraintsServices.ConvertToFoodItem(new double[][] { results });

                        // עיבוד התוצאות - בחר את הפריטים המקוריים עם הערכים המתאימים
                        for (int i = 0; i < results.Length && i < randomFoodItems.Count; i++)
                        {
                            // אם הערך של התוצאה גדול מאפס, זה אומר שהמאכל נבחר
                            if (results[i] > 0.01)
                            {
                                var originalItem = randomFoodItems[i];

                                // הוסף את המאכל עם הפרטים המלאים שלו
                                foodItemsWithMealNumbers.Add(new
                                {
                                    MealNumber = mealIndex + 1, // מזהה הארוחה (1-based index)
                                    Food = originalItem.Food,
                                    Measure = originalItem.Measure,
                                    Quantity = Math.Round(results[i], 1), // הכמות שנבחרה מעוגלת לספרה אחת אחרי הנקודה
                                    Calories = originalItem.Calories,
                                    Proteins = originalItem.Protein,
                                    Carbs = originalItem.Carbs,
                                    Fats = originalItem.Fat,
                                    Category = originalItem.Category
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // לוג השגיאה ונסיון להמשיך לארוחה הבאה
                        Console.WriteLine($"Error in simplex for meal {mealIndex + 1}: {ex.Message}");
                        // אופציה: החזרת שגיאה מפורטת יותר למשתמש
                        // return StatusCode(500, $"שגיאה בחישוב ארוחה {mealIndex + 1}: {ex.Message}");
                    }
                }

                // בדיקה אם נמצאו פריטי מזון כלשהם
                if (foodItemsWithMealNumbers.Count == 0)
                {
                    return StatusCode(500, "לא נמצאו פריטי מזון מתאימים לדרישות התזונתיות");
                }

                // החזרת התוצאה
                return Ok(foodItemsWithMealNumbers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה כללית: {ex.Message}");
            }
        }
    }
}

