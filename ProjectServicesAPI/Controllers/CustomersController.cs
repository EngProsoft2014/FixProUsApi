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
    [RoutePrefix("api/Customers")]
    public class CustomersController : ApiController
    {

        [Route("GetAllCustInBranch")]
        public IEnumerable<PropertyCustomersDTO> GetAllCustInBranch(int? AccountId)
        {           
            RepositoryCustomersDAL ClsCustomersDAL = new RepositoryCustomersDAL();
            try
            {
                return ClsCustomersDAL.GetAllCustInBranch(AccountId);
            }
            finally
            {
                ClsCustomersDAL.Dispose();
            }
        }

        [Route("GetAllCustInCall")]
        public IEnumerable<PropertyCustomersDTO> GetAllCustInCall(int? AccountId)
        {    
            RepositoryCustomersDAL ClsCustomersDAL = new RepositoryCustomersDAL();
            try
            {
                return ClsCustomersDAL.GetAllCustInCall(AccountId);
            }
            finally
            {
                ClsCustomersDAL.Dispose();
            }
        }

        [Route("GetAllCustSuppliers")]
        public IEnumerable<PropertyCustomersDTO> GetAllCustSuppliers(int? AccountId)
        { 
            RepositoryCustomersDAL ClsCustomersDAL = new RepositoryCustomersDAL();
            try
            {
                return ClsCustomersDAL.GetAllCustSuppliers(AccountId);
            }
            finally
            {
                ClsCustomersDAL.Dispose();
            }
        }

        [Route("GetOneCustDetails")]
        public PropertyCustomersDTO GetOneCustDetails(int? CustId)
        {   
            RepositoryCustomersDAL ClsCustomersDAL = new RepositoryCustomersDAL();
            try
            {
                return ClsCustomersDAL.GetOneCustDetails(CustId);
            }
            finally
            {
                ClsCustomersDAL.Dispose();
            }
        }

        [Route("GetListsOfCustomer")]
        public PropertyCustomersDTO GetListsOfCustomer(int? CustomerId)
        {            
            RepositoryCustomersDAL ClsCustomersDAL = new RepositoryCustomersDAL();
            try
            {
                return ClsCustomersDAL.GetListsOfCustomer(CustomerId);
            }
            finally
            {
                ClsCustomersDAL.Dispose();
            }
        }


        [Route("GetObjectOfCustomer")]
        public PropertyObjectCustomerDTO GetObjectOfCustomer(int? InvoiceId, int? EstimateId)
        {     
            RepositoryCustomersDAL ClsCustomersDAL = new RepositoryCustomersDAL();
            try
            {
                return ClsCustomersDAL.GetObjectOfCustomerDetails(InvoiceId, EstimateId);
            }
            finally
            {
                ClsCustomersDAL.Dispose();
            }
        }

        [Route("GetCustomerFeatures")]
        public PropertyCustomerFeaturesDTO GetCustomerFeatures(int? AccountId)
        {   
            RepositoryCustomersDAL ClsCustomersDAL = new RepositoryCustomersDAL();
            try
            {
                return ClsCustomersDAL.GetAllCustomerFeatures(AccountId);
            }
            finally
            {
                ClsCustomersDAL.Dispose();
            }
        }

        [Route("PostCustomer")]
        public HttpResponseMessage PostCustomer([FromBody] PropertyCustomersDTO CustomerModel)
        {
            RepositoryCustomersDAL ClsCustomersDAL = new RepositoryCustomersDAL();
            try
            {
                var Custmodel = ClsCustomersDAL.InsertCustomer(CustomerModel);

                return Request.CreateResponse(HttpStatusCode.Created, Custmodel);
            }
            catch (Exception ex)
            {
                if(ex.Message == "The Customer already exists.")
                {
                    return Request.CreateErrorResponse(HttpStatusCode.MultipleChoices, ex);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            finally
            {
                ClsCustomersDAL.Dispose();
            }
        }

        [Route("PutCustomerAddress")]
        public HttpResponseMessage PutCustomerAddress(PropertyCustomersDTO model)
        {
            RepositoryCustomersDAL ClsCustomersDAL = new RepositoryCustomersDAL();
            try
            {
                string MsgReturn = ClsCustomersDAL.UpdateCustomers(model);

                return Request.CreateResponse(HttpStatusCode.Created, MsgReturn);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            finally
            {
                ClsCustomersDAL.Dispose();
            }
        }

        [Route("PutCustomer")]
        public HttpResponseMessage PutCustomer(PropertyCustomersDTO model)
        {
            RepositoryCustomersDAL ClsCustomersDAL = new RepositoryCustomersDAL();
            try
            {
                ClsCustomersDAL.EditCustomer(model);

                return Request.CreateResponse(HttpStatusCode.Created, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            finally
            {
                ClsCustomersDAL.Dispose();
            }
        }

    }
}
