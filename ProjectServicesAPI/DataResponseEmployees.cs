using FixProUsApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi
{
    public class DataResponseEmployees
    {
        public IEnumerable<PropertyEmployeeDTO> EmployeesInPage { get; set; } = new List<PropertyEmployeeDTO>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}