using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertyCom_MainDTO
    {
        public int Id { get; set; }
        public string TwilioAccountSid { get; set; }
        public string TwilioauthToken { get; set; }
        public string TwilioFromPhoneNumber { get; set; }
        public string RealtyRapidApi { get; set; }
        public string AddressAutoCompleteKey { get; set; }
    }
}