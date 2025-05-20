
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Repository.Entities;

namespace Service.Services
{
    public class Simplex
    {
        public List<ConstraintsDTO> Constraints { get; set; }
        public ConstraintsDTO ObjectiveFunction { get; set; }
        public List<string> Variables { get; set; }

        // אתחול טבלת הסימפלקס
        public double[,] InitializeSimplexTable(Simplex model)
        {
            int numVariables = model.Variables.Count;
            int numConstraints = model.Constraints.Count;

            // חישוב מספר עמודות ושורות בטבלה
            int rows = numConstraints + 1;  // אילוצים + פונקציית מטרה
            int cols = numVariables + numConstraints + 1;  // משתנים + משתני עזר + עמודת RHS

            double[,] table = new double[rows, cols];

            // מילוי טבלת הסימפלקס - אילוצים
            for (int i = 0; i < numConstraints; i++)
            {
                // משתני ההחלטה
                for (int j = 0; j < numVariables; j++)
                {
                    if (j < model.Constraints[i].Coefficients.Count)
                    {
                        table[i, j] = model.Constraints[i].Coefficients[j];
                    }
                    else
                    {
                        table[i, j] = 0; // מילוי אפסים אם המערך קצר מדי
                    }
                }

                // משתני עזר (סלאק/סרפלוס)
                for (int j = 0; j < numConstraints; j++)
                {
                    if (i == j)
                    {
                        switch (model.Constraints[i].type)
                        {
                            case ConstraintType.LessThanOrEqual:
                                table[i, numVariables + j] = 1;  // סלאק
                                break;
                            case ConstraintType.GreaterThanOrEqual:
                                table[i, numVariables + j] = -1;  // סרפלוס
                                break;
                            case ConstraintType.Equal:
                                // לא נדרש משתנה עזר לאילוץ שוויון
                                break;
                            default:
                                table[i, numVariables + j] = 1; // ברירת מחדל - סלאק
                                break;
                        }
                    }
                    else
                    {
                        table[i, numVariables + j] = 0;
                    }
                }

                // עמודת הפתרון (RHS)
                table[i, cols - 1] = model.Constraints[i].Limit;
            }

            // פונקציית מטרה בשורה האחרונה (בניגוד לסימן עבור מקסימיזציה)
            for (int j = 0; j < numVariables; j++)
            {
                if (j < model.ObjectiveFunction.Coefficients.Count)
                {
                    // מקדמי המשתנים בפונקציית המטרה - עם סימן הפוך
                    table[rows - 1, j] = -model.ObjectiveFunction.Coefficients[j];
                }
                else
                {
                    table[rows - 1, j] = 0; // מילוי אפסים אם המערך קצר מדי
                }
            }

            // אפסים למשתני העזר בפונקציית המטרה
            for (int j = numVariables; j < cols - 1; j++)
            {
                table[rows - 1, j] = 0;
            }

            return table;
        }

        // בחירת העמודה הנכנסת (הפיבוט)
        public int GetEnteringColumn(double[,] table, int rowCount, int colCount)
        {
            int enteringColumn = -1;
            double minValue = 0;

            // מחפש את הערך השלילי הקטן ביותר בשורת פונקציית המטרה
            for (int j = 0; j < colCount - 1; j++)
            {
                if (table[rowCount - 1, j] < minValue)
                {
                    minValue = table[rowCount - 1, j];
                    enteringColumn = j;
                }
            }

            return enteringColumn;
        }

        // בחירת השורה היוצאת
        public int GetLeavingRow(double[,] table, int rowCount, int colCount, int enteringColumn)
        {
            int leavingRow = -1;
            double minRatio = double.MaxValue;

            // חישוב יחסי בי/איי (b/a) לכל שורה
            for (int i = 0; i < rowCount - 1; i++)
            {
                // רק אם המקדם בעמודת הפיבוט הוא חיובי
                if (table[i, enteringColumn] > 0.00001)  // הימנעות מחלוקה באפס
                {
                    double ratio = table[i, colCount - 1] / table[i, enteringColumn];

                    // עדכון היחס המינימלי
                    if (ratio < minRatio && ratio >= 0)
                    {
                        minRatio = ratio;
                        leavingRow = i;
                    }
                }
            }

            return leavingRow;
        }

        // עדכון טבלת הסימפלקס לאחר בחירת פיבוט
        public void UpdateTable(double[,] table, int pivotRow, int pivotColumn, int rowCount, int colCount)
        {
            // ערך הפיבוט
            double pivotValue = table[pivotRow, pivotColumn];

            // ודא שערך הפיבוט אינו אפס
            if (Math.Abs(pivotValue) < 0.00001)
            {
                throw new Exception("Pivot value is too close to zero");
            }

            // נרמול שורת הפיבוט
            for (int j = 0; j < colCount; j++)
            {
                table[pivotRow, j] /= pivotValue;
            }

            // עדכון יתר השורות באמצעות אלימינציה גאוסית
            for (int i = 0; i < rowCount; i++)
            {
                if (i != pivotRow)
                {
                    double factor = table[i, pivotColumn];

                    for (int j = 0; j < colCount; j++)
                    {
                        table[i, j] -= factor * table[pivotRow, j];
                    }
                }
            }
        }

