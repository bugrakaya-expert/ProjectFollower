using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.IMainRepository
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        void Update(ApplicationUser applicationUser);
    }
}
