using eCommerceService.Entities;
using eCommerceService.Exceptions;

namespace eCommerceService.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly ICustomersRepository _customersRepository;
        public CustomersService(ICustomersRepository customersRepository)
        {
            _customersRepository = customersRepository;
        }

        public async Task<Customer> AddCustomerAsync(string firstName, string lastName, string email, string phone, string streetAddress, string postalCode, string city, string country)
        {
            var customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                StreetAddress = streetAddress,
                PostalCode = postalCode,
                City = city,
                Country = country,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            await _customersRepository.AddCustomerAsync(customer);
            return customer;
        }

        public async Task<Customer> UpdateCustomerAsync(int customerId, string firstName, string lastName, string email, string phone, string streetAddress, string postalCode, string city, string country)
        {
            var customer = await GetCustomerAsync(customerId);

            customer.FirstName = firstName;
            customer.LastName = lastName;
            customer.Email = email;
            customer.Phone = phone;
            customer.StreetAddress = streetAddress;
            customer.PostalCode = postalCode;
            customer.City = city;
            customer.Country = country;
            customer.UpdatedDate = DateTime.Now;

            await _customersRepository.UpdateCustomerAsync(customer);
            return customer;
        }

        public async Task<Customer> GetCustomerAsync(int customerId)
            => await _customersRepository.GetCustomerAsync(customerId) ?? throw new ECommerceException(ErrorCode.NotFound(typeof(Customer), customerId));

        public async Task<IEnumerable<Customer>> GetCustomersAsync(string searchText = "") => await _customersRepository.GetCustomersAsync(searchText);

        public async Task InActiveCustomerAsync(int customerId)
        {
            var customer = await GetCustomerAsync(customerId);
            customer.IsActive = false;
            await _customersRepository.UpdateCustomerAsync(customer);
        }
    }
}