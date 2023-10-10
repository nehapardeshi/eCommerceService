using eCommerceService.Database;
using eCommerceService.Entities;
using eCommerceService.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace eCommerceService.Repositories
{
    public class CustomersRepository : ICustomersRepository
    {
        private readonly ECommerceDbContext _dbContext;
        public CustomersRepository(ECommerceDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Customer> GetCustomerAsync(int id)
            => await _dbContext.Customers.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

        public async Task<IEnumerable<Customer>> GetCustomersAsync(string searchText = "")
        {
            if (string.IsNullOrEmpty(searchText))
                return await _dbContext.Customers.ToListAsync();

            searchText = searchText.ToLower();
            return _dbContext.Customers
                .Where(customer => customer.FirstName.ToLower().Contains(searchText) ||
                customer.LastName.ToLower().Contains(searchText) ||
                customer.Email.ToLower().Contains(searchText) ||
                customer.Phone.ToLower().Contains(searchText) ||
                customer.StreetAddress.ToLower().Contains(searchText) ||
                customer.PostalCode.ToLower().Contains(searchText) ||
                customer.City.ToLower().Contains(searchText) ||
                customer.Country.ToLower().Contains(searchText)
                ).ToList();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            //First check customer:
            var cust = await GetCustomerAsync(customer.Id) ?? throw new ECommerceException(ErrorCode.NotFound(typeof(Customer), customer.Id));

            // Updates the customer properties:
            cust.City = customer.City;
            cust.Country = customer.Country;
            cust.Email = customer.Email;
            cust.Phone = customer.Phone;
            cust.FirstName = customer.FirstName;
            cust.LastName = customer.LastName;
            cust.StreetAddress = customer.StreetAddress;
            cust.UpdatedDate = customer.UpdatedDate;
            cust.IsActive = customer.IsActive;

            await _dbContext.SaveChangesAsync();
        }
    }
}
