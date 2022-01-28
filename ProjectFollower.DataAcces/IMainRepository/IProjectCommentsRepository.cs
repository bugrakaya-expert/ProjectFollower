using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.IMainRepository
{
    public interface IProjectCommentsRepository : IRepository<ProjectComments>
    {
        void Update(ProjectComments projectComments);
    }
}
