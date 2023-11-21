using System;
using Microsoft.EntityFrameworkCore;
using Noested.Models;

namespace Noested.Data
{
    public interface IServiceOrderRepository
    {
        Task<IEnumerable<ServiceOrder>> GetAllServiceOrdersAsync();
        Task<ServiceOrder?> GetOrderByIdAsync(int id);
        Task AddServiceOrderAsync(ServiceOrder newOrder);
        Task UpdateOrderAsync(ServiceOrder updatedOrder);

        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task AddCustomerAsync(Customer newCustomer);
        Task UpdateCustomerAsync(Customer updatedCustomer);

        Task<IEnumerable<Checklist>> GetAllCheckListsAsync();
        Task<Checklist?> GetChecklistByIdAsync(int id);
        Task AddChecklistAsync(Checklist newChecklist);
        Task UpdateChecklistAsync(Checklist updatedChecklist);

        Task<IEnumerable<WinchChecklist>> GetAllWinchChecklistsAsync();
        Task<WinchChecklist?> GetWinchChecklistByIdAsync(int id);
        Task AddWinchChecklistAsync(WinchChecklist newWinchChecklist);
        Task UpdateWinchChecklistAsync(WinchChecklist updatedWinchChecklist);
        Task<IEnumerable<ServiceOrder>> Search(string query);

    }

    public class ServiceOrderRepository : IServiceOrderRepository
    {
        private readonly ApplicationDbContext _database;
        private readonly ILogger<ServiceOrderRepository> _logger;

        public ServiceOrderRepository(ApplicationDbContext database, ILogger<ServiceOrderRepository> logger)
        {
            _database = database;
            _logger = logger;
        }

        // SERVICE ORDERS
        public async Task<IEnumerable<ServiceOrder>> GetAllServiceOrdersAsync()
        {
            return await _database.ServiceOrder.ToListAsync();
        }

        public async Task<ServiceOrder?> GetOrderByIdAsync(int id)
        {
            return await _database.ServiceOrder.FindAsync(id);
        }

        public async Task AddServiceOrderAsync(ServiceOrder newOrder)
        {
            await _database.ServiceOrder.AddAsync(newOrder);
            await _database.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(ServiceOrder updatedOrder)
        {
            _database.ServiceOrder.Update(updatedOrder);
            await _database.SaveChangesAsync();
        }
        // CUSTOMERS
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _database.Customer.ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _database.Customer.FindAsync(id);
        }

        public async Task AddCustomerAsync(Customer newCustomer)
        {
            await _database.Customer.AddAsync(newCustomer);
            await _database.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer updatedCustomer)
        {
            _database.Customer.Update(updatedCustomer);
            await _database.SaveChangesAsync();
        }
        // CHECKLISTS
        public async Task<IEnumerable<Checklist>> GetAllCheckListsAsync()
        {
            return await _database.Checklist.ToListAsync();
        }

        public async Task<Checklist?> GetChecklistByIdAsync(int id)
        {
            return await _database.Checklist.FindAsync(id);
        }

        public async Task AddChecklistAsync(Checklist newChecklist)
        {
            await _database.Checklist.AddAsync(newChecklist);
            await _database.SaveChangesAsync();
        }

        public async Task UpdateChecklistAsync(Checklist updatedChecklist)
        {
            _database.Checklist.Update(updatedChecklist);
            await _database.SaveChangesAsync();
        }
        // WINCHCHECKLISTS
        public async Task<IEnumerable<WinchChecklist>> GetAllWinchChecklistsAsync()
        {
            return await _database.WinchChecklist.ToListAsync();
        }

        public async Task<WinchChecklist?> GetWinchChecklistByIdAsync(int id)
        {
            return await _database.WinchChecklist.FindAsync(id);
        }

        public async Task AddWinchChecklistAsync(WinchChecklist newWinchChecklist)
        {
            _database.WinchChecklist.Add(newWinchChecklist);
            await _database.SaveChangesAsync();
        }

        public async Task UpdateWinchChecklistAsync(WinchChecklist updatedWinchChecklist)
        {
            _database.Entry(updatedWinchChecklist).State = EntityState.Modified;
            await _database.SaveChangesAsync();
        }

        public async Task<IEnumerable<ServiceOrder>> Search(string query)
        {
            //Om query er et tall, returner kun ordre basert på id
            if (int.TryParse(query, out int id))
            {
                return await _database.ServiceOrder.Where(o => o.OrderId == id).ToListAsync();
            }

            return await _database.ServiceOrder.Where(o =>
                         o.Customer!.FirstName.ToLower().Contains(query) ||
                         o.Customer!.LastName.ToLower().Contains(query) ||
                         o.Customer!.Email.ToLower().Contains(query) ||    
                         o.ProductName!.ToLower().Contains(query)
                           //|| o.Product.ToString().ToLower().Contains(query)

                           ).ToListAsync();
        }
    }
}