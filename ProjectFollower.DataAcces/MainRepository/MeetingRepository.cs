using ProjectFollower.DataAcces.Data;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.MainRepository
{
    public class MeetingRepository : Repository<Meetings>, IMeetingRepository
    {
        private readonly ApplicationDbContext _db;

        public MeetingRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }

        public void Update(Meetings meetings)
        {
            _db.Update(meetings);
        }
    }
}
