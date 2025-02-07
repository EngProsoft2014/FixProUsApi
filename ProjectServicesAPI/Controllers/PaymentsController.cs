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
    [RoutePrefix("api/Payments")]
    public class PaymentsController : ApiController
    {

        [Route("GetStripeAccount")]
        public PropertyStripeAccountDTO GetStripeAccount(int? AccountId ,int? BranchId)
        {
            RepositoryPaymentsDAL ClsPaymentsDAL = new RepositoryPaymentsDAL();       
            try
            {
                return ClsPaymentsDAL.GetStripeAccountInBranch(AccountId,BranchId);
            }
            finally
            {
                ClsPaymentsDAL.Dispose();
            }
        }

        [Route("InsertPayment")]
        public HttpResponseMessage InsertPayment([FromBody] PropertyPaymentsDTO PaymentModel)
        {
            RepositoryPaymentsDAL ClsPaymentsDAL = new RepositoryPaymentsDAL();
            try
            {
                ClsPaymentsDAL.InsertPayment(PaymentModel);

                return Request.CreateResponse(HttpStatusCode.Created, PaymentModel);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            finally
            {
                ClsPaymentsDAL.Dispose();
            }
        }
    }
}
