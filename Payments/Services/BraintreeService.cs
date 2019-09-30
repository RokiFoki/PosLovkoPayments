using Braintree;
using Braintree.Exceptions;
using Microsoft.Extensions.Configuration;
using Payments.Services.Interfaces;

namespace Payments.Services
{
    public class BraintreeService : IBraintreeService
    {
        private BraintreeGateway gateway;

        public object testFunction()
        {
            foreach(Customer customer in gateway.Customer.All())
            {
                gateway.Customer.Delete(customer.Id);
            }

            return gateway.Customer.All();
        }

        public BraintreeService(IConfiguration configuration)
        {
            gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = configuration["Braintree:MerchantId"],
                PrivateKey = configuration["Braintree:PrivateKey"],
                PublicKey = configuration["Braintree:PublicKey"],
            };
        }

        public Result<Customer> CreateCustomerIfDoesntExist(
            string customerId,
            string firstname,
            string lastname,
            string email)
        {
            return gateway.Customer.Create(new CustomerRequest
            {
                Id = customerId,
                FirstName = firstname,
                LastName = lastname,
                Email = email
            });
        }

        public Customer GetCustomer(string customerId)
        {
            return gateway.Customer.Find(customerId);
        }

        public Result<Customer> DeleteCustomer(string id)
        {
            return gateway.Customer.Delete(id);
        }

        public bool DeletePaymentMethod(string customerId, string token)
        {
            if (DoesCustomerHavePaymentMethod(customerId, token))
            {
                gateway.PaymentMethod.Delete(token);
                return true;
            }

            return false;
        }

        public Result<PaymentMethod> CreatePaymentMethod(string customerId, string paymentMethodNonce)
        {
            if (DoesCustomerExist(customerId))
            {
                return gateway.PaymentMethod.Create(new PaymentMethodRequest
                {
                    CustomerId = customerId,
                    PaymentMethodNonce = paymentMethodNonce
                });
            }

            return null;
        }

        public PaymentMethod GetDefaultPaymentMethod(string customerId)
        {
            return gateway.Customer.Find(customerId).DefaultPaymentMethod;
        }

        public PaymentMethod[] GetPaymentMethods(string customerId)
        {
            return gateway.Customer.Find(customerId).PaymentMethods;
        }

        public string GenerateClientToken(string customerId)
        {
            return gateway.ClientToken.Generate(new ClientTokenRequest
            {
                CustomerId = customerId,
            });
        }

        public bool PaymentMethodMakeDefault(string customerId, string token)
        {
            if (DoesCustomerHavePaymentMethod(customerId, token))
            {
                var updateRequest = new PaymentMethodRequest
                {
                    Options = new PaymentMethodOptionsRequest
                    {
                        MakeDefault = true
                    }
                };

                gateway.PaymentMethod.Update(token, updateRequest);

                return true;
            }

            return false;            
        }

        private bool DoesCustomerHavePaymentMethod(string customerId, string token)
        {
            try
            {
                var paymentMethod = gateway.PaymentMethod.Find(token);
                return paymentMethod.CustomerId == customerId;
            } catch (NotFoundException)
            {
                return true;
            }
        }
        
        private bool DoesCustomerExist(string customerId)
        {
            try
            {
                gateway.Customer.Find(customerId);
                return true;
            }
            catch (NotFoundException)
            {
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
