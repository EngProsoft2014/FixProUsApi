using FixProUsApi.DAL;
using FixProUsApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace FixProUsApi.Controllers
{
    [BasicAuthentication]
    [RoutePrefix("api/TimeSheet")]
    public class TimeSheetController : ApiController
    {

        [Route("GetCheckInOut")]
        public List<PropertyTimeSheetDTO> GetCheckInOut(string date, int? userId, string userRole)
        {
            RepositoryTimeSheetDAL ClsTimeSheetDAL = new RepositoryTimeSheetDAL();
            try
            {
                //string Date = "15-11-2022";
                return ClsTimeSheetDAL.AllCheckInOutdayByEmployees_DateOf(date, userId, userRole);
            }
            finally
            {
                ClsTimeSheetDAL.Dispose();
            }
        }

        // PUT: api/TimeSheet/5
        [Route("PutCheckInOut/{id:int}")]
        public HttpResponseMessage PutCheckInOut(int id, [FromBody] PropertyTimeSheetDTO model)
        {
            RepositoryTimeSheetDAL ClsTimeSheetDAL = new RepositoryTimeSheetDAL();
            try
            {
                if (id == model.EmployeeId)
                {
                    ClsTimeSheetDAL.UpdateCheckInOut(model);
                }

                return Request.CreateResponse(HttpStatusCode.Created, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsTimeSheetDAL.Dispose();
            }
        }

        // POST: api/TimeSheet
        [Route("PostCheckInOut")]
        public HttpResponseMessage PostCheckInOut([FromBody] PropertyTimeSheetDTO model)
        {
            RepositoryTimeSheetDAL ClsTimeSheetDAL = new RepositoryTimeSheetDAL();
            try
            {
                PropertyTimeSheetDTO TmSheetModel = ClsTimeSheetDAL.InsertCheckInOut(model);

                return Request.CreateResponse(HttpStatusCode.Created, TmSheetModel);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsTimeSheetDAL.Dispose();
            }
        }

    }
}