        // הרצת אלגוריתם הסימפלקס
        public double[] RunSimplex(Simplex model)
        {
            try
            {
                // אתחול טבלת הסימפלקס
                var table = InitializeSimplexTable(model);
                int rowCount = table.GetLength(0);
                int colCount = table.GetLength(1);

                // הדפסת הטבלה ההתחלתית (לצורכי דיבוג)
                Console.WriteLine("Initial Simplex Table:");
                PrintTable(table, rowCount, colCount);

                int maxIterations = 50;  // מספר מקסימלי של איטרציות
                int iteration = 0;

                while (iteration < maxIterations)
                {
                    iteration++;

                    // בחירת העמודה הנכנסת
                    int enteringColumn = GetEnteringColumn(table, rowCount, colCount);
                    Console.WriteLine($"Entering Column: {enteringColumn}");

                    // אם אין עמודה נכנסת - הגענו לפתרון אופטימלי
                    if (enteringColumn == -1)
                    {
                        Console.WriteLine("Optimal solution found!");
                        break;
                    }

                    // בחירת השורה היוצאת
                    int leavingRow = GetLeavingRow(table, rowCount, colCount, enteringColumn);
                    Console.WriteLine($"Leaving Row: {leavingRow}");

                    // אם אין שורה יוצאת - אין פתרון אפשרי
                    if (leavingRow == -1)
                    {
                        Console.WriteLine("Problem is unbounded!");
                        // במקום לזרוק שגיאה, נחזיר תוצאה ריקה
                        return new double[model.Variables.Count];
                    }

                    // עדכון הטבלה
                    UpdateTable(table, leavingRow, enteringColumn, rowCount, colCount);

                    // הדפסת הטבלה המעודכנת
                    Console.WriteLine($"Iteration {iteration} Simplex Table:");
                    PrintTable(table, rowCount, colCount);
                }

                if (iteration >= maxIterations)
                {
                    Console.WriteLine("Warning: Maximum number of iterations reached");
                }

                // חילוץ הפתרון מהטבלה הסופית
                double[] solution = ExtractSolution(table, rowCount, colCount, model.Variables.Count);

                // הדפסת הפתרון
                Console.WriteLine("Solution:");
                for (int i = 0; i < solution.Length; i++)
                {
                    if (i < model.Variables.Count)
                    {
                        Console.WriteLine($"{model.Variables[i]}: {solution[i]:F2}");
                    }
                    else
                    {
                        Console.WriteLine($"Var{i}: {solution[i]:F2}");
                    }
                }

                return solution;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Simplex: {ex.Message}");
                Console.WriteLine(ex.StackTrace);

                // במקרה של כישלון, החזר מערך עם ערכים אפסיים
                return new double[model.Variables.Count];
            }
        }

        // הדפסת טבלת הסימפלקס
        private void PrintTable(double[,] table, int rowCount, int colCount)
        {
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    Console.Write($"{table[i, j]:F2}\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        // חילוץ הפתרון מהטבלה הסופית
        private double[] ExtractSolution(double[,] table, int rowCount, int colCount, int numVariables)
        {
            double[] solution = new double[numVariables];

            // אתחול הפתרון לאפס
            for (int i = 0; i < numVariables; i++)
            {
                solution[i] = 0;
            }

            // מציאת משתנים בסיסיים ועדכון ערכיהם
            for (int j = 0; j < numVariables; j++)
            {
                // בדיקה אם העמודה מכילה בדיוק ערך 1 אחד וכל השאר 0 (משתנה בסיסי)
                int oneCount = 0;
                int oneRow = -1;

                for (int i = 0; i < rowCount - 1; i++)
                {
                    if (Math.Abs(table[i, j] - 1.0) < 0.00001)
                    {
                        oneCount++;
                        oneRow = i;
                    }
                    else if (Math.Abs(table[i, j]) > 0.00001)
                    {
                        // אם יש ערך שאינו אפס ואינו 1, זו אינה עמודת יחידה
                        oneCount = -1;
                        break;
                    }
                }

                // אם זהו משתנה בסיסי, הערך שלו הוא ב-RHS
                if (oneCount == 1 && oneRow != -1)
                {
                    solution[j] = table[oneRow, colCount - 1];
                }
            }

            return solution;
        }
    }
}

