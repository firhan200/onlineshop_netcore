using Schema;

namespace Repositories.UserRepository
{
    public interface IUserRepository {
        User? Login(string username, string password);
        User? GetByEmailAddress(string emailAddress);
        User? Register(string fullName, string emailAddress, string password);
    }
}