using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.Models.ViewModel
{
	public class ApiStatsResult
	{
		public string ApiName { get; set; }
		public int RequestCount { get; set; }
		public double AverageCpuUsage { get; set; }
	}

}
