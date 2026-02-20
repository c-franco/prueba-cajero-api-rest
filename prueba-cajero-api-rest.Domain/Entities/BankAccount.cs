using prueba_cajero_api_rest.Domain.Constants;
using prueba_cajero_api_rest.Domain.Entities.Base;

namespace prueba_cajero_api_rest.Domain.Entities
{
    public class BankAccount : BaseEntity
    {
        public string AccountNumber { get; set; }

        public string BankName { get; set; }

        public decimal Balance { get; set; }

        public Person Person { get; set; } = null!;

        public void Deposit(decimal amount)
        {
            if (amount > 3000)
                throw new InvalidOperationException(GlobalErrors.DepositLimitExceeded);

            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount > 1000)
                throw new InvalidOperationException(GlobalErrors.WithdrawLimitExceeded);

            if (amount > Balance)
                throw new InvalidOperationException(GlobalErrors.InsufficientFunds);

            Balance -= amount;
        }
    }
}
