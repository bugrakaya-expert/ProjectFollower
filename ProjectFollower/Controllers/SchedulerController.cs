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


        [Route("scheduler")]
        public IActionResult Index()
        {
            #region Authentication Index
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                return View();//Go Dashboard
            }

            #endregion Authentication Index

            return RedirectToAction("Index", "SignIn");
        }

        #region API
        [HttpGet("getscheduler")]
        public JsonResult GetScheduler()
        {
            List<SchedulerCustomerVM> schedulerCustomer = new List<SchedulerCustomerVM>();
            var scheduler = _uow.Scheduler.GetAll();
            var schedulerpriorities = _uow.SchedulerPriority.GetAll();
            var companies = _uow.Customers.GetAll();

            foreach (var item in companies)
            {
                var _schedulerCustomer = new SchedulerCustomerVM()
                {
                    Id = item.Id.ToString(),
                    Text = item.Name
                };
                schedulerCustomer.Add(_schedulerCustomer);
            }
            var _schedulerVM = new SchedulerVM()
            {
                Scheduler = scheduler,
                SchedulerPriority = schedulerpriorities,
                Customers = schedulerCustomer
            };

            return Json(_schedulerVM);

        }
        #endregion API
    }
}
