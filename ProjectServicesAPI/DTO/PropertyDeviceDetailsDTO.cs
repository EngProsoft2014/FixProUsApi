using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertyDeviceDetailsDTO
    {
        public string app_id { get; set; }
        public int? device_type { get; set; }
        public string language { get; set; }
        public string timezone { get; set; }
        public string game_version { get; set; }
        public string device_model { get; set; }
        public string device_os { get; set; }
        public int? session_count { get; set; }

    }
}