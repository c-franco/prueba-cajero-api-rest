using MediatR;
using prueba_cajero_api_rest.Application.Commands;
using prueba_cajero_api_rest.Domain.Constants;
using prueba_cajero_api_rest.Domain.Interfaces.Repositories;

namespace prueba_cajero_api_rest.Application.Handlers
{
    public class DepositCommandHandler : IRequestHandler<DepositCommand, Unit>
    {
        private readonly IBankRepository _repository;

        public DepositCommandHandler(IBankRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DepositCommand command, CancellationToken cancellationToken)
        {
            var account = await _repository.GetByAccountNumber(command.Request.AccountNumber);

            if (account == null)
                throw new Exception(GlobalErrors.AccountNotFound);

            account.Deposit(command.Request.Amount);

            await _repository.SaveAsync();

            return Unit.Value;
        }
    }
}
