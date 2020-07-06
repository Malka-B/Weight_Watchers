using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber.Data.Entities
{
    public class SubscriberEntity
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public virtual CardEntity CardEntity { get; set; }
    }
}
