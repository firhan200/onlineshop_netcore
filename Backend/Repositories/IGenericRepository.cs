namespace Repositories {
    public interface IGenericRepository<T> where T : class {
        List<T> GetAll();
        T? GetById(int Id);
        T? Create(T entity);
        T? Update(T entity);
        T? Delete(int Id);
    }
}