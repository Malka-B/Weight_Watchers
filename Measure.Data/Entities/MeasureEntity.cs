using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Measure.Data.Entities
{
    public class MeasureEntity
    {
        public int Id { get; set; }

        public int CardId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Weight { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public string Comments { get; set; }

    }
}
