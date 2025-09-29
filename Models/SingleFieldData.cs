using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Models
{
    public class SingleFieldTestData
    {
        public string Value { get; set; }               //For single value field

        public List<string> Values { get; set; }       // For multi-values  field like Tags using array of strings
        public string ExpectedMessage { get; set; }
    }



}
