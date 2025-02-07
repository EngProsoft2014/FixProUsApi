//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FixProUsApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_Schedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Schedule()
        {
            this.Tbl_Calls = new HashSet<Tbl_Calls>();
            this.Tbl_Estimate = new HashSet<Tbl_Estimate>();
            this.Tbl_ScheduleDate = new HashSet<Tbl_ScheduleDate>();
            this.Tbl_SchedulePictures = new HashSet<Tbl_SchedulePictures>();
            this.Tbl_ScheduleMaterialReceipt = new HashSet<Tbl_ScheduleMaterialReceipt>();
            this.Tbl_ScheduleItemsServices = new HashSet<Tbl_ScheduleItemsServices>();
            this.Tbl_Invoice = new HashSet<Tbl_Invoice>();
            this.Tbl_OneSignalNotification = new HashSet<Tbl_OneSignalNotification>();
        }
    
        public int Id { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<int> BrancheId { get; set; }
        public Nullable<int> ContractId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Recurring { get; set; }
        public Nullable<int> FrequencyType { get; set; }
        public string StartDate { get; set; }
        public string ScheduleDate { get; set; }
        public string Time { get; set; }
        public Nullable<int> EndType { get; set; }
        public string EndDate { get; set; }
        public string CalendarColor { get; set; }
        public Nullable<bool> ShowMoreOptions { get; set; }
        public Nullable<bool> InvoiceableTask { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string Location { get; set; }
        public Nullable<int> EmployeeCategoryId { get; set; }
        public string Employees { get; set; }
        public string Notes { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> CreateUser { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> CallId { get; set; }
        public Nullable<int> PriorityId { get; set; }
        public string TimeEnd { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Calls> Tbl_Calls { get; set; }
        public virtual Tbl_Calls Tbl_Calls1 { get; set; }
        public virtual Tbl_Customers Tbl_Customers { get; set; }
        public virtual Tbl_EmployeeCategory Tbl_EmployeeCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Estimate> Tbl_Estimate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_ScheduleDate> Tbl_ScheduleDate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_SchedulePictures> Tbl_SchedulePictures { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_ScheduleMaterialReceipt> Tbl_ScheduleMaterialReceipt { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_ScheduleItemsServices> Tbl_ScheduleItemsServices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Invoice> Tbl_Invoice { get; set; }
        public virtual Tbl_Account Tbl_Account { get; set; }
        public virtual Tbl_Branches Tbl_Branches { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_OneSignalNotification> Tbl_OneSignalNotification { get; set; }
        public virtual Tbl_Employee Tbl_Employee { get; set; }
    }
}
