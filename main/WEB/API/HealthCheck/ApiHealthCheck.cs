using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace API.HealthCheck
{
	public class ApiHealthCheck : IHealthCheck
	{
		private readonly HttpClient _httpClient;

		public ApiHealthCheck(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)

		{
			// Danh sách các endpoint cần kiểm tra
			var endpoints = new List<string>
		{
			"http://localhost:5031/api/Product",
			"http://localhost:5031/api/login"
		};

			var unhealthyEndpoints = new List<string>();
			var degradedEndpoints = new List<string>();

			foreach (var endpoint in endpoints)
			{
				try
				{
					var response = await _httpClient.GetAsync(endpoint, cancellationToken);
					if (!response.IsSuccessStatusCode)
					{
						degradedEndpoints.Add($"{endpoint} returned status code {response.StatusCode}");
					}
				}
				catch (HttpRequestException ex)
				{
					unhealthyEndpoints.Add($"{endpoint} is unhealthy: {ex.Message}");
				}
				catch (Exception ex)
				{
					unhealthyEndpoints.Add($"{endpoint} encountered an error: {ex.Message}");
				}
			}

			// Kiểm tra kết quả từ các endpoint
			if (unhealthyEndpoints.Count > 0)
			{
				return HealthCheckResult.Unhealthy("Some endpoints are unhealthy", null, data: new Dictionary<string, object>
			{
				{ "UnhealthyEndpoints", unhealthyEndpoints }
			});
			}
			else if (degradedEndpoints.Count > 0)
			{
				return HealthCheckResult.Degraded("Some endpoints are degraded", null, data: new Dictionary<string, object>
			{
				{ "DegradedEndpoints", degradedEndpoints }
			});
			}
			else
			{
				return HealthCheckResult.Healthy("All endpoints are healthy.");
			}
		}















	}

	
}
