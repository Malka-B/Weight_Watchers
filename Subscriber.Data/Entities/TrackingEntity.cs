using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Subscriber.Data.Entities
{
    public class TrackingEntity
    {
        public int Id { get; set; }

        public int CardId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Weight { get; set; }

        public DateTime Date { get; set; }

        public int trend { get; set; }

        public float BMI { get; set; }

        public string Comments { get; set; }
    }
}
