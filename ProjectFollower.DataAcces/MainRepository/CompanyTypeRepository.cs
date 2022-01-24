using ProjectFollower.DataAcces.Data;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.MainRepository
{
    public class CompanyTypeRepository : Repository<CompanyType>, ICompanyTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public CompanyTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(CompanyType companyType)
        {
            _db.Update(companyType);
        }
    }
}
