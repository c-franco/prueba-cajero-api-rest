using prueba_cajero_api_rest.Domain.Entities;

namespace prueba_cajero_api_rest.Infrastructure.Context
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (!context.Persons.Any())
            {
                Person person = new Person { DNI = "11111111A", Name = "Pepe", Surname = "García" };

                BankAccount account1 = new BankAccount { AccountNumber = "ES1920956893611111113923", BankName = "BBVA", Balance = 1251.74m, Person = person };
                BankAccount account2 = new BankAccount { AccountNumber = "ES6420386343135175761749", BankName = "BBVA", Balance = 438.61m, Person = person };

                context.Persons.Add(person);
                context.BankAccounts.AddRange(account1, account2);

                context.SaveChanges();
            }
        }
    }
}
