using Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using CustomerEntity = DataAccess.Data.Customer;

namespace API.Controllers.Customer
{
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly BaseService<CustomerEntity> CustomerService;
        private readonly ILogger Logger;

        public CustomerController(BaseService<CustomerEntity> customerService, ILogger<CustomerController> logger)
        {
            CustomerService = customerService;
            Logger = logger;
        }


        [HttpGet()]
        public ActionResult<IQueryable<CustomerEntity>> GetAll()
        {
            try
            {
                var customers = CustomerService.GetAll();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost()]
        public ActionResult<CustomerEntity> Create([FromBodyAttribute] CustomerEntity entity)
        {
            try
            {
                var customer = CustomerService.Create(entity);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut()]
        public ActionResult<CustomerEntity> Update(CustomerEntity entity)
        {
            try
            {
                var customer = CustomerService.Update(entity.CustomerId, entity, out bool changed);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete()]
        public ActionResult<CustomerEntity> Delete([FromBodyAttribute] CustomerEntity entity)
        {
            try
            {
                var customer = CustomerService.Delete(entity);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
