using eCommerceService.Entities;

namespace eCommerceService
{
    public interface ICustomersRepository
    {
        Task AddCustomerAsync(Customer customer);
        Task<Customer> GetCustomerAsync(int id);
        Task<IEnumerable<Customer>> GetCustomersAsync(string searchText = "");
        Task UpdateCustomerAsync(Customer customer);
    }
}