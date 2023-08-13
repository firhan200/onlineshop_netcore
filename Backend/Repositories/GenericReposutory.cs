using Data;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DataContext _dataContext;
        private DbSet<T>? _table = null;

        public GenericRepository(
            DataContext dataContext
        ){
            _dataContext = dataContext;
            _table = _dataContext.Set<T>();
        }

        public virtual T? Create(T entity)
        {
            if(_table is null){
                return null;
            }

            _table.Add(entity);
            _dataContext.SaveChanges();
            return entity;
        }

        public virtual T? Delete(int Id)
        {
            if(_table is null){
                return null;
            }

            T? entity = GetById(Id);
            if(entity is null){
                return null;
            }

            _table.Remove(entity);

            return entity;
        }

        public List<T> GetAll()
        {
            if(_table is null){
                return new List<T>();
            }

            return _table.ToList();
        }

        public T? GetById(int Id)
        {
            if(_table is null){
                return null;
            }

            return _table.Find(Id);
        }

        public virtual T? Update(T entity)
        {
            if(_table is null){
                return null;
            }

            _table.Update(entity);
            _dataContext.SaveChanges();
            return entity;
        }
    }
}