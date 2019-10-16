﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Braintree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Payments.Responses;
using Payments.Services.Interfaces;

namespace Payments.Controllers
{
    [Authorize]
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
            return Response<Result<Customer>>.Ok(braintreeService.CreateCustomerIfDoesntExist(customer.CustomerId, customer.FirstName, customer.LastName, customer.Email));
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
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }

        [JsonProperty("firstname")]
        public string FirstName { get; set; }

        [JsonProperty("lastname")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
