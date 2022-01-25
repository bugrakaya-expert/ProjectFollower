using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using ProjectFollower.Models.ViewModels;
using static ProjectFollower.Utility.ProjectConstant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectFollower.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IUnitOfWork _uow;

        public HomeController(
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
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [BindProperty]
        public UserRegisterVM Input { get; set; }


        [AllowAnonymous]
        public IActionResult Index()
        {
            #region Authentication Index
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                return RedirectToAction("Dashboard","Home");//Go Dashboard
            }

            #endregion Authentication Index

            return RedirectToAction("Index", "SignIn");
        }

        //[Authorize(Roles = UserRoles.Admin)]
        [Route("dashboard")]
        public IActionResult Dashboard() 
        {
            return View(); 
        } 

        [Authorize(Roles = UserRoles.Admin)]
        [Route("project-details")]
        public IActionResult Details()
        {
            return View();
        }

        [Authorize(Roles = UserRoles.Admin)]
        [Route("project-new")]
        public IActionResult ProjectNew()
        {
            //var GetCustomers = _uow.
            return View();
        }
        [HttpGet("jsonresult/getalluserswithdep")]
        public JsonResult GetJson()
        {
            //var UserswithDep = _uow.ApplicationUser.GetAll(includeProperties: "Department");
            List<DepartmentsVM> departmentsVMs = new List<DepartmentsVM>();
            var GetDepartments = _uow.Department.GetAll();
            foreach (var item in GetDepartments)
            {
                var UserWidtDep = _uow.ApplicationUser.GetAll(i => i.DepartmentId == item.Id);
                var DepartmentItem = new DepartmentsVM()
                {
                    Id = item.Id,
                    Name = item.Name,
                    ApplicationUser = UserWidtDep,
                };
                departmentsVMs.Add(DepartmentItem);
            }

            return Json(departmentsVMs);
        }
    }
}
