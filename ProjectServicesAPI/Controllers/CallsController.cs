using FluentAssertions.Execution;
using FixProUsApi.DAL;
using FixProUsApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive.Concurrency;
using System.Web.Http;

namespace FixProUsApi.Controllers
{
    [BasicAuthentication]
    [RoutePrefix("api/Calls")]
    public class CallsController : ApiController
    {

        [Route("GetAllCalls")]
        public IEnumerable<PropertyCallsDTO> GetAllCalls(int? AccountId)
        {
            RepositoryCallsDAL ClsCallsDAL = new RepositoryCallsDAL();
            try
            {
                return ClsCallsDAL.GetAllCalls(AccountId);
            }
            finally
            {
                ClsCallsDAL.Dispose();
            }
        }

        [Route("GetFilterCalls")]
        public IEnumerable<PropertyCallsDTO> GetFilterCalls(string StartDate, string EndDate, string PhoneNum, string ReasonId, string CampaignId, string EmployeeId, string SchTitle)
        {
            RepositoryCallsDAL ClsCallsDAL = new RepositoryCallsDAL();
            try
            {
                return ClsCallsDAL.GetFilterCalls(StartDate, EndDate, PhoneNum, ReasonId, CampaignId, EmployeeId, SchTitle);
            }
            finally
            {
                ClsCallsDAL.Dispose();
            } 
        }

        [Route("GetReasons")]
        public IEnumerable<PropertyReasonDTO> GetReasons(int? AccountId)
        {       
            RepositoryCallsDAL ClsCallsDAL = new RepositoryCallsDAL();
            try
            {
                return ClsCallsDAL.GetReasons(AccountId);
            }
            finally
            {
                ClsCallsDAL.Dispose();
            }
        }

        [Route("GetCampaigns")]
        public IEnumerable<PropertyCampaignDTO> GetCampaigns(int? AccountId)
        {            
            RepositoryCallsDAL ClsCallsDAL = new RepositoryCallsDAL();
            try
            {
                return ClsCallsDAL.GetCampaigns(AccountId);
            }
            finally
            {
                ClsCallsDAL.Dispose();
            }
        }

        [Route("PostCall")]
        public HttpResponseMessage PostCall([FromBody] PropertyCallsDTO CallModel)
        {
            RepositoryCallsDAL ClsCallsDAL = new RepositoryCallsDAL();
            try
            {
                var Call = ClsCallsDAL.InsertCall(CallModel);
                CallModel = Call;

                return Request.CreateResponse(HttpStatusCode.Created, CallModel);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsCallsDAL.Dispose();
            }
        }

        [Route("PutCall")]
        public HttpResponseMessage PutCall([FromBody] PropertyCallsDTO CallModel)
        {
            RepositoryCallsDAL ClsCallsDAL = new RepositoryCallsDAL();
            try
            {
                var Call = ClsCallsDAL.UpdateCall(CallModel);
                CallModel = Call;

                return Request.CreateResponse(HttpStatusCode.Created, CallModel);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsCallsDAL.Dispose();
            }
        }

        [Route("DeleteCall/{CallId:int}")]
        public HttpResponseMessage DeleteCall(int CallId)
        {
            RepositoryCallsDAL ClsCallsDAL = new RepositoryCallsDAL();
            try
            {
                ClsCallsDAL.RemoveCall(CallId);

                return Request.CreateResponse(HttpStatusCode.OK, CallId);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsCallsDAL.Dispose();
            }
        }
    }
}
