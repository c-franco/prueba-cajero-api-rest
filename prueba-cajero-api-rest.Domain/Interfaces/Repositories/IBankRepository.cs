using prueba_cajero_api_rest.Domain.Entities;
using prueba_cajero_api_rest.Domain.Interfaces.Repositories.Base;

namespace prueba_cajero_api_rest.Domain.Interfaces.Repositories
{
    public interface IBankRepository : IBaseRepository<BankAccount>
    {
        Task<BankAccount?> GetByAccountNumber(string accountNumber);
    }
}
