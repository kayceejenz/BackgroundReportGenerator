using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundReportGenerator.Test.Models
{
    public class ReportGenerationTracker
    {
        public int Id { get; set; }
        public string ReportName { get; set;}
        public string FilePath { get; set; }
        public bool IsCompleted { get; set; }
    }
}
