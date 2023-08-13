using Data;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _dataContext;
        internal DbSet<Product> _dbSet;

        public ProductRepository(DataContext dataContext){
            _dataContext = dataContext;
            _dbSet = _dataContext.Set<Product>();
        }

        public List<Product> GetAll()
        {
            return _dbSet.ToList();
        }

        public Product? GetById(int Id)
        {
            return GetAll().Where(x => x.Id == Id).FirstOrDefault();
        }

        public bool Create(Product entity)
        {
            _dbSet.Add(entity);
            _dataContext.SaveChanges();
            return true;
        }

        public bool Update(Product entity)
        {
            _dbSet.Update(entity);
            _dataContext.SaveChanges();
            return true;
        }

        public bool Delete(int Id)
        {
            var deletedProduct = _dbSet.Where(x => x.Id == Id).ExecuteDelete();
            if(deletedProduct > 0){
                return true;
            }

            return false;
        }

        public List<Product> GetProductsByIds(List<int> productIds)
        {
            return _dbSet.Where(x => productIds.Contains(x.Id)).ToList();
        }
    }
}