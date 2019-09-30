using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Payments.Services;
using Payments.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace ConsoleDevelopmentHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new MockConfiguration();
            configuration["Braintree:MerchantId"] = "rw7z4nmpv6nkrncc";
            configuration["Braintree:PrivateKey"] = "b49e60782f6fc38ec1646995b1883fd8";
            configuration["Braintree:PublicKey"] = "8hm6h2dybjr89s7s";

            BraintreeService service = new BraintreeService(configuration);
            var customer = service.GetCustomer("2");
            Console.Write(customer);
        }
    }
}
