namespace prueba_cajero_api_rest.Application.DTO
{
    public class BankAccountResponseDTO
    {
        public string AccountNumber { get; set; }

        public string Entity { get; set; }

        public decimal Balance { get; set; }
    }
}
