using ProjectFollower.DataAcces.Data;
using ProjectFollower.DataAcces.IMainRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.MainRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            Department = new DepartmentRepository(_db);
            Customers = new CustomersRepository(_db);
            CompanyDocuments = new CompanyDocumentsRepository(_db);
            Project = new ProjectRepository(_db);
            ProjectTasks = new ProjectTaskRepository(_db);
            ResponsibleUsers = new ResponsibleUsersRepository(_db);
            ProjectDocuments = new ProjectDocumentsRepository(_db);
            ProjectComments = new ProjectCommentsRepository(_db);
            SchedulerPriority = new SchedulerPriorityRepository(_db);
            Scheduler = new SchedulerRepository(_db);
        }
        //public ICategoryRepository Category { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public ICustomersRepository Customers { get; private set; }
        public ICompanyDocumentsRepository CompanyDocuments { get; private set; }
        public IProjectRepository Project { get; private set; }
        public IProjectTaskRepository ProjectTasks { get; private set; }
        public IResponsibleUsersRepository ResponsibleUsers { get; private set; }
        public IProjectDocumentsRepository ProjectDocuments { get; private set; }
        public IProjectCommentsRepository ProjectComments { get; private set; }
        public ISchedulerPriorityRepository SchedulerPriority { get; private set; }
        public ISchedulerRepository Scheduler { get; private set; }
        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
