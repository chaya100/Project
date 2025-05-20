//using Microsoft.EntityFrameworkCore;
//using Repository.Interface;
//using Repository.Entities;
//using OfficeOpenXml;
//using System.Globalization;
//using OfficeOpenXml;
//using Repository.Entities;
//using System;
//using System.Globalization;
//using System.IO;

//namespace Mock
//{
//    public class FoodDbContext : DbContext, IContext
//    {
//        // בנאי שמקבל אפשרויות - נדרש עבור הזרקת תלויות ב-ASP.NET Core
//        public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options)
//        {
//        }

//        // בנאי ללא פרמטרים - שימושי עבור מיגרציות ומקרי שימוש אחרים
//        public FoodDbContext()
//        {
//        }

//        // מאפיין DbSet עם F גדולה - לפי ההסכם המקובל וכדי להתאים לממשק IContext
//        public DbSet<FoodItem> FoodItems { get; set; }

//        // דריסת שיטת OnConfiguring כדי להגדיר את מחרוזת החיבור עבור הבנאי ללא פרמטרים
//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//                // מחרוזת החיבור תשמש רק כאשר לא מועברות אפשרויות בבנאי
//                optionsBuilder.UseSqlServer("Data Source=CHAYA-LURIA;Initial Catalog=dietDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
//            }
//        }

//        // ישום של Save מממשק IContext אם נדרש
        

//        // ישום של SaveChanges מממשק IContext אם יש צורך בדריסה
//        public new int SaveChanges()
//        {
//         return base.SaveChanges();
//        }
      

//        // אופציונלי: הגדרות נוספות של המודל אם יש צורך
//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            // כאן אפשר להוסיף הגדרות נוספות למודל, כמו יחסים בין טבלאות, מפתחות זרים, וכדומה
//            // לדוגמה:
//            // modelBuilder.Entity<FoodItem>().HasKey(f => f.ID);
//        }

//        void IContext.Save()
//        {
//            throw new NotImplementedException();
//        }

//        int IContext.SaveChanges()
//        {
//            throw new NotImplementedException();
//        }
//    }
//    public class ExcelImporter
//    {
//        public static void ImportExcelToDatabase(string filePath)
//        {
//            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
//            using var package = new ExcelPackage(new FileInfo(filePath));
//            using var context = new FoodDbContext();

//            foreach (var worksheet in package.Workbook.Worksheets)
//            {
//                var start = worksheet.Dimension.Start;
//                var end = worksheet.Dimension.End;

//                for (int row = start.Row + 1; row <= end.Row; row++) // דילוג על כותרות העמודות
//                {
//                    var foodItem = new FoodItem
//                    {
//                        Food = worksheet.Cells[row, 1].Text,
//                        Measure = worksheet.Cells[row, 2].Text,
//                        Calories = int.TryParse(worksheet.Cells[row, 3].Text, out var calories) ? calories : (int?)null,
//                        Carbs = double.TryParse(worksheet.Cells[row, 4].Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var carbs) ? carbs : 0,
//                        Fat = double.TryParse(worksheet.Cells[row, 5].Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var fat) ? fat : 0,
//                        Protein = double.TryParse(worksheet.Cells[row, 6].Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var protein) ? protein : 0,
//                        Category = worksheet.Cells[row, 7].Text
//                    };

//                    context.FoodItems.Add(foodItem);
//                }
//            }

//            context.SaveChanges();
//            Console.WriteLine("הייבוא הושלם בהצלחה!");
//        }
//    }
//}





  

////using Microsoft.EntityFrameworkCore;
////using OfficeOpenXml;
////using System;
////using System.Collections.Generic;
////using System.ComponentModel.DataAnnotations.Schema;
////using System.ComponentModel.DataAnnotations;
////using System.Globalization;
////using System.IO;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using Repository.Interface;
////using Repository.Entities;

////namespace Mock
////{

////    public class FoodDbContext : DbContext, IContext
////    {


////        public DbSet<FoodItem> foodItems { get; set; }
////        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
////        {
////            optionsBuilder.UseSqlServer("Server=192.168.8.100;Database=dietDB;Trusted_Connection=True;");
////        }
////        public void Save()
////        {
////            SaveChanges();
////        }
////        public class ExcelImporter
////        {
////            public static void ImportExcelToDatabase(string filePath)
////            {
////                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
////                using var package = new ExcelPackage(new FileInfo(filePath));
////                using var context = new FoodDbContext();

////                foreach (var worksheet in package.Workbook.Worksheets)
////                {
////                    var start = worksheet.Dimension.Start;
////                    var end = worksheet.Dimension.End;

////                    for (int row = start.Row + 1; row <= end.Row; row++) // דילוג על כותרות העמודות
////                    {
////                        var foodItem1 = new FoodItem
////                        {
////                            Food = worksheet.Cells[row, 1].Text,
////                            Measure = worksheet.Cells[row, 2].Text,
////                            Calories = int.TryParse(worksheet.Cells[row, 3].Text, out var calories) ? calories : (int?)null,
////                            Carbs = double.TryParse(worksheet.Cells[row, 4].Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var carbs) ? carbs : 0,
////                            Fat = double.TryParse(worksheet.Cells[row, 5].Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var fat) ? fat : 0,
////                            Protein = double.TryParse(worksheet.Cells[row, 6].Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var protein) ? protein : 0,
////                            Category = worksheet.Cells[row, 7].Text
////                        };
////                        context.foodItems.Add(foodItem1);
////                    }
////                }
////                context.SaveChanges();
////            }
////        }

////        class Program
////        {
////            static void Main()
////            {
////                string filePath = "C:\\Users\\user\\Pictures\\דיאטה\\Merged_Food_Data.csv";
////                ExcelImporter.ImportExcelToDatabase(filePath);
////                Console.WriteLine("הייבוא הושלם בהצלחה!");
////            }
////        }
////    }
////}


