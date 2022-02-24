using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using ProjectFollower.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectFollower.Controllers
{
    public class SchedulerController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IUnitOfWork _uow;


        public SchedulerController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ILogger<AccountController> logger,
        IUnitOfWork uow
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _uow = uow;
        }


        [HttpPost("scheduler")]
        public IActionResult Index(string customerid)
        {
            #region Authentication Index
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                var customer = _uow.Customers.GetFirstOrDefault(i => i.Id == Guid.Parse(customerid));
                return View(customer);//Go Dashboard
            }

            #endregion Authentication Index

            return RedirectToAction("Index", "SignIn");
        }


        #region API
        [HttpGet("getscheduler/{id}")]
        public JsonResult GetScheduler(string id)
        {
            List<SchedulerCustomerVM> schedulerCustomer = new List<SchedulerCustomerVM>();
            var companies = _uow.Customers.GetFirstOrDefault(i => i.Id == Guid.Parse(id));
            var scheduler = _uow.Scheduler.GetAll(i=>i.CustomersId==Guid.Parse(id));
            var schedulerpriorities = _uow.SchedulerPriority.GetAll();

            /*
            foreach (var item in companies)
            {
                var _schedulerCustomer = new SchedulerCustomerVM()
                {
                    Id = item.Id.ToString(),
                    Text = item.Name
                };
                schedulerCustomer.Add(_schedulerCustomer);
            }*/
            var _schedulerCustomerItem = new  SchedulerCustomerVM()
            {
                Id = companies.Id.ToString(),
                Text = companies.Name
            };
            var _schedulerCustomer = new List<SchedulerCustomerVM>();
            _schedulerCustomer.Add(_schedulerCustomerItem);
            var _schedulerVM = new SchedulerVM()
            {
                Scheduler = scheduler,
                SchedulerPriority = schedulerpriorities,
                Customers = _schedulerCustomer
            };

            return Json(_schedulerVM);

        }
        [HttpPost("postscheduler")]
        public JsonResult SetScheduler([FromBody] Scheduler scheduler)
        {
            _uow.Scheduler.Add(scheduler);
            _uow.Save();
            return Json(scheduler);
        }
        [HttpPost("updatescheduler")]
        public JsonResult UpdateScheduler([FromBody] Scheduler scheduler)
        {
            _uow.Scheduler.Update(scheduler);
            _uow.Save();
            return Json(scheduler);
        }
        [HttpGet("deletescheduler/{id}")]
        public JsonResult DeleteScheduler(int id)
        {
            var scheduler = _uow.Scheduler.GetFirstOrDefault(i => i.Id == id);
            _uow.Scheduler.Remove(scheduler);
            _uow.Save();
            return Json(null);
        }
        #endregion API
    }
}
