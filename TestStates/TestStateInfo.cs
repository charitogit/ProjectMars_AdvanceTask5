using ProjectMars_AdvanceTask_NUnit.Models;
using RazorEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.TestStates
{
    public class TestStateInfo
    {

        
        // Language State
        public Languages? CurrentLanguage { get; set; }
        public List<Languages> LanguageDataList { get; set; } = new List<Languages>();
        public bool IsLanguageAdded { get; set; }

        public Languages? OriginalLanguage { get; set; }
        public Languages? NewLanguage { get; set; }

        // Skill State
        public Skill? CurrentSkill { get; set; }
        public List<Skill> SkillDataList { get; set; } = new List<Skill>();
        public bool IsSkillAdded { get; set; }

        public Skill? OriginalSkill { get; set; }
        public Skill? NewSkill { get; set; }
       
              
        //Share Skill State 
        public ShareSkillModel? CurrentShareSkill { get; set; }
        public bool IsShareSkillAdded { get; set; }
        public List<ShareSkillModel> ShareSkillDataList { get; set; } = new List<ShareSkillModel>();

       
        //Search Skill State

        public SearchSkill? CurrentSearchSkill { get; set; }    
        public bool IsSearchSkillAdded { get; set; }

        //Notification State 
        public NotificationData? CurrentNotification { get; set; }

        //For SingleField Test Data (e.g Profile Availability, Earn Target, Hourly Rate, etc.)
        public SingleFieldTestData? CurrentSingleFieldShareSkill { get; set; }

    }
}

