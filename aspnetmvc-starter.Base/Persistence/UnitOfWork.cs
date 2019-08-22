using aspnetmvc_starter.Main.Core.Domain;

namespace aspnetmvc_starter.Base
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