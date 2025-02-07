using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public enum ActionType
    {
        OpenApp,
        OpenUrl,
    }
    public class NotificationAction
    {
        public string ActionID { get; set; }
        public ActionType Type { get; set; }
        public string URL { get; set; }
    }
}