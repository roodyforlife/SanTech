using SanTech.Interfaces;

namespace SanTech.Services
{
    public class AuthorizatService : IAuthorizatService
    {
        private readonly IDbUserService _dbUserService;
        private readonly IFileService _fileService;
        public AuthorizatService(IDbUserService dbUserService, IFileService fileService)
        {
            _dbUserService = dbUserService;
            _fileService = fileService;
        }

        public bool IsRegistered(string email)
        {
            return _dbUserService.Get(email) is not null;
        }

        public bool PasswordIsCorrect(string email, string password)
        {
            if (!IsRegistered(email))
            {
                return false;
            }

            return _dbUserService.Get(email).Password == _fileService.HashData(password);
        }
    }
}
