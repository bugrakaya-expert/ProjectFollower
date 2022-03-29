using Microsoft.AspNetCore.Mvc;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using ProjectFollower.Models.ViewModels;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
        public JsonResult GetMeetings()
        {
            //List<MeetingsVM> meetingsVM = new List<MeetingsVM>();
            List<CustomerMeetingVM> customerMeetingVM = new List<CustomerMeetingVM>();
            List<ResponsibleMeetings> ResponsibleMeetingVM = new List<ResponsibleMeetings>();
            List<Meetings> MeetingList = new List<Meetings>();
            var _customers = _uow.Customers.GetAll();
            var _meetings = _uow.Meeting.GetAll();
            var _responsiblesAll = _uow.ResponsibleMeeting.GetAll();
            int i = 0;/*
            foreach (var item in _responsiblesAll)
            {
                var respUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == item.UserId);
                var respmeet = new ResponsibleMeetings()
                {
                    Id = item.Id,
                    Text = respUser.FirstName + " " + respUser.Lastname,
                    MeetingId = item.MeetingId,
                    UserId = item.UserId
                };
                ResponsibleMeetingVM.Add(respmeet);
            }*/
            var _appUser = _uow.ApplicationUser.GetAll(i => i.Active);
            foreach (var item in _appUser)
            {
                var respmeet = new ResponsibleMeetings()
                {
                    UserId = item.Id,
                    Text = item.FirstName + " " + item.Lastname
                };
                ResponsibleMeetingVM.Add(respmeet);
            }
            foreach (var item in _customers)
            {
                var _customer = new CustomerMeetingVM()
                {
                    Id = item.Id,
                    Text = item.Name
                };
                customerMeetingVM.Add(_customer);
            }
            foreach (var item in _meetings)
            {
                var _respMeet = _uow.ResponsibleMeeting.GetAll(k => k.MeetingId == item.Id.ToString());
                string[] _userId = new string[_respMeet.ToList().Count()];
                foreach (var itemresp in _respMeet)
                {
                    _userId[i] = itemresp.UserId;
                    i++;
                }
                i = 0;
                var _meetingItem = new Meetings()
                {
                    Id = item.Id,
                    Customers = item.Customers,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    AllDay = item.AllDay,
                    CustomersId = item.CustomersId,
                    Text = item.Text,
                    Description=item.Description,
                    UserId= _userId
                };
                MeetingList.Add(_meetingItem);
            }

            var meetingVM = new MeetingsVM()
            {
                Meetings = MeetingList,
                Customer = customerMeetingVM,
                ResponsibleMeetings = ResponsibleMeetingVM
            };
            return Json(meetingVM);
        }

        [HttpPost("api/addMeetings")]
        public JsonResult Add([FromBody] Meetings meetings)
        {

            var customer = _uow.Customers.GetFirstOrDefault(i => i.Id == meetings.CustomersId);
            meetings.Customers = customer;
            _uow.Meeting.Add(meetings);
            foreach (var item in meetings.UserId)
            {
                var respMeetItem = new ResponsibleMeetings()
                {
                    MeetingId = meetings.Id.ToString(),
                    UserId = item
                };
                _uow.ResponsibleMeeting.Add(respMeetItem);
            }
            _uow.Save();
            return Json(meetings);
        }
        [HttpPost("api/updateMeetings")]
        public JsonResult Update([FromBody] Meetings meetings)
        {
            var customer = _uow.Customers.GetFirstOrDefault(i => i.Id == meetings.CustomersId);
            meetings.Customers = customer;
            _uow.Meeting.Update(meetings);/*
            foreach (var item in meetings.UserId)
            {
                var respMeetItem = new ResponsibleMeetings()
                {
                    MeetingId = meetings.Id.ToString(),
                    UserId = item
                };
                var respId=_uow.ResponsibleMeeting.GetFirstOrDefault(i=>i.UserId == respMeetItem.UserId).Id;
                respMeetItem.Id = respId;

            }*/
            var meeting = _uow.ResponsibleMeeting.GetAll(i => i.MeetingId == meetings.Id.ToString());
            _uow.ResponsibleMeeting.RemoveRange(meeting);
            _uow.Save();
            foreach (var item in meetings.UserId)
            {
                var respMeetItem = new ResponsibleMeetings()
                {
                    MeetingId = meetings.Id.ToString(),
                    UserId = item
                };
                _uow.ResponsibleMeeting.Add(respMeetItem);
            }
            _uow.Save();
            return Json(meetings);
        }
        [HttpPost("api/removeMeetings")]
        public JsonResult Remove([FromBody] Meetings meetings)
        {
            var customer = _uow.Customers.GetFirstOrDefault(i => i.Id == meetings.CustomersId);
            meetings.Customers = customer;
            var meeting = _uow.ResponsibleMeeting.GetAll(i => i.MeetingId == meetings.Id.ToString());
            _uow.ResponsibleMeeting.RemoveRange(meeting);
            _uow.Save();
            _uow.Meeting.Remove(meetings);
            _uow.Save();
            return Json(meetings);
        }
    }
}
