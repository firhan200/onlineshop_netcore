using Data;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Repositories.UserRepository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly DbSet<User>? _dbSet;

        public UserRepository(DataContext dataContext) : base(dataContext)
        {
            _dbSet = dataContext.Set<User>();
        } 

        public User? Login(string username, string password)
        {
            return _dbSet?.Where(x => x.EmailAddress == username && x.Password == password).FirstOrDefault();
        }

        public User? GetByEmailAddress(string emailAddress)
        {
            //check if email already taken
            User? user = _dbSet?.FirstOrDefault(x => x.EmailAddress.Equals(emailAddress));
            if(user is null){
                return null;
            }

            return user;
        }

        public User? Register(string fullName, string emailAddress, string password)
        {
            User? user = new User{
                EmailAddress = emailAddress,
                FullName = fullName,
                Password = password,
                Role = Enums.Roles.User
            };

            _dbSet?.Add(user);
            this._dataContext.SaveChanges();

            return user;
        }
    }
}