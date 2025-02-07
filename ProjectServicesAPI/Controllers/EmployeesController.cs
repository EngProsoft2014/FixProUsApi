using FixProUsApi.DAL;
using FixProUsApi.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive.Concurrency;
using System.Web.Http;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace FixProUsApi.Controllers
{
    [BasicAuthentication]
    [RoutePrefix("api/Employee")]
    public class EmployeesController : ApiController
    {
        [Route("GetPermission")]
        public HttpResponseMessage GetPermission(string UserName, string Password)
        {
            RepositoryEmployeeDAL ClsEmployeeDAL = new RepositoryEmployeeDAL();
            try
            {
                var entity = ClsEmployeeDAL.CheckPassword(UserName, Password);

                return Request.CreateResponse(HttpStatusCode.OK, entity);   
            }
            finally
            {
                ClsEmployeeDAL.Dispose();
            }
        }

        [Route("GetEmployeesInAccountId")]
        public IEnumerable<PropertyEmployeeDTO> GetEmployeesInBranch(int? AccountId)
        {         
            RepositoryEmployeeDAL ClsEmployeeDAL = new RepositoryEmployeeDAL();
            try
            {
                return ClsEmployeeDAL.GetEmployeesInAccountId(AccountId);
            }
            finally
            {
                ClsEmployeeDAL.Dispose();
            }
        }

        [Route("GetEmpWorking")]
        public IEnumerable<PropertyEmployeeDTO> GetEmpWorking(int? AccountId, string ScheduleDate)
        {
            RepositoryEmployeeDAL ClsEmployeeDAL = new RepositoryEmployeeDAL();
            try
            {
                return ClsEmployeeDAL.ToListTracking(AccountId, ScheduleDate);
            }
            finally
            {
                ClsEmployeeDAL.Dispose();
            }
        }

        [Route("GetEmpCategory")]
        public IEnumerable<PropertyEmployeeCategoryDTO> GetEmpCategory(int? AccountId)
        {   
            RepositoryEmployeeDAL ClsEmployeeDAL = new RepositoryEmployeeDAL();
            try
            {
                return ClsEmployeeDAL.GetEmployeeCategory(AccountId);
            }
            finally
            {
                ClsEmployeeDAL.Dispose();
            }
        }

        [Route("GetEmployeePlayerId")]
        public string GetEmployeePlayerId(int? AccountId, int? EmpId)
        {      
            RepositoryEmployeeDAL ClsEmployeeDAL = new RepositoryEmployeeDAL();
            try
            {
                return ClsEmployeeDAL.GetEmpPlayerId(AccountId, EmpId);
            }
            finally
            {
                ClsEmployeeDAL.Dispose();
            }
        }

        [Route("GetEmpInOneCategory/{BranchId:int}/{CategoryId:int}/{AccountId:int}/{UserRole:int}/{UserId:int}")]
        public IEnumerable<PropertyEmployeeDTO> GetEmpInOneCategory(int? BranchId, int? CategoryId, int? AccountId, int? UserRole, int? UserId)
        {    
            RepositoryEmployeeDAL ClsEmployeeDAL = new RepositoryEmployeeDAL();
            try
            {
                return ClsEmployeeDAL.GetEmployeeInOneCategory(BranchId, CategoryId, AccountId, UserRole, UserId);
            }
            finally
            {
                ClsEmployeeDAL.Dispose();
            }
        }

        [Route("GetEmpInCall/{AccountId:int}/{UserRole:int}/{UserId:int}")]
        public IEnumerable<PropertyEmployeeDTO> GetEmpInCall(int? AccountId, int? UserRole, int? UserId)
        { 
            RepositoryEmployeeDAL ClsEmployeeDAL = new RepositoryEmployeeDAL();
            try
            {
                return ClsEmployeeDAL.GetEmployeeInCall(AccountId, UserRole, UserId);
            }
            finally
            {
                ClsEmployeeDAL.Dispose();
            }
        }

        [Route("GetEmpInPage/{page:int}/{AccountId:int}/{BranchId:int}/{UserRole:int}/{UserId:int}")]
        public DataResponseEmployees GetEmpInPage(int page, int? AccountId, int? BranchId, int? UserRole, int? UserId)
        {
            RepositoryEmployeeDAL ClsEmployeeDAL = new RepositoryEmployeeDAL();
            try
            {
                IEnumerable<PropertyEmployeeDTO> AllEmployees = ClsEmployeeDAL.ToList(AccountId, BranchId, UserRole, UserId);
                //if (AllEmployees == null)
                //    return NotFound();

                var pageResult = 10f;
                var pageCount = Math.Ceiling(AllEmployees.Count() / pageResult);

                IEnumerable<PropertyEmployeeDTO> Employees = AllEmployees
                    .Skip((page - 1) * (int)pageResult)
                    .Take((int)pageResult)
                    .ToList();

                var response = new DataResponseEmployees()
                {
                    EmployeesInPage = Employees,
                    CurrentPage = page,
                    Pages = (int)pageCount
                };

                return response;
            }
            finally
            {
                ClsEmployeeDAL.Dispose();
            }
           
        }


        [Route("GetEmpolyeeBranches")]
        public IEnumerable<PropertyBranchesDTO> GetEmpolyeeBranches(int? AccountId, int? EmpId)
        {
            RepositoryEmployeeDAL ClsEmployeeDAL = new RepositoryEmployeeDAL();
            try
            {
                return ClsEmployeeDAL.GetBranchesOfEmployee(AccountId, EmpId);
            }
            finally
            {
                ClsEmployeeDAL.Dispose();
            }
        }
    }
}
