namespace prueba_cajero_api_rest.Domain.Constants
{
    public class GlobalErrors
    {
        public const string UsernameRequired = "Username is required.";
        public const string UsernameLength = "The username must be between 3 and 50 characters.";
        public const string PasswordRequired = "Password is required.";
        public const string PasswordLength = "The password must be between 6 and 50 characters long.";

        public const string AccountNumberRequired = "Account number is required.";
        public const string AmountRequired = "Amount is required.";

        public const string AccountNumberInvalidFormat = "Invalid account number format.";
        public const string AmountInvalid = "Amount must be greater than 0.";

        public const string DepositLimitExceeded = "Cannot deposit more than 3000 EUR in a single operation.";
        public const string WithdrawLimitExceeded = "Cannot withdraw more than 1000 EUR in a single operation.";
        public const string InsufficientFunds = "Insufficient funds.";
        public const string AccountNotFound = "Account not found.";

        public const string InvalidJwtToken = "Unauthorized. The token is invalid or expired.";
        public const string InvalidCredentials = "Invalid credentials.";
    }
}
