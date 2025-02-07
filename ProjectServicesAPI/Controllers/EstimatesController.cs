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
    [RoutePrefix("api/Estimates")]
    public class EstimatesController : ApiController
    {

        [Route("PostEstimate")]
        public HttpResponseMessage PostEstimate([FromBody] PropertyEstimateDTO EstimateModel)
        {
            RepositoryEstimatesDAL ClsEstimatesDAL = new RepositoryEstimatesDAL();
            try
            {
                ClsEstimatesDAL.InsertEstimate(EstimateModel);

                return Request.CreateResponse(HttpStatusCode.Created, EstimateModel);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            finally
            {
                ClsEstimatesDAL.Dispose();
            }
        }

        //[Route("PostEstimateItemService")]
        //public HttpResponseMessage PostEstimateItemService([FromBody] PropertyEstimateItemServicesDTO Item)
        //{
        //    try
        //    {
        //        ClsEstimatesDAL.PostEstimateItemService(Item);

        //        return Request.CreateResponse(HttpStatusCode.Created, Item);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        //    }
        //}

        [Route("PutEstimate")]
        public HttpResponseMessage PutEstimate([FromBody] PropertyEstimateDTO EstimateModel)
        {
            RepositoryEstimatesDAL ClsEstimatesDAL = new RepositoryEstimatesDAL();
            try
            {
                ClsEstimatesDAL.UpdateEstimate(EstimateModel);

                return Request.CreateResponse(HttpStatusCode.Created, EstimateModel);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            finally
            {
                ClsEstimatesDAL.Dispose();
            }
        }


        [Route("PostEstimateEmail")]
        public HttpResponseMessage PostEstimateEmail([FromBody] PropertyEstimateDTO EstimateModel)
        {
            RepositoryEstimatesDAL ClsEstimatesDAL = new RepositoryEstimatesDAL();
            try
            {
                ClsEstimatesDAL.SendEstimateMail(EstimateModel.Id.ToString(), EstimateModel.CustomerEmail);
                return Request.CreateResponse(HttpStatusCode.Created, "Send Success");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            finally
            {
                ClsEstimatesDAL.Dispose();
            }
        }

        [Route("DeleteEstimate/{EstId:int}")]
        public HttpResponseMessage DeleteEstimate(int EstId)
        {
            RepositoryEstimatesDAL ClsEstimatesDAL = new RepositoryEstimatesDAL();
            try
            {
                ClsEstimatesDAL.DeleteEstimate(EstId);

                return Request.CreateResponse(HttpStatusCode.OK, EstId);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //[Route("DeleteEstimateItemService/{ItemId:int}")]
        //public HttpResponseMessage DeleteEstimateItemService(int ItemId)
        //{
        //    try
        //    {
        //        ClsEstimatesDAL.DeleteEstimateItemService(ItemId);

        //        return Request.CreateResponse(HttpStatusCode.OK, ItemId);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        //    }
        //}
    }
}
