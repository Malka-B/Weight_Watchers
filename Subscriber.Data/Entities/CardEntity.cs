using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Subscriber.Data.Entities
{
    public class CardEntity
    {
        public int Id { get; set; }        

        public DateTime OpenDate { get; set; }

        public float BMI { get; set; }

        public int Height { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Weight { get; set; }

        public DateTime UpdateDate { get; set; }

        public Guid SubscriberId { get; set; }

        public virtual SubscriberEntity SubscriberEntity { get; set; }

    }
}
