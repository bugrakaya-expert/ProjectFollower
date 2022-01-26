using ProjectFollower.DataAcces.Data;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.MainRepository
{
    public class ProjectTaskRepository : Repository<ProjectTasks>, IProjectTaskRepository
    {
        private readonly ApplicationDbContext _db;

        public ProjectTaskRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }
        public void Update(ProjectTasks ProjectTask)
        {
            _db.Update(ProjectTask);
        }
    }
}
