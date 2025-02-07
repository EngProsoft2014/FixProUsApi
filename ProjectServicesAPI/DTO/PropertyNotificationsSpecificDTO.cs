using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertyNotificationsSpecificDTO
    {
        public int? AccountId { get; set; }
        public string app_id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public string[] include_player_ids { get; set; }
        public string Employees { get; set; }
        public int CreateUser { get; set; }
    }
}