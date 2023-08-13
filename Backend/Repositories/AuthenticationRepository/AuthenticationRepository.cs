using Data;
using Schema;

namespace Repositories.AuthenticationRepository
{
    public class AuthenticationRepository : GenericRepository<Authentication>
    {
        public AuthenticationRepository(DataContext dataContext) : base(dataContext){
        }
    }
}