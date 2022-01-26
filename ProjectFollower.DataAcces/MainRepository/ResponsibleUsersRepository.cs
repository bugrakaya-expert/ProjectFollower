using ProjectFollower.DataAcces.Data;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.MainRepository
{
    public class ResponsibleUsersRepository : Repository<ResponsibleUsers>, IResponsibleUsersRepository
    {
        private readonly ApplicationDbContext _db;

        public ResponsibleUsersRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }

        public void Update(ResponsibleUsers ResponsibleUsers)
        {
            _db.Update(ResponsibleUsers);
        }

    }
}
