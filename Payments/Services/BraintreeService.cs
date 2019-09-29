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

        public Result<PaymentMethod> CreatePaymentMethod(string customerId, string paymentMethodNonce)
        {
            if (DoesCustomerExist(customerId))
            {
                var result = gateway.PaymentMethod.Create(new PaymentMethodRequest
                {
                    CustomerId = customerId,
                    PaymentMethodNonce = paymentMethodNonce
                });

                return result;
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
