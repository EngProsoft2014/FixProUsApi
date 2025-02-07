using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertyObjectCustomerDTO
    {
        public PropertySchedulesDTO ObjSchedule { get; set; }
        public PropertyEstimateDTO ObjEstimate { get; set; }
        public PropertyInvoiceDTO ObjInvoice { get; set; }
    }
}