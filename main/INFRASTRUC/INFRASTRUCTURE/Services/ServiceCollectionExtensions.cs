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
using APPLICATIONCORE.Interface.Favorite;
using INFRASTRUCTURE.Services.Favorite;
using APPLICATIONCORE.Interface.Email;
using INFRASTRUCTURE.Services.Email;
using Microsoft.Extensions.Configuration;
using APPLICATIONCORE.Interface.Cart;
using INFRASTRUCTURE.Services.Cart;
using APPLICATIONCORE.Interface.Order;
using INFRASTRUCTURE.Services.Order;


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
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();


            services.AddScoped<IEmailService>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                var emailSender = config["Email:Sender"];
                var emailPassword = config["Email:Password"];
                return new EmailService(emailSender, emailPassword);
            });
            return services;
        }
    }
}
