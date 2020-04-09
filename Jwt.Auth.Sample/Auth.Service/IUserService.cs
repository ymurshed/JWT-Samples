using System.Collections.Generic;
using Auth.Contracts;

namespace Auth.Services
{
    public interface IUserService
    {
        public User GetUser(string name, string email);
        public IEnumerable<User> GetAllUsers();
    }
}
