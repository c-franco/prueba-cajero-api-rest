using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prueba_cajero_api_rest.Application.Commands;
using prueba_cajero_api_rest.Application.DTO;
using prueba_cajero_api_rest.Domain.Entities;

namespace prueba_cajero_api_rest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Auth(LoginRequestDTO request)
        {
            var token = await _mediator.Send(new AuthenticateCommand(request));

            return Ok(ApiResponse<string>.Ok(token));
        }
    }
}
