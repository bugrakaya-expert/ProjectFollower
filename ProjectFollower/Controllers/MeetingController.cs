using Microsoft.AspNetCore.Mvc;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using System.Collections.Generic;

namespace ProjectFollower.Controllers
{
    public class MeetingController : Controller
    {
        private readonly IUnitOfWork _uow;
        public MeetingController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [Route("meeting")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("api/getMeetings")]
        public IEnumerable<Meetings> GetMeetings()
        {
            var _meetings = _uow.Meeting.GetAll();
            return null;
        }
    }
}
