using aspnetmvc_starter.Core;
using aspnetmvc_starter.Core.Repositories;
using aspnetmvc_starter.Models;
using aspnetmvc_starter.Persistence.Repositories;

namespace aspnetmvc_starter.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DefaultConnection _context;

        public UnitOfWork(DefaultConnection context)
        {
            _context = context;
            Users = new UserRepository(_context);
        }
        
        public IUserRepository Users { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}