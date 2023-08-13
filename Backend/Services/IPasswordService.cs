namespace Services
{
    public interface IPasswordService {
        string EncryptPassword(string rawPassword);
    }
}