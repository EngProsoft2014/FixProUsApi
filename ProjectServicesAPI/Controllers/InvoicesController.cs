using FixProUsApi.DAL;
using FixProUsApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FixProUsApi.Controllers
{
    [BasicAuthentication]
    [RoutePrefix("api/Invoices")]
    public class InvoicesController : ApiController
    {

        //[Route("GetAllPaymentsForInvoice")]
        //public IEnumerable<PropertyPaymentsDTO> GetAllPaymentsForInvoice(int? InvoiceId)
        //{
        //    return ClsInvoicesDAL.GetAllPaymentsForInvoice(InvoiceId);
        //}


        [Route("PostInvoice")]
        public HttpResponseMessage PostInvoice([FromBody] PropertyInvoiceDTO InvoiceModel)
        {
            RepositoryInvoicesDAL ClsInvoicesDAL = new RepositoryInvoicesDAL();
            try
            {
                int IdInv = ClsInvoicesDAL.InsertInvoice(InvoiceModel);

                return Request.CreateResponse(HttpStatusCode.Created, IdInv);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            finally
            {
                ClsInvoicesDAL.Dispose();
            }
        }

        //[Route("PostInvoiceItemService")]
        //public HttpResponseMessage PostInvoiceItemService([FromBody] PropertyInvoiceItemServicesDTO Item)
        //{
        //    try
        //    {
        //        ClsInvoicesDAL.PostInvoiceItemService(Item);

        //        return Request.CreateResponse(HttpStatusCode.Created, Item);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        //    }
        //}

        [Route("PutInvoice")]
        public HttpResponseMessage PutInvoice([FromBody] PropertyInvoiceDTO InvoiceModel)
        {
            RepositoryInvoicesDAL ClsInvoicesDAL = new RepositoryInvoicesDAL();
            try
            {
                ClsInvoicesDAL.UpdateInvoice(InvoiceModel);

                return Request.CreateResponse(HttpStatusCode.Created, InvoiceModel);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            finally
            {
                ClsInvoicesDAL.Dispose();
            }
        }

        //[Route("DeleteInvoiceItemService/{ItemId:int}")]
        //public HttpResponseMessage DeleteInvoiceItemService(int ItemId)
        //{
        //    try
        //    {
        //        ClsInvoicesDAL.DeleteInvoiceItemService(ItemId);

        //        return Request.CreateResponse(HttpStatusCode.OK, ItemId);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        //    }
        //}


        [Route("DeleteInvoice/{InvoiceId:int}")]
        public HttpResponseMessage DeleteInvoice(int InvoiceId)
        {
            RepositoryInvoicesDAL ClsInvoicesDAL = new RepositoryInvoicesDAL();
            try
            {
                ClsInvoicesDAL.RemoveInvoice(InvoiceId);

                return Request.CreateResponse(HttpStatusCode.OK, InvoiceId);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            finally
            {
                ClsInvoicesDAL.Dispose();
            }
        }


        [Route("PostInvoiceEmail")]
        public HttpResponseMessage PostInvoiceEmail([FromBody] PropertyInvoiceDTO InvoiceModel)
        {
            RepositoryInvoicesDAL ClsInvoicesDAL = new RepositoryInvoicesDAL();
            try
            {
                ClsInvoicesDAL.SendInvoiceMail(InvoiceModel.Id.ToString(), InvoiceModel.CustomerEmail); 
                return Request.CreateResponse(HttpStatusCode.Created, "Send Success");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            finally
            {
                ClsInvoicesDAL.Dispose();
            }
        }
    }
}
