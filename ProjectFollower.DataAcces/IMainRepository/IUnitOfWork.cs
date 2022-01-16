using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.IMainRepository
{
    public interface IUnitOfWork : IDisposable
    {
        //ICategoryRepository Category { get; }
        IDepartmentRepository Department { get; }
        IApplicationUserRepository ApplicationUser { get; }

        void Save();
    }
}
