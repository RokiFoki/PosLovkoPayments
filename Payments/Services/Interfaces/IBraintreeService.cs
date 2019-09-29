﻿using Braintree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.Services.Interfaces
{
    public interface IBraintreeService
    {
        object testFunction();
        Result<Customer> CreateCustomerIfDoesntExist(string customerId, string firstname, string lastname, string email);
        Result<PaymentMethod> CreatePaymentMethod(string customerId, string paymentMethodNonce);
        Customer GetCustomer(string customerId);
        Result<Customer> DeleteCustomer(string id);
        PaymentMethod GetDefaultPaymentMethod(string customerId);
        PaymentMethod[] GetPaymentMethods(string customerId);
        string GenerateClientToken(string customerId);
    }
}
