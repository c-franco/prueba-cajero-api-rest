using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prueba_cajero_api_rest.Application.Commands;
using prueba_cajero_api_rest.Application.DTO;
using prueba_cajero_api_rest.Application.Queries;
using prueba_cajero_api_rest.Domain.Constants;
using prueba_cajero_api_rest.Domain.Entities;

namespace prueba_cajero_api_rest.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankController : Controller
    {
        private readonly IMediator _mediator;

        #region Constructor

        public BankController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion

        #region Get

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<BankAccountResponseDTO>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllAccountsQuery());

            return Ok(ApiResponse<List<BankAccountResponseDTO>>.Ok(result));
        }

        #endregion

        #region Post

        [Authorize]
        [HttpPost("getByAccountNumber")]
        public async Task<ActionResult<BankAccountResponseDTO>> GetByAccountNumber(AccountRequestDTO request)
        {
            var result = await _mediator.Send(new GetAccountByNumberQuery(request));

            return Ok(ApiResponse<BankAccountResponseDTO>.Ok(result));
        }

        [Authorize]
        [HttpPost("deposit")]
        public async Task<ActionResult<string>> Deposit(DepositRequestDTO request)
        {
            await _mediator.Send(new DepositCommand(request));

            return Ok(ApiResponse<string>.Ok(GlobalMessages.SuccessfulDeposit));
        }

        [Authorize]
        [HttpPost("withdraw")]
        public async Task<ActionResult<string>> Withdraw(WithdrawRequestDTO request)
        {
            await _mediator.Send(new WithdrawCommand(request));

            return Ok(ApiResponse<string>.Ok(GlobalMessages.SuccessfulWithdrawal));
        }

        #endregion
    }
}
