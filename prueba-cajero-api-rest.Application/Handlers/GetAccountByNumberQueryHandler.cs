using MediatR;
using prueba_cajero_api_rest.Application.DTO;
using prueba_cajero_api_rest.Application.Queries;
using prueba_cajero_api_rest.Domain.Constants;
using prueba_cajero_api_rest.Domain.Interfaces.Repositories;

namespace prueba_cajero_api_rest.Application.Handlers
{
    public class GetAccountByNumberQueryHandler : IRequestHandler<GetAccountByNumberQuery, BankAccountResponseDTO>
    {
        private readonly IBankRepository _repository;

        public GetAccountByNumberQueryHandler(IBankRepository repository)
        {
            _repository = repository;
        }

        public async Task<BankAccountResponseDTO> Handle(GetAccountByNumberQuery query, CancellationToken cancellationToken)
        {
            var account = await _repository.GetByAccountNumber(query.Request.AccountNumber);

            if (account == null)
                throw new Exception(GlobalErrors.AccountNotFound);

            return new BankAccountResponseDTO
            {
                AccountNumber = account.AccountNumber,
                Balance = account.Balance,
                Entity = account.BankName,
            };
        }
    }
}
