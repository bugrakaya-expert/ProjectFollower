using ProjectFollower.DataAcces.Data;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.MainRepository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }

        public void Update(ApplicationUser applicationUser)
        {
            _db.Update(applicationUser);
        }
    }
}
