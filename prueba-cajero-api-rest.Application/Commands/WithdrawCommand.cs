using MediatR;
using prueba_cajero_api_rest.Application.DTO;

namespace prueba_cajero_api_rest.Application.Commands
{
    public record WithdrawCommand(WithdrawRequestDTO Request) : IRequest<Unit>;
}
