using ProjectFollower.DataAcces.Data;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.MainRepository
{
    public class CompanyDocumentsRepository : Repository<CompanyDocuments>, ICompanyDocumentsRepository
    {
        private readonly ApplicationDbContext _db;

        public CompanyDocumentsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update (CompanyDocuments companyDocuments)
        {
            _db.Update(companyDocuments);
        }
    }
}
