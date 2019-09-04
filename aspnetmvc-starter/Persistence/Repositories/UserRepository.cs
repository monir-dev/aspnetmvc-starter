using System.Collections.Generic;
using System.Linq;
using aspnetmvc_starter.Core;
using aspnetmvc_starter.Main.Core.Domain;

namespace aspnetmvc_starter.Persistence
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