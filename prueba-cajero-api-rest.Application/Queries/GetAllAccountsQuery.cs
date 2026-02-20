using MediatR;
using prueba_cajero_api_rest.Application.DTO;

namespace prueba_cajero_api_rest.Application.Queries
{
    public record GetAllAccountsQuery() : IRequest<List<BankAccountResponseDTO>>;
}
