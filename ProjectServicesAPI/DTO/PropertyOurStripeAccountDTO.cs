using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertyOurStripeAccountDTO : PropertyBaseDTO
    {
        public int Id { get; set; }

        public int? AccountId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string SecretKey { get; set; }

        public bool? Active { get; set; }
    }
}