using APPLICATIONCORE.Models;
using INFRASTRUCTURE.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRASTRUCTURE.Middleware
{
	public class ApiLoggingMiddleware
	{
		private readonly RequestDelegate _next;

		public ApiLoggingMiddleware(RequestDelegate next)
		{
			_next = next;
		}
		public async Task InvokeAsync(HttpContext context)
		{
			// Lấy `MyDbContext` từ RequestServices
			var dbContext = context.RequestServices.GetService<MyDbContext>();
			if (dbContext == null)
			{
				Console.WriteLine("MyDbContext not available.");
				await _next(context); // Bỏ qua nếu không có DbContext
				return;
			}

			try
			{
				// Ghi log hoặc thao tác với dbContext
				await LogRequestAsync(dbContext, context);

				// Tiếp tục chuỗi middleware
				await _next(context);
			}
			catch (Exception ex)
			{
				// Ghi log lỗi (nếu cần)
				Console.WriteLine($"Middleware Error: {ex.Message}");
				throw; // Ném lại lỗi cho pipeline xử lý
			}
		}

		private async Task LogRequestAsync(MyDbContext dbContext, HttpContext context)
		{
			// Lấy đường dẫn API dưới dạng chuỗi
			var apiName = context.Request.Path.Value?.ToLower(); // Chuyển đường dẫn về chữ thường

			if (string.IsNullOrEmpty(apiName)) return;

			var cpuUsage = GetCurrentCpuUsage(); // Hàm đo mức sử dụng CPU
			var status = "Healthy"; // Đặt trạng thái mặc định

			// Tìm log hoặc tạo log mới
			var log = await dbContext.ApiUsageLogs
				.FirstOrDefaultAsync(l => l.ApiName.ToLower() == apiName);

			if (log != null)
			{
				log.RequestCount++; // Tăng số lượng request
				log.RequestTime = DateTime.Now; // Cập nhật thời gian request
			}
			else
			{
				log = new ApiUsageLog
				{
					ApiName = apiName,
					RequestCount = 1,
					Status = status,
					CpuUsage = cpuUsage,
					RequestTime = DateTime.Now
				};

				await dbContext.ApiUsageLogs.AddAsync(log);
			}

			await dbContext.SaveChangesAsync(); // Lưu thay đổi vào database
		}

		private PerformanceCounter _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

		private double GetCurrentCpuUsage()
		{
			return Math.Round(_cpuCounter.NextValue(), 2); // Lấy giá trị CPU (%) và làm tròn 2 chữ số
		}
	}

}
