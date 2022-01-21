using ProjectFollower.DataAcces.Data;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.MainRepository
{
    public class CustomersRepository : Repository<Customers> , ICustomersRepository
    {
        private readonly ApplicationDbContext _db;

        public CustomersRepository(ApplicationDbContext db)
    : base(db)
        {
            _db = db;
        }

        public void Update(Customers customers)
        {
            _db.Update(customers);
        }
    }
}
