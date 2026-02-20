using prueba_cajero_api_rest.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace prueba_cajero_api_rest.Application.DTO
{
    public class WithdrawRequestDTO
    {
        [Required(ErrorMessage = GlobalErrors.AccountNumberRequired)]
        [RegularExpression(@"^[A-Z]{2}[0-9A-Z]{13,30}$", ErrorMessage = GlobalErrors.AccountNumberInvalidFormat)]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = GlobalErrors.AmountRequired)]
        [Range(0.01, double.MaxValue, ErrorMessage = GlobalErrors.AmountInvalid)]
        public decimal Amount { get; set; }
    }
}
