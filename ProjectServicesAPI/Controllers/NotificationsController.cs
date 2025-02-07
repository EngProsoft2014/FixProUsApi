using Newtonsoft.Json;
using FixProUsApi.DAL;
using FixProUsApi.DTO;
using FixProUsApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace FixProUsApi.Controllers
{
    [BasicAuthentication]
    [RoutePrefix("api/Notifications")]
    public class NotificationsController : ApiController
    {

        [Route("GetDevice")]
        public async Task<string> GetDevice(int AccountId, string PlayerId)
        {
            RepositoryNotificationsDAL ClsNotificationsDAL = new RepositoryNotificationsDAL();
            try
            {
                return await ClsNotificationsDAL.ViewDevice(AccountId, PlayerId);
            }
            finally
            {
                ClsNotificationsDAL.Dispose();
            }          
        }

        [Route("GetAccountKeys")]
        public async Task<PropertyAccountDTO> GetAccountKeys(int AccountId)
        {
            RepositoryNotificationsDAL ClsNotificationsDAL = new RepositoryNotificationsDAL();
            try
            {
                return await ClsNotificationsDAL.GetAccountKeys(AccountId);
            }
            finally
            {
                ClsNotificationsDAL.Dispose();
            }    
        }

        [Route("GetNotifications")]
        public List<PropertyOneSignalNotificationDTO> GetNotifications(int? EmployeeId)
        {      
            RepositoryNotificationsDAL ClsNotificationsDAL = new RepositoryNotificationsDAL();
            try
            {
                return ClsNotificationsDAL.GetOneSignalNotificationByEmp(EmployeeId);
            }
            finally
            {
                ClsNotificationsDAL.Dispose();
            }
        }

        [Route("PostNotification")]
        public HttpResponseMessage PostNotification([FromBody] object obj)
        {
            RepositoryNotificationsDAL ClsNotificationsDAL = new RepositoryNotificationsDAL();
            try
            {
                string MsgReturn = ClsNotificationsDAL.CreateNotification(obj);

                return Request.CreateResponse(HttpStatusCode.Created, MsgReturn);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsNotificationsDAL.Dispose();
            }
        }

        [Route("PostNotificationSpecific")]
        public HttpResponseMessage PostNotificationSpecific([FromBody] PropertyNotificationsSpecificDTO model)
        {
            RepositoryNotificationsDAL ClsNotificationsDAL = new RepositoryNotificationsDAL();
            try
            {
                ClsNotificationsDAL.SendSpecificNotifications(model);

                return Request.CreateResponse(HttpStatusCode.Created, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsNotificationsDAL.Dispose();
            }
        }

        [Route("PostDevice")]
        public async Task<HttpResponseMessage> PostDevice(int AccountId, int EmpId, [FromBody] PropertyDeviceDetailsDTO DeviceBody)
        {
            RepositoryNotificationsDAL ClsNotificationsDAL = new RepositoryNotificationsDAL();
            try
            {
                string MsgReturn = await ClsNotificationsDAL.AddDevice(AccountId, EmpId, DeviceBody);

                return Request.CreateResponse(HttpStatusCode.Created, MsgReturn);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsNotificationsDAL.Dispose();
            }
        }

        [Route("PostPlayerId")]
        public HttpResponseMessage PostPlayerId([FromBody] PropertyUpdatePlayerIdDTO model)
        {
            RepositoryNotificationsDAL ClsNotificationsDAL = new RepositoryNotificationsDAL();
            try
            {
                string MsgReturn =  ClsNotificationsDAL.UpdateEmpPlayerId(int.Parse(model.AccountId), model.PlayerId, int.Parse(model.UserId));

                return Request.CreateResponse(HttpStatusCode.Created, MsgReturn);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsNotificationsDAL.Dispose();
            }
        }

        [Route("PutDeactiveNotify")]
        public HttpResponseMessage PutDeactiveNotify(PropertyOneSignalNotificationDTO model)
        {
            RepositoryNotificationsDAL ClsNotificationsDAL = new RepositoryNotificationsDAL();
            try
            {
                bool active = ClsNotificationsDAL.DeactiveNotifyforEmp(model);
                model.Active = active;

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex); 
            }
            finally
            {
                ClsNotificationsDAL.Dispose();
            }
        }
    }
}
