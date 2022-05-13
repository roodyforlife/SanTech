namespace SanTech.Interfaces
{
    public interface IAuthorizatService
    {
        public bool IsRegistered(string email);
        public bool PasswordIsCorrect(string email, string password);
    }
}
