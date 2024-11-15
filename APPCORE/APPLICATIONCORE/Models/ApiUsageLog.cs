using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Models
{
	public class ApiUsageLog
	{
		[Key]
		public int Id { get; set; }
		public string? ApiName { get; set; }  //-- Tên API
		public int? RequestCount { get; set; }  //-- Số lượng truy cập API
		public string? Status { get; set; }  //-- Trạng thái của API (Healthy, Degraded, Unhealthy)
		public double CpuUsage { get; set; }  // -- Tốc độ xử lý CPU (%)
		public DateTime RequestTime { get; set; }  // -- Thời điểm truy cập API
	}
}
