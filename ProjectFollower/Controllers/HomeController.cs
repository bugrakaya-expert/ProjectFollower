using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using ProjectFollower.Models.ViewModels;
using static ProjectFollower.Utility.ProjectConstant;
using ProjectFollower.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ProjectFollower.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ProjectFollower.Controllers
{

    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        protected IHubContext<HomeHub> _context;
        private readonly IUnitOfWork _uow;

        public HomeController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ILogger<AccountController> logger,
        IHubContext<HomeHub> context,
        IUnitOfWork uow
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _uow = uow;
        }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [BindProperty]
        public UserRegisterVM Input { get; set; }
        public int Sequence = 0;
        public int Delayeds = 0;

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        public IActionResult Index()
        {
            #region Authentication Index
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                return RedirectToAction("Dashboard", "Home");//Go Dashboard
            }

            #endregion Authentication Index

            return RedirectToAction("Index", "SignIn");
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [Route("proje-detaylari/{id}")]
        public IActionResult Details(string id)
        {
            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id));
            var _tasks = _uow.ProjectTasks.GetAll(i => i.ProjectsId == Guid.Parse(id));
            var _documents = _uow.ProjectDocuments.GetAll(i => i.ProjectsId == Guid.Parse(id));
            var _comments = _uow.ProjectComments.GetAll(i => i.ProjectsId == Guid.Parse(id));

            var _projectDetailVM = new ProjectDetailVM()
            {
                Project = _project,
                ProjectTasks = _tasks,
                ProjectDocuments = _documents,
                ProjectComments = _comments
            };
            return View(_projectDetailVM);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [Route("yeni-proje")]
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
                DepartmentsVMs = departmentsVM
            };

            return View(Project);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("yeni-proje")]
        public async Task<IActionResult> ProjectNewPost(ProjectVM ProjectVM)
        {
            var Users = new List<ApplicationUser>();

            ProjectVM.Customers = _uow.Customers.GetFirstOrDefault(i => i.Id == ProjectVM.CustomersId);
            ProjectVM.CreationDate = DateTime.Now.ToString("dd/MM/yyyy");

            var Project = new Projects()
            {
                CreationDate = ProjectVM.CreationDate,
                CustomersId = ProjectVM.CustomersId,
                Description = ProjectVM.Description,
                EndingDate = ProjectVM.EndingDate,
                Name = ProjectVM.Name
            };
            _uow.Project.Add(Project);
            foreach (var item in ProjectVM.UserId)
            {
                var User = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == item, includeProperties: "Department");
                //Users.Add(User);
                var ResponsibleUser = new ResponsibleUsers()
                {
                    ProjectId = Project.Id,
                    UserId = Guid.Parse(User.Id)
                };
                _uow.ResponsibleUsers.Add(ResponsibleUser);
            }
            foreach (var item in ProjectVM.TaskDesc)
            {
                var ProjectTask = new ProjectTasks()
                {
                    Description = item,
                    ProjectsId = Project.Id
                };
                _uow.ProjectTasks.Add(ProjectTask);
            }
            _uow.Save();
            WebSocketActionExtensions WebSocAct = new WebSocketActionExtensions(_context, _uow);
            await WebSocAct.ListProjects_WebSocket();
            return Redirect("/");
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("proje-detaylari")]
        #region API

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("jsonresult/getprojectlist/{ArchiveStatus}")]
        public JsonResult ListJson(int ArchiveStatus)
        {
            IEnumerable<Projects> Projects;
            List<Projects> _projects = new List<Projects>();
            List<ProjectListVM> ProjectListVMs = new List<ProjectListVM>();
            if (ArchiveStatus == 0)
            {
                Projects = _uow.Project.GetAll(i => i.Archived == false, includeProperties: "Customers");
            }
            else if (ArchiveStatus == 1)
            {
                Projects = _uow.Project.GetAll(i => i.Archived == true, includeProperties: "Customers");
            }
            else if (ArchiveStatus == 2)
                Projects = _uow.Project.GetAll(includeProperties: "Customers");
            else
                return Json(null);

            var FilteredProject = Projects.OrderBy(d => Convert.ToDateTime(d.EndingDate));
            foreach (var item in FilteredProject)
            {
                item.SequanceDate = Sequence++;
                if (DateTime.Now.Date > Convert.ToDateTime(item.EndingDate))
                {
                    item.ProjectSequence = 1;
                    Delayeds++;
                }
                else
                    item.ProjectSequence = 2;
                /*
                var ProjectListVM = new ProjectListVM()
                {
                    Archived=item.Archived,
                    CustomersId = item.CustomersId,
                    Name=item.Name,
                    Customers=item.Customers,
                    EndingDate=item.EndingDate,
                    Status = item.Status,
                    ProjectSequence = Sequence
                };
                ProjectListVMs.Add(ProjectListVM);*/
                _projects.Add(item);
            }
            var ProjectListVM = new ProjectListVM()
            {
                Projects = _projects,
                DelayedProjects = Delayeds
            };

            return Json(ProjectListVM);
        }
        public JsonResult UpSetJson()
        {

            return Json(null);
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
        #endregion API

    }
}
