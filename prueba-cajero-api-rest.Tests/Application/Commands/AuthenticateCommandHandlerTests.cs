using FluentAssertions;
using Microsoft.Extensions.Options;
using prueba_cajero_api_rest.Application.Commands;
using prueba_cajero_api_rest.Application.Handlers;
using prueba_cajero_api_rest.Domain.Constants;
using prueba_cajero_api_rest.Domain.Exceptions;
using prueba_cajero_api_rest.Infrastructure.Settings;
using System.Security.Claims;
namespace prueba_cajero_api_rest.Tests.Application.Commands
{
    public class AuthenticateCommandHandlerTests
    {
        private readonly AuthenticateCommandHandler _handler;
        private readonly JwtSettings _jwtSettings;

        public AuthenticateCommandHandlerTests()
        {
            _jwtSettings = new JwtSettings
            {
                Key = "ThisIsASecretKeyForJwtTest123456",
                Issuer = "TestIssuer",
                Audience = "TestAudience"
            };

            var options = Options.Create(_jwtSettings);
            _handler = new AuthenticateCommandHandler(options);
        }

        [Fact]
        public async Task Handle_ShouldReturnToken_WhenCredentialsAreValid()
        {
            var command = new AuthenticateCommand(new() { Username = "admin", Password = "123456" });

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNullOrEmpty();

            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(result);

            token.Issuer.Should().Be(_jwtSettings.Issuer);
            token.Audiences.Should().Contain(_jwtSettings.Audience);
            token.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == "admin");
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedException_WhenCredentialsAreInvalid()
        {
            var command = new AuthenticateCommand(new() { Username = "admin", Password = "wrongpassword" });

            var act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<UnauthorizedException>().WithMessage(GlobalErrors.InvalidCredentials);
        }
    }
}
