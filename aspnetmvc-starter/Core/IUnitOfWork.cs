using System;
using aspnetmvc_starter.Core.Repositories;

namespace aspnetmvc_starter.Core
{
    public interface IUnitOfWork : IDisposable
    {
//        ICourseRepository Courses { get; }
//        IAuthorRepository Authors { get; }
        IUserRepository Users { get; }
        int Complete();
    }
}