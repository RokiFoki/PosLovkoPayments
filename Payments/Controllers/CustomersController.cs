using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Braintree;
using Microsoft.AspNetCore.Mvc;
using Payments.Responses;
using Payments.Services.Interfaces;

namespace Payments.Controllers
{
    [Route("api/[controller]", Name = "CustomersRoute")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IBraintreeService braintreeService;

        public CustomersController(IBraintreeService braintreeService)
        {
            this.braintreeService = braintreeService;
        }

        [HttpGet("{id}")]
        public Response<Customer> Get(string id)
        {
            return Response<Customer>.Ok(braintreeService.GetCustomer(id));
        }

        // POST api/customers
        [HttpPost]
        public Response<Result<Customer>> Post([FromBody] CustomerModel customer)
        {
            return Response<Result<Customer>>.Ok(braintreeService.CreateCustomerIfDoesntExist(customer.customerId, customer.firstname, customer.lastname, customer.email));
        }
        
        // DELETE api/customers/5
        [HttpDelete("{id}")]
        public Response<Result<Customer>> Delete(string id)
        {
            return Response<Result<Customer>>.Ok(braintreeService.DeleteCustomer(id));
        }
    }
    public class CustomerModel
    {
        public string customerId { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
    }
}
