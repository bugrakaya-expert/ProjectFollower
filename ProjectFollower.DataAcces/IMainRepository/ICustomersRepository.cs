using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.IMainRepository
{
    public interface ICustomersRepository : IRepository<Customers>
    {
        void Update(Customers customers);
    }
}
