using ProjectFollower.DataAcces.Data;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.MainRepository
{
    public class ProjectDocumentsRepository : Repository<ProjectDocuments>, IProjectDocumentsRepository
    {
        private readonly ApplicationDbContext _db;

        public ProjectDocumentsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update (ProjectDocuments projectDocuments)
        {
            _db.Update(projectDocuments);
        }
    }
}
