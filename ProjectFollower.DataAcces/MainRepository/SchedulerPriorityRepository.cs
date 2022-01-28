using ProjectFollower.DataAcces.Data;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.MainRepository
{
    public class SchedulerPriorityRepository : Repository<SchedulerPriority>, ISchedulerPriorityRepository
    {
        private readonly ApplicationDbContext _db;

        public SchedulerPriorityRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }

        public void Update(SchedulerPriority schedulerPriority)
        {
            _db.Update(schedulerPriority);
        }
    }
}
