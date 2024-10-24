using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using INFRASTRUCTURE.Repository;
using Microsoft.EntityFrameworkCore;
using APPLICATIONCORE.Interface.Product;
using INFRASTRUCTURE.Services.Product;



var builder = WebApplication.CreateBuilder(args);
// C?u h�nh JWT Authentication
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyHuyThailendthichcodedaoyeucuocsong12345")) // ??t kh�a b?o m?t b� m?t
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Th�m c�c d?ch v? v�o container
builder.Services.AddScoped<IProductService, ProductService>();

// Th�m d?ch v? CORS cho ph�p t?t c? c�c url truy c?p v�o
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

// Th�m d?ch v? CORS cho ch? cho ph�p url ???c thi?t l?p truy c?p
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

// Sau khi ?� th�m xong c�c d?ch v?, g?i Build()
var app = builder.Build();
// C?u h�nh ?? ph?c v? c�c t?p t?nh
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
    RequestPath = "/uploads"
});
// Seeding d? li?u
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<MyDbContext>();
    SeedData.SeedDingData(dbContext);
}

// C?u h�nh pipeline HTTP request
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// S? d?ng CORS
app.UseCors("AllowAll");

app.UseSession();

app.UseAuthentication(); // K�ch ho?t Authentication Middleware
app.UseAuthorization(); // K�ch ho?t Authorization Middleware

app.MapControllers();

app.Run();

