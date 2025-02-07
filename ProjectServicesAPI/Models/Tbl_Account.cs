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
    
    public partial class Tbl_Account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Account()
        {
            this.Tbl_CallReason = new HashSet<Tbl_CallReason>();
            this.Tbl_Calls = new HashSet<Tbl_Calls>();
            this.Tbl_Calls1 = new HashSet<Tbl_Calls>();
            this.Tbl_Campaigns = new HashSet<Tbl_Campaigns>();
            this.Tbl_Customers = new HashSet<Tbl_Customers>();
            this.Tbl_CustomersCategory = new HashSet<Tbl_CustomersCategory>();
            this.Tbl_CustomersCustomField = new HashSet<Tbl_CustomersCustomField>();
            this.Tbl_EmployeeAssignItems = new HashSet<Tbl_EmployeeAssignItems>();
            this.Tbl_EmployeeAssignItemsLogs = new HashSet<Tbl_EmployeeAssignItemsLogs>();
            this.Tbl_EmployeeCategory = new HashSet<Tbl_EmployeeCategory>();
            this.Tbl_EmployeeCustomField = new HashSet<Tbl_EmployeeCustomField>();
            this.Tbl_Equipments = new HashSet<Tbl_Equipments>();
            this.Tbl_EquipmentsCustomField = new HashSet<Tbl_EquipmentsCustomField>();
            this.Tbl_Estimate = new HashSet<Tbl_Estimate>();
            this.Tbl_EstimateItemsServices = new HashSet<Tbl_EstimateItemsServices>();
            this.Tbl_Invoice = new HashSet<Tbl_Invoice>();
            this.Tbl_InvoiceItemsServices = new HashSet<Tbl_InvoiceItemsServices>();
            this.Tbl_InvoiceScheduleDate = new HashSet<Tbl_InvoiceScheduleDate>();
            this.Tbl_ItemsServices = new HashSet<Tbl_ItemsServices>();
            this.Tbl_Member = new HashSet<Tbl_Member>();
            this.Tbl_Payment = new HashSet<Tbl_Payment>();
            this.Tbl_Schedule = new HashSet<Tbl_Schedule>();
            this.Tbl_ScheduleDate = new HashSet<Tbl_ScheduleDate>();
            this.Tbl_ScheduleEmployees = new HashSet<Tbl_ScheduleEmployees>();
            this.Tbl_ScheduleItemsServices = new HashSet<Tbl_ScheduleItemsServices>();
            this.Tbl_ScheduleMaterialReceipt = new HashSet<Tbl_ScheduleMaterialReceipt>();
            this.Tbl_SchedulePictures = new HashSet<Tbl_SchedulePictures>();
            this.Tbl_StripeAccount = new HashSet<Tbl_StripeAccount>();
            this.Tbl_Tax = new HashSet<Tbl_Tax>();
            this.Tbl_TimeSheet = new HashSet<Tbl_TimeSheet>();
            this.Tbl_TransactionCustomersCustomField = new HashSet<Tbl_TransactionCustomersCustomField>();
            this.Tbl_EstimateScheduleDate = new HashSet<Tbl_EstimateScheduleDate>();
            this.Tbl_Branches = new HashSet<Tbl_Branches>();
            this.Tbl_EmployeeBranches = new HashSet<Tbl_EmployeeBranches>();
            this.Tbl_ItemsServicesCategory = new HashSet<Tbl_ItemsServicesCategory>();
            this.Tbl_ItemsServicesSubCategory = new HashSet<Tbl_ItemsServicesSubCategory>();
            this.Tbl_OneSignalNotification = new HashSet<Tbl_OneSignalNotification>();
            this.Tbl_Employee = new HashSet<Tbl_Employee>();
            this.Tbl_AccountPayment = new HashSet<Tbl_AccountPayment>();
        }
    
        public int Id { get; set; }
        public Nullable<int> PlanId { get; set; }
        public string EamilAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<int> DayExpire { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public string AccountSubdomainURL { get; set; }
        public string AccountSubdomainApiURL { get; set; }
        public string HostName { get; set; }
        public string DBConnection { get; set; }
        public string DBDataSource { get; set; }
        public string DBName { get; set; }
        public string DBUserId { get; set; }
        public string DBPassword { get; set; }
        public string OneSignalAuthApi { get; set; }
        public string OneSignalRestApikey { get; set; }
        public string OneSignalAppId { get; set; }
        public Nullable<int> TypeTrackingSch_Invo { get; set; }
        public Nullable<int> TimeOutLogout { get; set; }
        public string PathFileUpload { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> CreateUser { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CompanyNameWithSpace { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_CallReason> Tbl_CallReason { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Calls> Tbl_Calls { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Calls> Tbl_Calls1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Campaigns> Tbl_Campaigns { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Customers> Tbl_Customers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_CustomersCategory> Tbl_CustomersCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_CustomersCustomField> Tbl_CustomersCustomField { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_EmployeeAssignItems> Tbl_EmployeeAssignItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_EmployeeAssignItemsLogs> Tbl_EmployeeAssignItemsLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_EmployeeCategory> Tbl_EmployeeCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_EmployeeCustomField> Tbl_EmployeeCustomField { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Equipments> Tbl_Equipments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_EquipmentsCustomField> Tbl_EquipmentsCustomField { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Estimate> Tbl_Estimate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_EstimateItemsServices> Tbl_EstimateItemsServices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Invoice> Tbl_Invoice { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_InvoiceItemsServices> Tbl_InvoiceItemsServices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_InvoiceScheduleDate> Tbl_InvoiceScheduleDate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_ItemsServices> Tbl_ItemsServices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Member> Tbl_Member { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Payment> Tbl_Payment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Schedule> Tbl_Schedule { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_ScheduleDate> Tbl_ScheduleDate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_ScheduleEmployees> Tbl_ScheduleEmployees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_ScheduleItemsServices> Tbl_ScheduleItemsServices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_ScheduleMaterialReceipt> Tbl_ScheduleMaterialReceipt { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_SchedulePictures> Tbl_SchedulePictures { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_StripeAccount> Tbl_StripeAccount { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Tax> Tbl_Tax { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_TimeSheet> Tbl_TimeSheet { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_TransactionCustomersCustomField> Tbl_TransactionCustomersCustomField { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_EstimateScheduleDate> Tbl_EstimateScheduleDate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Branches> Tbl_Branches { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_EmployeeBranches> Tbl_EmployeeBranches { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_ItemsServicesCategory> Tbl_ItemsServicesCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_ItemsServicesSubCategory> Tbl_ItemsServicesSubCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_OneSignalNotification> Tbl_OneSignalNotification { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Employee> Tbl_Employee { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_AccountPayment> Tbl_AccountPayment { get; set; }
        public virtual Tbl_Plans Tbl_Plans { get; set; }
    }
}
