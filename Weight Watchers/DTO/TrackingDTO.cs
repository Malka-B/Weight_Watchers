using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weight_Watchers.DTO
{
    public class TrackingDTO
    {                
        public decimal Weight { get; set; }

        public DateTime Date { get; set; }

        public int trend { get; set; }

        public float BMI { get; set; }

        public string Comments { get; set; }
    }
}
