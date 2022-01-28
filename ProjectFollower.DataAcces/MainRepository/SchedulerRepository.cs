using ProjectFollower.DataAcces.Data;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.MainRepository
{
    public class SchedulerRepository : Repository<Scheduler>, ISchedulerRepository
    {
        private readonly ApplicationDbContext _db;

        public SchedulerRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }

        public void Update(Scheduler scheduler)
        {
            _db.Update(scheduler);
        }
    }
}
