using APPLICATIONCORE.Interface.Address;
using APPLICATIONCORE.Interface.AuthLogin;
using APPLICATIONCORE.Interface.Category;
using APPLICATIONCORE.Interface.Product;
using APPLICATIONCORE.Interface.Supplier;
using APPLICATIONCORE.Interface.Type;



using INFRASTRUCTURE.Services.Category;
using INFRASTRUCTURE.Services.Product;
using INFRASTRUCTURE.Services.Supplier;
using INFRASTRUCTURE.Services.Type;
using INFRASTRUCTURE.Services.Answer;
using INFRASTRUCTURE.Services.Address;



using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFRASTRUCTURE.Services.AuthService;
using APPLICATIONCORE.Interface.Answer;


namespace INFRASTRUCTURE.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IAuthService, INFRASTRUCTURE.Services.AuthService.AuthService>();
            services.AddScoped<ITypeService, TypeService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IAnswerService, AnswerService>();

            return services;
        }
    }
}
