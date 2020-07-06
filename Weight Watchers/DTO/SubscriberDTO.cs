using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Weight_Watchers.DTO
{
    public class SubscriberDTO
    {        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public int Height { get; set; }
    }
}
