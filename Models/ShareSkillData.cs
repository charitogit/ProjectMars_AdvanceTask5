using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Models
{
    public class ShareSkillModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string[] Tags { get; set; }

        // Radio button selections
        public string ServiceType { get; set; }         // "Hourly" or "One-off"
        public string LocationType { get; set; }        // "Online" or "On-site"
        public string SkillTradeType { get; set; }      // "Skill-exchange" or "Credit"

        // Skill exchange tags (if SkillTradeType == "Skill-exchange")
        public string[] SkillExchange { get; set; }
        public decimal? Credit { get; set; }

        public string Status { get; set; }               // "Active" or "Hidden"
        public string ExpectedMessage { get; set; } 
    }
}
