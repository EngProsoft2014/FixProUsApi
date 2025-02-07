
using FixProUsApi.DAL;
using FixProUsApi.DTO;
using FixProUsApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace FixProUsApi.Controllers
{
    [BasicAuthentication]
    [RoutePrefix("api/Schedules")]
    public class SchedulesController : ApiController
    {

        [Route("GetSchedules")]
        public IEnumerable<PropertySchedulesDTO> GetSchedules(int? AccountId, int EmpId, int EmpRole, string lstEmp, string TextSearch)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                return ClsSchedulesDAL.GetAllScheduleInBranch(AccountId, EmpId, EmpRole, lstEmp, TextSearch);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }

        [Route("GetScheduleDetails")]
        public PropertySchedulesDTO GetScheduleDetails(int? ScheduleId, int ScheduleDateId)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                return ClsSchedulesDAL.GetOneScheduleDetails(ScheduleId, ScheduleDateId);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }

        //[Route("GetTestNotify")]
        //public void GetTestNotify()
        //{
        //    string Notify = $"Customer Name : Ahmed Mohamed \n " +
        //                          $"Address : 15 elsalam street \n" +
        //                          $"Data : 08/03/2023  09:15 AM";

        //    //NotificationsService.SendNotifyAsync(Notify);
        //}

        [Route("GetContractDetails")]
        public PropertySchedulesDTO GetContractDetails(int? ScheduleId, int? ContractId, string SchDate)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                return ClsSchedulesDAL.GetOneContractDetails(ScheduleId, ContractId, SchDate);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }

        [Route("GetScheduleDates")]
        public List<PropertyScheduleDateDTO> GetScheduleDates(int? ScheduleId, int Type)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                return ClsSchedulesDAL.GetScheduleDate(ScheduleId,Type);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }

        [Route("GetItemsInventory")]
        public IEnumerable<PropertyItemsServicesDTO> GetItemsInventory(int? AccountId)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL(); 
            try
            {
                return ClsSchedulesDAL.GetAllItemsInventory(AccountId);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
            
        }

        [Route("GetAllItemsServices")]
        public IEnumerable<PropertyItemsServicesDTO> GetAllItemsServices(int? AccountId)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                return ClsSchedulesDAL.GetAllItemsServices(AccountId);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }
        

        [Route("GetServices")]
        public IEnumerable<PropertyItemsServicesDTO> GetServices(int? AccountId)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                return ClsSchedulesDAL.GetAllServices(AccountId);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }

        [Route("GetItemsServicesCategories")]
        public IEnumerable<PropertyItemsServicesCategoryDTO> GetItemsServicesCategories(int? AccountId)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                return ClsSchedulesDAL.GetAllItemsServicesCategories(AccountId);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }

        [Route("GetItemsServicesSubCategories")]
        public IEnumerable<PropertyItemsServicesSubCategoryDTO> GetItemsServicesSubCategories(int? AccountId)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                return ClsSchedulesDAL.GetAllItemsServicesSubCategories(AccountId);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }

        [Route("GetServiceCost")]
        public PropertyItemsServicesDTO GetServiceCost(int? AccountId, int? ServiceId)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                return ClsSchedulesDAL.GetServiceDetails(AccountId, ServiceId);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }

        [Route("GetPictures")]
        public IEnumerable<PropertySchedulePicturesDTO> GetPictures(int? ScheduleId)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                return ClsSchedulesDAL.GetAllSchedulePictures(ScheduleId);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }

        [Route("PostPictures")]
        public HttpResponseMessage PostPictures([FromBody] List<PropertySchedulePicturesDTO> LstPictures)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                ClsSchedulesDAL.InsertSchedulePictures(LstPictures);

                return Request.CreateResponse(HttpStatusCode.Created, LstPictures);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }


        [Route("PutPictures")]
        public HttpResponseMessage PutPictures([FromBody] List<PropertySchedulePicturesDTO> LstPictures)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                ClsSchedulesDAL.UpdateSchedulePictures(LstPictures);

                return Request.CreateResponse(HttpStatusCode.Created, LstPictures);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }

        [Route("PutOutPicture")]
        public HttpResponseMessage PutOutPicture([FromBody] PropertySchedulePicturesDTO Picture)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                ClsSchedulesDAL.UpdateScheduleOutPicture(Picture);

                return Request.CreateResponse(HttpStatusCode.Created, Picture.Active);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }


        [Route("PostSchedule")]
        public HttpResponseMessage PostSchedule([FromBody] PropertySchedulesDTO ScheduleModel)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                ClsSchedulesDAL.InsertSchedule(ScheduleModel);

                return Request.CreateResponse(HttpStatusCode.Created, ScheduleModel);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }

        [Route("PostAddItemService")]
        public HttpResponseMessage PostAddItemService([FromBody] PropertyItemsServicesDTO ItemService)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                ClsSchedulesDAL.InsertCreateItemService(ItemService);

                return Request.CreateResponse(HttpStatusCode.Created, ItemService);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }

        [Route("PostAddScheduleDate")]
        public HttpResponseMessage PostAddScheduleDate([FromBody] PropertySchedulesDTO ScheduleModel)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                ClsSchedulesDAL.AddScheduleDate(ScheduleModel);

                return Request.CreateResponse(HttpStatusCode.Created, ScheduleModel);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }


        [Route("PostScheduleMaterials")]
        public HttpResponseMessage PostScheduleMaterials([FromBody] PropertyScheduleItemsServicesDTO Model)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                ClsSchedulesDAL.InsertScheduleMaterials(Model);

                return Request.CreateResponse(HttpStatusCode.Created, Model);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }

        [Route("PostScheduleFreeServices")]
        public HttpResponseMessage PostScheduleFreeServices([FromBody] PropertyScheduleItemsServicesDTO Model)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                ClsSchedulesDAL.InsertScheduleFreeServices(Model);

                return Request.CreateResponse(HttpStatusCode.Created, Model);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }


        [Route("PostScheduleMaterialReceipt")]
        public HttpResponseMessage PostScheduleMaterialReceipt([FromBody] PropertyScheduleMaterialReceiptDTO Model)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                ClsSchedulesDAL.InsertPostScheduleMaterialReceipt(Model);

                return Request.CreateResponse(HttpStatusCode.Created, Model);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }

        //[Route("PostScheduleItemService")]
        //public HttpResponseMessage PostScheduleItemService([FromBody] PropertyScheduleItemsServicesDTO Item)
        //{
        //    try
        //    {
        //        ClsSchedulesDAL.PostScheduleItemService(Item);

        //        return Request.CreateResponse(HttpStatusCode.Created, Item);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        //    }
        //}


        [Route("PutScheduleDate")]
        public HttpResponseMessage PutScheduleDate([FromBody] PropertyScheduleDateDTO Model)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                string Msg = ClsSchedulesDAL.UpdateScheduleDate(Model);

                if(Msg != "Not Done All Employee")
                {
                    return Request.CreateResponse(HttpStatusCode.Created, Model);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Created, Msg);
                }      
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }

        [Route("PutScheduleEmployees")]
        public HttpResponseMessage PutScheduleEmployees([FromBody] PropertyScheduleDateDTO Model)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                ClsSchedulesDAL.UpdateScheduleEmployees(Model);

                return Request.CreateResponse(HttpStatusCode.Created, Model);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }


       [Route("PutSchedule")]
        public HttpResponseMessage PutSchedule([FromBody] PropertySchedulesDTO ScheduleModel)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                ClsSchedulesDAL.UpdateSchedule(ScheduleModel);

                //PostSendNotification("You have new schedule is" + ScheduleModel.Title + "on date is " + ScheduleModel.StartDate);

                return Request.CreateResponse(HttpStatusCode.Created, ScheduleModel);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }

        [Route("PutMaterial")] // Delete Material
        public HttpResponseMessage PutMaterial(PropertyScheduleItemsServicesDTO model)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                ClsSchedulesDAL.DeleteScheduleMaterial(model);

                return Request.CreateResponse(HttpStatusCode.Created, model);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }

        [Route("PutFreeService")] // Delete Material
        public HttpResponseMessage PutFreeService(PropertyScheduleItemsServicesDTO model)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                ClsSchedulesDAL.DeleteScheduleFreeService(model);

                return Request.CreateResponse(HttpStatusCode.Created, model);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }


        [Route("PutMaterialReceipt")] // Delete Material Receipt
        public HttpResponseMessage PutMaterialReceipt(PropertyScheduleMaterialReceiptDTO model)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                ClsSchedulesDAL.DeleteMaterialReceipt(model);

                return Request.CreateResponse(HttpStatusCode.Created, model);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }


        [Route("PutScheduleDispatch")]
        public HttpResponseMessage PutScheduleDispatch([FromBody] PropertyScheduleDateDTO ScheduleModel)
        {
            RepositorySchedulesDAL ClsSchedulesDAL = new RepositorySchedulesDAL();
            try
            {
                ClsSchedulesDAL.SelectDispatch(ScheduleModel);

                return Request.CreateResponse(HttpStatusCode.Created, "Success Dispatch");
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            finally
            {
                ClsSchedulesDAL.Dispose();
            }
        }

        //private readonly ProjectServicesDBEntities1 _db = new ProjectServicesDBEntities1();

        //[HttpPost]
        //public HttpResponseMessage SendNotification(NotifModels obj)
        //{
        //    NotificationHub objNotifHub = new NotificationHub();
        //    Notification objNotif = new Notification();
        //    objNotif.SentTo = obj.UserID;

        //    _db.Configuration.ProxyCreationEnabled = false;
        //    _db.Notifications.Add(objNotif);
        //    _db.SaveChanges();

        //    objNotifHub.SendNotification(objNotif.SentTo);

        //    //var query = (from t in context.Notifications  
        //    //             select t).ToList();  

        //    return Request.CreateResponse(HttpStatusCode.OK);
        //}

        //private readonly IHubContext<NotificationHub> _hubContext;

        //public SchedulesController(IHubContext<NotificationHub> hubContext)
        //{
        //    _hubContext = hubContext;
        //}

        //[HttpPost]
        //public HttpResponseMessage PostSendNotification([FromBody] string message)
        //{
        //    //_hubContext.Clients.All.SendCoreAsync("ReceiveNotification", message);
        //    //_hubContext.Clients.All.SendCoreAsync("ReceiveNotification", new[] { message });
        //    _hubContext.Clients.All.SendMessage("ReceiveNotification", message);
        //    return Request.CreateResponse(HttpStatusCode.OK);
        //}

        //[HttpPost]
        //public IActionResult SendNotifications([FromBody] string message)
        //{
        //    _hubContext.Clients.All.SendAsync("ReceiveNotification", message);

        //    return (IActionResult)Ok();
        //}


        //        ClsSchedulesDAL.DeleteScheduleItemService(ItemId);

        //        return Request.CreateResponse(HttpStatusCode.OK, ItemId);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        //    }
        //}
    }
}
