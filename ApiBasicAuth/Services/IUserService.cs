using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiBasicAuth.Services
{
    public interface IUserService
    {
        public bool IsUser(string email, string password);
    }
}
