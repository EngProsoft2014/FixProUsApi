using Antlr.Runtime.Misc;
using Antlr.Runtime.Tree;
using Autofac;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using FixProUsApi.Controllers;
using FixProUsApi.DTO;
using FixProUsApi.Models;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive.Concurrency;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Razor.Tokenizer;
using System.Web.Razor.Tokenizer.Symbols;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Windows.Interop;
using Microsoft.AspNet.SignalR;
using System.Dynamic;
using System.Reactive;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;
using Org.BouncyCastle.Ocsp;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.SqlServer.Management.Smo.Agent;
namespace FixProUsApi.DAL
{

    public class RepositorySchedulesDAL
    {
        RepositoryNotificationsDAL ClsNotificationsDAL = new RepositoryNotificationsDAL();

        public readonly Entities _db;
        private bool disposed = false;
        public RepositorySchedulesDAL()
        {
            _db = new Entities();
            _db.Database.Connection.ConnectionString = PropertyBaseDTO.ConnectSt;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<PropertySchedulesDTO> GetAllScheduleInBranch(int? AccountId, int EmpId, int EmpRole, string lstEmp, string TextSearch)
        {
            //var Schedules = _db.Tbl_Schedule.Where(x => x.BrancheId == BranchId).ToList();

            if (!string.IsNullOrEmpty(TextSearch))
            {
                var SchedulesDates1 = _db.Sp_Search_In_All_Jobs(TextSearch);

                List<Tbl_ScheduleDate> LstDT = new List<Tbl_ScheduleDate>();
                foreach (var cc in SchedulesDates1)
                {
                    Tbl_ScheduleDate oTbl_ScheduleDate = new Tbl_ScheduleDate();

                    oTbl_ScheduleDate = _db.Tbl_ScheduleDate.Where(x => x.Id == cc.Id).FirstOrDefault();

                    LstDT.Add(oTbl_ScheduleDate);
                }

                return ReturnSchedules(EmpId, EmpRole, lstEmp, LstDT);
            }
            else
            {
                var SchedulesDates2 = _db.Tbl_ScheduleDate.Where(x => x.AccountId == AccountId).ToList();

                List<Tbl_ScheduleDate> SchedulesDates = SchedulesDates2.Skip(Math.Max(0, SchedulesDates2.Count() - 50)).ToList();

                return ReturnSchedules(EmpId, EmpRole, lstEmp, SchedulesDates);
            }
        }

        List<PropertySchedulesDTO> ReturnSchedules(int EmpId, int EmpRole, string lstEmp, List<Tbl_ScheduleDate> LstScheduleDates)
        {

            List<PropertySchedulesDTO> lstSchedules = new List<PropertySchedulesDTO>();
            List<int> EmpIds = new List<int>();

            if (lstEmp != null)
            {
                if (EmpRole == 3)
                {
                    EmpIds = lstEmp?.Split(',').Select(t => int.Parse(t)).ToList();
                }
            }


            foreach (var ScheduleDate in LstScheduleDates)
            {
                //List<int> empsIds = Schedule?.Tbl_Schedule.Employees != "" ? Schedule?.Tbl_Schedule.Employees?.Split(',').Select(t => int.Parse(t)).ToList() : new List<int>();

                List<int> empsIds = ScheduleDate.Tbl_ScheduleEmployees.Where(w => w.ScheduleDateId == ScheduleDate.Id).Select(s => s.EmpId.Value).ToList();

                PropertySchedulesDTO Oobj = new PropertySchedulesDTO
                {
                    Id = ScheduleDate.ScheduleId.Value,
                    ScheduleDateId = ScheduleDate.Id,
                    Title = ScheduleDate.Tbl_Schedule.Title,
                    CalendarColor = ScheduleDate.Tbl_Schedule.CalendarColor,
                    Time = ScheduleDate.StartTime,
                    TimeEnd = ScheduleDate.EndTime,
                    StartDate = ScheduleDate.Date,
                    StartTimeAc = ScheduleDate.Tbl_ScheduleEmployees?.FirstOrDefault()?.StartTime,
                    EndTimeAc = ScheduleDate.Tbl_ScheduleEmployees?.FirstOrDefault()?.EndTime,
                    Status = ScheduleDate.Status,
                    Location = ScheduleDate.Tbl_Schedule.Tbl_Customers.Address,
                    CustomerName = ScheduleDate.Tbl_Schedule.Tbl_Customers.FirstName + " " + ScheduleDate.Tbl_Schedule.Tbl_Customers.LastName,
                    CustomerPhone = PropertyBaseDTO.CustomerPhone.ToLower() == "true" ? ScheduleDate.Tbl_Schedule.Tbl_Customers.Phone1 : "xxxxxxxx", 
                    CallId = ScheduleDate.Tbl_Schedule.CallId,
                    EmpsID = empsIds,
                    OneScheduleDate = _db.Tbl_ScheduleDate.Where(d => d.Id == ScheduleDate.Id).Select(s => new PropertyScheduleDateDTO
                    {
                        Id = s.Id,
                        Date = s.Date,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        Active = s.Active,
                        Status = s.Status,
                    }).FirstOrDefault(),
                    LstScheduleEmployeeDTO = _db.Tbl_ScheduleEmployees.Where(p => empsIds.Contains(p.EmpId.Value) == true && p.ScheduleDateId == ScheduleDate.Id).Select(y => new PropertyScheduleEmployeesDTO
                    {
                        EmpId = y.EmpId.Value,
                        EmpFirstName = y.Tbl_Employee.FirstName,
                        EmpLastName = y.Tbl_Employee.LastName,
                        EmpUserName = y.Tbl_Employee.UserName,
                        EmpFullName = y.Tbl_Employee.FirstName + " " + y.Tbl_Employee.LastName,
                    }).ToList()
                };

                //Oobj.EmpsID.Concat(EmpIds);
                switch (EmpRole)
                {
                    case 1://staff
                        {
                            if (Oobj.EmpsID.Contains(EmpId) == true)
                            {
                                lstSchedules.Add(Oobj);
                            }
                        }
                        break;
                    case 3://manger
                        {
                            int In = 0;
                            foreach (var item in EmpIds)
                            {
                                if (Oobj.EmpsID.Contains(item) == true)
                                {
                                    In = 1;
                                }
                            }
                            if (In == 1)
                            {
                                lstSchedules.Add(Oobj);
                            }
                            if (Oobj.EmpsID.Contains(EmpId) == true)
                            {
                                lstSchedules.Add(Oobj);
                            }
                        }
                        break;
                    case 4://owner
                        {
                            lstSchedules.Add(Oobj);
                        }
                        break;
                }
            }

            return lstSchedules.ToList();
        }


        public PropertySchedulesDTO GetOneScheduleDetails(int? ScheduleId, int SchDateId)
        {
            PropertySchedulesDTO Schedule = new PropertySchedulesDTO();

            //if (string.IsNullOrEmpty(SchDate) == true)
            //{
            //    SchDate = _db.Tbl_ScheduleDate.Where(c => c.ScheduleId == ScheduleId).FirstOrDefault().Date;
            //}


            //Ahmed
            //var ScheduleDate = _db.Tbl_ScheduleDate.Where(x => x.ScheduleId == ScheduleId && x.Id == SchDateId).FirstOrDefault();
            Schedule = _db.Tbl_Schedule.Where(x => x.Id == ScheduleId).Select(x => new PropertySchedulesDTO
            {
                Id = x.Id,
                ScheduleDateId = SchDateId,
                CountPhotos = x.Tbl_SchedulePictures.Count,
                AccountId = x.AccountId,
                BrancheId = x.BrancheId,
                ContractId = x.ContractId,
                Title = x.Title,
                Description = x.Description,
                Recurring = x.Recurring,
                FrequencyType = x.FrequencyType,
                StartDate = x.StartDate,
                ScheduleDate = x.ScheduleDate,
                StartTimeAc = _db.Tbl_ScheduleEmployees.FirstOrDefault(h => h.ScheduleDateId == SchDateId).StartTime,
                EndTimeAc = _db.Tbl_ScheduleEmployees.FirstOrDefault(h => h.ScheduleDateId == SchDateId).EndTime,
                Time = x.Time,
                EndType = x.EndType,
                EndDate = x.EndDate,
                CalendarColor = x.CalendarColor,
                ShowMoreOptions = x.ShowMoreOptions,
                InvoiceableTask = x.InvoiceableTask,
                CustomerId = x.CustomerId,
                TimeEnd = x.TimeEnd,
                PriorityId = x.PriorityId,
                OneScheduleDate = x.Tbl_ScheduleDate.Where(c => c.ScheduleId == ScheduleId && c.Id == SchDateId).Select(d => new PropertyScheduleDateDTO
                {
                    Id = d.Id,
                    AccountId = d.AccountId,
                    BrancheId = d.BrancheId,
                    ScheduleId = d.ScheduleId,
                    Date = d.Date,
                    ScheduleStartTime = d.StartTime,
                    ScheduleEndTime = d.EndTime,
                    StartTime = _db.Tbl_ScheduleEmployees.FirstOrDefault(h => h.ScheduleDateId == d.Id).StartTime,
                    EndTime = _db.Tbl_ScheduleEmployees.FirstOrDefault(h => h.ScheduleDateId == d.Id).EndTime,
                    SpentTimeHour = _db.Tbl_ScheduleEmployees.FirstOrDefault(h => h.ScheduleDateId == d.Id).SpentTimeHour,
                    SpentTimeMin = _db.Tbl_ScheduleEmployees.FirstOrDefault(h => h.ScheduleDateId == d.Id).SpentTimeMin,
                    CreateOrginal_Custom = d.CreateOrginal_Custom,
                    CustomerName = d.Tbl_Schedule.Tbl_Customers.FirstName + " " + d.Tbl_Schedule.Tbl_Customers.LastName,
                    CustomerPhone = PropertyBaseDTO.CustomerPhone.ToLower() == "true" ? d.Tbl_Schedule.Tbl_Customers.Phone1 : "xxxxxxxx", 
                    CustomerEmail = d.Tbl_Schedule.Tbl_Customers.Email,
                    GoogleReviewLink = d.Tbl_Branches.GoogleReviewLink,
                    Status = d.Status,
                    Reasonnotserve = d.Reasonnotserve,
                    CalendarColor = d.CalendarColor,
                    InvoiceId = d.InvoiceId,
                    EstimateId = d.EstimateId,
                    Active = d.Active,
                    Notes = d.Notes,
                    CreateUser = d.CreateUser,
                    CreateDate = d.CreateDate,
                }).FirstOrDefault(),
                CustomerDTO = new PropertyCustomersDTO
                {
                    Id = x.Tbl_Customers.Id,
                    AccountId = x.Tbl_Customers.AccountId,
                    BrancheId = x.Tbl_Customers.BrancheId,
                    FirstName = x.Tbl_Customers.FirstName,
                    LastName = x.Tbl_Customers.LastName,
                    Email = x.Tbl_Customers.Email,
                    CategoryId = x.Tbl_Customers.CategoryId,
                    CustomerType = x.Tbl_Customers.CustomerType,
                    AllowLogin = x.Tbl_Customers.AllowLogin,
                    Source = x.Tbl_Customers.Source,
                    Country = x.Tbl_Customers.Country,
                    Address = x.Tbl_Customers.Address,
                    City = x.Tbl_Customers.City,
                    State = x.Tbl_Customers.State,
                    YearBuit = x.Tbl_Customers.YearBuit == null ? "None" : x.Tbl_Customers.YearBuit,
                    Squirefootage = x.Tbl_Customers.Squirefootage == null ? "None" : x.Tbl_Customers.Squirefootage,
                    EstimedValue = x.Tbl_Customers.EstimedValue == null ? "None" : x.Tbl_Customers.EstimedValue,
                    YearEstimedValue = x.Tbl_Customers.YearEstimedValue,
                    Discount = x.Tbl_Customers.Discount != null ? x.Tbl_Customers.Discount : x.Tbl_Customers.Tbl_Member.MemberValue,
                    Taxable = x.Tbl_Customers.Taxable,
                    TaxId = x.Tbl_Customers.TaxId,
                    MemeberId = x.Tbl_Customers.MemeberId,
                    MemeberType = x.Tbl_Customers.MemeberType,
                    MemberName = x.Tbl_Customers.Tbl_Member.Name,
                    locationlatitude = x.Tbl_Customers.locationlatitude,
                    locationlongitude = x.Tbl_Customers.locationlongitude,
                    Phone1 = PropertyBaseDTO.CustomerPhone.ToLower() == "true" ? x.Tbl_Customers.Phone1 : "xxxxxxxx" ,
                    Mobile = x.Tbl_Customers.Mobile,
                    MemeberExpireDate = x.Tbl_Customers.MemeberExpireDate,
                    MemberDTO = _db.Tbl_Member.Where(c => c.Id == x.Tbl_Customers.MemeberId).Select(s => new PropertyMemberDTO
                    {
                        Id = s.Id,
                        Name = s.Name,
                        MemberType = s.MemberType,
                        MemberValue = s.MemberValue,
                    }).FirstOrDefault(),
                    TaxDTO = _db.Tbl_Tax.Where(c => c.Id == x.Tbl_Customers.TaxId).Select(s => new PropertyTaxDTO
                    {
                        Id = s.Id,
                        Taxname = s.Taxname,
                        Rate = s.Rate,
                        Notes = s.Notes,
                        Active = s.Active,
                    }).FirstOrDefault(),
                    CampaignDTO = _db.Tbl_Campaigns.Where(c => c.Id == x.Tbl_Customers.Source).Select(s => new PropertyCampaignDTO
                    {
                        Id = s.Id,
                        AccountId = s.AccountId,
                        BrancheId = s.BrancheId,
                        Lable = s.Lable,
                        Description = s.Description,
                        Active = s.Active,
                    }).FirstOrDefault(),
                    Credit = x.Tbl_Customers.Credit,
                    Notes = x.Tbl_Customers.Notes,
                    CreateDate = x.Tbl_Customers.CreateDate,
                    CreateUser = x.Tbl_Customers.CreateUser,
                    CustomerCategory = new PropertyCustomersCategoryDTO
                    {
                        CategoryName = x.Tbl_Customers.Tbl_CustomersCategory.CategoryName,
                    },
                },
                Location = x.Location,
                EmployeeCategoryId = x.EmployeeCategoryId,
                Employees = x.Employees,
                CallId = x.CallId,
                //LstEmployeeDTO = _db.Tbl_Employee.Where(p => x.Employees.Contains(p.Id.ToString()) == true && p.BrancheId == x.BrancheId).Select(y => new PropertyEmployeeDTO
                //{
                //    Id = y.Id,
                //    AccountId = y.AccountId,
                //    BrancheId = y.BrancheId,
                //    BranchName = y.Tbl_Branches1.Name,
                //    FirstName = y.FirstName,
                //    LastName = y.LastName,
                //    UserName = y.UserName,
                //}).ToList(),

                Notes = x.Notes,
                Active = x.Active,
                CreateUser = x.CreateUser,
                CreateDate = x.CreateDate,

                //OneScheduleService = x.Tbl_ScheduleItemsServices.Where(f => f.ScheduleId == x.Id && f.ScheduleDateId == null).Select(y => new PropertyScheduleItemsServicesDTO
                //{
                //    Id = y.Id,
                //    AccountId = y.AccountId,
                //    BrancheId = y.BrancheId,
                //    ScheduleId = y.ScheduleId,
                //    ItemServiceDescription = y.ItemServiceDescription,
                //    ItemsServicesId = y.Tbl_ItemsServices.Id,
                //    TaxId = y.TaxId,
                //    Tax = y.Tax,
                //    ItemsServicesName = y.Tbl_ItemsServices.Name,
                //    CostRate = y.CostRate,
                //    Price = y.Price,
                //    Total = y.Total,

                //}).FirstOrDefault(),


                LstMaterialReceipt = x.Tbl_ScheduleMaterialReceipt.Where(f => f.ScheduleId == x.Id).Select(y => new PropertyScheduleMaterialReceiptDTO
                {
                    Id = y.Id,
                    AccountId = y.AccountId,
                    BrancheId = y.BrancheId,
                    ScheduleId = y.ScheduleId,
                    ScheduleDateId = y.ScheduleDateId,
                    SupplierId = y.SupplierId,
                    SupplierName = y.Tbl_Customers.FirstName + " " + y.Tbl_Customers.LastName,
                    TechnicianId = y.TechnicianId,
                    Cost = y.Cost,
                    Notes = y.Notes,
                    ReceiptPhoto = y.ReceiptPhoto,
                    CreateUser = y.CreateUser,
                    CreateDate = y.CreateDate,
                }).ToList(),



                EstimateDTO = _db.Tbl_EstimateScheduleDate.Where(g => g.ScheduleDateId == SchDateId).Select(s => new PropertyEstimateDTO
                {
                    Id = s.Tbl_Estimate.Id,
                    AccountId = s.Tbl_Estimate.AccountId,
                    BrancheId = s.Tbl_Estimate.BrancheId,
                    ScheduleId = s.Tbl_Estimate.ScheduleId,
                    SignatureDraw = s.Tbl_Estimate.SignatureDraw,
                    //LstScdDate = _db.Tbl_ScheduleDate.Where(c => c.ScheduleId == ScheduleId).Select(D => new PropertyScheduleDateDTO
                    //{
                    //    Id = D.Id,
                    //    Date = D.Date,
                    //}).ToList(),

                }).FirstOrDefault(),

                InvoiceDTO = _db.Tbl_InvoiceScheduleDate.Where(g => g.ScheduleDateId == SchDateId).Select(s => new PropertyInvoiceDTO
                {
                    Id = s.Tbl_Invoice.Id,
                    AccountId = s.Tbl_Invoice.AccountId,
                    BrancheId = s.Tbl_Invoice.BrancheId,
                    ScheduleId = s.Tbl_Invoice.ScheduleId,
                    Status = s.Tbl_Invoice.Status,
                    //LstScdDate = _db.Tbl_ScheduleDate.Where(c => c.ScheduleId == ScheduleId).Select(D => new PropertyScheduleDateDTO
                    //{
                    //    Id = D.Id,
                    //    Date = D.Date,
                    //}).ToList(),

                }).FirstOrDefault(),


                //LstEmployeeDTO = _db.Tbl_Employee.Where(p => x.Employees.Split(',').Select(t => int.Parse(t)).ToList().Contains(p.Id) == true).Select(y => new PropertyEmployeeDTO
                //{
                //    Id = y.Id,
                //    AccountId = y.AccountId,
                //    BrancheId = y.BrancheId,
                //    BranchName = y.Tbl_Branches1.Name,
                //    FirstName = y.FirstName,
                //    LastName = y.LastName,
                //    UserName = y.UserName,
                //}).ToList(),

            }).FirstOrDefault();

            if (Schedule.CustomerDTO.MemberDTO != null)
            {
                if (Schedule.CustomerDTO.MemeberExpireDate < DateTime.Parse(DateTime.Now.ToShortDateString()))
                {
                    Schedule.CustomerDTO.MemberDTO.MemberValue = 0;
                    Schedule.CustomerDTO.Discount = 0;
                }
            }

            //if (string.IsNullOrEmpty(Schedule?.Employees) != true)
            //{
            //    List<int> IDList = Schedule?.Employees?.Split(',').Select(t => int.Parse(t)).ToList();

            //    if (IDList.Count != 0)
            //    {
            //        Schedule.LstEmployeeDTO = _db.Tbl_Employee.Where(p => IDList.Contains(p.Id) == true).Select(y => new PropertyEmployeeDTO
            //        {
            //            Id = y.Id,
            //            AccountId = y.AccountId,
            //            BrancheId = y.BrancheId,
            //            BranchName = y.Tbl_Branches1.Name,
            //            FirstName = y.FirstName,
            //            LastName = y.LastName,
            //            UserName = y.UserName,
            //        }).ToList();
            //    }
            //}

            if (Schedule.EstimateDTO != null)
            {
                Schedule.EstimateDTO.LstScdDate = _db.Tbl_ScheduleDate.Where(c => c.ScheduleId == ScheduleId).Select(D => new PropertyScheduleDateDTO
                {
                    Id = D.Id,
                    Date = D.Date,
                }).ToList();
            }

            if (Schedule.InvoiceDTO != null)
            {
                Schedule.InvoiceDTO.LstScdDate = _db.Tbl_ScheduleDate.Where(c => c.ScheduleId == ScheduleId).Select(D => new PropertyScheduleDateDTO
                {
                    Id = D.Id,
                    Date = D.Date,
                }).ToList();
            }
             
            Schedule.LstScheduleEmployeeDTO = _db.Tbl_ScheduleEmployees.Where(p => p.ScheduleDateId == Schedule.OneScheduleDate.Id).Select(y => new PropertyScheduleEmployeesDTO
            {
                Id = y.Id,
                ScheduleDateId = y.ScheduleDateId,
                EmployeeCategoryId = y.EmployeeCategoryId,
                EmpId = y.EmpId,
                StartTime = y.StartTime,
                EndTime = y.EndTime,
                SpentTimeHour = y.SpentTimeMin,
                SpentTimeMin = y.SpentTimeMin,
                Duration = y.Duration,
                Labor = y.Labor,
                Status = y.Status,
                Reasonnotserve = y.Reasonnotserve,
                Notes = y.Notes,
                CreateUser = y.CreateUser,
                CreateDate = y.CreateDate,
                ScheduleDate = y.Tbl_ScheduleDate.Date,
                EmpFirstName = y.Tbl_Employee.FirstName,
                EmpLastName = y.Tbl_Employee.LastName,
                EmpUserName = y.Tbl_Employee.UserName,
                EmpFullName = y.Tbl_Employee.FirstName + " " + y.Tbl_Employee.LastName,
            }).ToList();


            Schedule.LstFirstCreateServices = _db.Tbl_ScheduleItemsServices.Where(f => f.ScheduleId == Schedule.Id && f.ScheduleDateId == null).Select(y => new PropertyScheduleItemsServicesDTO
            {
                Id = y.Id,
                AccountId = y.AccountId,
                BrancheId = y.BrancheId,
                ScheduleId = y.ScheduleId,
                ItemServiceDescription = y.ItemServiceDescription,
                ItemsServicesId = y.Tbl_ItemsServices.Id,
                TaxId = y.TaxId,
                Tax = y.Tax,
                ItemsServicesName = y.Tbl_ItemsServices.Name,
                CostRate = y.CostRate,
                Price = y.Price,
                Total = y.Total,
                Quantity = y.Quantity,

            }).ToList();

            Schedule.LstScheduleItemsServices = _db.Tbl_ScheduleItemsServices.Where(f => f.ScheduleId == Schedule.Id && f.ScheduleDateId != null && f.TypeMaterial_Services == 1).Select(y => new PropertyScheduleItemsServicesDTO
            {
                Id = y.Id,
                AccountId = y.AccountId,
                BrancheId = y.BrancheId,
                ScheduleId = y.ScheduleId,
                ItemServiceDescription = y.ItemServiceDescription,
                ItemsServicesId = y.Tbl_ItemsServices.Id,
                TaxId = y.TaxId,
                Tax = y.Tax,
                ItemsServicesName = y.Tbl_ItemsServices.Name,
                CostRate = y.CostRate,
                Price = y.Price,
                Total = y.Total,
                Quantity = y.Quantity,

            }).ToList();

            Schedule.LstFreeServices = _db.Tbl_ScheduleItemsServices.Where(f => f.ScheduleId == Schedule.Id && f.ScheduleDateId != null && f.TypeMaterial_Services == 2).Select(y => new PropertyScheduleItemsServicesDTO
            {
                Id = y.Id,
                AccountId = y.AccountId,
                BrancheId = y.BrancheId,
                ScheduleId = y.ScheduleId,
                ItemServiceDescription = y.ItemServiceDescription,
                ItemsServicesId = y.Tbl_ItemsServices.Id,
                TaxId = y.TaxId,
                Tax = y.Tax,
                ItemsServicesName = y.Tbl_ItemsServices.Name,
                CostRate = y.CostRate,
                Price = y.Price,
                Total = y.Total,
                Quantity = y.Quantity,

            }).ToList();

            return Schedule;
        }


        public PropertySchedulesDTO GetOneContractDetails(int? ScheduleId, int? ContractId, string SchDate)
        {
            PropertySchedulesDTO Schedule = new PropertySchedulesDTO();

            if (string.IsNullOrEmpty(SchDate) == true)
            {
                SchDate = _db.Tbl_ScheduleDate.Where(c => c.ScheduleId == ScheduleId).FirstOrDefault().Date;
            }

            var ScheduleDate = _db.Tbl_ScheduleDate.Where(x => x.ScheduleId == ScheduleId && x.Date == SchDate).FirstOrDefault();
            Schedule = _db.Tbl_Schedule.Where(x => x.Id == ScheduleId && x.ContractId == ContractId).Select(x => new PropertySchedulesDTO
            {
                Id = x.Id,
                AccountId = x.AccountId,
                BrancheId = x.BrancheId,
                ContractId = x.ContractId,
                Title = x.Title,
                Description = x.Description,
                Recurring = x.Recurring,
                FrequencyType = x.FrequencyType,
                StartDate = ScheduleDate.Date,
                ScheduleDate = x.ScheduleDate,
                Time = x.Time,
                EndType = x.EndType,
                EndDate = x.EndDate,
                CalendarColor = x.CalendarColor,
                ShowMoreOptions = x.ShowMoreOptions,
                InvoiceableTask = x.InvoiceableTask,
                CustomerId = x.CustomerId,
                TimeEnd = x.TimeEnd,
                PriorityId = x.PriorityId,
                OneScheduleDate = x.Tbl_ScheduleDate.Where(c => c.ScheduleId == ScheduleId && c.Date == SchDate).Select(d => new PropertyScheduleDateDTO
                {
                    Id = d.Id,
                    AccountId = d.AccountId,
                    BrancheId = d.BrancheId,
                    ScheduleId = d.ScheduleId,
                    Date = d.Date,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                    CreateOrginal_Custom = d.CreateOrginal_Custom,
                    Status = d.Status,
                    Reasonnotserve = d.Reasonnotserve,
                    CalendarColor = d.CalendarColor,
                    InvoiceId = d.InvoiceId,
                    EstimateId = d.EstimateId,
                    Active = d.Active,
                    Notes = d.Notes,
                    CreateUser = d.CreateUser,
                    CreateDate = d.CreateDate,
                }).FirstOrDefault(),
                CustomerDTO = new PropertyCustomersDTO
                {
                    Id = x.Tbl_Customers.Id,
                    AccountId = x.Tbl_Customers.AccountId,
                    BrancheId = x.Tbl_Customers.BrancheId,
                    FirstName = x.Tbl_Customers.FirstName,
                    LastName = x.Tbl_Customers.LastName,
                    Email = x.Tbl_Customers.Email,
                    CategoryId = x.Tbl_Customers.CategoryId,
                    CustomerType = x.Tbl_Customers.CustomerType,
                    AllowLogin = x.Tbl_Customers.AllowLogin,
                    Source = x.Tbl_Customers.Source,
                    Country = x.Tbl_Customers.Country,
                    Address = x.Tbl_Customers.Address,
                    City = x.Tbl_Customers.City,
                    State = x.Tbl_Customers.State,
                    Discount = x.Tbl_Customers.Discount != null ? x.Tbl_Customers.Discount : x.Tbl_Customers.Tbl_Member.MemberValue,
                    Taxable = x.Tbl_Customers.Taxable,
                    TaxId = x.Tbl_Customers.TaxId,
                    MemeberId = x.Tbl_Customers.MemeberId,
                    MemeberType = x.Tbl_Customers.MemeberType,
                    MemberName = x.Tbl_Customers.Tbl_Member.Name,
                    locationlatitude = x.Tbl_Customers.locationlatitude,
                    locationlongitude = x.Tbl_Customers.locationlongitude,
                    Phone1 = PropertyBaseDTO.CustomerPhone.ToLower() == "true" ? x.Tbl_Customers.Phone1 : "xxxxxxxx",
                    Mobile = x.Tbl_Customers.Mobile,
                    MemberDTO = _db.Tbl_Member.Where(c => c.Id == x.Tbl_Customers.MemeberId).Select(s => new PropertyMemberDTO
                    {
                        Id = s.Id,
                        Name = s.Name,
                        MemberType = s.MemberType,
                        MemberValue = s.MemberValue,
                    }).FirstOrDefault(),
                    TaxDTO = _db.Tbl_Tax.Where(c => c.Id == x.Tbl_Customers.TaxId).Select(s => new PropertyTaxDTO
                    {
                        Id = s.Id,
                        Taxname = s.Taxname,
                        Rate = s.Rate,
                        Notes = s.Notes,
                        Active = s.Active,
                    }).FirstOrDefault(),
                    Credit = x.Tbl_Customers.Credit,
                    Notes = x.Tbl_Customers.Notes,
                    CreateDate = x.Tbl_Customers.CreateDate,
                    CreateUser = x.Tbl_Customers.CreateUser,
                    CustomerCategory = new PropertyCustomersCategoryDTO
                    {
                        CategoryName = x.Tbl_Customers.Tbl_CustomersCategory.CategoryName,
                    },
                },
                Location = x.Location,
                EmployeeCategoryId = x.EmployeeCategoryId,
                Employees = x.Employees,
                CallId = x.CallId,
                //LstEmployeeDTO = _db.Tbl_Employee.Where(p => x.Employees.Contains(p.Id.ToString()) == true && p.BrancheId == x.BrancheId).Select(y => new PropertyEmployeeDTO
                //{
                //    Id = y.Id,
                //    AccountId = y.AccountId,
                //    BrancheId = y.BrancheId,
                //    BranchName = y.Tbl_Branches1.Name,
                //    FirstName = y.FirstName,
                //    LastName = y.LastName,
                //    UserName = y.UserName,
                //}).ToList(),

                Notes = x.Notes,
                Active = x.Active,
                CreateUser = x.CreateUser,
                CreateDate = x.CreateDate,

                LstScheduleItemsServices = x.Tbl_ScheduleItemsServices.Where(f => f.ScheduleId == x.Id).Select(y => new PropertyScheduleItemsServicesDTO
                {
                    Id = y.Id,
                    AccountId = y.AccountId,
                    BrancheId = y.BrancheId,
                    ScheduleId = y.ScheduleId,
                    ItemServiceDescription = y.ItemServiceDescription,
                    ItemsServicesId = y.Tbl_ItemsServices.Id,
                    TaxId = y.TaxId,
                    Tax = y.Tax,
                    ItemsServicesName = y.Tbl_ItemsServices.Name,
                    CostRate = y.CostRate,

                }).ToList(),

                EstimateDTO = _db.Tbl_Estimate.Where(g => g.ScheduleId.Value == ScheduleId).Select(s => new PropertyEstimateDTO
                {
                    Id = s.Id,
                    AccountId = s.AccountId,
                    BrancheId = s.BrancheId,
                    ScheduleId = s.ScheduleId,

                }).FirstOrDefault(),

                InvoiceDTO = _db.Tbl_Invoice.Where(g => g.ScheduleId.Value == ScheduleId).Select(s => new PropertyInvoiceDTO
                {
                    Id = s.Id,
                    AccountId = s.AccountId,
                    BrancheId = s.BrancheId,
                    ScheduleId = s.ScheduleId,
                    Status = s.Status,

                }).FirstOrDefault(),
            }).FirstOrDefault();

            if (string.IsNullOrEmpty(Schedule?.Employees) != true)
            {
                List<int> IDList = Schedule?.Employees?.Split(',').Select(t => int.Parse(t)).ToList();

                if (IDList.Count != 0)
                {
                    Schedule.LstEmployeeDTO = _db.Tbl_Employee.Where(p => IDList.Contains(p.Id) == true).Select(y => new PropertyEmployeeDTO
                    {
                        Id = y.Id,
                        AccountId = y.AccountId,
                        BrancheId = _db.Tbl_EmployeeBranches.Where(d => d.EmployeeId == y.Id).Select(s => s.BrancheId).FirstOrDefault(),
                        BranchName = _db.Tbl_Branches.Where(d => d.Id == _db.Tbl_EmployeeBranches.Where(f => f.EmployeeId == y.Id).Select(g => g.BrancheId).FirstOrDefault()).Select(s => s.Name).FirstOrDefault(),
                        FirstName = y.FirstName,
                        LastName = y.LastName,
                        UserName = y.UserName,
                    }).ToList();
                }
            }

            return Schedule;
        }

        public List<PropertyScheduleDateDTO> GetScheduleDate(int? ScheduleId, int Type)
        {
            List<PropertyScheduleDateDTO> LstScheduleDates = new List<PropertyScheduleDateDTO>();
            if (Type == 1) //All In Schedual
            {
                LstScheduleDates = _db.Tbl_ScheduleDate.Where(m => m.ScheduleId == ScheduleId).Select(j => new PropertyScheduleDateDTO { Id = j.Id, Date = j.Date }).ToList();
            }
            else if (Type == 2) //All ScheduleDate Not Have Invoice
            {
                LstScheduleDates = _db.Tbl_ScheduleDate.Where(m => m.ScheduleId == ScheduleId && m.InvoiceId == null).Select(j => new PropertyScheduleDateDTO { Id = j.Id, Date = j.Date }).ToList();
            }
            else if (Type == 3) //All ScheduleDate Not Have Estimet
            {
                LstScheduleDates = _db.Tbl_ScheduleDate.Where(m => m.ScheduleId == ScheduleId && m.EstimateId == null).Select(j => new PropertyScheduleDateDTO { Id = j.Id, Date = j.Date }).ToList();
            }
            return LstScheduleDates;
        }

        public PropertyItemsServicesDTO GetServiceDetails(int? AccountId, int? ServiceId)
        {
            var service = _db.Tbl_ItemsServices.Where(c => c.AccountId == AccountId && c.Id == ServiceId).Select(y => new PropertyItemsServicesDTO
            {
                Id = y.Id,
                AccountId = y.AccountId,
                BrancheId = y.BrancheId,
                Name = y.Name,
                CostperUnit = y.CostperUnit,
                Type = y.Type,
                CategoryId = y.CategoryId,
                InventoryItem = y.InventoryItem,
                QTYTime = y.QTYTime,
                Unit = y.Unit,
                MemeberType = y.MemeberType,
                MemeberId = y.MemeberId,
                TaxId = y.TaxId,
                Tax = y.Tax,
                Taxable = y.Taxable,
                Description = y.Description,
                Details = y.Details,
                Picture = y.Picture,
                SKU = y.SKU,
                Notes = y.Notes,
                Active = y.Active,
                CreateUser = y.CreateUser,
                CreateDate = y.CreateDate,
            }).FirstOrDefault();

            return service;
        }

        public void InsertSchedule(PropertySchedulesDTO model)
        {
            int Check = 0;

            //Check Customer
            var entityScheduleCustomer = _db.Tbl_Schedule.FirstOrDefault(x => x.CustomerId == model.CustomerId && x.ScheduleDate == model.ScheduleDate && x.Time == model.Time);
            if (entityScheduleCustomer != null)
            {
                Check = 1;
                throw new ArgumentException(message: $"The Schedule time {model.Time} already exists.");
            }

            if (Check == 0)
            {
                var entitySchedule = new Tbl_Schedule
                {
                    AccountId = model.AccountId,
                    BrancheId = model.BrancheId,
                    ContractId = model.ContractId,
                    Title = model.Title,
                    Description = model.Description,
                    Recurring = model.Recurring,
                    FrequencyType = model.FrequencyType,
                    StartDate = model.StartDate,
                    ScheduleDate = model.ScheduleDate,
                    Time = model.Time,
                    EndType = model.EndType,
                    EndDate = model.EndDate,
                    CalendarColor = model.CalendarColor,
                    ShowMoreOptions = model.ShowMoreOptions,
                    InvoiceableTask = model.InvoiceableTask,
                    CustomerId = model.CustomerId,
                    Location = model.Location,
                    EmployeeCategoryId = model.EmployeeCategoryId,
                    Employees = model.Employees,
                    CallId = model.CallId,
                    Notes = model.Notes,
                    Active = model.Active,
                    CreateUser = model.CreateUser,
                    CreateDate = model.CreateDate,
                    TimeEnd = model.TimeEnd,
                    PriorityId = model.PriorityId,
                };

                _db.Tbl_Schedule.Add(entitySchedule);
                _db.SaveChanges();

                int ScheduleId = entitySchedule.Id;


                var ScheduleDateDTO = new Tbl_ScheduleDate
                {
                    AccountId = model.AccountId,
                    BrancheId = model.BrancheId,
                    ScheduleId = ScheduleId,
                    Date = model.ScheduleDate,
                    StartTime = model.Time,
                    EndTime = model.TimeEnd,
                    CreateOrginal_Custom = 1, //first schedule
                    Status = 1,
                    CalendarColor = model.CalendarColor,
                    Reasonnotserve = null,
                    InvoiceId = null,
                    EstimateId = null,
                    Notes = null,
                    Active = false,// Dispatch 
                    CreateUser = model.CreateUser,
                    CreateDate = DateTime.Now,
                };

                _db.Tbl_ScheduleDate.Add(ScheduleDateDTO);
                _db.SaveChanges();

                int? ScheduleDateId = ScheduleDateDTO.Id;

                List<int> EmpIds = new List<int>();

                if (ScheduleDateId != null)
                {
                    EmpIds = model.Employees.Split(',').Select(t => int.Parse(t)).ToList();

                    foreach (int empId in EmpIds)
                    {
                        var EmpDTO = _db.Tbl_Employee.Where(x => x.Id == empId).FirstOrDefault();
                        var ScheduleEmployeesDTO = new Tbl_ScheduleEmployees
                        {
                            AccountId = model.AccountId,
                            ScheduleDateId = ScheduleDateId,
                            EmployeeCategoryId = EmpDTO.CategoryId,
                            EmpId = EmpDTO.Id,
                            StartTime = null,
                            EndTime = null,
                            SpentTimeHour = null,
                            SpentTimeMin = null,
                            Labor = null,
                            Duration = null,
                            Status = 1,
                            Reasonnotserve = null,
                            Notes = null,
                            CreateUser = model.CreateUser,
                            CreateDate = DateTime.Now,
                        };

                        _db.Tbl_ScheduleEmployees.Add(ScheduleEmployeesDTO);
                        _db.SaveChanges();
                    }
                }

                var item = model.OneScheduleService;

                var entityService = new Tbl_ScheduleItemsServices
                {
                    AccountId = model.AccountId,
                    BrancheId = model.BrancheId,
                    ScheduleId = ScheduleId,
                    ScheduleDateId = null,
                    ItemsServicesId = item.ItemsServicesId,
                    ItemServiceDescription = item.ItemServiceDescription,
                    CostRate = item.CostRate,
                    Quantity = null,
                    Price = item.Price,
                    Total = item.Total,
                    TypeMaterial_Services = null, //first one Service only without cost
                    TaxId = item.TaxId,
                    Tax = item.Tax,
                    Notes = item.Notes,
                    Active = item.Active,
                    CreateUser = model.CreateUser,
                    CreateDate = DateTime.Now,
                };

                _db.Tbl_ScheduleItemsServices.Add(entityService);
                _db.SaveChanges();

                if (model.CallId != 0 && model.CallId != null)
                {
                    var modelcall = _db.Tbl_Calls.Where(l => l.Id == model.CallId).FirstOrDefault();
                    modelcall.ScheduleId = ScheduleId;
                    modelcall.ServiceId = item.ItemsServicesId;
                    _db.Entry(modelcall).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                SendNotify(model, "", ScheduleId, ScheduleDateId.Value);

                //foreach (var item in model.LstScheduleItemsServices)
                //{
                //    var entityItemsServices = new Tbl_ScheduleItemsServices
                //    {
                //        AccountId = model.AccountId,
                //        BrancheId = model.BrancheId,
                //        ScheduleId = ScheduleId,
                //        ItemsServicesId = item.ItemsServicesId,
                //        ItemServiceDescription = item.ItemServiceDescription,
                //        CostRate = item.CostRate,
                //        TaxId = item.TaxId,
                //        Tax = item.Tax,
                //        Notes = item.Notes,
                //        Active = item.Active,
                //        CreateUser = model.CreateUser,
                //        CreateDate = DateTime.Now,
                //    };

                //    _db.Tbl_ScheduleItemsServices.Add(entityItemsServices);
                //    _db.SaveChanges();
                //}





                //if (model.LstMaterialReceipt.Count > 0)
                //{
                //    foreach (PropertyScheduleMaterialReceiptDTO materialReceipt in model.LstMaterialReceipt)
                //    {
                //        string FileNameSchduleImage = "";
                //        if (!string.IsNullOrEmpty(materialReceipt.ReceiptPhoto))
                //        {
                //            string pathPhoto = PropertyBaseDTO.PathUrlImage + "\\";

                //            Guid obj = Guid.NewGuid();
                //            pathPhoto += obj + ".jpg";
                //            FileNameSchduleImage = obj + ".jpg";

                //            byte[] imageData = Convert.FromBase64String(materialReceipt.ReceiptPhoto);
                //            MemoryStream ms = new MemoryStream(imageData);
                //            System.Drawing.Image postedFile = System.Drawing.Image.FromStream(ms);
                //            postedFile.Save(pathPhoto);
                //        }

                //        var MaterialReceipt = new Tbl_ScheduleMaterialReceipt
                //        {
                //            AccountId = materialReceipt.AccountId,
                //            BrancheId = materialReceipt.BrancheId,
                //            ScheduleId = materialReceipt.ScheduleId,
                //            ScheduleDateId = materialReceipt.ScheduleDateId,
                //            SupplierId = materialReceipt.SupplierId,
                //            TechnicianId = materialReceipt.TechnicianId,
                //            Cost = materialReceipt.Cost,
                //            Notes = materialReceipt.Notes,
                //            ReceiptPhoto = FileNameSchduleImage,
                //            CreateUser = materialReceipt.CreateUser,
                //            CreateDate = DateTime.Now,
                //        };

                //        _db.Tbl_ScheduleMaterialReceipt.Add(MaterialReceipt);
                //        _db.SaveChanges();
                //    }
                //}


            }
        }

        public void UpdateSchedule(PropertySchedulesDTO model)
        {
            var entityId = _db.Tbl_Schedule.FirstOrDefault(x => x.Id == model.Id);
            if (entityId != null)
            {
                string MsgEmpRemove = "";
                string MsgEmpAdd = "";
                ClsNotificationsDAL.GetEmp_Remove_Add_CreateSchedule(model.Id, model.Employees, out MsgEmpRemove, out MsgEmpAdd);

                var entityDateTb = _db.Tbl_ScheduleDate.FirstOrDefault(x => x.ScheduleId == model.Id && (String.IsNullOrEmpty(x.StartTime) == true || (String.IsNullOrEmpty(x.StartTime) == false && x.Date == model.ScheduleDate)));

                if (entityDateTb != null)
                {
                    entityId.AccountId = model.AccountId;
                    entityId.BrancheId = model.BrancheId;
                    entityId.ContractId = model.ContractId;
                    entityId.Title = model.Title;
                    entityId.Description = model.Description;
                    entityId.Recurring = model.Recurring;
                    entityId.FrequencyType = model.FrequencyType;
                    entityId.StartDate = model.StartDate;
                    entityId.ScheduleDate = model.ScheduleDate;
                    entityId.Time = model.Time;
                    entityId.EndType = model.EndType;
                    entityId.EndDate = model.EndDate;
                    entityId.CalendarColor = model.CalendarColor;
                    entityId.ShowMoreOptions = model.ShowMoreOptions;
                    entityId.InvoiceableTask = model.InvoiceableTask;
                    entityId.CustomerId = model.CustomerId;
                    entityId.Location = model.Location;
                    entityId.EmployeeCategoryId = model.EmployeeCategoryId;
                    entityId.Employees = model.Employees;
                    entityId.CallId = model.CallId;
                    entityId.Notes = model.Notes;
                    entityId.Active = model.Active;
                    entityId.CreateUser = model.CreateUser;
                    entityId.CreateDate = model.CreateDate;
                    entityId.PriorityId = model.PriorityId;
                    entityId.TimeEnd = model.TimeEnd;


                    _db.Entry(entityId).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                var entityDate = _db.Tbl_ScheduleDate.FirstOrDefault(x => x.Id == model.ScheduleDateId && String.IsNullOrEmpty(x.StartTime) == true);

                if (entityDate != null)
                {
                    entityDate.Date = model.ScheduleDate;
                    entityDate.CreateDate = DateTime.Now;

                    _db.Entry(entityDate).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                var entityDate1 = _db.Tbl_ScheduleDate.FirstOrDefault(x => x.Id == model.ScheduleDateId);

                List<int> EmpIds = model.Employees.Split(',').Select(t => int.Parse(t)).ToList();

                foreach (int empId in EmpIds)
                {
                    var EmpDTO = _db.Tbl_Employee.Where(x => x.Id == empId).FirstOrDefault();

                    var SchEmpDTO = _db.Tbl_ScheduleEmployees.Where(x => x.EmpId == EmpDTO.Id && x.ScheduleDateId == entityDate1.Id).FirstOrDefault();

                    if (SchEmpDTO == null)
                    {
                        var ScheduleEmployeesDTO = new Tbl_ScheduleEmployees
                        {
                            AccountId = model.AccountId,
                            ScheduleDateId = entityDate1.Id,
                            EmployeeCategoryId = EmpDTO.CategoryId,
                            EmpId = EmpDTO.Id,
                            StartTime = null,
                            EndTime = null,
                            SpentTimeHour = null,
                            SpentTimeMin = null,
                            Labor = null,
                            Duration = null,
                            Status = 1,
                            Reasonnotserve = null,
                            Notes = null,
                            CreateUser = model.CreateUser,
                            CreateDate = DateTime.Now,
                        };

                        _db.Tbl_ScheduleEmployees.Add(ScheduleEmployeesDTO);
                        _db.SaveChanges();
                    }

                }


                SendNotify_For_Remove_Emps(model, model.OneScheduleDate.CreateUser.Value, MsgEmpRemove, MsgEmpAdd);


                //var entityItems = _db.Tbl_ScheduleItemsServices.Where(x => x.ScheduleId == model.Id).ToList();

                //if (entityItems.Count > 0)
                //{
                //    _db.Tbl_ScheduleItemsServices.RemoveRange(entityItems);
                //    _db.SaveChanges();
                //}


                //foreach (var item in model.LstScheduleItemsServices)
                //{
                //    var entityItemsServices = new Tbl_ScheduleItemsServices
                //    {
                //        AccountId = model.AccountId,
                //        BrancheId = model.BrancheId,
                //        ScheduleId = item.ScheduleId,
                //        ItemsServicesId = item.ItemsServicesId,
                //        ItemServiceDescription = item.ItemServiceDescription,
                //        CostRate = item.CostRate,
                //        TaxId = item.TaxId,
                //        Tax = item.Tax,
                //        Notes = item.Notes,
                //        Active = item.Active,
                //        CreateUser = model.CreateUser,
                //        CreateDate = DateTime.Now,
                //    };

                //    _db.Tbl_ScheduleItemsServices.Add(entityItemsServices);
                //    _db.SaveChanges();
                //}




                //var entityMaterialReceipt = _db.Tbl_ScheduleMaterialReceipt.Where(x => x.ScheduleId == model.Id).ToList();

                //if (entityMaterialReceipt.Count > 0)
                //{
                //    _db.Tbl_ScheduleMaterialReceipt.RemoveRange(entityMaterialReceipt);
                //    _db.SaveChanges();
                //}


                //if (model.LstMaterialReceipt.Count > 0)
                //{
                //    foreach (var MaterialR in model.LstMaterialReceipt)
                //    {
                //        string FileNameSchduleImage = "";
                //        if (!string.IsNullOrEmpty(MaterialR.ReceiptPhoto))
                //        {
                //            string pathPhoto = PropertyBaseDTO.PathUrlImage + "\\";

                //            Guid obj = Guid.NewGuid();
                //            pathPhoto += obj + ".jpg";
                //            FileNameSchduleImage = obj + ".jpg";

                //            byte[] imageData = Convert.FromBase64String(MaterialR.ReceiptPhoto);
                //            MemoryStream ms = new MemoryStream(imageData);
                //            System.Drawing.Image postedFile = System.Drawing.Image.FromStream(ms);
                //            postedFile.Save(pathPhoto);
                //        }

                //        var entityMaterialRec = new Tbl_ScheduleMaterialReceipt
                //        {
                //            AccountId = MaterialR.AccountId,
                //            BrancheId = MaterialR.BrancheId,
                //            ScheduleId = MaterialR.ScheduleId,
                //            ScheduleDateId = MaterialR.ScheduleDateId,
                //            SupplierId = MaterialR.SupplierId,
                //            TechnicianId = MaterialR.TechnicianId,
                //            Cost = MaterialR.Cost,
                //            Notes = MaterialR.Notes,
                //            ReceiptPhoto = "",
                //            CreateUser = MaterialR.CreateUser,
                //            CreateDate = DateTime.Now,
                //        };

                //        _db.Tbl_ScheduleMaterialReceipt.Add(entityMaterialRec);
                //        _db.SaveChanges();
                //    }
                //}

            }
            else
            {
                throw new ArgumentException(message: $"This Title " + model.Title + " Is Deleted");
            }

        }

        public string UpdateScheduleDate(PropertyScheduleDateDTO model)
        {
            string Msg = "";
            var entityId = _db.Tbl_ScheduleDate.FirstOrDefault(x => x.Id == model.Id);
            if (entityId.Status != 2)
            {
                if (model.Status == 2)
                {
                    var entityEmployeesId = _db.Tbl_ScheduleEmployees.Where(x => x.ScheduleDateId == model.Id && x.Status == 1).ToList();
                    if (entityEmployeesId.Count > 0)
                    {
                        //error
                        Msg = "Not Done All Employee";
                        //throw new ArgumentException(message: "Not Done All Employee");
                    }
                }
                else if (model.Status == 0) // Not Serviced
                {
                    var ScheduleEmployeesList = _db.Tbl_ScheduleEmployees.Where(m => m.ScheduleDateId == entityId.Id && m.Status == 1).ToList();
                    foreach (var item in ScheduleEmployeesList)
                    {
                        item.Reasonnotserve = model.Reasonnotserve;
                        item.Status = model.Status;
                        _db.Entry(item).State = EntityState.Modified;
                        _db.SaveChanges();
                    }
                }
                else if (model.Status == 1)// Re Open
                {
                    var ScheduleEmployeesList = _db.Tbl_ScheduleEmployees.Where(m => m.ScheduleDateId == entityId.Id && m.Status == 0).ToList();
                    foreach (var item in ScheduleEmployeesList)
                    {
                        item.Status = model.Status;
                        _db.Entry(item).State = EntityState.Modified;
                        _db.SaveChanges();
                    }
                }
            }
            if (entityId != null && Msg != "Not Done All Employee")
            {
                entityId.AccountId = model.AccountId;
                entityId.BrancheId = model.BrancheId;
                entityId.ScheduleId = model.ScheduleId;
                entityId.Date = model.Date;
                entityId.StartTime = model.ScheduleStartTime;
                entityId.EndTime = model.ScheduleEndTime;
                entityId.Status = model.Status;
                entityId.CalendarColor = model.CalendarColor;
                entityId.Reasonnotserve = model.Reasonnotserve;
                entityId.CalendarColor = model.CalendarColor;
                entityId.Notes = model.Notes;
                entityId.Active = model.Active;
                entityId.CreateUser = model.CreateUser;
                entityId.CreateDate = model.CreateDate;

                _db.Entry(entityId).State = EntityState.Modified;
                _db.SaveChanges();

                Msg = "Success";
            }

            return Msg;
        }


        public void InsertScheduleMaterials(PropertyScheduleItemsServicesDTO ScheduleItemsServices)
        {
            string Msg = "";
            var Productmodel = _db.Tbl_ItemsServices.Where(x => x.Id == ScheduleItemsServices.ItemsServicesId).FirstOrDefault();

            if (Productmodel != null && Productmodel.Id != 0 && Productmodel.InventoryItem == true)
            {
                if (Productmodel.QTYTime < ScheduleItemsServices.Quantity)
                {
                    Msg = "The Quantity Of Product(s) " + Productmodel.Name + " N.O# " + Productmodel.Id + " Not_Enough ";
                }
            }

            if (!string.IsNullOrEmpty(Msg))
            {
                throw new ArgumentException(message: Msg);
            }

            var entityItems = _db.Tbl_ScheduleItemsServices.FirstOrDefault(x => x.Id == ScheduleItemsServices.Id && x.ScheduleId == ScheduleItemsServices.ScheduleId);

            if (entityItems != null)
            {
                _db.Tbl_ScheduleItemsServices.Remove(entityItems);
                _db.SaveChanges();
            }

            if (ScheduleItemsServices != null)
            {
                var entityItemsServices = new Tbl_ScheduleItemsServices
                {
                    AccountId = ScheduleItemsServices.AccountId,
                    BrancheId = ScheduleItemsServices.BrancheId,
                    ScheduleId = ScheduleItemsServices.ScheduleId,
                    ScheduleDateId = ScheduleItemsServices.ScheduleDateId,
                    ItemsServicesId = ScheduleItemsServices.ItemsServicesId,
                    ItemServiceDescription = ScheduleItemsServices.ItemServiceDescription,
                    CostRate = ScheduleItemsServices.CostRate,
                    Quantity = ScheduleItemsServices.Quantity,
                    Price = ScheduleItemsServices.Price,
                    Total = ScheduleItemsServices.Total,
                    TaxId = ScheduleItemsServices.TaxId,
                    Tax = ScheduleItemsServices.Tax,
                    TypeMaterial_Services = 1, // Material only
                    Notes = ScheduleItemsServices.Notes,
                    Active = ScheduleItemsServices.Active,
                    CreateUser = ScheduleItemsServices.CreateUser,
                    CreateDate = DateTime.Now,
                };

                _db.Tbl_ScheduleItemsServices.Add(entityItemsServices);
                _db.SaveChanges();

                ScheduleItemsServices.Id = entityItemsServices.Id;

                var Items = _db.Tbl_ItemsServices.Where(x => x.Id == ScheduleItemsServices.ItemsServicesId).FirstOrDefault();
                Items.QTYTime -= ScheduleItemsServices.Quantity;
                _db.Entry(Items).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        public void InsertScheduleFreeServices(PropertyScheduleItemsServicesDTO ScheduleItemsServices)
        {
            var entityItems = _db.Tbl_ScheduleItemsServices.FirstOrDefault(x => x.Id == ScheduleItemsServices.Id && x.ScheduleId == ScheduleItemsServices.ScheduleId);

            if (entityItems != null)
            {
                _db.Tbl_ScheduleItemsServices.Remove(entityItems);
                _db.SaveChanges();
            }

            if (ScheduleItemsServices != null)
            {
                var entityItemsServices = new Tbl_ScheduleItemsServices
                {
                    AccountId = ScheduleItemsServices.AccountId,
                    BrancheId = ScheduleItemsServices.BrancheId,
                    ScheduleId = ScheduleItemsServices.ScheduleId,
                    ScheduleDateId = ScheduleItemsServices.ScheduleDateId,
                    ItemsServicesId = ScheduleItemsServices.ItemsServicesId,
                    ItemServiceDescription = ScheduleItemsServices.ItemServiceDescription,
                    CostRate = ScheduleItemsServices.CostRate,
                    Quantity = ScheduleItemsServices.Quantity,
                    Price = ScheduleItemsServices.Price,
                    Total = ScheduleItemsServices.Total,
                    TaxId = ScheduleItemsServices.TaxId,
                    Tax = ScheduleItemsServices.Tax,
                    TypeMaterial_Services = 2, // Free Service only
                    Notes = ScheduleItemsServices.Notes,
                    Active = ScheduleItemsServices.Active,
                    CreateUser = ScheduleItemsServices.CreateUser,
                    CreateDate = DateTime.Now,
                };

                _db.Tbl_ScheduleItemsServices.Add(entityItemsServices);
                _db.SaveChanges();

                ScheduleItemsServices.Id = entityItemsServices.Id;
            }
        }


        public void DeleteScheduleMaterial(PropertyScheduleItemsServicesDTO model)
        {
            var entity = _db.Tbl_ScheduleItemsServices.FirstOrDefault(x => x.Id == model.Id);
            if (entity != null)
            {
                _db.Tbl_ScheduleItemsServices.Remove(entity);
                _db.SaveChanges();

                var Items = _db.Tbl_ItemsServices.Where(x => x.Id == model.ItemsServicesId).FirstOrDefault();
                Items.QTYTime += model.Quantity;
                _db.Entry(Items).State = EntityState.Modified;
                _db.SaveChanges();
            }
            else
            {
                throw new ArgumentException(message: $"This CheckInOut Id " + model.Id + " Is Deleted");
            }
        }

        public void DeleteScheduleFreeService(PropertyScheduleItemsServicesDTO model)
        {
            var entity = _db.Tbl_ScheduleItemsServices.FirstOrDefault(x => x.Id == model.Id);
            if (entity != null)
            {
                _db.Tbl_ScheduleItemsServices.Remove(entity);
                _db.SaveChanges();
            }
            else
            {
                throw new ArgumentException(message: $"This CheckInOut Id " + model.Id + " Is Deleted");
            }
        }


        public void InsertPostScheduleMaterialReceipt(PropertyScheduleMaterialReceiptDTO ScheduleMaterialReceipt)
        {
            var entityMaterialReceipt = _db.Tbl_ScheduleMaterialReceipt.FirstOrDefault(x => x.Id == ScheduleMaterialReceipt.Id && x.ScheduleId == ScheduleMaterialReceipt.ScheduleId);

            if (entityMaterialReceipt != null)
            {
                _db.Tbl_ScheduleMaterialReceipt.Remove(entityMaterialReceipt);
                _db.SaveChanges();
            }
            if (ScheduleMaterialReceipt != null)
            {

                string FileNameSchduleImage = "";
                if (!string.IsNullOrEmpty(ScheduleMaterialReceipt.ReceiptPhoto))
                {
                    string pathPhoto = PropertyBaseDTO.PathUrlImageMaterialReceipt + "\\";

                    Guid obj = Guid.NewGuid();
                    pathPhoto += obj + ".jpg";
                    FileNameSchduleImage = obj + ".jpg";

                    byte[] imageData = Convert.FromBase64String(ScheduleMaterialReceipt.ReceiptPhoto);
                    MemoryStream ms = new MemoryStream(imageData);
                    System.Drawing.Image postedFile = System.Drawing.Image.FromStream(ms);
                    postedFile.Save(pathPhoto);
                }

                var entityMaterialRec = new Tbl_ScheduleMaterialReceipt
                {
                    AccountId = ScheduleMaterialReceipt.AccountId,
                    BrancheId = ScheduleMaterialReceipt.BrancheId,
                    ScheduleId = ScheduleMaterialReceipt.ScheduleId,
                    ScheduleDateId = ScheduleMaterialReceipt.ScheduleDateId,
                    SupplierId = ScheduleMaterialReceipt.SupplierId,
                    TechnicianId = ScheduleMaterialReceipt.TechnicianId,
                    Cost = ScheduleMaterialReceipt.Cost,
                    Notes = ScheduleMaterialReceipt.Notes,
                    ReceiptPhoto = ScheduleMaterialReceipt.ReceiptPhoto = FileNameSchduleImage,
                    CreateUser = ScheduleMaterialReceipt.CreateUser,
                    CreateDate = DateTime.Now,
                };

                _db.Tbl_ScheduleMaterialReceipt.Add(entityMaterialRec);
                _db.SaveChanges();

                ScheduleMaterialReceipt.Id = entityMaterialRec.Id;
            }
        }


        public void DeleteMaterialReceipt(PropertyScheduleMaterialReceiptDTO model)
        {
            var entity = _db.Tbl_ScheduleMaterialReceipt.FirstOrDefault(x => x.Id == model.Id);
            if (entity != null)
            {
                _db.Tbl_ScheduleMaterialReceipt.Remove(entity);
                _db.SaveChanges();

                string pathDelete = PropertyBaseDTO.PathUrlImageMaterialReceipt + "\\" + entity.ReceiptPhoto;
                if (System.IO.File.Exists(pathDelete))
                {
                    System.IO.File.Delete(pathDelete);
                }

            }
            else
            {
                throw new ArgumentException(message: $"This CheckInOut Id " + model.Id + " Is Deleted");
            }
        }


        //public void PostScheduleItemService(PropertyScheduleItemsServicesDTO item)
        //{
        //    var entityItemsServices = new Tbl_ScheduleItemsServices
        //    {
        //        AccountId = item.AccountId,
        //        BrancheId = item.BrancheId,
        //        ScheduleId = item.ScheduleId,
        //        ItemsServicesId = item.ItemsServicesId,
        //        ItemServiceDescription = item.ItemServiceDescription,
        //        CostRate = item.CostRate,
        //        TaxId = item.TaxId,
        //        Tax = item.Tax,
        //        Notes = item.Notes,
        //        Active = item.Active,
        //        CreateUser = item.CreateUser,
        //        CreateDate = DateTime.Now,
        //    };

        //    _db.Tbl_ScheduleItemsServices.Add(entityItemsServices);
        //    _db.SaveChanges();
        //}

        public IEnumerable<PropertyItemsServicesDTO> GetAllItemsInventory(int? AccountId)
        {
            var Items = _db.Tbl_ItemsServices.Where(x => x.AccountId == AccountId && (x.Type == 2 || x.Type == 4)).Select(y => new PropertyItemsServicesDTO
            {
                Id = y.Id,
                AccountId = y.AccountId,
                BrancheId = y.BrancheId,
                Name = y.Name,
                CostperUnit = y.CostperUnit,
                Type = y.Type,
                CategoryId = y.CategoryId,
                InventoryItem = y.InventoryItem,
                QTYTime = y.QTYTime,
                Unit = y.Unit,
                MemeberType = y.MemeberType,
                MemeberId = y.MemeberId,
                TaxId = y.TaxId,
                Tax = y.Tax,
                Taxable = y.Taxable,
                Description = y.Description,
                Details = y.Details,
                Picture = y.Picture,
                SKU = y.SKU,
                Notes = y.Notes,
                Active = y.Active,
                CreateUser = y.CreateUser,
                CreateDate = y.CreateDate,
            });

            var ll = Items.ToList();

            return Items.ToList();
        }


        public IEnumerable<PropertyItemsServicesDTO> GetAllItemsServices(int? AccountId)
        {
            var Items = _db.Tbl_ItemsServices.Where(x => x.AccountId == AccountId).Select(y => new PropertyItemsServicesDTO
            {
                Id = y.Id,
                AccountId = y.AccountId,
                BrancheId = y.BrancheId,
                Name = y.Name,
                CostperUnit = y.CostperUnit,
                Type = y.Type,
                CategoryId = y.CategoryId,
                InventoryItem = y.InventoryItem,
                QTYTime = y.QTYTime,
                Unit = y.Unit,
                MemeberType = y.MemeberType,
                MemeberId = y.MemeberId,
                TaxId = y.TaxId,
                Tax = y.Tax,
                Taxable = y.Taxable,
                Description = y.Description,
                Details = y.Details,
                Picture = y.Picture,
                SKU = y.SKU,
                Notes = y.Notes,
                Active = y.Active,
                CreateUser = y.CreateUser,
                CreateDate = y.CreateDate,
            });

            return Items.ToList();
        }




        public IEnumerable<PropertyItemsServicesDTO> GetAllServices(int? AccountId)
        {
            var Items = _db.Tbl_ItemsServices.Where(x => x.AccountId == AccountId && (x.Type == 1 || x.Type == 3)).Select(y => new PropertyItemsServicesDTO
            {
                Id = y.Id,
                AccountId = y.AccountId,
                BrancheId = y.BrancheId,
                Name = y.Name,
                CostperUnit = y.CostperUnit,
                Type = y.Type,
                CategoryId = y.CategoryId,
                InventoryItem = y.InventoryItem,
                QTYTime = y.QTYTime,
                Unit = y.Unit,
                MemeberType = y.MemeberType,
                MemeberId = y.MemeberId,
                TaxId = y.TaxId,
                Tax = y.Tax,
                Taxable = y.Taxable,
                Description = y.Description,
                Details = y.Details,
                Picture = y.Picture,
                SKU = y.SKU,
                Notes = y.Notes,
                Active = y.Active,
                CreateUser = y.CreateUser,
                CreateDate = y.CreateDate,
            });

            return Items.ToList();
        }

        public void AddScheduleDate(PropertySchedulesDTO model)
        {

            var ScheduleDateDTO = new Tbl_ScheduleDate
            {
                AccountId = model.OneScheduleDate.AccountId,
                BrancheId = model.OneScheduleDate.BrancheId,
                ScheduleId = model.OneScheduleDate.ScheduleId,
                Date = model.OneScheduleDate.Date,
                StartTime = model.OneScheduleDate.StartTime,
                EndTime = model.OneScheduleDate.EndTime,
                CreateOrginal_Custom = 2, //Second schedule
                Status = 1,
                CalendarColor = model.OneScheduleDate.CalendarColor,
                Reasonnotserve = null,
                InvoiceId = null,
                EstimateId = null,
                Notes = null,
                Active = model.OneScheduleDate.Active,
                CreateUser = model.OneScheduleDate.CreateUser,
                CreateDate = DateTime.Now,
            };

            _db.Tbl_ScheduleDate.Add(ScheduleDateDTO);
            _db.SaveChanges();

            int? ScheduleDateId = ScheduleDateDTO.Id;

            var ScheduleEmployeesDTO = new Tbl_ScheduleEmployees
            {
                AccountId = model.OneScheduleDate.AccountId,
                ScheduleDateId = ScheduleDateId,
                EmployeeCategoryId = model.OneScheduleDate.OneEmployee.CategoryId,
                EmpId = model.OneScheduleDate.OneEmployee.Id,
                StartTime = null,
                EndTime = null,
                SpentTimeHour = null,
                SpentTimeMin = null,
                Labor = null,
                Duration = null,
                Status = 1,
                Reasonnotserve = null,
                Notes = null,
                CreateUser = model.CreateUser,
                CreateDate = DateTime.Now,
            };

            _db.Tbl_ScheduleEmployees.Add(ScheduleEmployeesDTO);
            _db.SaveChanges();

            model.StartDate = ScheduleDateDTO.Date;

            SendNotify(model, "", model.Id, ScheduleDateId.Value);
        }



        public void UpdateScheduleEmployees(PropertyScheduleDateDTO model)
        {
            var entityId = _db.Tbl_ScheduleEmployees.FirstOrDefault(x => x.ScheduleDateId == model.Id);
            if (entityId != null)
            {
                int OldStatus = _db.Tbl_ScheduleEmployees.Where(c => c.ScheduleDateId == model.Id).Select(x => x.Status.Value).FirstOrDefault();

                var EmpDTO = _db.Tbl_Employee.Where(x => x.Id == entityId.EmpId).FirstOrDefault();
                decimal? ScheduleEmployeesSalaryPerHour = EmpDTO.SalaeryPer == 0 ? EmpDTO.Salary : 0;
                entityId.AccountId = model.AccountId;
                entityId.ScheduleDateId = model.Id;
                entityId.EmployeeCategoryId = EmpDTO.CategoryId;
                entityId.EmpId = EmpDTO.Id;
                entityId.StartTime = model.StartTime;
                entityId.EndTime = model.EndTime;
                entityId.SpentTimeHour = (model.EndTime != null && model.StartTime != null) ? (TimeSpan.Parse(model.EndTime) - TimeSpan.Parse(model.StartTime)).Hours.ToString() : null;
                entityId.SpentTimeMin = (model.EndTime != null && model.StartTime != null) ? (TimeSpan.Parse(model.EndTime) - TimeSpan.Parse(model.StartTime)).Minutes.ToString() : null;
                entityId.Duration = (model.StartTime != null && model.EndTime != null) ? (decimal.Parse(entityId.SpentTimeHour) + Math.Round((decimal.Parse(entityId.SpentTimeMin) / (decimal)60), 2)) : 0;
                entityId.Labor = Math.Round((decimal)entityId.Duration * (decimal)ScheduleEmployeesSalaryPerHour, 2);
                entityId.Status = model.Status;
                entityId.Reasonnotserve = model.Reasonnotserve;
                entityId.Notes = model.Notes;
                entityId.CreateUser = model.CreateUser;
                entityId.CreateDate = model.CreateDate;

                _db.Entry(entityId).State = EntityState.Modified;
                _db.SaveChanges();

                //SendNotify_For_Update_Schedule_Status(model, OldStatus);
            }
        }

        public IEnumerable<PropertySchedulePicturesDTO> GetAllSchedulePictures(int? ScheduleId)
        {
            var Pictures = _db.Tbl_SchedulePictures.Where(x => x.ScheduleDateId == ScheduleId).Select(x => new PropertySchedulePicturesDTO
            {
                Id = x.Id,
                AccountId = x.AccountId,
                BrancheId = x.BrancheId,
                ScheduleId = x.ScheduleId,
                FileName = x.FileName,
                Active = x.Active,
                ShowToCust = x.ShowToCust,
                CreateUser = x.CreateUser,
                CreateDate = x.CreateDate,
                ScheduleDateId = x.ScheduleDateId,
                ScheduleDate = x.Tbl_ScheduleDate.Date
            }).ToList();

            return Pictures.ToList();
        }


        public void InsertSchedulePictures(List<PropertySchedulePicturesDTO> LstPictures)
        {
            var entity = _db.Tbl_SchedulePictures.Where(x => x.ScheduleId == LstPictures[0].ScheduleId).FirstOrDefault();
            if (entity == null)
            {
                foreach (var Picture in LstPictures)
                {
                    var entitySchedulePicture = new Tbl_SchedulePictures
                    {
                        AccountId = Picture.AccountId,
                        BrancheId = Picture.BrancheId,
                        ScheduleId = Picture.ScheduleId,
                        FileName = Picture.FileName,
                        Active = Picture.Active,
                        ShowToCust = Picture.ShowToCust,
                        CreateUser = Picture.CreateUser,
                        CreateDate = Picture.CreateDate,
                        ScheduleDateId = Picture.ScheduleDateId,
                    };

                    _db.Tbl_SchedulePictures.Add(entitySchedulePicture);
                    _db.SaveChanges();
                }
            }
        }


        public void UpdateSchedulePictures(List<PropertySchedulePicturesDTO> LstPictures)
        {
            var LstEntity = _db.Tbl_SchedulePictures.Where(x => x.ScheduleId == LstPictures[0].ScheduleId).ToList();

            if (LstEntity != null)
            {
                foreach (var pic in LstEntity)
                {
                    var entityItem = _db.Tbl_SchedulePictures.FirstOrDefault(x => x.ScheduleId == pic.ScheduleId);

                    if (entityItem != null)
                    {
                        _db.Tbl_SchedulePictures.Remove(entityItem);
                        _db.SaveChanges();
                    }
                }
            }

            foreach (var Picture in LstPictures)
            {
                var entitySchedulePicture = new Tbl_SchedulePictures
                {
                    AccountId = Picture.AccountId,
                    BrancheId = Picture.BrancheId,
                    ScheduleId = Picture.ScheduleId,
                    FileName = Picture.FileName,
                    Active = Picture.Active,
                    CreateUser = Picture.CreateUser,
                    CreateDate = Picture.CreateDate,
                    ScheduleDateId = Picture.ScheduleDateId,
                };

                _db.Tbl_SchedulePictures.Add(entitySchedulePicture);
                _db.SaveChanges();
            }

        }

        public void UpdateScheduleOutPicture(PropertySchedulePicturesDTO Picture)
        {
            var Entity = _db.Tbl_SchedulePictures.Where(x => x.ScheduleId == Picture.ScheduleId && x.Id == Picture.Id).FirstOrDefault();

            if (Entity != null)
            {

                Entity.ShowToCust = Picture.ShowToCust;

                _db.Entry(Entity).State = EntityState.Modified;
                _db.SaveChanges();
            }

        }



        public string[] GetEmpPlayerId(string Emp)
        {
            string[] EmpIds = { };
            string[] EmpPlayersIds = { };
            if (string.IsNullOrEmpty(Emp) == false)
            {
                EmpIds = Emp.Trim().Split(',');
                var Employee = _db.Tbl_Employee.Where(v => EmpIds.Contains(v.Id.ToString()) && string.IsNullOrEmpty(v.OneSignalPlayerId) != true).Select(m => m.OneSignalPlayerId).ToArray();
                EmpPlayersIds = Employee;
            }

            return EmpPlayersIds;
        }

        public void SendNotify(PropertySchedulesDTO model, string specificEmps, int ScheduleId_New, int ScheduleDateId_New)
        {
            string[] EmpIds = { };
            if (string.IsNullOrEmpty(specificEmps))
            {
                EmpIds = GetEmpPlayerId(model.Employees);
            }
            else
            {
                EmpIds = GetEmpPlayerId(specificEmps);
            }

            string Notify = $"Customer Name : {model.CustomerDTO.FirstName + model.CustomerDTO.LastName} \n" +
              $"Address : {model.CustomerDTO.Address} \n" +
              $"Date : {model.StartDate}";

            var obj = new
            {
                app_id = _db.Tbl_Account.FirstOrDefault(c => c.Id == model.AccountId).OneSignalAppId,
                contents = new Dictionary<string, string>
                {
                    ["en"] = Notify
                },
                headings = new Dictionary<string, string>
                {
                    ["en"] = "New job added"
                },
                data = new Dictionary<string, string>
                {
                    ["deeplink"] = String.Format("Schedule,{0},{1}", ScheduleId_New, ScheduleDateId_New)
                },

                //buttons= new Dictionary<string, string>
                //{
                //    ["action"] = "like-button",
                //    ["title"] = "Like",
                //    ["icon"] = "http://i.imgur.com/N8SN8ZS.png",
                //    ["url"] = "https://example.com",
                //},
                //action = "like-button",
                //       {
                //      "action": "like-button",
                //    "title": "Like",
                //    "icon": "http://i.imgur.com/N8SN8ZS.png",
                //    "url": "https://example.com"
                //},
                //"[" +
                //    "{\"id\": \"id1\", \"text\": \"button1\", \"icon\": \"ic_menu_share\"}," +
                //    "{\"id\": \"id2\", \"text\": \"button2\", \"icon\": \"ic_menu_send\"}" +
                //"]" +

                //data = new Dictionary<string, string>
                //{
                //    ["page"]  = "Home"
                //},
                //app_url = "FixPro://HandleDeepLinking/page1",
                //ActionButtons = new List<ActionButton>
                //{
                //    new ActionButton
                //    {
                //        ID = "button1",
                //        Text = "Open Page 1",
                //        //Icon = "ic_button1"
                //    },
                //},
                //Actions = new List<NotificationAction>
                //{
                //    new NotificationAction
                //    {
                //        ActionID = "button1",
                //        Type = ActionType.OpenApp,
                //        URL = "FixPro://deeplink/page1"
                //    },
                //},
                include_player_ids = EmpIds
            };

            if (EmpIds.Count() > 0)
            {
                ClsNotificationsDAL.CreateNotification(obj);
            }

            string Employees = string.IsNullOrEmpty(specificEmps) ? model.Employees : specificEmps;

            ClsNotificationsDAL.InsertOneSignalNotification(Employees, model.AccountId.Value, ScheduleId_New, ScheduleDateId_New, 1, "New job added", Notify, true, model.CreateUser.Value, DateTime.Now); // Create in notification table in SQL

        }

        public void SendNotify_For_Remove_Emps(PropertySchedulesDTO model, int UserUpdate, string MsgEmpRemove, string MsgEmpAdd)
        {
            if (!string.IsNullOrEmpty(MsgEmpRemove))
            {
                string[] EmpIds = GetEmpPlayerId(MsgEmpRemove);
                string Notify = $"Customer Name : {model.CustomerDTO.FirstName + model.CustomerDTO.LastName} \n" +
                  $"Address : {model.CustomerDTO.Address} \n" +
                  $"Date : {model.StartDate}";

                var obj = new
                {
                    app_id = _db.Tbl_Account.FirstOrDefault(c => c.Id == model.AccountId).OneSignalAppId,
                    contents = new Dictionary<string, string>
                    {
                        ["en"] = Notify
                    },
                    headings = new Dictionary<string, string>
                    {
                        ["en"] = "Cancel Job"
                    },
                    include_player_ids = EmpIds
                };

                if (EmpIds.Count() > 0)
                {
                    ClsNotificationsDAL.CreateNotification(obj);
                }

                ClsNotificationsDAL.UpdateOneSignalNotification(null, MsgEmpRemove, model.Id, model.ScheduleDateId.Value, "Cancel Job", Notify, 3, UserUpdate, DateTime.Now);

            }

            if (!string.IsNullOrEmpty(MsgEmpAdd))
            {
                SendNotify(model, MsgEmpAdd, model.Id, model.ScheduleDateId.Value);
            }
        }

        public void SendNotify_For_Update_Schedule_Status(PropertyScheduleDateDTO modelDate, int? OldStatus)
        {
            var ScheduleDate = _db.Tbl_ScheduleDate.FirstOrDefault(d => d.Id == modelDate.Id)/*.ScheduleId.Value*/;
            var ScheduleEmployees = ScheduleDate.Tbl_ScheduleEmployees.Select(k => k.EmpId.ToString()).Aggregate((f, s) => f + "," + s);
            if (ScheduleDate != null)
            {
                var model = _db.Tbl_Schedule.FirstOrDefault(x => x.Id == ScheduleDate.ScheduleId);
                if (model != null)
                {
                    if (modelDate.Status == 0 || modelDate.Status == 2) //Not Service || Completed
                    {
                        string[] EmpIds = { model.CreateUser.Value.ToString() };
                        string Notify = $"Customer Name : {model.Tbl_Customers.FirstName + model.Tbl_Customers.LastName} \n" +
                          $"Address : {model.Tbl_Customers.Address} \n" +
                          $"Date : {model.StartDate}";

                        var obj = new
                        {
                            app_id = _db.Tbl_Account.FirstOrDefault(c => c.Id == model.AccountId).OneSignalAppId,
                            contents = new Dictionary<string, string>
                            {
                                ["en"] = Notify
                            },
                            headings = modelDate.Status == 0 ? new Dictionary<string, string>
                            {
                                ["en"] = "Not Service Job"
                            } : new Dictionary<string, string>
                            {
                                ["en"] = "Completed Job"
                            },
                            include_player_ids = EmpIds
                        };

                        if (EmpIds.Count() > 0)
                        {
                            ClsNotificationsDAL.CreateNotification(obj);
                        }

                        string Header = modelDate.Status == 0 ? "Not Service Job" : "Completed Job";

                        ClsNotificationsDAL.InsertOneSignalNotification(model.CreateUser.Value.ToString(), model.AccountId.Value, ScheduleDate.ScheduleId.Value, modelDate.Id, 3, Header, Notify, true, model.CreateUser.Value, DateTime.Now);

                        ClsNotificationsDAL.UpdateOneSignalNotification(null, ScheduleEmployees, model.Id, modelDate.Id, Header, Notify, 2, modelDate.CreateUser.Value, DateTime.Now);
                    }


                    if (modelDate.Status == 1 && OldStatus != modelDate.Status) // Reopen 
                    {
                        string[] EmpIds = GetEmpPlayerId(ScheduleEmployees);
                        string Notify = $"Customer Name : {model.Tbl_Customers.FirstName + model.Tbl_Customers.LastName} \n" +
                          $"Address : {model.Tbl_Customers.Address} \n" +
                          $"Date : {model.StartDate}";

                        var obj = new
                        {
                            app_id = _db.Tbl_Account.FirstOrDefault(c => c.Id == model.AccountId).OneSignalAppId,
                            contents = new Dictionary<string, string>
                            {
                                ["en"] = Notify
                            },
                            headings = new Dictionary<string, string>
                            {
                                ["en"] = "Reopen Job"
                            },
                            include_player_ids = EmpIds
                        };

                        if (EmpIds.Count() > 0)
                        {
                            ClsNotificationsDAL.CreateNotification(obj);
                        }

                        ClsNotificationsDAL.UpdateOneSignalNotification(null, ScheduleEmployees, model.Id, modelDate.Id, "Reopen Job", Notify, 1, modelDate.CreateUser.Value, DateTime.Now);

                    }

                }
            }


        }

        //public void DeleteScheduleItemService(int ItemId)
        //{

        //    //var entity = _db.Tbl_ScheduleItemsServices.FirstOrDefault(x => x.ItemsServicesId == item.ItemsServicesId && x.ScheduleId == item.ScheduleId);
        //    var entityItem = _db.Tbl_ScheduleItemsServices.FirstOrDefault(x => x.Id == ItemId);

        //    if (entityItem != null)
        //    {
        //        _db.Tbl_ScheduleItemsServices.Remove(entityItem);
        //        _db.SaveChanges();
        //    }
        //}


        public void SelectDispatch(PropertyScheduleDateDTO model)
        {
            var entityId = _db.Tbl_ScheduleDate.FirstOrDefault(x => x.Id == model.Id);

            if (entityId != null && entityId.Active != true)
            {
                entityId.Active = true;
                _db.Entry(entityId).State = EntityState.Modified;
                _db.SaveChanges();

                // Send Email
                if (!string.IsNullOrEmpty(model.CustomerEmail))
                {
                    SendSchedulDateMail(model.Id.ToString(), model.CustomerEmail);
                }

                // Send Notification
                var ScheduleEmployees = entityId.Tbl_ScheduleEmployees.Select(k => k.EmpId.ToString()).Aggregate((f, s) => f + "," + s);

                var entitySchedule = _db.Tbl_Schedule.FirstOrDefault(x => x.Id == model.ScheduleId);

                string[] EmpIds = { };
                if(ScheduleEmployees != null)
                {
                    EmpIds = GetEmpPlayerId(ScheduleEmployees);
                }

                if (entitySchedule != null)
                {
                    string Notify = $"Customer Name : {entitySchedule.Tbl_Customers.FirstName + entitySchedule.Tbl_Customers.LastName} \n" +
                                    $"Address : {entitySchedule.Tbl_Customers.Address} \n" +
                                    $"Date : {entitySchedule.StartDate}";
                    var obj = new
                    {
                        app_id = _db.Tbl_Account.FirstOrDefault(c => c.Id == model.AccountId).OneSignalAppId,
                        contents = new Dictionary<string, string>
                        {
                            ["en"] = Notify
                        },
                        headings = new Dictionary<string, string>
                        {
                            ["en"] = "Job is Dispatched"
                        },
                        data = new Dictionary<string, string>
                        {
                            ["deeplink"] = String.Format("Schedule,{0},{1}", model.ScheduleId, model.Id)
                        },
                        include_player_ids = EmpIds
                    };

                    if (EmpIds.Count() > 0)
                    {
                        ClsNotificationsDAL.CreateNotification(obj);
                    }

                    ClsNotificationsDAL.InsertOneSignalNotification(ScheduleEmployees, model.AccountId.Value, model.ScheduleId.Value, model.Id, 1, "Job is Dispatched", Notify, true, model.CreateUser.Value, DateTime.Now); // Create in notification table in SQL
                }
            }
            else
            {
                throw new ArgumentException(message: $"This Schedule number #" + model.Id + " already Dispatched");
            }
        }

        public string SendSchedulDateMail(string ScheduleDateId, string MailTo)
        {


            //var urlpath = Path.Combine(test + Basics.FilesName.InvoiceTemplate + ".txt");
            //var template = System.IO.File.ReadAllText(urlpath);

            //string UrlVew = Domain + "/Client/WL/Portal/" + EncUrl.Encrypt(InvoiceId);
            //Domain = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority); 

            //var replacements = new Dictionary<string, string>
            //    {
            //    { "{CompanyLogo}", Url },
            //    { "{CompanyName}", modelScheduleDate.Tbl_Account.CompanyName  },
            //    { "{CompanyPhone}", modelScheduleDate.Tbl_Branches.Phone },
            //    { "{CompanyAddress}", modelScheduleDate.Tbl_Branches.Address },
            //    { "{Email}", modelScheduleDate.Tbl_Branches.Email },
            //    { "{InvoiceId}", InvoiceId.ToString() },
            //    { "{InvoiceTitle}","Emergency Call" },
            //    { "{InvoiceDetails}", modelScheduleDate.NotesForCustomer},
            //    { "{InvoiceViewUrl}", UrlVew},
            //    };

            //foreach (var item in replacements)
            //    template = template.Replace(item.Key, item.Value);
            //====================================
            int SchDT = int.Parse(ScheduleDateId);
            var modelScheduleDate = _db.Tbl_ScheduleDate.Where(h => h.Id == SchDT).FirstOrDefault();
            EncryptUrl EncUrl = new EncryptUrl();
            dynamic result = new ExpandoObject();
            var test = PropertyBaseDTO.PathFolderUpload + "StaticeImages\\";
            string template = "";
            var Domain = PropertyBaseDTO.DomainUrl;
            string Url = Domain + "/BrancheLogo/" + modelScheduleDate.Tbl_Branches.Logo;

            var StatusTitle = "";

            if (modelScheduleDate.Status == null)
                StatusTitle = "Draft";
            else if (modelScheduleDate.Status == 1)
                StatusTitle = "booked on our system";
            else if (modelScheduleDate.Status == 2)
                StatusTitle = "Done";
            else if (modelScheduleDate.Status == 0)
                StatusTitle = "Not Serviced";


            var replacements = new Dictionary<string, string>
            {
            { "{CompanyLogo}",Url },
            { "{CompanyName}",modelScheduleDate.Tbl_Account.CompanyName},
            { "{CompanyPhone}", modelScheduleDate.Tbl_Branches.Phone },
            { "{CompanyAddress}",modelScheduleDate.Tbl_Branches.Address   },
            { "{EmailTitle}", "Booking Notification" },
            { "{Email}", modelScheduleDate.Tbl_Branches.Email },
            { "{TaskStatus}",  StatusTitle},
            { "{TaskDate}", " On "+ modelScheduleDate.Date },
            { "{TaskExpectedTime}", " between " + modelScheduleDate.StartTime + "-" + modelScheduleDate.EndTime + " CDT"} ,
            { "{TaskAddress}", modelScheduleDate.Tbl_Schedule.Tbl_Customers.Address}
            };

            var Employees = _db.Tbl_ScheduleEmployees.Where(k => k.ScheduleDateId == SchDT).ToList();
            //var StaffPdfFilePath = Path.Combine(Server.MapPath("/PDFStaffPhotos/PDFTask.pdf"));

            //string StaffPdfFilePath = PropertyBaseDTO.PathFolderUpload + "PDFStaffPhotos\\PDFTask.pdf";
            if (Employees?.Count() > 0)
            {
                if (Employees.Count() == 1)
                {
                    string UrlEmp = Domain + "/EmployeePic/" + Employees.FirstOrDefault().Tbl_Employee.Picture;
                    result.staffPdf = null;
                    template = System.IO.File.ReadAllText(Path.Combine(test + Basics.FilesName.JobNotifiyTemplate + ".txt"));
                    replacements.Add("{EmployeeName}", Employees.FirstOrDefault().Tbl_Employee.FirstName + " " + Employees.FirstOrDefault().Tbl_Employee.LastName);
                    replacements.Add("{EmployeePhoto}", UrlEmp);
                }
                else
                {
                    template = System.IO.File.ReadAllText(Path.Combine(test + Basics.FilesName.JobNotifiyMultiStaffTemplate + ".txt"));

                    var EmployeesPhotos = new Dictionary<string, string>();
                    if (Employees == null)
                        throw new InvalidOperationException("Employees is not initialized.");
                    StringBuilder st = new StringBuilder();

                    foreach (var staff in Employees)
                    {
                        //string Img = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/EmployeePic/" + staff.ScheduleEmployeesEmpPicture;
                        string Img = Domain + "/EmployeePic/" + staff.Tbl_Employee.Picture;

                        st.AppendLine("<div class='col-sm-6'> <div class='company-logo'>  <img style='width:200px;height:200px' src='" + Img + "' alt='Employee Photo'> </div> <div> <h4>" + staff.Tbl_Employee.FirstName + " " + staff.Tbl_Employee.LastName + "</h4>  </div>  </div>");
                    }
                    if (string.IsNullOrEmpty(st.ToString()) != true)
                    {
                        replacements.Add("{EmployeePhotos}", st.ToString());
                    }
                    else
                    {
                        replacements.Add("{EmployeePhotos}", "");
                    }
                }

                foreach (var item in replacements)
                    template = template.Replace(item.Key, item.Value);

                PropertyBaseDTO.SendMail(MailTo, template, "Schedule", null);
            }

            return template;
        }

        public IEnumerable<PropertyItemsServicesCategoryDTO> GetAllItemsServicesCategories(int? AccountId)
        {
            var Categories = _db.Tbl_ItemsServicesCategory.Where(x => x.AccountId == AccountId).Select(y => new PropertyItemsServicesCategoryDTO
            {
                Id = y.Id,
                AccountId = y.AccountId,
                BrancheId = y.BrancheId,
                CategoryName = y.CategoryName,
                Description = y.Description,
                Picture = y.Picture,
                Notes = y.Notes,
                Active = y.Active,
                CreateUser = y.CreateUser,
                CreateDate = y.CreateDate,
            });

            return Categories.ToList();
        }

        public IEnumerable<PropertyItemsServicesSubCategoryDTO> GetAllItemsServicesSubCategories(int? AccountId)
        {
            var Categories = _db.Tbl_ItemsServicesSubCategory.Where(x => x.AccountId == AccountId).Select(y => new PropertyItemsServicesSubCategoryDTO
            {
                Id = y.Id,
                AccountId = y.AccountId,
                BrancheId = y.BrancheId,
                ItemsServicesCategoryId = y.ItemsServicesCategoryId,
                ItemsServicesSubCategory = y.ItemsServicesSubCategory,
                Picture = y.Picture,
                Active = y.Active,
                CreateUser = y.CreateUser,
                CreateDate = y.CreateDate,
            });

            return Categories.ToList();
        }



        public PropertyItemsServicesDTO InsertCreateItemService(PropertyItemsServicesDTO ItemService)
        {
            var entityItemService = new Tbl_ItemsServices
            {
                AccountId = ItemService.AccountId,
                BrancheId = ItemService.BrancheId,
                Name = ItemService.Name,
                Type = ItemService.Type,
                CategoryId = ItemService.CategoryId,
                SubCategoryId = ItemService.SubCategoryId,
                InventoryItem = ItemService.InventoryItem,
                QTYTime = ItemService.QTYTime,
                Unit = ItemService.Unit,
                CostperUnit = ItemService.CostperUnit,
                MemeberType = ItemService.MemeberType,
                MemeberId = ItemService.MemeberId,
                TaxId = ItemService.TaxId,
                Tax = ItemService.Tax,
                Taxable = ItemService.Taxable,
                Description = ItemService.Description,
                Details = ItemService.Details,
                Picture = ItemService.Picture,
                SKU = ItemService.SKU,
                Notes = ItemService.Notes,
                Active = ItemService.Active,
                CreateUser = ItemService.CreateUser,
                CreateDate = ItemService.CreateDate,
            };

            _db.Tbl_ItemsServices.Add(entityItemService);
            _db.SaveChanges();

            return ItemService;
        }


    }
}
