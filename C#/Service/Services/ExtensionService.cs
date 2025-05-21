using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public static class ExtensionService
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddRepository();
            services.AddScoped<IConstraintsServices,ConstraintsServices>();
            services.AddScoped<ISelectCategoryService,SelectCategoryService>();
            return services;
        }
    }
}
