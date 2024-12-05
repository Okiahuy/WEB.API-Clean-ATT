using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
///
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using INFRASTRUCTURE.Repository;
using Microsoft.EntityFrameworkCore;
using INFRASTRUCTURE.Services;
using API.HealthCheck;
using Serilog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using Serilog.Sinks.MSSqlServer;
using INFRASTRUCTURE.Middleware;
using APPLICATIONCORE.Domain.Momo;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = "Cookies";
	options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie("Cookies") 
.AddGoogle(options =>
{
	options.ClientId = "477903384113-7fk5jd7r3uesfu8a4ndrtuo0kjfk4cn3.apps.googleusercontent.com"; 
	options.ClientSecret = "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";
	options.Scope.Add("email");
	options.SaveTokens = true;  
});
builder.Services.AddHttpClient();
builder.Services.Configure<MoMoSettings>(builder.Configuration.GetSection("MoMoSettings"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<MoMoSettings>>().Value);

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
        ValidateLifetime = true, //kiểm tra token đã hết hạng hay chưa
        ValidateIssuerSigningKey = true,
        ValidIssuer = "Domainmasuphilami.com", // ??t Issuer 
        ValidAudience = "Domainmasuphilami.com", // ??t Audience 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyHuyThailendthichcodedaoyeucuocsong12345")),
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.RequireClaim("roleID", "1")); // Yêu cầu roleID là 1
});

builder.Services.AddControllers();

// Đăng ký các dịch vụ
builder.Services.AddApplicationServices();

builder.Services.AddHttpContextAccessor();

//them dich vu Cross origin request sharing vào tất cả các url
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

builder.Services.AddDbContext<MyDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MyDB");
    options.UseSqlServer(connectionString, sqlOptions => sqlOptions.CommandTimeout(180));
});

builder.Services.AddHttpClient<ApiHealthCheck>();
builder.Services.AddHealthChecks()
.AddCheck("SQL Database", new SqlConnectionHealthCheck(
		builder.Configuration.GetConnectionString("MyDB")
		?? throw new InvalidOperationException("Connection string 'MyDB' is not configured.")
	))
.AddCheck<ApiHealthCheck>(nameof(ApiHealthCheck))
.AddDbContextCheck<MyDbContext>()
.AddCheck<SystemHealthCheck>("CPU Use");

builder.Host.UseSerilog((context, config) =>
{
	config.MinimumLevel.Information()
	.MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
	.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
	.WriteTo.Console()
	.WriteTo.Debug()
	.WriteTo.File("Logs\\log-.txt",
	rollingInterval: RollingInterval.Day,
	rollOnFileSizeLimit: true,
	buffered: false,
	restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
	.WriteTo.MSSqlServer(
	connectionString: builder.Configuration.GetConnectionString("MyDB"),
	sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
	restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning);
});
builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout của Session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddDistributedMemoryCache();


var app = builder.Build();  // Sau khi dã thêm xong các dich vu, gui Build()
app.MapHealthChecks("/health", new HealthCheckOptions   
{
	ResponseWriter = async (context, report) =>
	{
		context.Response.ContentType = "application/json";
		var result = JsonSerializer.Serialize(new
		{
			status = report.Status.ToString(),
			checks = report.Entries.Select(entry => new
			{
				name = entry.Key,
				status = entry.Value.Status.ToString(),

				exception = entry.Value.Exception?.Message,

				duration = entry.Value.Duration.ToString()

			})

		});

		await context.Response.WriteAsync(result);

	}

});

app.UseStaticFiles(new StaticFileOptions  // Cau hình các tệp tĩnh 
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
    RequestPath = "/uploads"
});


app.UseMiddleware<ApiLoggingMiddleware>(); //Middleware ghi log khi API được truy cập.

using (var scope = app.Services.CreateScope()) // Seeding du lieu
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<MyDbContext>();
    SeedData.SeedDingData(dbContext);

}
app.UseSession();

if (app.Environment.IsDevelopment()) // Cau hình pipeline HTTP request
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors("AllowAll"); // Su dung CORS



app.UseSerilogRequestLogging();//serilog
app.UseAuthentication(); // Kích hoat Authentication Middleware
app.UseAuthorization(); // Kích hoat Authorization Middleware

app.MapControllers();


app.Run();

