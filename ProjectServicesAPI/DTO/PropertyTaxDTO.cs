using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class PropertyTaxDTO
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? BrancheId { get; set; }
        public string Taxname { get; set; }
        public string Description { get; set; }
        public decimal? Rate { get; set; }
        public bool? Globaldefaultrate { get; set; }
        public string Notes { get; set; }
        public bool? Active { get; set; }
        public int? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}