using System;
using System.Collections.Generic;
using Auth.Contracts;

namespace Auth.Services
{
    public class UserService : IUserService
    {
        public User GetUser(string name, string email)
        {
            var role =  name.Equals(Constants.AdminUserName, StringComparison.CurrentCultureIgnoreCase) &&
                                email.Equals(Constants.AdminUserEmail, StringComparison.CurrentCultureIgnoreCase) ? 
                                Constants.AdminUserRole : Constants.OtherUserRole;
            return new User
            {
                Name = name,
                Email = email,
                Role = role
            };
        }

        public IEnumerable<User> GetAllUsers()
        {
            // Dummy user list
            return new List<User>
            {
                new User {Name = "user1", Email = "user1@gmail.com", Role = Constants.OtherUserRole},
                new User {Name = "user2", Email = "user2@gmail.com", Role = Constants.OtherUserRole},
                new User {Name = "user3", Email = "user3@gmail.com", Role = Constants.OtherUserRole},
                new User {Name = Constants.AdminUserName, Email = Constants.AdminUserEmail, Role = Constants.AdminUserRole}
            };
        }
    }
}