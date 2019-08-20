using System.Collections.Generic;
using System.Linq;
using aspnetmvc_starter.Core.Repositories;
using aspnetmvc_starter.Models;

namespace aspnetmvc_starter.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DefaultConnection context) : base(context)
        {
            
        }
        
        public IEnumerable<User> GetAllUsers()
        {
            return DefaultConnection.Users.ToList();
        }
        
        
        public DefaultConnection DefaultConnection
        {
            get { return Context as DefaultConnection; }
        }
    }
}