﻿using ProjectFollower.DataAcces.Data;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.DataAcces.MainRepository
{
    public class ResponsibleMeetingRepository : Repository<ResponsibleMeetings>, IResponsibleMeetingRepository
    {
        private readonly ApplicationDbContext _db;

        public ResponsibleMeetingRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }

        public void Update(ResponsibleMeetings responsibleMeetings)
        {
            _db.Update(responsibleMeetings);
        }

    }
}
