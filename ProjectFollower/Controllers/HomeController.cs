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
            List<DepartmentsVM> departmentsVM = new List<DepartmentsVM>();
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
                departmentsVM.Add(DepartmentItem);
            }

            ProjectVM Project = new ProjectVM()
            {
                DepartmentsVMs= departmentsVM
            };

            return View(Project);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("project-new")]
        public IActionResult ProjectNewPost(ProjectVM ProjectVM)
        {
            var Users = new List<ApplicationUser>();

            ProjectVM.Customers = _uow.Customers.GetFirstOrDefault(i => i.Id == ProjectVM.CustomersId);
            ProjectVM.CreationDate = DateTime.Now.ToString("dd/MM/yyyy");

            var Project = new Projects()
            {
                CreationDate=ProjectVM.CreationDate,
                CustomersId=ProjectVM.CustomersId,
                Description=ProjectVM.Description,
                EndingDate=ProjectVM.EndingDate,
                Name=ProjectVM.Name
            };
            _uow.Project.Add(Project);
            foreach (var item in ProjectVM.UserId)
            {
                var User = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == item, includeProperties: "Department");
                //Users.Add(User);
                var ResponsibleUser = new ResponsibleUsers()
                {
                    ProjectId=Project.Id,
                    UserId= Guid.Parse(User.Id)
                };
                _uow.ResponsibleUsers.Add(ResponsibleUser);
            }
            foreach (var item in ProjectVM.TaskDesc)
            {
                var ProjectTask = new ProjectTasks()
                {
                    Description=item,
                    ProjectsId= Project.Id
                };
                _uow.ProjectTasks.Add(ProjectTask);
            }
            _uow.Save();
            return Redirect("/project-new");
        }
        [HttpGet("jsonresult/getalluserswithdep")]
        public JsonResult GetJson()
        {
            //var UserswithDep = _uow.ApplicationUser.GetAll(includeProperties: "Department");
            var Project = new ProjectVM();
            var GetDepartments = _uow.Department.GetAll();
            int i = 0;
            foreach (var item in GetDepartments)
            {
                var UserWidtDep = _uow.ApplicationUser.GetAll(i => i.DepartmentId == item.Id);
                var DepartmentItem = new DepartmentsVM()
                {
                    Id = item.Id,
                    Name = item.Name,
                    ApplicationUser = UserWidtDep,
                };
                Project.DepartmentsVMs.Add(DepartmentItem);
            }



            return Json(Project);
        }
    }
}
