using ProjectFollower.DataAcces.Data;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.MainRepository
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _db;

        public DepartmentRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }
        public void Update(Department department)
        {
            _db.Update(department);
        }
    }
}
