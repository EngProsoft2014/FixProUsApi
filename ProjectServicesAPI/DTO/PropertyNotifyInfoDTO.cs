using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertyNotifyInfoDTO
    {
        public int AccountId { get; set; }
        public string NotifyHeader { get; set; }
        public string NotifyContent { get; set; }
        public List<string> Include_Ids { get; set; }
    }
}