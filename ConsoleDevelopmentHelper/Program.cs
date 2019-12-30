using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Payments.Services;
using Payments.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleDevelopmentHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = GetConfiguration();

            var bs = new BraintreeService(configuration);

            var creditCards = bs.GetPaymentMethods("2");
            var defaultCreditCard = bs.GetDefaultPaymentMethod("2");

            var result = bs.CreateTransaction("2", 20, 10);
            Console.WriteLine("Done! {0} {1}", result.IsSuccess(), result.Message);
        }

        private static IConfiguration GetConfiguration()
        {
            var configuration = new MockConfiguration();

            configuration["Braintree:MerchantId"] = "rw7z4nmpv6nkrncc";
            configuration["Braintree:PrivateKey"] = "b49e60782f6fc38ec1646995b1883fd8";
            configuration["Braintree:PublicKey"] = "8hm6h2dybjr89s7s";

            return configuration;
        }
    }
}
