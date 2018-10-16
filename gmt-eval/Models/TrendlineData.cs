using System.Collections.Generic;

namespace gmt_eval.Models
{
    public class TrendlineData
    {
        public IEnumerable<XYAxes> data { get; set; }
        public IEnumerable<XYAxes> trendData { get; set; }
    }
}