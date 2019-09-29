using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Braintree;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payments.Responses;
using Payments.Services.Interfaces;

namespace Payments.Controllers
{
    [Route("api/customers/{customerId}/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IBraintreeService braintreeService;

        public PaymentMethodsController(IBraintreeService braintreeService)
        {
            this.braintreeService = braintreeService;
        }

        public Response<PaymentMethod[]> Get(string customerId)
        {
            return Response<PaymentMethod[]>.Ok(braintreeService.GetPaymentMethods(customerId));
        }

        [HttpGet("default")]
        public Response<PaymentMethod> GetDefault(string customerId)
        {
            return Response<PaymentMethod>.Ok(braintreeService.GetDefaultPaymentMethod(customerId));
        }


        public class PaymentMethodModel
        {
            public string nonce { get; set; }
        }
        [HttpPost]
        public Response<Result<PaymentMethod>> Post(string customerId, PaymentMethodModel model)
        {
            return Response<Result<PaymentMethod>>.Ok(braintreeService.CreatePaymentMethod(customerId, model.nonce));
        }
    }
}