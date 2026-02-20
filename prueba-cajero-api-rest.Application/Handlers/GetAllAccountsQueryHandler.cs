using MediatR;
using prueba_cajero_api_rest.Application.DTO;
using prueba_cajero_api_rest.Application.Queries;
using prueba_cajero_api_rest.Domain.Interfaces.Repositories;

namespace prueba_cajero_api_rest.Application.Handlers
{
    public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, List<BankAccountResponseDTO>>
    {
        private readonly IBankRepository _repository;

        public GetAllAccountsQueryHandler(IBankRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<BankAccountResponseDTO>> Handle(GetAllAccountsQuery query, CancellationToken cancellationToken)
        {
            var accounts = await _repository.GetAll();

            return accounts.Select(a => new BankAccountResponseDTO
            {
                AccountNumber = a.AccountNumber,
                Entity = a.BankName,
                Balance = a.Balance
            }).ToList();
        }
    }
}
