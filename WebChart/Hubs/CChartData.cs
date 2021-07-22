using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebChart.Hubs
{
    public class CChartData
    {
        public string user { get; set; }
        public string message { get; set; }
        public IFormFile Photo { get; set; }
    }
}
