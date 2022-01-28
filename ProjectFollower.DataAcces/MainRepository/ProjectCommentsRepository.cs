using ProjectFollower.DataAcces.Data;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.MainRepository
{
    public class ProjectCommentsRepository : Repository<ProjectComments>, IProjectCommentsRepository
    {
        private readonly ApplicationDbContext _db;
        public ProjectCommentsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ProjectComments projectComments)
        {
            _db.Update(projectComments);
        }
    }
}
