using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Repository.Interface
{
    // מייצג את שכבת הגישה למסד הנתונים
    public interface IContext
    {
        DbSet<FoodItem> FoodItems { get; set; }
        void Save();
    }
}

