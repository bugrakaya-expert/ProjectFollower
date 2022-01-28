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
        ICustomersRepository Customers { get; }
        ICompanyDocumentsRepository CompanyDocuments { get; }
        ICompanyTypeRepository CompanyType { get; }
        IProjectRepository Project { get; }
        IProjectTaskRepository ProjectTasks { get; }
        IResponsibleUsersRepository ResponsibleUsers { get; }
        IProjectDocumentsRepository ProjectDocuments { get; }
        IProjectCommentsRepository ProjectComments { get; }
        ISchedulerPriorityRepository SchedulerPriority { get; }
        ISchedulerRepository Scheduler { get; }
        void Save();
    }
}
