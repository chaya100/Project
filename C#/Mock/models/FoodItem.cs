using System;
using System.Collections.Generic;

namespace Mock.models;

public partial class FoodItem
{
    public string? Food { get; set; }

    public string? Measure { get; set; }

    public double? Grams { get; set; }

    public double? Calories { get; set; }

    public double? Protein { get; set; }

    public double? Fat { get; set; }

    public double? SatFat { get; set; }

    public double? Fiber { get; set; }

    public double? Carbs { get; set; }

    public string? Category { get; set; }

    public string? SourceFile { get; set; }
}
