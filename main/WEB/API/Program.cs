using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using INFRASTRUCTURE.Repository;
using Microsoft.EntityFrameworkCore;
using APPLICATIONCORE.Interface.Product;
using INFRASTRUCTURE.Services.Product;

var builder = WebApplication.CreateBuilder(args);
// Cau hình JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "Domainmasuphilami.com", // ??t Issuer 
        ValidAudience = "Domainmasuphilami.com", // ??t Audience 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyHuyThailendthichcodedaoyeucuocsong12345")) // ??t khóa b?o m?t bí m?t
    };
});
builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Thêm các d?ch v? vào container
builder.Services.AddScoped<IProductService, ProductService>();
// Thêm d?ch v? CORS cho phép t?t c? các url truy c?p vào
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
// Thêm d?ch v? CORS cho ch? cho phép url ???c thi?t l?p truy c?p
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowSpecificOrigin",
//        builder =>
//        {
//            builder.WithOrigins("http://localhost:3000") 
//                   .AllowAnyMethod()
//                   .AllowAnyHeader();
//        });
//});
builder.Services.AddDbContext<MyDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MyDB");
    options.UseSqlServer(connectionString, sqlOptions => sqlOptions.CommandTimeout(180));
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(1000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Sau khi dã thêm xong các dich vu, gui Build()
var app = builder.Build();
// Cau hình 
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
    RequestPath = "/uploads"
});
// Seeding du lieu
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<MyDbContext>();
    SeedData.SeedDingData(dbContext);
}
// Cau hình pipeline HTTP request
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Su dung CORS
app.UseCors("AllowAll");

app.UseSession();

app.UseAuthentication(); // Kích hoat Authentication Middleware
app.UseAuthorization(); // Kích hoat Authorization Middleware

app.MapControllers();

app.Run();

