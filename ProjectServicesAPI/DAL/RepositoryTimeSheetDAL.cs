using Antlr.Runtime.Misc;
using FixProUsApi.DTO;
using FixProUsApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace FixProUsApi.DAL
{
    public class RepositoryTimeSheetDAL
    {

        public readonly Entities _db;
        private bool disposed = false;
        public RepositoryTimeSheetDAL()
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


        public PropertyTimeSheetDTO InsertCheckInOut(PropertyTimeSheetDTO model)
        {
            var entity = _db.Tbl_TimeSheet.FirstOrDefault(x => x.EmployeeId == model.EmployeeId && x.Date == model.Date);
            if (entity == null)
            {
                entity = new Tbl_TimeSheet
                {
                    AccountId = model.AccountId,
                    BrancheId = model.BrancheId,
                    EmployeeId = model.EmployeeId,
                    Date = model.Date,
                    HoursFrom = model.HoursFrom,
                    HoursTo = model.HoursTo,
                    DurationHours = model.DurationHours,
                    DurationMinutes = model.DurationMinutes,
                    SheetColor = model.SheetColor,
                    Notes = model.Notes,
                    Active = model.Active,
                    CreateUser = model.CreateUser,
                    CreateDate = model.CreateDate,
                };

                _db.Tbl_TimeSheet.Add(entity);
                _db.SaveChanges();

                if(entity.Id != 0)
                {
                    model.Id = entity.Id;
                }

                return model;
            }
            else
            {
                throw new ArgumentException(message: $"The Check In Out Name already exists.");
            }
        }

        public void UpdateCheckInOut(PropertyTimeSheetDTO model)
        {
            var entityId = _db.Tbl_TimeSheet.FirstOrDefault(x => x.Id == model.Id);
            if (entityId != null)
            {
                entityId.Id = model.Id;
                entityId.AccountId = model.AccountId;
                entityId.BrancheId = model.BrancheId;
                entityId.EmployeeId = model.EmployeeId;
                entityId.Date = model.Date;
                entityId.HoursFrom = model.HoursFrom;
                entityId.HoursTo = model.HoursTo;
                entityId.DurationHours = model.DurationHours;
                entityId.DurationMinutes = model.DurationMinutes;
                entityId.SheetColor = model.SheetColor;
                entityId.Notes = model.Notes;
                entityId.Active = model.Active;
                entityId.CreateUser = model.CreateUser;
                entityId.CreateDate = model.CreateDate;

                _db.Entry(entityId).State = EntityState.Modified;
                _db.SaveChanges();
            }
            else
            {
                throw new ArgumentException(message: $"This CheckInOut  Is Deleted");
            }
        }

        public void DeleteCheckInOut(int Id)
        {
            var entity = _db.Tbl_TimeSheet.FirstOrDefault(x => x.Id == Id);
            if (entity != null)
            {
                _db.Tbl_TimeSheet.Remove(entity);
                _db.SaveChanges();
            }
            else
            {
                throw new ArgumentException(message: $"This CheckInOut Id " + Id + " Is Deleted");
            }
        }

        public List<PropertyTimeSheetDTO> AllCheckInOutdayByEmployees_DateOf(string DateOf, int? userId, string userRole)
        {
            List<PropertyTimeSheetDTO> CheckInOuts = new List<PropertyTimeSheetDTO>();

            var DateFormat = DateTime.Parse(DateOf).ToString("yyyy-MM-dd");

            if (userRole != "" && userRole != null & (userRole == "4" || userRole == "2" || userRole == "3"))
            {
                //var DateOf_ = DateTime.Parse(DateFormat);
                CheckInOuts = _db.Tbl_TimeSheet.Where(model => model.Date == DateFormat).Select(model => new PropertyTimeSheetDTO
                {
                    Id = model.Id,
                    EmployeeName = model.Tbl_Employee.FirstName + " " + model.Tbl_Employee.LastName,
                    BranchName = model.Tbl_Branches.Name,
                    AccountId = model.AccountId,
                    BrancheId = model.BrancheId,
                    EmployeeId = model.EmployeeId,
                    Date = model.Date,
                    HoursFrom = model.HoursFrom,
                    HoursTo = model.HoursTo,
                    DurationHours = model.DurationHours,
                    DurationMinutes = model.DurationMinutes,
                    SheetColor = model.SheetColor,
                    Notes = model.Notes,
                    Active = model.Active,
                    CreateUser = model.CreateUser,
                    CreateDate = model.CreateDate,
                }).ToList();

                string HoursFrom = _db.Tbl_TimeSheet.Where(c => c.Date == DateFormat && c.EmployeeId == userId && String.IsNullOrEmpty(c.HoursFrom) != true).Select(s => s.HoursFrom).FirstOrDefault();

                if (HoursFrom == null || HoursFrom == "")
                {
                    PropertyTimeSheetDTO myObj = _db.Tbl_Employee.Where(model => model.Id == userId).Select(model => new PropertyTimeSheetDTO
                    {
                        EmployeeName = model.FirstName + " " + model.LastName,
                        BranchName = _db.Tbl_Branches.Where(d => d.Id == _db.Tbl_EmployeeBranches.Where(f => f.EmployeeId == model.Id).Select(g => g.BrancheId).FirstOrDefault()).Select(s => s.Name).FirstOrDefault(),
                        AccountId = model.AccountId,
                        BrancheId = _db.Tbl_EmployeeBranches.Where(d => d.EmployeeId == model.Id).Select(s => s.BrancheId).FirstOrDefault(),
                        EmployeeId = model.Id,
                        Active = model.Active,
                        Notes = model.Notes,
                    }).FirstOrDefault();
                    if (myObj != null)
                        CheckInOuts.Add(myObj);
                }
                
            }
            //else if (userRole != "" && userRole != null & (userRole == "2" || userRole == "3"))
            //{
            //    string Emps = _db.Tbl_Employee.Where(x => x.Id == userId).Select(s => s.Employees).FirstOrDefault();
            //    List<int> EmpIds = Emps.Split(',').Select(t => int.Parse(t)).ToList();

            //    if (EmpIds != null)
            //    {
            //        foreach(int id in EmpIds)
            //        {
            //            PropertyTimeSheetDTO ObjCheckInOut = _db.Tbl_TimeSheet.Where(model => model.Date == DateFormat && model.EmployeeId == id).Select(model => new PropertyTimeSheetDTO
            //            {
            //                Id = model.Id,
            //                EmployeeName = model.Tbl_Employee.FirstName + " " + model.Tbl_Employee.LastName,
            //                BranchName = model.Tbl_Branches.Name,
            //                AccountId = model.AccountId,
            //                BrancheId = model.BrancheId,
            //                EmployeeId = model.EmployeeId,
            //                Date = model.Date,
            //                HoursFrom = model.HoursFrom,
            //                HoursTo = model.HoursTo,
            //                DurationHours = model.DurationHours,
            //                DurationMinutes = model.DurationMinutes,
            //                SheetColor = model.SheetColor,
            //                Notes = model.Notes,
            //                Active = model.Active,
            //                CreateUser = model.CreateUser,
            //                CreateDate = model.CreateDate,
            //            }).FirstOrDefault();
            //            if(ObjCheckInOut != null)
            //                CheckInOuts.Add(ObjCheckInOut);
            //        }
            //    }

            //    string HoursFrom = _db.Tbl_TimeSheet.Where(c => c.Date == DateFormat && c.EmployeeId == userId && String.IsNullOrEmpty(c.HoursFrom) != true).Select(s => s.HoursFrom).FirstOrDefault();

            //    if (HoursFrom == null || HoursFrom == "")
            //    {
            //        PropertyTimeSheetDTO myObj = _db.Tbl_Employee.Where(model => model.Id == userId).Select(model => new PropertyTimeSheetDTO
            //        {
            //            EmployeeName = model.FirstName + " " + model.LastName,
            //            BranchName = _db.Tbl_Branches.Where(d => d.Id == _db.Tbl_EmployeeBranches.Where(f => f.EmployeeId == model.Id).Select(g => g.BrancheId).FirstOrDefault()).Select(s => s.Name).FirstOrDefault(),
            //            AccountId = model.AccountId,
            //            BrancheId = _db.Tbl_EmployeeBranches.Where(d => d.EmployeeId == model.Id).Select(s => s.BrancheId).FirstOrDefault(),
            //            EmployeeId = model.Id,
            //            Active = model.Active,
            //            Notes = model.Notes,
            //        }).FirstOrDefault();
            //        if (myObj != null)
            //            CheckInOuts.Add(myObj);
            //    }
            //    else
            //    {
            //        PropertyTimeSheetDTO myObj2 = _db.Tbl_TimeSheet.Where(model => model.Date == DateFormat && model.EmployeeId == userId).Select(model => new PropertyTimeSheetDTO
            //        {
            //            Id = model.Id,
            //            EmployeeName = model.Tbl_Employee.FirstName + " " + model.Tbl_Employee.LastName,
            //            BranchName = model.Tbl_Branches.Name,
            //            AccountId = model.AccountId,
            //            BrancheId = model.BrancheId,
            //            EmployeeId = model.EmployeeId,
            //            Date = model.Date,
            //            HoursFrom = model.HoursFrom,
            //            HoursTo = model.HoursTo,
            //            DurationHours = model.DurationHours,
            //            DurationMinutes = model.DurationMinutes,
            //            SheetColor = model.SheetColor,
            //            Notes = model.Notes,
            //            Active = model.Active,
            //            CreateUser = model.CreateUser,
            //            CreateDate = model.CreateDate,
            //        }).FirstOrDefault();
            //        if (myObj2 != null)
            //            CheckInOuts.Add(myObj2);
            //    }
            //}
            else if (userRole != "" && userRole != null & userRole == "1")
            {

                string HoursFrom = _db.Tbl_TimeSheet.Where(c => c.Date == DateFormat && c.EmployeeId == userId && String.IsNullOrEmpty(c.HoursFrom) != true).Select(s => s.HoursFrom).FirstOrDefault();

                if (HoursFrom == null || HoursFrom == "")
                {
                    PropertyTimeSheetDTO myObj = _db.Tbl_Employee.Where(model => model.Id == userId).Select(model => new PropertyTimeSheetDTO
                    {
                        EmployeeName = model.FirstName + " " + model.LastName,
                        BranchName = _db.Tbl_Branches.Where(d => d.Id == _db.Tbl_EmployeeBranches.Where(f => f.EmployeeId == model.Id).Select(g => g.BrancheId).FirstOrDefault()).Select(s => s.Name).FirstOrDefault(),
                        AccountId = model.AccountId,
                        BrancheId = _db.Tbl_EmployeeBranches.Where(d => d.EmployeeId == model.Id).Select(s => s.BrancheId).FirstOrDefault(),
                        EmployeeId = model.Id,
                        Active = model.Active,
                        Notes = model.Notes,
                    }).FirstOrDefault();
                    if (myObj != null)
                        CheckInOuts.Add(myObj);
                }
                else
                {
                    PropertyTimeSheetDTO myObj2 = _db.Tbl_TimeSheet.Where(model => model.Date == DateFormat && model.EmployeeId == userId).Select(model => new PropertyTimeSheetDTO
                    {
                        Id = model.Id,
                        EmployeeName = model.Tbl_Employee.FirstName + " " + model.Tbl_Employee.LastName,
                        BranchName = model.Tbl_Branches.Name,
                        AccountId = model.AccountId,
                        BrancheId = model.BrancheId,
                        EmployeeId = model.EmployeeId,
                        Date = model.Date,
                        HoursFrom = model.HoursFrom,
                        HoursTo = model.HoursTo,
                        DurationHours = model.DurationHours,
                        DurationMinutes = model.DurationMinutes,
                        SheetColor = model.SheetColor,
                        Notes = model.Notes,
                        Active = model.Active,
                        CreateUser = model.CreateUser,
                        CreateDate = model.CreateDate,
                    }).FirstOrDefault();
                    if (myObj2 != null)
                        CheckInOuts.Add(myObj2);
                }

            }
            return CheckInOuts;
        }

        public List<PropertyTimeSheetDTO> FindCheckInOutById_EmployeeId(int? id, int? EmployeeId)
        {
            if (id != null)
            {
                var CheckInOuts = _db.Tbl_TimeSheet.Where(model => model.Id == id).Select(model => new PropertyTimeSheetDTO
                {
                    Id = model.Id,
                    AccountId = model.AccountId,
                    BrancheId = model.BrancheId,
                    EmployeeId = model.EmployeeId,
                    Date = model.Date,
                    HoursFrom = model.HoursFrom,
                    HoursTo = model.HoursTo,
                    DurationHours = model.DurationHours,
                    DurationMinutes = model.DurationMinutes,
                    SheetColor = model.SheetColor,
                    Notes = model.Notes,
                    Active = model.Active,
                    CreateUser = model.CreateUser,
                    CreateDate = model.CreateDate,
                });
                return CheckInOuts.ToList();
            }
            else
            {
                var CheckInOuts = _db.Tbl_TimeSheet.Where(model => model.EmployeeId == EmployeeId).Select(model => new PropertyTimeSheetDTO
                {
                    Id = model.Id,
                    AccountId = model.AccountId,
                    BrancheId = model.BrancheId,
                    EmployeeId = model.EmployeeId,
                    Date = model.Date,
                    HoursFrom = model.HoursFrom,
                    HoursTo = model.HoursTo,
                    DurationHours = model.DurationHours,
                    DurationMinutes = model.DurationMinutes,
                    SheetColor = model.SheetColor,
                    Notes = model.Notes,
                    Active = model.Active,
                    CreateUser = model.CreateUser,
                    CreateDate = model.CreateDate,
                });
                return CheckInOuts.ToList();
            }
        }

        public List<PropertyTimeSheetDTO> FindCheckInOutdayByEmployeeId_DateOf(int? EmployeeId, string DateOf)
        {
            //var DateOf_ = DateTime.Parse(DateOf);
            var DateFormat = string.Format(DateOf, ("MM-dd-yyyy"));
            var CheckInOuts = _db.Tbl_TimeSheet.Where(model => model.EmployeeId == EmployeeId && model.Date == DateFormat).Select(model => new PropertyTimeSheetDTO
            {
                Id = model.Id,
                AccountId = model.AccountId,
                BrancheId = model.BrancheId,
                EmployeeId = model.EmployeeId,
                Date = model.Date,
                HoursFrom = model.HoursFrom,
                HoursTo = model.HoursTo,
                DurationHours = model.DurationHours,
                DurationMinutes = model.DurationMinutes,
                SheetColor = model.SheetColor,
                Notes = model.Notes,
                Active = model.Active,
                CreateUser = model.CreateUser,
                CreateDate = model.CreateDate,
            }).ToList();
            return CheckInOuts;
        }

        public List<PropertyTimeSheetDTO> FindCheckInOutMonthByEmployeeId_DateFrom_DateTo(int? EmployeeId, string DateFrom, string DateTo)
        {
            var DateFrom_ = DateTime.Parse(DateFrom);
            var DateTo_ = DateTime.Parse(DateTo);
            //var DateFrom_ = string.Format(DateFrom, ("MM-dd-yyyy"));
            //var DateTo_ = string.Format(DateTo, ("MM-dd-yyyy"));
            if (EmployeeId == null)
            {
                var CheckInOuts = _db.Tbl_TimeSheet.Where(model => DateTime.Parse(model.Date) >= DateFrom_ && DateTime.Parse(model.Date) <= DateTo_).Select(model => new PropertyTimeSheetDTO
                {
                    Id = model.Id,
                    AccountId = model.AccountId,
                    BrancheId = model.BrancheId,
                    EmployeeId = model.EmployeeId,
                    Date = model.Date,
                    HoursFrom = model.HoursFrom,
                    HoursTo = model.HoursTo,
                    DurationHours = model.DurationHours,
                    DurationMinutes = model.DurationMinutes,
                    SheetColor = model.SheetColor,
                    Notes = model.Notes,
                    Active = model.Active,
                    CreateUser = model.CreateUser,
                    CreateDate = model.CreateDate,
                });
                return CheckInOuts.ToList();
            }
            else
            {
                var CheckInOuts = _db.Tbl_TimeSheet.Where(model => model.EmployeeId == EmployeeId && DateTime.Parse(model.Date) >= DateFrom_ && DateTime.Parse(model.Date) <= DateTo_).Select(model => new PropertyTimeSheetDTO
                {
                    Id = model.Id,
                    AccountId = model.AccountId,
                    BrancheId = model.BrancheId,
                    EmployeeId = model.EmployeeId,
                    Date = model.Date,
                    HoursFrom = model.HoursFrom,
                    HoursTo = model.HoursTo,
                    DurationHours = model.DurationHours,
                    DurationMinutes = model.DurationMinutes,
                    SheetColor = model.SheetColor,
                    Notes = model.Notes,
                    Active = model.Active,
                    CreateUser = model.CreateUser,
                    CreateDate = model.CreateDate,
                });
                return CheckInOuts.ToList();
            }

        }

       
    }
}