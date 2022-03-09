using ProjectFollower.DataAcces.Data;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.MainRepository
{
    public class TaskPlayersRepository : Repository<TaskPlayers>, ITaskPlayersRepository
    {
        private readonly ApplicationDbContext _db;
        public TaskPlayersRepository(ApplicationDbContext db) 
            : base(db)
        {
            _db = db;
        }
        public void Update(TaskPlayers taskPlayers)
        {
            _db.Update(taskPlayers);
        }
    }
}
