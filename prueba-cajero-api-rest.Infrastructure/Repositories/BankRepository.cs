using Microsoft.EntityFrameworkCore;
using prueba_cajero_api_rest.Domain.Entities;
using prueba_cajero_api_rest.Domain.Interfaces.Repositories;
using prueba_cajero_api_rest.Infrastructure.Context;

namespace prueba_cajero_api_rest.Infrastructure.Repositories
{
    public class BankRepository : IBankRepository
    {
        private readonly AppDbContext _context;

        public BankRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BankAccount>> GetAll()
        {
            return await _context.BankAccounts.ToListAsync();
        }

        public async Task<BankAccount?> GetByAccountNumber(string accountNumber)
        {
            return await _context.BankAccounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
