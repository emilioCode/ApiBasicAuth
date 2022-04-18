using ApiBasicAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiBasicAuth.Services
{
    public class UserService : IUserService
    {
        List<User> users = new List<User>()
        {
            new User(){ Email = "dummy@email.com", Password = "123456"},
            new User(){ Email = "email@dummy.com", Password = "123456"}
        };

        public bool IsUser(string email, string password)
        {
            return users.Where(x => x.Email == email && x.Password == password).Count() > 0;
        }
    }
}
