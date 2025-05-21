using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;
using Repository.Interface;
using Service.Interface;

namespace Service.Services
{
    public class SelectCategoryService : ISelectCategoryService
    {
        private readonly ISelectCategory _selectRepository;
        public SelectCategoryService(ISelectCategory selectRepository)
        {
            _selectRepository = selectRepository;
        }

        public List<FoodItem> GetRandomFoodItems()
        {
            return _selectRepository.GetRandomFoodItemsByCategories().ToList();
        }
    }
}
