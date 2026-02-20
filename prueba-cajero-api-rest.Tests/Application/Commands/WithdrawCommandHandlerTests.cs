using FluentAssertions;
using Moq;
using prueba_cajero_api_rest.Application.Commands;
using prueba_cajero_api_rest.Application.Handlers;
using prueba_cajero_api_rest.Domain.Constants;
using prueba_cajero_api_rest.Domain.Entities;
using prueba_cajero_api_rest.Domain.Interfaces.Repositories;

namespace prueba_cajero_api_rest.Tests.Application.Commands
{
    public class WithdrawCommandHandlerTests
    {
        private readonly Mock<IBankRepository> _repositoryMock;
        private readonly WithdrawCommandHandler _handler;

        public WithdrawCommandHandlerTests()
        {
            _repositoryMock = new Mock<IBankRepository>();
            _handler = new WithdrawCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldWithdraw_WhenAccountExistsAndAmountIsValid()
        {
            var account = new BankAccount
            {
                AccountNumber = "ES1920956893611111113923",
                Balance = 1000
            };

            _repositoryMock.Setup(r => r.GetByAccountNumber("ES1920956893611111113923")).ReturnsAsync(account);

            var command = new WithdrawCommand(new() { AccountNumber = "ES1920956893611111113923", Amount = 500 });

            await _handler.Handle(command, CancellationToken.None);

            account.Balance.Should().Be(500);
            _repositoryMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenAccountDoesNotExist()
        {
            _repositoryMock.Setup(r => r.GetByAccountNumber("ES1920956893613923")).ReturnsAsync((BankAccount)null);

            var command = new WithdrawCommand(new() { AccountNumber = "ES1920956893613923", Amount = 500 });

            var act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>().WithMessage(GlobalErrors.AccountNotFound);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenAmountExceedsWithdrawLimit()
        {
            var account = new BankAccount
            {
                AccountNumber = "ES1920956893611111113923",
                Balance = 2000
            };

            _repositoryMock.Setup(r => r.GetByAccountNumber("ES1920956893611111113923")).ReturnsAsync(account);

            var command = new WithdrawCommand(new() { AccountNumber = "ES1920956893611111113923", Amount = 1500 });

            var act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage(GlobalErrors.WithdrawLimitExceeded);

            _repositoryMock.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenInsufficientFunds()
        {
            var account = new BankAccount
            {
                AccountNumber = "ES1920956893611111113923",
                Balance = 300
            };

            _repositoryMock.Setup(r => r.GetByAccountNumber("ES1920956893611111113923")).ReturnsAsync(account);

            var command = new WithdrawCommand(new() { AccountNumber = "ES1920956893611111113923", Amount = 500 });

            var act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage(GlobalErrors.InsufficientFunds);

            _repositoryMock.Verify(r => r.SaveAsync(), Times.Never);
        }
    }
}
