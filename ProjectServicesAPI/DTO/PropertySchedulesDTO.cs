using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertySchedulesDTO
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? BrancheId { get; set; }
        public int? ContractId { get; set; }
        public int? ScheduleDateId { get; set; }
        public int? CountPhotos { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? Recurring { get; set; }
        public int? FrequencyType { get; set; }
        public string StartDate { get; set; }
        public string ScheduleDate { get; set; }
        public string Time { get; set; }
        public int? EndType { get; set; }
        public string EndDate { get; set; }
        public string CalendarColor { get; set; }
        public bool? ShowMoreOptions { get; set; }
        public bool? InvoiceableTask { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        
        public string Location { get; set; }
        public int? EmployeeCategoryId { get; set; }
        public string Employees { get; set; }
        public int? CallId { get; set; }
        public string Notes { get; set; }
        public bool? Active { get; set; }
        public int? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? PriorityId { get; set; }
        public string TimeEnd { get; set; }

        public PropertyScheduleDateDTO OneScheduleDate { get; set; }

        public PropertyScheduleItemsServicesDTO OneScheduleService { get; set; }
        public List<PropertyScheduleItemsServicesDTO> LstScheduleItemsServices { get; set; }
        public List<PropertyScheduleItemsServicesDTO> LstFreeServices { get; set; }
        public List<PropertyScheduleItemsServicesDTO> LstFirstCreateServices { get; set; }

        public PropertyCustomersDTO CustomerDTO { get; set; }

        public List<PropertyScheduleEmployeesDTO> LstScheduleEmployeeDTO { get; set; }
        public List<PropertyEmployeeDTO> LstEmployeeDTO { get; set; }

        public List<int> EmpsID { get; set; }

        public PropertyEstimateDTO EstimateDTO { get; set; }
        public PropertyInvoiceDTO InvoiceDTO { get; set; }

        public List<PropertyScheduleMaterialReceiptDTO> LstMaterialReceipt { get; set; }

        public string StartTimeAc { get; set; }
        public string EndTimeAc { get; set; }
        public int? Status { get; set; }//0 = Not service // 1 = open // 2 = complete
    }
}