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
        public ServerResponse Get(string customerId)
        {
            try
            {
                return ServerResponse.Ok(braintreeService.GetPaymentMethods(customerId));
            } catch (NotFoundException)
            {
                return ServerResponse.CustomerDoesNotExist(null);
            }
        }

        [HttpGet("default")]
        public ServerResponse GetDefault(string customerId)
        {
            return ServerResponse.Ok(braintreeService.GetDefaultPaymentMethod(customerId));
        }

        [HttpDelete("{token}")]
        public ServerResponse Delete(string customerId, string token)
        {
            if (braintreeService.DeletePaymentMethod(customerId, token))
            {
                return ServerResponse.Ok("");
            }

            return ServerResponse.Error("");
        }
        
        [HttpPost]
        public ServerResponse Post(string customerId, PaymentMethodModel model)
        {
            return ServerResponse.Ok(braintreeService.CreatePaymentMethod(customerId, model.Nonce));
        }

        [HttpPut("{token}/makedefault")]
        public ServerResponse MakeDefault(string customerId, string token)
        {
            if (braintreeService.PaymentMethodMakeDefault(customerId, token))
            {
                return ServerResponse.Ok("");
            }

            return ServerResponse.Error("");
        }

        [HttpPost("maketransaction")]
        public ServerResponse MakeTransaction(string customerId, TransactionModel model)
        {
            var result = braintreeService.CreateTransaction(customerId, model.Amount, model.OrderId);

            if (result.IsSuccess())
            {
                return ServerResponse.Ok("");
            } 
            else
            {
                return ServerResponse.Error(result.Message);
            }
        }

        [HttpPost("{token}/maketransaction")]
        public ServerResponse MakeTransaction(string customerId, string token, TransactionModel model)
        {
            var result = braintreeService.CreateTransaction(customerId, model.Amount, model.OrderId, token);

            if (result.IsSuccess())
            {
                return ServerResponse.Ok("");
            }
            else
            {
                return ServerResponse.Error(result.Message);
            }
        }

        [ModelBinder(BinderType = typeof(DecryptBodyModelBinder<TransactionModel>))]
        public class TransactionModel
        {
            [Required]
            [JsonProperty("amount")]
            public decimal Amount { get; set; }
            [Required]
            [JsonProperty("orderId")]
            public string OrderId { get; set; }
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