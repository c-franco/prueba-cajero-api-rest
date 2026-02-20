using prueba_cajero_api_rest.Domain.Entities.Base;

namespace prueba_cajero_api_rest.Domain.Entities
{
    public class Person : BaseEntity
    {
        public string DNI { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        //public List<BankAccount> BankAccounts { get; set; } = new();
    }
}
