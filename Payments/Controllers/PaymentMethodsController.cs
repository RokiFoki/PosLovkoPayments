using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Braintree;
using Braintree.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Payments.Helpers.ModelBinders;
using Payments.Responses;
using Payments.Services.Interfaces;

namespace Payments.Controllers
{
    [Authorize]
    [Route("api/customers/{customerId}/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IBraintreeService braintreeService;

        public PaymentMethodsController(IBraintreeService braintreeService)
        {
            this.braintreeService = braintreeService;
        }

        [HttpGet]
        public Response<CreditCard[]> Get(string customerId)
        {
            try
            {
                return Response<CreditCard[]>.Ok(braintreeService.GetPaymentMethods(customerId));
            } catch (NotFoundException)
            {
                return Response<CreditCard[]>.CustomerDoesNotExist(null);
            }
        }

        [HttpGet("default")]
        public Response<PaymentMethod> GetDefault(string customerId)
        {
            return Response<PaymentMethod>.Ok(braintreeService.GetDefaultPaymentMethod(customerId));
        }

        [HttpDelete("{token}")]
        public Response<string> Delete(string customerId, string token)
        {
            if (braintreeService.DeletePaymentMethod(customerId, token))
            {
                return Response<string>.Ok("");
            }

            return Response<string>.Error("");
        }
        
        [HttpPost]
        public Response<Result<PaymentMethod>> Post(string customerId, PaymentMethodModel model)
        {
            return Response<Result<PaymentMethod>>.Ok(braintreeService.CreatePaymentMethod(customerId, model.Nonce));
        }

        [HttpPut("{token}/makedefault")]
        public Response<string> MakeDefault(string customerId, string token)
        {
            if (braintreeService.PaymentMethodMakeDefault(customerId, token))
            {
                return Response<string>.Ok("");
            }

            return Response<string>.Error("");
        }

        [ModelBinder(BinderType = typeof(DecryptBodyModelBinder<PaymentMethodModel>))]
        public class PaymentMethodModel
        {
            [Required]
            [JsonProperty("nonce")]
            public string Nonce { get; set; }
        }

    }
}