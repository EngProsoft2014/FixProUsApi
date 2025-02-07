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
    [RoutePrefix("api/Plans")]
    public class PlansController : ApiController
    {
        [Route("GetPlans")]
        public List<PropertyPlansDTO> GetPlans()
        {
            RepositoryPlansDAL ClsPlansDAL = new RepositoryPlansDAL();
            try
            {
                return ClsPlansDAL.ToList();
            }
            finally
            {
                ClsPlansDAL.Dispose();
            }
        }
    }
}
