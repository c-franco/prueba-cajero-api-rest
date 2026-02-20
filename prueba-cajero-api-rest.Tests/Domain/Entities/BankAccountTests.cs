using FluentAssertions;
using prueba_cajero_api_rest.Domain.Constants;
using prueba_cajero_api_rest.Domain.Entities;

namespace prueba_cajero_api_rest.Tests.Domain.Entities
{
    public class BankAccountTests
    {
        [Fact]
        public void Deposit_ShouldIncreaseBalance_WhenAmountIsValid()
        {
            var account = new BankAccount { Balance = 1000m };
            decimal depositAmount = 3000m;

            account.Deposit(depositAmount);

            account.Balance.Should().Be(4000m);
        }

        [Fact]
        public void Deposit_ShouldThrowException_WhenAmountExceedsLimit()
        {
            var account = new BankAccount { Balance = 1000m };
            decimal depositAmount = 3001m;

            Action act = () => account.Deposit(depositAmount);

            act.Should().Throw<InvalidOperationException>().WithMessage(GlobalErrors.DepositLimitExceeded);
        }

        [Fact]
        public void Withdraw_ShouldDecreaseBalance_WhenAmountIsValid()
        {
            var account = new BankAccount { Balance = 2000m };
            decimal withdrawAmount = 1000m;

            account.Withdraw(withdrawAmount);

            account.Balance.Should().Be(1000m);
        }

        [Fact]
        public void Withdraw_ShouldThrowException_WhenAmountExceedsWithdrawLimit()
        {
            var account = new BankAccount { Balance = 2000m };
            decimal withdrawAmount = 1001m;

            Action act = () => account.Withdraw(withdrawAmount);

            act.Should().Throw<InvalidOperationException>().WithMessage(GlobalErrors.WithdrawLimitExceeded);
        }

        [Fact]
        public void Withdraw_ShouldThrowException_WhenInsufficientFunds()
        {
            var account = new BankAccount { Balance = 500m };
            decimal withdrawAmount = 600m;

            Action act = () => account.Withdraw(withdrawAmount);

            act.Should().Throw<InvalidOperationException>().WithMessage(GlobalErrors.InsufficientFunds);
        }
    }
}
