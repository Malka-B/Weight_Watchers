using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Measure.WepApi.DTO
{
    public class MeasureDTO
    {
        [Required]
        public int CardId { get; set; }

        [Range(5,300)]
        public decimal weight { get; set; }
    }
}
