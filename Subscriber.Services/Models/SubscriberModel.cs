using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber.Services.Models
{
    public class SubscriberModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int Height { get; set; }

    }
}
