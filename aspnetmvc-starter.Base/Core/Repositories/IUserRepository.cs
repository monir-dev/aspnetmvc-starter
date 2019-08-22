using System.Collections.Generic;
using aspnetmvc_starter.Main.Core.Domain;

namespace aspnetmvc_starter.Base
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetAllUsers();
    }
}