using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Models
{
    public class SearchSkill
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Keyword { get; set; }
        public string LocationType { get; set; }  

        public int ExpectedCount { get; set; }
        public string ExpectedMessage { get; set; }
    }

}
