using eCommerceService.Entities;

namespace eCommerceService
{
    public interface ICustomersService
    {
        Task<Customer> AddCustomerAsync(string firstName, string lastName, string email, string phone, string streetAddress, string postalCode, string city, string country);
        Task<Customer> UpdateCustomerAsync(int customerId, string firstName, string lastName, string email, string phone, string streetAddress, string postalCode, string city, string country);
        Task<Customer> GetCustomerAsync(int customerId);
        Task<IEnumerable<Customer>> GetCustomersAsync(string searchText = "");
        Task InActiveCustomerAsync(int customerId);
    }
}