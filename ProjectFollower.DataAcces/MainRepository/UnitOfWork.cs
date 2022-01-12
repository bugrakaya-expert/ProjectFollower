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
        }
        //public ICategoryRepository Category { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
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
