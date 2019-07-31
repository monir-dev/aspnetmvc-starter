using System.Collections.Generic;
using aspnetmvc_starter.Models;

namespace aspnetmvc_starter.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetAllUsers();
    }
}