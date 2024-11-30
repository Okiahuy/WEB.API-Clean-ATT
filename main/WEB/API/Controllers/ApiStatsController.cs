using APPLICATIONCORE.Models.ViewModel;
using INFRASTRUCTURE.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ApiStatsController : ControllerBase
	{
		private readonly MyDbContext _dbContext;

		public ApiStatsController(MyDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet("{apiName}")]
		public async Task<IActionResult> GetApiStats(string apiName)
		{
			try
			{
				// Truy vấn dữ liệu từ bảng ApiUsageLogs
				var logs = await _dbContext.ApiUsageLogs
					.Where(log => log.ApiName == apiName)
					.ToListAsync();

				if (logs == null || logs.Count == 0)
				{
					return NotFound(new { Message = $"No logs found for API: {apiName}" });
				}

				// Tính toán RequestCount và AverageCpuUsage
				var totalRequests = logs.Sum(log => log.RequestCount);
				var averageCpuUsage = logs.Average(log => log.CpuUsage);

				var result = new ApiStatsResult
				{
					ApiName = apiName,
					RequestCount = Convert.ToInt32(totalRequests),
					AverageCpuUsage = Math.Round(averageCpuUsage, 2)
				};

				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Message = "An error occurred while retrieving API stats.", Details = ex.Message });
			}
		}
	}
}
