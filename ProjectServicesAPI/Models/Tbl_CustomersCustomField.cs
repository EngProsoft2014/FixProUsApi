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
    
    public partial class Tbl_CustomersCustomField
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_CustomersCustomField()
        {
            this.Tbl_TransactionCustomersCustomField = new HashSet<Tbl_TransactionCustomersCustomField>();
        }
    
        public int Id { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<int> BrancheId { get; set; }
        public string CustomFieldName { get; set; }
        public Nullable<int> FieldType { get; set; }
        public string DefaultValue { get; set; }
        public Nullable<bool> Required { get; set; }
        public string Notes { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> CreateUser { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_TransactionCustomersCustomField> Tbl_TransactionCustomersCustomField { get; set; }
        public virtual Tbl_Account Tbl_Account { get; set; }
        public virtual Tbl_Branches Tbl_Branches { get; set; }
        public virtual Tbl_Employee Tbl_Employee { get; set; }
    }
}
