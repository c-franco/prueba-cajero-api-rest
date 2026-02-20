using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using prueba_cajero_api_rest.Application.Commands;
using prueba_cajero_api_rest.Domain.Constants;
using prueba_cajero_api_rest.Domain.Exceptions;
using prueba_cajero_api_rest.Infrastructure.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace prueba_cajero_api_rest.Application.Handlers
{
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, string>
    {
        private readonly JwtSettings _jwtSettings;

        public AuthenticateCommandHandler(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<string> Handle(AuthenticateCommand command, CancellationToken cancellationToken)
        {
            var username = command.Request.Username;
            var password = command.Request.Password;

            if (username == "admin" && password == "123456")
            {
                var claims = new[]
                {
                new Claim(ClaimTypes.Name, username),
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            throw new UnauthorizedException(GlobalErrors.InvalidCredentials);
        }
    }
}
