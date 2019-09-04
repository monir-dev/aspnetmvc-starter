using System.Collections.Generic;
using aspnetmvc_starter.Main.Core.Domain;

namespace aspnetmvc_starter.Core
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetAllUsers();
    }
}