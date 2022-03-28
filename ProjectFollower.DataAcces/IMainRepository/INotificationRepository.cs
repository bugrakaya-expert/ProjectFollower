using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.IMainRepository
{
    public interface INotificationRepository : IRepository<Notifications>
    {
        void Update(Notifications notifications);
    }
}
