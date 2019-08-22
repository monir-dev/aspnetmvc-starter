using System;

namespace aspnetmvc_starter.Base
{
    public interface IUnitOfWork : IDisposable
    {
//        ICourseRepository Courses { get; }
//        IAuthorRepository Authors { get; }
        IUserRepository Users { get; }
        int Complete();
    }
}