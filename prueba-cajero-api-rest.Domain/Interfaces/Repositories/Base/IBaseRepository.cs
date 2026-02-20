namespace prueba_cajero_api_rest.Domain.Interfaces.Repositories.Base
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task SaveAsync();
    }
}
