using Antlr.Runtime.Misc;
using FixProUsApi.DTO;
using FixProUsApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer;
using System.Web.Security;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace FixProUsApi.DAL
{
    public class RepositoryEmployeeDAL
    {
        public Entities _db;
        private const string PROPERTY_Employee_CACHE_KEY = "Property Employee";
        private bool disposed = false;
        public RepositoryEmployeeDAL()
        {
            _db = new Entities();
            _db.Database.Connection.ConnectionString = string.IsNullOrEmpty(PropertyBaseDTO.ConnectSt) == true ? _db.Database.Connection.ConnectionString : PropertyBaseDTO.ConnectSt;
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

        public string GetEmpPlayerId(int? AccountId, int? EmpId)
        {
            return _db.Tbl_Employee.Where(x => x.AccountId == AccountId && x.Id == EmpId).Select(s => s.OneSignalPlayerId).FirstOrDefault();
        }

        public string GetAllEmployees(int? AccountId)
        {
            return _db.Tbl_Employee.Where(x => x.AccountId == AccountId).Select(s => s.OneSignalPlayerId).FirstOrDefault();
        }

        public string UpdateEmpPlayerId(int? AccountId, int? EmpId, string PlayerId)
        {
            var entity = _db.Tbl_Employee.FirstOrDefault(x => x.AccountId == AccountId && x.Id == EmpId);
            if (entity != null)
            {
                entity.OneSignalPlayerId = PlayerId;
            }
            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();

            return entity.OneSignalPlayerId;
        }

        public int InsertEmployee(PropertyEmployeeDTO model)
        {
            var entity = _db.Tbl_Employee.FirstOrDefault(x => x.UserName.ToLower() == model.UserName.ToLower());
            if (entity != null)
            {
                throw new ArgumentException(message: $"The Employee Name {model.UserName} already exists.");
            }
            else
            {
                entity = new Tbl_Employee
                {
                    Id = model.Id,
                    AccountId = model.AccountId,
                    //BrancheId = model.BrancheId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Birthday = model.Birthday,
                    Since = model.Since,
                    Salary = model.Salary,
                    SalaeryPer = model.SalaeryPer,
                    CategoryId = model.CategoryId,
                    Phone1 = model.Phone1,
                    Phone2 = model.Phone2,
                    UserName = model.UserName,
                    EmailUserName = model.EmailUserName,
                    Password = model.Password,
                    Address = model.Address,
                    City = model.City,
                    State = model.State,
                    PostalcodeZIP = model.PostalcodeZIP,
                    locationlatitude = model.locationlatitude,
                    locationlongitude = model.locationlongitude,
                    Country = model.Country,
                    SSN = model.SSN,
                    DriveLicense = model.DriveLicense,
                    Picture = model.Picture,
                    UserRole = model.UserRole,
                    UserType = model.UserType,
                    ExpireDate = model.ExpireDate,
                    TheBranchs = model.TheBranchs,
                    Employees = model.Employees,
                    ActiveHome = model.ActiveHome,
                    ActiveBranches = model.ActiveBranches,
                    ActiveContract = model.ActiveContract,
                    ActiveCustomers = model.ActiveCustomers,
                    ActiveCustomersCategory = model.ActiveCustomersCategory,
                    ActiveCustomersCustomField = model.ActiveCustomersCustomField,
                    ActiveEmployee = model.ActiveEmployee,
                    ActiveEmployeeCategory = model.ActiveEmployeeCategory,
                    ActiveEmployeeCustomField = model.ActiveEmployeeCustomField,
                    ActiveEstimate = model.ActiveEstimate,
                    ActiveEstimateEmailTemplate = model.ActiveEstimateEmailTemplate,
                    ActiveExpenses = model.ActiveExpenses,
                    ActiveExpensesCategory = model.ActiveExpensesCategory,
                    ActiveEquipments = model.ActiveEquipments,
                    ActiveEquipmentsCustomField = model.ActiveEmployeeCustomField,
                    ActiveItemsServices = model.ActiveItemsServices,
                    ActiveItemsServicesCategory = model.ActiveItemsServicesCategory,
                    ActiveItemsServicesCustomField = model.ActiveItemsServicesCustomField,
                    ActiveMember = model.ActiveMember,
                    ActiveTimeSheet = model.ActiveTimeSheet,
                    ActiveInvoice = model.ActiveInvoice,
                    ActiveInvoiceEmailTemplate = model.ActiveInvoiceEmailTemplate,
                    ActiveMap = model.ActiveMap,
                    ActiveNotes = model.ActiveNotes,
                    ActiveNotificationSettings = model.ActiveNotificationSettings,
                    ActivePayment = model.ActivePayment,
                    ActiveReminderRules = model.ActiveReminderRules,
                    ActiveRoute = model.ActiveRoute,
                    ActiveSchedule = model.ActiveSchedule,
                    ActiveSettings = model.ActiveSettings,
                    ActiveTax = model.ActiveTax,
                    ActiveReport = model.ActiveReport,
                    ActiveStripeAccount = model.ActiveStripeAccount,
                    ActiveMessage = model.ActiveMessage,
                    ActiveItemsServicesSubCategory = model.ActiveItemsServicesSubCategory,
                    ActiveAccount = model.ActiveAccount,
                    ActiveEditCustomer = model.ActiveEditCustomer,
                    Notes = model.Notes,
                    Active = model.Active,
                    CreateUser = model.CreateUser,
                    CreateDate = model.CreateDate,
                    ActiveEditPrice = model.ActiveEditPrice,
                    ActiveMobileLogin = model.ActiveMobileLogin,
                    ActiveMobileTrackStaff = model.ActiveMobileTrackStaff,
                    ActiveCreateSchedule = model.ActiveCreateSchedule,
                    OneSignalPlayerId = model.OneSignalPlayerId,
                    ActiveAllScdTr_FaorTrOnly = model.ActiveAllScdTr_FaorTrOnly,
                    ActiveEditEstimate_Invoice = model.ActiveEditEstimate_Invoice,
                };

                _db.Tbl_Employee.Add(entity);
            }
            _db.SaveChanges();

            return entity.Id;
        }

        public void UpdateEmployee(PropertyEmployeeDTO model)
        {
            var entity = _db.Tbl_Employee.FirstOrDefault(x => x.UserName.ToLower() == model.UserName.ToLower() && x.Id != model.Id);
            if (entity == null)
            {
                var entityId = _db.Tbl_Employee.FirstOrDefault(x => x.Id == model.Id);
                if (entityId != null)
                {
                    entityId.Id = model.Id;
                    entityId.AccountId = model.AccountId;
                    //entityId.BrancheId = model.BrancheId;
                    entityId.FirstName = model.FirstName;
                    entityId.LastName = model.LastName;
                    entityId.Birthday = model.Birthday;
                    entityId.Since = model.Since;
                    entityId.Salary = model.Salary;
                    entityId.SalaeryPer = model.SalaeryPer;
                    entityId.CategoryId = model.CategoryId;
                    entityId.Phone1 = model.Phone1;
                    entityId.Phone2 = model.Phone2;
                    entityId.UserName = model.UserName;
                    entityId.EmailUserName = model.EmailUserName;
                    entityId.Password = model.Password;
                    entityId.Address = model.Address;
                    entityId.City = model.City;
                    entityId.State = model.State;
                    entityId.PostalcodeZIP = model.PostalcodeZIP;
                    entityId.locationlatitude = model.locationlatitude;
                    entityId.locationlongitude = model.locationlongitude;
                    entityId.Country = model.Country;
                    entityId.SSN = model.SSN;
                    entityId.DriveLicense = model.DriveLicense;
                    entityId.Picture = model.Picture;
                    entityId.UserRole = model.UserRole;
                    entityId.UserType = model.UserType;
                    entityId.ExpireDate = model.ExpireDate;
                    entityId.TheBranchs = model.TheBranchs;
                    entityId.Employees = model.Employees;
                    entityId.ActiveHome = model.ActiveHome;
                    entityId.ActiveBranches = model.ActiveBranches;
                    entityId.ActiveContract = model.ActiveContract;
                    entityId.ActiveCustomers = model.ActiveCustomers;
                    entityId.ActiveCustomersCategory = model.ActiveCustomersCategory;
                    entityId.ActiveCustomersCustomField = model.ActiveCustomersCustomField;
                    entityId.ActiveEmployee = model.ActiveEmployee;
                    entityId.ActiveEmployeeCategory = model.ActiveEmployeeCategory;
                    entityId.ActiveEmployeeCustomField = model.ActiveEmployeeCustomField;
                    entityId.ActiveEstimate = model.ActiveEstimate;
                    entityId.ActiveEstimateEmailTemplate = model.ActiveEstimateEmailTemplate;
                    entityId.ActiveExpenses = model.ActiveExpenses;
                    entityId.ActiveExpensesCategory = model.ActiveExpensesCategory;
                    entityId.ActiveEquipments = model.ActiveEquipments;
                    entityId.ActiveEquipmentsCustomField = model.ActiveEmployeeCustomField;
                    entityId.ActiveItemsServices = model.ActiveItemsServices;
                    entityId.ActiveItemsServicesCategory = model.ActiveItemsServicesCategory;
                    entityId.ActiveItemsServicesCustomField = model.ActiveItemsServicesCustomField;
                    entityId.ActiveMember = model.ActiveMember;
                    entityId.ActiveTimeSheet = model.ActiveTimeSheet;
                    entityId.ActiveInvoice = model.ActiveInvoice;
                    entityId.ActiveInvoiceEmailTemplate = model.ActiveInvoiceEmailTemplate;
                    entityId.ActiveMap = model.ActiveMap;
                    entityId.ActiveNotes = model.ActiveNotes;
                    entityId.ActiveNotificationSettings = model.ActiveNotificationSettings;
                    entityId.ActivePayment = model.ActivePayment;
                    entityId.ActiveReminderRules = model.ActiveReminderRules;
                    entityId.ActiveRoute = model.ActiveRoute;
                    entityId.ActiveSchedule = model.ActiveSchedule;
                    entityId.ActiveSettings = model.ActiveSettings;
                    entityId.ActiveTax = model.ActiveTax;
                    entityId.ActiveReport = model.ActiveReport;
                    entityId.ActiveStripeAccount = model.ActiveStripeAccount;
                    entityId.ActiveMessage = model.ActiveMessage;
                    entityId.ActiveItemsServicesSubCategory = model.ActiveItemsServicesSubCategory;
                    entityId.ActiveAccount = model.ActiveAccount;
                    entityId.ActiveEditCustomer = model.ActiveEditCustomer;
                    entityId.ActiveEditEstimate_Invoice = model.ActiveEditEstimate_Invoice;
                    entityId.Notes = model.Notes;
                    entityId.Active = model.Active;
                    entityId.CreateUser = model.CreateUser;
                    entityId.CreateDate = model.CreateDate;
                    entityId.ActiveEditPrice = model.ActiveEditPrice;
                    entityId.ActiveMobileLogin = model.ActiveMobileLogin;
                    entityId.ActiveMobileTrackStaff = model.ActiveMobileTrackStaff;
                    entityId.ActiveCreateSchedule = model.ActiveCreateSchedule;
                    entityId.OneSignalPlayerId = model.OneSignalPlayerId;
                    entityId.UserIdMysql = model.UserIdMysql;

                    _db.Entry(entityId).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                else
                {
                    throw new ArgumentException(message: $"This Employee " + model.UserName + " Is Deleted");
                }
            }
            else
            {
                throw new ArgumentException(message: $"The Employee Name {model.UserName} already exists.");
            }
        }

        public void DeleteEmployee(PropertyEmployeeDTO model)
        {
            var entity = _db.Tbl_Employee.FirstOrDefault(x => x.Id == model.Id);
            if (entity != null)
            {
                _db.Tbl_Employee.Remove(entity);
                _db.SaveChanges();
            }
            else
            {
                throw new ArgumentException(message: $"This Employee " + model.UserName + " Is Deleted");
            }
        }

        public IEnumerable<PropertyEmployeeDTO> ToList(int? AccountId, int? BranchId, int? UserRole, int? UserId)
        {
            List<PropertyEmployeeDTO> lstEmployees = new List<PropertyEmployeeDTO>();

            var Employees = _db.Tbl_Employee.Where(x => x.AccountId == AccountId).Select(x => new PropertyEmployeeDTO
            {
                Id = x.Id,
                AccountId = x.AccountId,
                BrancheId = _db.Tbl_EmployeeBranches.Where(d => d.EmployeeId == x.Id).Select(s => s.BrancheId).FirstOrDefault(),
                BranchName = _db.Tbl_Branches.Where(d => d.Id == _db.Tbl_EmployeeBranches.Where(f => f.EmployeeId == x.Id).Select(g => g.BrancheId).FirstOrDefault()).Select(s => s.Name).FirstOrDefault(),
                FirstName = x.FirstName,
                LastName = x.LastName,
                Birthday = x.Birthday,
                Since = x.Since,
                Salary = x.Salary,
                SalaeryPer = x.SalaeryPer,
                CategoryId = x.CategoryId,
                CategoryName = x.Tbl_EmployeeCategory.CategoryName,
                Phone1 = x.Phone1,
                Phone2 = x.Phone2,
                UserName = x.UserName,
                EmailUserName = x.EmailUserName,
                Password = x.Password,
                Address = x.Address,
                City = x.City,
                State = x.State,
                PostalcodeZIP = x.PostalcodeZIP,
                locationlatitude = x.locationlatitude,
                locationlongitude = x.locationlongitude,
                Country = x.Country,
                SSN = x.SSN,
                DriveLicense = x.DriveLicense,
                Picture = x.Picture,
                UserRole = x.UserRole,
                UserType = x.UserType,
                ExpireDate = x.ExpireDate,
                TheBranchs = x.TheBranchs,
                Employees = x.Employees,
                ActiveHome = x.ActiveHome,
                ActiveBranches = x.ActiveBranches,
                ActiveContract = x.ActiveContract,
                ActiveCustomers = x.ActiveCustomers,
                ActiveCustomersCategory = x.ActiveCustomersCategory,
                ActiveCustomersCustomField = x.ActiveCustomersCustomField,
                ActiveEmployee = x.ActiveEmployee,
                ActiveEmployeeCategory = x.ActiveEmployeeCategory,
                ActiveEmployeeCustomField = x.ActiveEmployeeCustomField,
                ActiveEstimate = x.ActiveEstimate,
                ActiveEstimateEmailTemplate = x.ActiveEstimateEmailTemplate,
                ActiveExpenses = x.ActiveExpenses,
                ActiveExpensesCategory = x.ActiveExpensesCategory,
                ActiveEquipments = x.ActiveEquipments,
                ActiveEquipmentsCustomField = x.ActiveEmployeeCustomField,
                ActiveItemsServices = x.ActiveItemsServices,
                ActiveItemsServicesCategory = x.ActiveItemsServicesCategory,
                ActiveItemsServicesCustomField = x.ActiveItemsServicesCustomField,
                ActiveMember = x.ActiveMember,
                ActiveTimeSheet = x.ActiveTimeSheet,
                ActiveInvoice = x.ActiveInvoice,
                ActiveInvoiceEmailTemplate = x.ActiveInvoiceEmailTemplate,
                ActiveMap = x.ActiveMap,
                ActiveNotes = x.ActiveNotes,
                ActiveNotificationSettings = x.ActiveNotificationSettings,
                ActivePayment = x.ActivePayment,
                ActiveReminderRules = x.ActiveReminderRules,
                ActiveRoute = x.ActiveRoute,
                ActiveSchedule = x.ActiveSchedule,
                ActiveSettings = x.ActiveSettings,
                ActiveTax = x.ActiveTax,
                ActiveReport = x.ActiveReport,
                ActiveStripeAccount = x.ActiveStripeAccount,
                ActiveMessage = x.ActiveMessage,
                ActiveItemsServicesSubCategory = x.ActiveItemsServicesSubCategory,
                ActiveAccount = x.ActiveAccount,
                ActiveEditCustomer = x.ActiveEditCustomer,
                Notes = x.Notes,
                Active = x.Active,
                CreateUser = x.CreateUser,
                CreateDate = x.CreateDate,
                ActiveEditPrice = x.ActiveEditPrice,
                ActiveMobileLogin = x.ActiveMobileLogin,
                ActiveMobileTrackStaff = x.ActiveMobileTrackStaff,
                ActiveCreateSchedule = x.ActiveCreateSchedule,
                ActiveAllScdTr_FaorTrOnly = x.ActiveAllScdTr_FaorTrOnly,
                ActiveEditEstimate_Invoice = x.ActiveEditEstimate_Invoice,
                ActiveCustomerPhone = x.ActiveCustomerPhone,
            });

            lstEmployees = Employees.ToList();

            //if (UserRole == 2 || UserRole == 3)
            //{
            //    List<int> EmpIds = Employees?.Where(e => e.Id == UserId).Select(s => s.Employees).FirstOrDefault().ToString().Split(',').Select(t => int.Parse(t)).ToList();

            //    foreach (var Emp in Employees)
            //    {
            //        if (EmpIds.Contains(Emp.Id))
            //        {
            //            lstEmployees.Add(Emp);
            //        }
            //    }
            //}
            //else
            //{
            //    lstEmployees = Employees.ToList();
            //}

            //public List<PropertyEmployeeDTO> ToList()
            //{
            //    int? AccountId = 0;
            //    if (EmployeeCookie != null && EmployeeCookie.AccountId != null && EmployeeCookie.AccountId != 0)
            //    {
            //        AccountId = EmployeeCookie.AccountId;
            //    }

            //    var Employee = _db.Tbl_Employee.Where(x => x.Id != 0).Select(x => new PropertyEmployeeDTO
            //    {
            //        Id = x.Id,
            //        BrancheName = x.Tbl_Branches1.Name,
            //        AccountId = x.AccountId,
            //        FirstName = x.FirstName,
            //        LastName = x.LastName,
            //        Birthday = x.Birthday,
            //        CategoryId = x.CategoryId,
            //        CategoryName = x.Tbl_EmployeeCategory.CategoryName,
            //        DriveLicense = x.DriveLicense,
            //        UserName = x.UserName,
            //        EmailUserName = x.EmailUserName,
            //        Picture = x.Picture,
            //        SalaeryPer = x.SalaeryPer,
            //        Salary = x.Salary,
            //        SSN = x.SSN,
            //        UserRole = x.UserRole,
            //        UserType = x.UserType,
            //        Phone1 = x.Phone1,
            //        Phone2 = x.Phone2,
            //        PostalcodeZIP = x.PostalcodeZIP,
            //        Address = x.Address,
            //        locationlatitude = x.locationlatitude,
            //        locationlongitude = x.locationlongitude,
            //        City = x.City,
            //        State = x.State,
            //        Country = x.Country,
            //        Since = x.Since,
            //        BrancheId = x.BrancheId,
            //        Password = x.Password,
            //        Notes = x.Notes,
            //        ExpireDate = x.ExpireDate,
            //        Employees = x.Employees,
            //        TheBranchs = x.TheBranchs,
            //        ActiveBranches = x.ActiveBranches,
            //        ActiveContract = x.ActiveContract,
            //        ActiveCustomers = x.ActiveCustomers,
            //        ActiveCustomersCategory = x.ActiveCustomersCategory,
            //        ActiveCustomersCustomField = x.ActiveCustomersCustomField,
            //        ActiveEmployee = x.ActiveEmployee,
            //        ActiveEmployeeCategory = x.ActiveEmployeeCategory,
            //        ActiveEmployeeCustomField = x.ActiveEmployeeCustomField,
            //        ActiveEquipments = x.ActiveEquipments,
            //        ActiveEquipmentsCustomField = x.ActiveEquipmentsCustomField,
            //        ActiveEstimate = x.ActiveEstimate,
            //        ActiveEstimateEmailTemplate = x.ActiveEstimateEmailTemplate,
            //        ActiveExpenses = x.ActiveExpenses,
            //        ActiveExpensesCategory = x.ActiveExpensesCategory,
            //        ActiveHome = x.ActiveHome,
            //        ActiveInvoice = x.ActiveInvoice,
            //        ActiveInvoiceEmailTemplate = x.ActiveInvoiceEmailTemplate,
            //        ActiveItemsServices = x.ActiveItemsServices,
            //        ActiveItemsServicesCategory = x.ActiveItemsServicesCategory,
            //        ActiveItemsServicesCustomField = x.ActiveItemsServicesCustomField,
            //        ActiveMap = x.ActiveMap,
            //        ActiveMember = x.ActiveMember,
            //        ActiveNotes = x.ActiveNotes,
            //        ActiveNotificationSettings = x.ActiveNotificationSettings,
            //        ActivePayment = x.ActivePayment,
            //        ActiveReminderRules = x.ActiveReminderRules,
            //        ActiveRoute = x.ActiveRoute,
            //        ActiveSchedule = x.ActiveSchedule,
            //        ActiveSettings = x.ActiveSettings,
            //        ActiveTax = x.ActiveTax,
            //        ActiveTimeSheet = x.ActiveTimeSheet,
            //        ActiveReport = x.ActiveReport,
            //        ActiveStripeAccount = x.ActiveStripeAccount,
            //        ActiveMessage = x.ActiveMessage,
            //        ActiveItemsServicesSubCategory = x.ActiveItemsServicesSubCategory,
            //        ActiveAccount = x.ActiveAccount,
            //        Active = x.Active,
            //        CreateUser = x.CreateUser,
            //        CreateDate = x.CreateDate,
            //    }).ToList();


            //    //if (EmployeeCookie != null)
            //    //{
            //    //    Employee = Employee.Where(x => x.Id != 0 && x.AccountId == AccountId).ToList();

            //    //    string Employees = "";
            //    //    if (string.IsNullOrEmpty(EmployeeCookie.Employees) == false)
            //    //    {
            //    //        Employees = EmployeeCookie.Employees;
            //    //    }
            //    //    var FilterModel = new List<PropertyEmployeeDTO>() { };
            //    //    if (EmployeeCookie.UserRole != null && EmployeeCookie.Id != 0 && Employee.ToList() != null && Employee.ToList().Count() != 0)
            //    //    {
            //    //        int? UserRole = EmployeeCookie.UserRole;
            //    //        int? EmployeeId = EmployeeCookie.Id;
            //    //        if (UserRole == 1)
            //    //        {
            //    //            Employee = Employee.Where(c => c.CreateUser == EmployeeId).ToList();
            //    //        }
            //    //        else if (UserRole == 2 || UserRole == 3 && EmployeeId != null)
            //    //        {
            //    //            int[] AllID = { 0 };
            //    //            List<PropertyEmployeeDTO> BranchesModel = new List<PropertyEmployeeDTO>() { };
            //    //            if (string.IsNullOrEmpty(Employees.ToString()) != true)
            //    //            {
            //    //                AllID = Employees.ToString().Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            //    //                if (AllID != null && AllID.Count() != 0)
            //    //                {
            //    //                    foreach (var item in AllID)
            //    //                    {
            //    //                        BranchesModel = Employee.Where(c => c.CreateUser == item).ToList();
            //    //                        FilterModel.AddRange(BranchesModel);
            //    //                    }
            //    //                    //Employee = FilterModel;
            //    //                }
            //    //            }
            //    //            //if (AllID.Contains(Convert.ToInt32(EmployeeId)) == false)
            //    //            //{
            //    //            BranchesModel = Employee.Where(c => c.CreateUser == Convert.ToInt32(EmployeeId)).ToList();
            //    //            FilterModel.AddRange(BranchesModel);
            //    //            //Employee = FilterModel;
            //    //            //}

            //    //            Employee = FilterModel;
            //    //        }
            //    //    }
            //    //}

            //    return Employee.OrderBy(c => c.Id).ToList();
            //}

            return lstEmployees;
        }

        public IEnumerable<PropertyEmployeeDTO> GetEmployeesInAccountId(int? AccountId)
        {
            var Employees = _db.Tbl_Employee.Where(x => x.AccountId == AccountId).Select(x => new PropertyEmployeeDTO
            {
                Id = x.Id,
                AccountId = x.AccountId,
                BrancheId = _db.Tbl_EmployeeBranches.Where(d => d.EmployeeId == x.Id).Select(s => s.BrancheId).FirstOrDefault(),
                BranchName = _db.Tbl_Branches.Where(d => d.Id == _db.Tbl_EmployeeBranches.Where(f => f.EmployeeId == x.Id).Select(g => g.BrancheId).FirstOrDefault()).Select(s => s.Name).FirstOrDefault(),
                FirstName = x.FirstName,
                LastName = x.LastName,
                CategoryName = x.Tbl_EmployeeCategory.CategoryName,
                Phone1 = x.Phone1,
                UserName = x.UserName,
                EmailUserName = x.EmailUserName,
                Address = x.Address,
                City = x.City,
                State = x.State,
                PostalcodeZIP = x.PostalcodeZIP,
                locationlatitude = x.locationlatitude,
                locationlongitude = x.locationlongitude,
                Country = x.Country,
                UserRole = x.UserRole,
                UserType = x.UserType,
                OneSignalPlayerId = x.OneSignalPlayerId,
            });

            return Employees.ToList();
        }

        public IEnumerable<PropertyEmployeeDTO> ToListTracking(int? AccountId, string ScheduleDate)
        {
            List<PropertyEmployeeDTO> Employees = new List<PropertyEmployeeDTO>();

            var SchDate = _db.Tbl_ScheduleDate.Where(x => x.AccountId == AccountId && x.Date == ScheduleDate).ToList();

            string lstWorkingEmployees = string.Empty;

            if(SchDate.Count > 0) 
            {
                foreach (var s in SchDate)
                {
                    lstWorkingEmployees += "," + s.Tbl_ScheduleEmployees.Select(x => x.EmpId.ToString()).Aggregate((a, b) => a + "," + b);
                }

                lstWorkingEmployees = lstWorkingEmployees.Remove(0, 1);

                if (!string.IsNullOrEmpty(lstWorkingEmployees))
                {
                    List<string> AllEmpId = lstWorkingEmployees.Split(',').Distinct().ToList();

                    foreach (var id in AllEmpId)
                    {
                        if (id != "" || id != null)
                        {
                            int IdEmp = int.Parse(id);

                            int emp = _db.Tbl_TimeSheet.Where(c => c.EmployeeId == IdEmp && c.Date == ScheduleDate).Select(s => s.Id).FirstOrDefault();

                            if (emp != 0) 
                            {
                                PropertyEmployeeDTO Employee = _db.Tbl_Employee.Where(x => x.Id == IdEmp).Select(x => new PropertyEmployeeDTO
                                {
                                    Id = x.Id,
                                    AccountId = x.AccountId,
                                    BrancheId = _db.Tbl_EmployeeBranches.Where(d => d.EmployeeId == x.Id).Select(s => s.BrancheId).FirstOrDefault(),
                                    //BranchName = _db.Tbl_Branches.Where(c => c.Id == x.BrancheId).Select(c => c.Name).FirstOrDefault(),
                                    BranchName = _db.Tbl_Branches.Where(d => d.Id == _db.Tbl_EmployeeBranches.Where(f => f.EmployeeId == x.Id).Select(g => g.BrancheId).FirstOrDefault()).Select(s => s.Name).FirstOrDefault(),
                                    FirstName = x.FirstName,
                                    LastName = x.LastName,
                                    Phone1 = x.Phone1
                                }).FirstOrDefault();

                                Employees.Add(Employee);
                            }
                        }
                    }
                }
            }

            return Employees.ToList();
        }

        public IEnumerable<PropertyEmployeeCategoryDTO> GetEmployeeCategory(int? AccountId)
        {
            var EmployeeCategory = _db.Tbl_EmployeeCategory.Where(x => x.Id != 0 && x.AccountId == AccountId && x.Active == true).Select(x => new PropertyEmployeeCategoryDTO
            {
                Id = x.Id,
                AccountId = x.AccountId,
                BrancheId = x.BrancheId,
                CategoryName = x.CategoryName,
                Description = x.Description,
                Notes = x.Notes,
                Active = x.Active,
                CreateUser = x.CreateUser,
                CreateDate = x.CreateDate,
            });

            return EmployeeCategory.ToList();
        }

        public IEnumerable<PropertyEmployeeDTO> GetEmployeeInOneCategory(int? BranchId, int? CategoryId, int? AccountId, int? UserRole, int? UserId)
        {
            List<PropertyEmployeeDTO> lstEmployees = new List<PropertyEmployeeDTO>();

            var Employees = _db.Tbl_Employee.Where(x => x.Id != 0 && x.AccountId == AccountId && x.CategoryId == CategoryId).Select(x => new PropertyEmployeeDTO
            {
                Id = x.Id,
                AccountId = x.AccountId,
                BrancheId = _db.Tbl_EmployeeBranches.Where(d => d.EmployeeId == x.Id).Select(s => s.BrancheId).FirstOrDefault(),
                BranchName = _db.Tbl_Branches.Where(d => d.Id == _db.Tbl_EmployeeBranches.Where(f => f.EmployeeId == x.Id).Select(g => g.BrancheId).FirstOrDefault()).Select(s => s.Name).FirstOrDefault(),
                FirstName = x.FirstName,
                LastName = x.LastName,
                Birthday = x.Birthday,
                Since = x.Since,
                Salary = x.Salary,
                SalaeryPer = x.SalaeryPer,
                CategoryId = x.CategoryId,
                CategoryName = x.Tbl_EmployeeCategory.CategoryName,
                Phone1 = x.Phone1,
                Phone2 = x.Phone2,
                UserName = x.UserName,
                EmailUserName = x.EmailUserName,
                Password = x.Password,
                Address = x.Address,
                City = x.City,
                State = x.State,
                PostalcodeZIP = x.PostalcodeZIP,
                locationlatitude = x.locationlatitude,
                locationlongitude = x.locationlongitude,
                Country = x.Country,
                SSN = x.SSN,
                DriveLicense = x.DriveLicense,
                Picture = x.Picture,
                UserRole = x.UserRole,
                UserType = x.UserType,
                ExpireDate = x.ExpireDate,
                TheBranchs = x.TheBranchs,
                Employees = x.Employees,
                ActiveHome = x.ActiveHome,
                ActiveBranches = x.ActiveBranches,
                ActiveContract = x.ActiveContract,
                ActiveCustomers = x.ActiveCustomers,
                ActiveCustomersCategory = x.ActiveCustomersCategory,
                ActiveCustomersCustomField = x.ActiveCustomersCustomField,
                ActiveEmployee = x.ActiveEmployee,
                ActiveEmployeeCategory = x.ActiveEmployeeCategory,
                ActiveEmployeeCustomField = x.ActiveEmployeeCustomField,
                ActiveEstimate = x.ActiveEstimate,
                ActiveEstimateEmailTemplate = x.ActiveEstimateEmailTemplate,
                ActiveExpenses = x.ActiveExpenses,
                ActiveExpensesCategory = x.ActiveExpensesCategory,
                ActiveEquipments = x.ActiveEquipments,
                ActiveEquipmentsCustomField = x.ActiveEmployeeCustomField,
                ActiveItemsServices = x.ActiveItemsServices,
                ActiveItemsServicesCategory = x.ActiveItemsServicesCategory,
                ActiveItemsServicesCustomField = x.ActiveItemsServicesCustomField,
                ActiveMember = x.ActiveMember,
                ActiveTimeSheet = x.ActiveTimeSheet,
                ActiveInvoice = x.ActiveInvoice,
                ActiveInvoiceEmailTemplate = x.ActiveInvoiceEmailTemplate,
                ActiveMap = x.ActiveMap,
                ActiveNotes = x.ActiveNotes,
                ActiveNotificationSettings = x.ActiveNotificationSettings,
                ActivePayment = x.ActivePayment,
                ActiveReminderRules = x.ActiveReminderRules,
                ActiveRoute = x.ActiveRoute,
                ActiveSchedule = x.ActiveSchedule,
                ActiveSettings = x.ActiveSettings,
                ActiveTax = x.ActiveTax,
                ActiveReport = x.ActiveReport,
                ActiveStripeAccount = x.ActiveStripeAccount,
                ActiveMessage = x.ActiveMessage,
                ActiveItemsServicesSubCategory = x.ActiveItemsServicesSubCategory,
                ActiveAccount = x.ActiveAccount,
                ActiveEditCustomer = x.ActiveEditCustomer,
                Notes = x.Notes,
                Active = x.Active,
                CreateUser = x.CreateUser,
                CreateDate = x.CreateDate,
                ActiveEditPrice = x.ActiveEditPrice,
                ActiveMobileLogin = x.ActiveMobileLogin,
                ActiveMobileTrackStaff = x.ActiveMobileTrackStaff,
                ActiveCreateSchedule = x.ActiveCreateSchedule,
                OneSignalPlayerId = x.OneSignalPlayerId,
                ActiveAllScdTr_FaorTrOnly = x.ActiveAllScdTr_FaorTrOnly,
            });

            lstEmployees = Employees.ToList();

            //if (UserRole == 2 || UserRole == 3)
            //{
            //    List<int> EmpIds = _db.Tbl_Employee.Where(e => e.Id == UserId).Select(s => s.Employees).FirstOrDefault().ToString().Split(',').Select(t => int.Parse(t)).ToList();

            //    foreach (var Emp in Employees)
            //    {
            //        if (EmpIds.Contains(Emp.Id))
            //        {
            //            lstEmployees.Add(Emp);
            //        }
            //    }
            //}
            //else
            //{
            //    lstEmployees = Employees.ToList();
            //}

            return lstEmployees;
        }

        public IEnumerable<PropertyEmployeeDTO> GetEmployeeInCall(int? AccountId, int? UserRole, int? UserId)
        {
            List<PropertyEmployeeDTO> lstEmployees = new List<PropertyEmployeeDTO>();

            var Employees = _db.Tbl_Employee.Where(x => x.Id != 0 && x.AccountId == AccountId).Select(x => new PropertyEmployeeDTO
            {
                Id = x.Id,
                AccountId = x.AccountId,
                BrancheId = _db.Tbl_EmployeeBranches.Where(d => d.EmployeeId == x.Id).Select(s => s.BrancheId).FirstOrDefault(),
                BranchName = _db.Tbl_Branches.Where(d => d.Id == _db.Tbl_EmployeeBranches.Where(f => f.EmployeeId == x.Id).Select(g => g.BrancheId).FirstOrDefault()).Select(s => s.Name).FirstOrDefault(),
                FirstName = x.FirstName,
                LastName = x.LastName,
                Birthday = x.Birthday,
                Since = x.Since,
                Salary = x.Salary,
                SalaeryPer = x.SalaeryPer,
                CategoryId = x.CategoryId,
                CategoryName = x.Tbl_EmployeeCategory.CategoryName,
                Phone1 = x.Phone1,
                Phone2 = x.Phone2,
                UserName = x.UserName,
                EmailUserName = x.EmailUserName,
                Password = x.Password,
                Address = x.Address,
                City = x.City,
                State = x.State,
                PostalcodeZIP = x.PostalcodeZIP,
                locationlatitude = x.locationlatitude,
                locationlongitude = x.locationlongitude,
                Country = x.Country,
                SSN = x.SSN,
                DriveLicense = x.DriveLicense,
                Picture = x.Picture,
                UserRole = x.UserRole,
                UserType = x.UserType,
                ExpireDate = x.ExpireDate,
                TheBranchs = x.TheBranchs,
                Employees = x.Employees,
                ActiveHome = x.ActiveHome,
                ActiveBranches = x.ActiveBranches,
                ActiveContract = x.ActiveContract,
                ActiveCustomers = x.ActiveCustomers,
                ActiveCustomersCategory = x.ActiveCustomersCategory,
                ActiveCustomersCustomField = x.ActiveCustomersCustomField,
                ActiveEmployee = x.ActiveEmployee,
                ActiveEmployeeCategory = x.ActiveEmployeeCategory,
                ActiveEmployeeCustomField = x.ActiveEmployeeCustomField,
                ActiveEstimate = x.ActiveEstimate,
                ActiveEstimateEmailTemplate = x.ActiveEstimateEmailTemplate,
                ActiveExpenses = x.ActiveExpenses,
                ActiveExpensesCategory = x.ActiveExpensesCategory,
                ActiveEquipments = x.ActiveEquipments,
                ActiveEquipmentsCustomField = x.ActiveEmployeeCustomField,
                ActiveItemsServices = x.ActiveItemsServices,
                ActiveItemsServicesCategory = x.ActiveItemsServicesCategory,
                ActiveItemsServicesCustomField = x.ActiveItemsServicesCustomField,
                ActiveMember = x.ActiveMember,
                ActiveTimeSheet = x.ActiveTimeSheet,
                ActiveInvoice = x.ActiveInvoice,
                ActiveInvoiceEmailTemplate = x.ActiveInvoiceEmailTemplate,
                ActiveMap = x.ActiveMap,
                ActiveNotes = x.ActiveNotes,
                ActiveNotificationSettings = x.ActiveNotificationSettings,
                ActivePayment = x.ActivePayment,
                ActiveReminderRules = x.ActiveReminderRules,
                ActiveRoute = x.ActiveRoute,
                ActiveSchedule = x.ActiveSchedule,
                ActiveSettings = x.ActiveSettings,
                ActiveTax = x.ActiveTax,
                ActiveReport = x.ActiveReport,
                ActiveStripeAccount = x.ActiveStripeAccount,
                ActiveMessage = x.ActiveMessage,
                ActiveItemsServicesSubCategory = x.ActiveItemsServicesSubCategory,
                ActiveAccount = x.ActiveAccount,
                ActiveEditCustomer = x.ActiveEditCustomer,
                Notes = x.Notes,
                Active = x.Active,
                CreateUser = x.CreateUser,
                CreateDate = x.CreateDate,
                ActiveEditPrice = x.ActiveEditPrice,
                ActiveMobileLogin = x.ActiveMobileLogin,
                ActiveMobileTrackStaff = x.ActiveMobileTrackStaff,
                ActiveCreateSchedule = x.ActiveCreateSchedule,
                OneSignalPlayerId = x.OneSignalPlayerId,
                ActiveAllScdTr_FaorTrOnly = x.ActiveAllScdTr_FaorTrOnly,
                ActiveCustomerPhone = x.ActiveCustomerPhone,
            });

            //if (UserRole == 2 || UserRole == 3) //Admin
            //{
            //    List<int> EmpIds = _db.Tbl_Employee.Where(e => e.Id == UserId).Select(s => s.Employees).FirstOrDefault().ToString().Split(',').Select(t => int.Parse(t)).ToList();

            //    foreach (var Emp in Employees)
            //    {
            //        if (EmpIds.Contains(Emp.Id))
            //        {
            //            lstEmployees.Add(Emp);
            //        }
            //    }
            //    var emp = Employees.Where(e => e.Id == UserId).FirstOrDefault();
            //    lstEmployees.Add(emp);
            //}
            if (UserRole == 1) //Staff
            {
                var emp = Employees.Where(e => e.Id == UserId).FirstOrDefault();
                lstEmployees.Add(emp);
            }
            else //Owner
            {
                lstEmployees = Employees.ToList();
            }



            return lstEmployees;
        }

        public PropertyEmployeeDTO FindEmployeeById(int? id)
        {
            var Employee = _db.Tbl_Employee.Where(x => x.Id == id).Select(x => new PropertyEmployeeDTO
            {
                Id = x.Id,
                AccountId = x.AccountId,
                BrancheId = _db.Tbl_EmployeeBranches.Where(d => d.EmployeeId == x.Id).Select(s => s.BrancheId).FirstOrDefault(),
                FirstName = x.FirstName,
                LastName = x.LastName,
                Birthday = x.Birthday,
                Since = x.Since,
                Salary = x.Salary,
                SalaeryPer = x.SalaeryPer,
                CategoryId = x.CategoryId,
                CategoryName = x.Tbl_EmployeeCategory.CategoryName,
                Phone1 = x.Phone1,
                Phone2 = x.Phone2,
                UserName = x.UserName,
                EmailUserName = x.EmailUserName,
                Password = x.Password,
                Address = x.Address,
                City = x.City,
                State = x.State,
                PostalcodeZIP = x.PostalcodeZIP,
                locationlatitude = x.locationlatitude,
                locationlongitude = x.locationlongitude,
                Country = x.Country,
                SSN = x.SSN,
                DriveLicense = x.DriveLicense,
                Picture = x.Picture,
                UserRole = x.UserRole,
                UserType = x.UserType,
                ExpireDate = x.ExpireDate,
                TheBranchs = x.TheBranchs,
                Employees = x.Employees,
                ActiveHome = x.ActiveHome,
                ActiveBranches = x.ActiveBranches,
                ActiveContract = x.ActiveContract,
                ActiveCustomers = x.ActiveCustomers,
                ActiveCustomersCategory = x.ActiveCustomersCategory,
                ActiveCustomersCustomField = x.ActiveCustomersCustomField,
                ActiveEmployee = x.ActiveEmployee,
                ActiveEmployeeCategory = x.ActiveEmployeeCategory,
                ActiveEmployeeCustomField = x.ActiveEmployeeCustomField,
                ActiveEstimate = x.ActiveEstimate,
                ActiveEstimateEmailTemplate = x.ActiveEstimateEmailTemplate,
                ActiveExpenses = x.ActiveExpenses,
                ActiveExpensesCategory = x.ActiveExpensesCategory,
                ActiveEquipments = x.ActiveEquipments,
                ActiveEquipmentsCustomField = x.ActiveEmployeeCustomField,
                ActiveItemsServices = x.ActiveItemsServices,
                ActiveItemsServicesCategory = x.ActiveItemsServicesCategory,
                ActiveItemsServicesCustomField = x.ActiveItemsServicesCustomField,
                ActiveMember = x.ActiveMember,
                ActiveTimeSheet = x.ActiveTimeSheet,
                ActiveInvoice = x.ActiveInvoice,
                ActiveInvoiceEmailTemplate = x.ActiveInvoiceEmailTemplate,
                ActiveMap = x.ActiveMap,
                ActiveNotes = x.ActiveNotes,
                ActiveNotificationSettings = x.ActiveNotificationSettings,
                ActivePayment = x.ActivePayment,
                ActiveReminderRules = x.ActiveReminderRules,
                ActiveRoute = x.ActiveRoute,
                ActiveSchedule = x.ActiveSchedule,
                ActiveSettings = x.ActiveSettings,
                ActiveTax = x.ActiveTax,
                ActiveReport = x.ActiveReport,
                ActiveStripeAccount = x.ActiveStripeAccount,
                ActiveMessage = x.ActiveMessage,
                ActiveItemsServicesSubCategory = x.ActiveItemsServicesSubCategory,
                ActiveAccount = x.ActiveAccount,
                ActiveEditCustomer = x.ActiveEditCustomer,
                Notes = x.Notes,
                Active = x.Active,
                CreateUser = x.CreateUser,
                CreateDate = x.CreateDate,
                ActiveEditPrice = x.ActiveEditPrice,
                ActiveMobileLogin = x.ActiveMobileLogin,
                ActiveMobileTrackStaff = x.ActiveMobileTrackStaff,
                ActiveCreateSchedule = x.ActiveCreateSchedule,
                OneSignalPlayerId = x.OneSignalPlayerId,
                ActiveAllScdTr_FaorTrOnly = x.ActiveAllScdTr_FaorTrOnly,
                ActiveEditEstimate_Invoice = x.ActiveEditEstimate_Invoice,
                UserIdMysql = x.UserIdMysql,
            });

            return Employee.FirstOrDefault();
        }

        public PropertyEmployeeDTO CheckPassword(string UserName, string Password)
        {
            var Employee = _db.Tbl_Employee.Where(x => (x.UserName.ToLower() == UserName.ToLower() && x.Password.ToLower() == Password.ToLower())
                           || (x.EmailUserName.ToLower() == UserName.ToLower() && x.Password.ToLower() == Password.ToLower())).Select(x => new PropertyEmployeeDTO
                           {
                               Id = x.Id,
                               AccountId = x.AccountId,
                               AccountName = x.Tbl_Account.CompanyName,
                               AccountExpireDate = x.Tbl_Account.ExpireDate,
                               CompanyNameWithSpace = x.Tbl_Account.CompanyNameWithSpace,
                               DBConnection = x.Tbl_Account.DBConnection,
                               PathFileUpload = x.Tbl_Account.PathFileUpload,
                               AccountSubdomainURL = x.Tbl_Account.AccountSubdomainURL,
                               AccountSubdomainApiURL = x.Tbl_Account.AccountSubdomainApiURL,
                               TypeTrackingSch_Invo = x.Tbl_Account.TypeTrackingSch_Invo,
                               BrancheId = _db.Tbl_EmployeeBranches.Where(d => d.EmployeeId == x.Id).Select(s => s.BrancheId).FirstOrDefault(),
                               BranchName = _db.Tbl_Branches.Where(d => d.Id == _db.Tbl_EmployeeBranches.Where(f => f.EmployeeId == x.Id).Select(g => g.BrancheId).FirstOrDefault()).Select(s => s.Name).FirstOrDefault(),
                               FirstName = x.FirstName,
                               LastName = x.LastName,
                               Birthday = x.Birthday,
                               Since = x.Since,
                               Salary = x.Salary,
                               SalaeryPer = x.SalaeryPer,
                               CategoryId = x.CategoryId,
                               CategoryName = x.Tbl_EmployeeCategory.CategoryName,
                               Phone1 = x.Phone1,
                               Phone2 = x.Phone2,
                               UserName = x.UserName,
                               EmailUserName = x.EmailUserName,
                               Password = x.Password,
                               Address = x.Address,
                               City = x.City,
                               State = x.State,
                               PostalcodeZIP = x.PostalcodeZIP,
                               locationlatitude = x.locationlatitude,
                               locationlongitude = x.locationlongitude,
                               Country = x.Country,
                               SSN = x.SSN,
                               DriveLicense = x.DriveLicense,
                               Picture = x.Picture,
                               UserRole = x.UserRole,
                               UserType = x.UserType,
                               ExpireDate = x.ExpireDate,
                               TheBranchs = x.TheBranchs,
                               Employees = x.Employees,
                               ActiveHome = x.ActiveHome,
                               ActiveBranches = x.ActiveBranches,
                               ActiveContract = x.ActiveContract,
                               ActiveCustomers = x.ActiveCustomers,
                               ActiveCustomersCategory = x.ActiveCustomersCategory,
                               ActiveCustomersCustomField = x.ActiveCustomersCustomField,
                               ActiveEmployee = x.ActiveEmployee,
                               ActiveEmployeeCategory = x.ActiveEmployeeCategory,
                               ActiveEmployeeCustomField = x.ActiveEmployeeCustomField,
                               ActiveEstimate = x.ActiveEstimate,
                               ActiveEstimateEmailTemplate = x.ActiveEstimateEmailTemplate,
                               ActiveExpenses = x.ActiveExpenses,
                               ActiveExpensesCategory = x.ActiveExpensesCategory,
                               ActiveEquipments = x.ActiveEquipments,
                               ActiveEquipmentsCustomField = x.ActiveEmployeeCustomField,
                               ActiveItemsServices = x.ActiveItemsServices,
                               ActiveItemsServicesCategory = x.ActiveItemsServicesCategory,
                               ActiveItemsServicesCustomField = x.ActiveItemsServicesCustomField,
                               ActiveMember = x.ActiveMember,
                               ActiveTimeSheet = x.ActiveTimeSheet,
                               ActiveInvoice = x.ActiveInvoice,
                               ActiveInvoiceEmailTemplate = x.ActiveInvoiceEmailTemplate,
                               ActiveMap = x.ActiveMap,
                               ActiveNotes = x.ActiveNotes,
                               ActiveNotificationSettings = x.ActiveNotificationSettings,
                               ActivePayment = x.ActivePayment,
                               ActiveReminderRules = x.ActiveReminderRules,
                               ActiveRoute = x.ActiveRoute,
                               ActiveSchedule = x.ActiveSchedule,
                               ActiveSettings = x.ActiveSettings,
                               ActiveTax = x.ActiveTax,
                               ActiveReport = x.ActiveReport,
                               ActiveStripeAccount = x.ActiveStripeAccount,
                               ActiveMessage = x.ActiveMessage,
                               ActiveItemsServicesSubCategory = x.ActiveItemsServicesSubCategory,
                               ActiveAccount = x.ActiveAccount,
                               ActiveEditCustomer = x.ActiveEditCustomer,
                               Notes = x.Notes,
                               Active = x.Active,
                               CreateUser = x.CreateUser,
                               CreateDate = x.CreateDate,
                               ActiveEditPrice = x.ActiveEditPrice,
                               ActiveMobileLogin = x.ActiveMobileLogin,
                               ActiveMobileTrackStaff = x.ActiveMobileTrackStaff,
                               ActiveCreateSchedule = x.ActiveCreateSchedule,
                               OneSignalPlayerId = x.OneSignalPlayerId,
                               ActiveAllScdTr_FaorTrOnly = x.ActiveAllScdTr_FaorTrOnly,
                               ActiveEditEstimate_Invoice = x.ActiveEditEstimate_Invoice,
                               UserIdMysql = x.UserIdMysql,
                               ActiveCustomerPhone = x.ActiveCustomerPhone,
                           });

            return Employee.FirstOrDefault();
        }

        public PropertyEmployeeDTO CheckPasswordOwner(string UserName, string Password)
        {
            var Employee = _db.Tbl_Employee.Where(x => x.UserRole == 4 && (x.EmailUserName.ToLower() == UserName.ToLower() && x.Password.ToLower() == Password.ToLower())).Select(x => new PropertyEmployeeDTO
            {
                Id = x.Id,
                AccountId = x.AccountId,
                AccountName = x.Tbl_Account.CompanyName,
                DBConnection = x.Tbl_Account.DBConnection,
                PathFileUpload = x.Tbl_Account.PathFileUpload,
                AccountSubdomainURL = x.Tbl_Account.AccountSubdomainURL,
                AccountSubdomainApiURL = x.Tbl_Account.AccountSubdomainApiURL,
                TypeTrackingSch_Invo = x.Tbl_Account.TypeTrackingSch_Invo,
                BrancheId = _db.Tbl_EmployeeBranches.Where(d => d.EmployeeId == x.Id).Select(s => s.BrancheId).FirstOrDefault(),
                BranchName = _db.Tbl_Branches.Where(d => d.Id == _db.Tbl_EmployeeBranches.Where(f => f.EmployeeId == x.Id).Select(g => g.BrancheId).FirstOrDefault()).Select(s => s.Name).FirstOrDefault(),
                FirstName = x.FirstName,
                LastName = x.LastName,
                Birthday = x.Birthday,
                Since = x.Since,
                Salary = x.Salary,
                SalaeryPer = x.SalaeryPer,
                CategoryId = x.CategoryId,
                CategoryName = x.Tbl_EmployeeCategory.CategoryName,
                Phone1 = x.Phone1,
                Phone2 = x.Phone2,
                UserName = x.UserName,
                EmailUserName = x.EmailUserName,
                Password = x.Password,
                Address = x.Address,
                City = x.City,
                State = x.State,
                PostalcodeZIP = x.PostalcodeZIP,
                locationlatitude = x.locationlatitude,
                locationlongitude = x.locationlongitude,
                Country = x.Country,
                SSN = x.SSN,
                DriveLicense = x.DriveLicense,
                Picture = x.Picture,
                UserRole = x.UserRole,
                UserType = x.UserType,
                ExpireDate = x.ExpireDate,
                TheBranchs = x.TheBranchs,
                Employees = x.Employees,
                ActiveHome = x.ActiveHome,
                ActiveBranches = x.ActiveBranches,
                ActiveContract = x.ActiveContract,
                ActiveCustomers = x.ActiveCustomers,
                ActiveCustomersCategory = x.ActiveCustomersCategory,
                ActiveCustomersCustomField = x.ActiveCustomersCustomField,
                ActiveEmployee = x.ActiveEmployee,
                ActiveEmployeeCategory = x.ActiveEmployeeCategory,
                ActiveEmployeeCustomField = x.ActiveEmployeeCustomField,
                ActiveEstimate = x.ActiveEstimate,
                ActiveEstimateEmailTemplate = x.ActiveEstimateEmailTemplate,
                ActiveExpenses = x.ActiveExpenses,
                ActiveExpensesCategory = x.ActiveExpensesCategory,
                ActiveEquipments = x.ActiveEquipments,
                ActiveEquipmentsCustomField = x.ActiveEmployeeCustomField,
                ActiveItemsServices = x.ActiveItemsServices,
                ActiveItemsServicesCategory = x.ActiveItemsServicesCategory,
                ActiveItemsServicesCustomField = x.ActiveItemsServicesCustomField,
                ActiveMember = x.ActiveMember,
                ActiveTimeSheet = x.ActiveTimeSheet,
                ActiveInvoice = x.ActiveInvoice,
                ActiveInvoiceEmailTemplate = x.ActiveInvoiceEmailTemplate,
                ActiveMap = x.ActiveMap,
                ActiveNotes = x.ActiveNotes,
                ActiveNotificationSettings = x.ActiveNotificationSettings,
                ActivePayment = x.ActivePayment,
                ActiveReminderRules = x.ActiveReminderRules,
                ActiveRoute = x.ActiveRoute,
                ActiveSchedule = x.ActiveSchedule,
                ActiveSettings = x.ActiveSettings,
                ActiveTax = x.ActiveTax,
                ActiveReport = x.ActiveReport,
                ActiveStripeAccount = x.ActiveStripeAccount,
                ActiveMessage = x.ActiveMessage,
                ActiveItemsServicesSubCategory = x.ActiveItemsServicesSubCategory,
                ActiveAccount = x.ActiveAccount,
                ActiveEditCustomer = x.ActiveEditCustomer,
                Notes = x.Notes,
                Active = x.Active,
                CreateUser = x.CreateUser,
                CreateDate = x.CreateDate,
                ActiveEditPrice = x.ActiveEditPrice,
                ActiveMobileLogin = x.ActiveMobileLogin,
                ActiveMobileTrackStaff = x.ActiveMobileTrackStaff,
                ActiveCreateSchedule = x.ActiveCreateSchedule,
                OneSignalPlayerId = x.OneSignalPlayerId,
                ActiveAllScdTr_FaorTrOnly = x.ActiveAllScdTr_FaorTrOnly,
                ActiveEditEstimate_Invoice = x.ActiveEditEstimate_Invoice,
                UserIdMysql = x.UserIdMysql,
                ActiveCustomerPhone = x.ActiveCustomerPhone,
            });

            return Employee.FirstOrDefault();
        }


        public PropertyEmployeeDTO GetAccountOfEmployee(string Email)
        {
            var Employee = _db.Tbl_Employee.Where(x => x.EmailUserName.ToLower() == Email.ToLower()).Select(x => new PropertyEmployeeDTO
            {
                Id = x.Id,
                AccountId = x.AccountId,
                AccountName = x.Tbl_Account.CompanyName,
            }).AsEnumerable();

            return Employee.FirstOrDefault();
        }


        //public PropertyEmployeeDTO CheckPasswordByPhone(string Phone, string Password)
        //{
        //    var Employee = _db.Tbl_Employee.Where(x => string.IsNullOrEmpty(x.Phone1) != true && x.Active == true).Select(x => new PropertyEmployeeDTO
        //    {
        //        Id = x.Id,
        //        AccountId = x.AccountId,
        //        BrancheId = x.BrancheId,
        //        FirstName = x.FirstName,
        //        LastName = x.LastName,
        //        Birthday = x.Birthday,
        //        Since = x.Since,
        //        Salary = x.Salary,
        //        SalaeryPer = x.SalaeryPer,
        //        CategoryId = x.CategoryId,
        //        Phone1 = x.Phone1,
        //        Phone2 = x.Phone2,
        //        UserName = x.UserName,
        //        EmailUserName = x.EmailUserName,
        //        Password = x.Password,
        //        Address = x.Address,
        //        City = x.City,
        //        State = x.State,
        //        PostalcodeZIP = x.PostalcodeZIP,
        //        locationlatitude = x.locationlatitude,
        //        locationlongitude = x.locationlongitude,
        //        Country = x.Country,
        //        SSN = x.SSN,
        //        DriveLicense = x.DriveLicense,
        //        Picture = x.Picture,
        //        UserRole = x.UserRole,
        //        UserType = x.UserType,
        //        ExpireDate = x.ExpireDate,
        //        TheBranchs = x.TheBranchs,
        //        Employees = x.Employees,
        //        ActiveHome = x.ActiveHome,
        //        ActiveBranches = x.ActiveBranches,
        //        ActiveContract = x.ActiveContract,
        //        ActiveCustomers = x.ActiveCustomers,
        //        ActiveCustomersCategory = x.ActiveCustomersCategory,
        //        ActiveCustomersCustomField = x.ActiveCustomersCustomField,
        //        ActiveEmployee = x.ActiveEmployee,
        //        ActiveEmployeeCategory = x.ActiveEmployeeCategory,
        //        ActiveEmployeeCustomField = x.ActiveEmployeeCustomField,
        //        ActiveEstimate = x.ActiveEstimate,
        //        ActiveEstimateEmailTemplate = x.ActiveEstimateEmailTemplate,
        //        ActiveExpenses = x.ActiveExpenses,
        //        ActiveExpensesCategory = x.ActiveExpensesCategory,
        //        ActiveEquipments = x.ActiveEquipments,
        //        ActiveEquipmentsCustomField = x.ActiveEmployeeCustomField,
        //        ActiveItemsServices = x.ActiveItemsServices,
        //        ActiveItemsServicesCategory = x.ActiveItemsServicesCategory,
        //        ActiveItemsServicesCustomField = x.ActiveItemsServicesCustomField,
        //        ActiveMember = x.ActiveMember,
        //        ActiveTimeSheet = x.ActiveTimeSheet,
        //        ActiveInvoice = x.ActiveInvoice,
        //        ActiveInvoiceEmailTemplate = x.ActiveInvoiceEmailTemplate,
        //        ActiveMap = x.ActiveMap,
        //        ActiveNotes = x.ActiveNotes,
        //        ActiveNotificationSettings = x.ActiveNotificationSettings,
        //        ActivePayment = x.ActivePayment,
        //        ActiveReminderRules = x.ActiveReminderRules,
        //        ActiveRoute = x.ActiveRoute,
        //        ActiveSchedule = x.ActiveSchedule,
        //        ActiveSettings = x.ActiveSettings,
        //        ActiveTax = x.ActiveTax,
        //        ActiveReport = x.ActiveReport,
        //        ActiveStripeAccount = x.ActiveStripeAccount,
        //        ActiveMessage = x.ActiveMessage,
        //        ActiveItemsServicesSubCategory = x.ActiveItemsServicesSubCategory,
        //        ActiveAccount = x.ActiveAccount,
        //        Notes = x.Notes,
        //        Active = x.Active,
        //        CreateUser = x.CreateUser,
        //        CreateDate = x.CreateDate,
        //        ActiveEditPrice = x.ActiveEditPrice,
        //    }).ToList();

        //    var Empmodel = Employee.Where(x => x.Phone1 == Phone && x.Password == Password);

        //    return Empmodel.FirstOrDefault();
        //}

        public List<SelectListItem> GetPropertyEmployee()
        {
            WebCache.Remove(PROPERTY_Employee_CACHE_KEY);
            var result = WebCache.Get(PROPERTY_Employee_CACHE_KEY) as List<SelectListItem>;
            result = _db.Tbl_Employee.ToList().Where(u => u.Active == true).Select(x => new SelectListItem { Text = x.FirstName + " " + x.LastName, Value = x.Id.ToString() }).ToList();
            WebCache.Set(PROPERTY_Employee_CACHE_KEY, result);
            return result;
        }

        //public List<PropertyEmployeeBranchesDTO> GetPropertyEmployeesInBranch(int? BranchId)
        //{
        //    var Employee = _db.Tbl_EmployeeBranches.Where(x => x.BranchId == BranchId).Select(x => new PropertyEmployeeBranchesDTO
        //    {
        //        Id = x.Id,
        //        BranchId = x.BranchId,
        //        EmployeeId = x.EmployeeId,
        //    });

        //    return Employee.ToList();
        //}


        public List<PropertyBranchesDTO> GetBranchesOfEmployee(int? AccountId, int? EmpId)
        {
            List<int> LstBranch = _db.Tbl_EmployeeBranches.Where(x => x.AccountId == AccountId && x.EmployeeId == EmpId).Select(x => x.BrancheId.Value).ToList();

            List<PropertyBranchesDTO> listBranch = new List<PropertyBranchesDTO>();
            foreach (var branchId in LstBranch)
            {
                var Branch = _db.Tbl_Branches.Where(x => x.Id == branchId).Select(x => new PropertyBranchesDTO
                {
                    Id = x.Id,
                    AccountId = x.AccountId,
                    Name = x.Name,
                    Address = x.Address,
                    Phone = x.Phone,
                    Fax = x.Fax,
                    Email = x.Email,
                    DefaultBranches = x.DefaultBranches,
                    GoogleReviewLink = x.GoogleReviewLink,
                    Notes = x.Notes,
                    Logo = x.Logo,
                    Active = x.Active,
                    CreateUser = x.CreateUser,
                    CreateDate = x.CreateDate,
                }).FirstOrDefault();

                listBranch.Add(Branch);
            }

            return listBranch;
        }

        public PropertyStripeAccountDTO GetStripeAccountInBranch(int? AccountId, int? BranchId)
        {
            var Stripe = _db.Tbl_StripeAccount.Where(x => x.AccountId == AccountId && x.BrancheId == BranchId).Select(x => new PropertyStripeAccountDTO
            {
                Id = x.Id,
                Username = x.Username,
                SecretKey = x.SecretKey,
            });
            return Stripe.FirstOrDefault();
        }

    }
}