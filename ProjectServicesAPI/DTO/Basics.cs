using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixProUsApi.DTO
{
    public class Basics
    {
        public enum EmailType
        {
            TaskNotify,
            InvoiceNotify
        }

        public enum FilesName
        {
            InvoiceTemplate,
            JobNotifiyTemplate,
            JobNotifiyMultiStaffTemplate,
            EstimateTemplate,
        }

        public enum VariablesTaskEmail
        {
            CompanyName,
            CompanyPhone,
            CompanyAddress,
            CompanyCity,
            CompanyState,
            CompanyLogo,
            TaskDate,
            TaskExpectedTime,
            TaskAddress,
        }

        public enum VariablesInvoiceEmail
        {
            CompanyName,
            CompanyPhone,
            CompanyAddress,
            CompanyCity,
            CompanyState,
            CompanyLogo,
            InvoiceNo,
            ViewInvoiceLink,
            DownloadInvoiceLink,
        }
    }
}