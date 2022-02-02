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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ProjectFollower.Controllers
{

    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        protected IHubContext<HomeHub> _context;
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ILogger<AccountController> logger,
        IHubContext<HomeHub> context,
        IUnitOfWork uow,
        IWebHostEnvironment hostEnvironment
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _uow = uow;
            _hostEnvironment = hostEnvironment;
        }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [BindProperty]
        public UserRegisterVM Input { get; set; }
        public int Sequence = 0;
        public int Delayeds = 0;

        //[Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
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
        //[Route("proje-detaylari/{id}&updated={status}")]
        [Route("proje-detaylari/{id}")]
        public IActionResult Details(string id)
        {
            List<Users> _users = new List<Users>();
            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id), includeProperties: "Customers");
            var _tasks = _uow.ProjectTasks.GetAll(i => i.ProjectsId == Guid.Parse(id));
            var _documents = _uow.ProjectDocuments.GetAll(i => i.ProjectsId == Guid.Parse(id));
            var _comments = _uow.ProjectComments.GetAll(i => i.ProjectsId == Guid.Parse(id));
            var _responsibles = _uow.ResponsibleUsers.GetAll(i => i.ProjectId == Guid.Parse(id));

            foreach (var item in _responsibles)
            {
                var AppUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == item.UserId.ToString(),includeProperties: "Department");
                var User = new Users()
                {
                    FirstName = AppUser.FirstName,
                    LastName = AppUser.Lastname,
                    FullName = AppUser.FirstName + " " + AppUser.Lastname,
                    Email = AppUser.Email,
                    Department=AppUser.Department,
                    DepartmentId=AppUser.DepartmentId,
                    AppUserName=AppUser.AppUserName,
                    ImageUrl=AppUser.ImageUrl
                };
                _users.Add(User);
            }
            var _projectDetailVM = new ProjectDetailVM()
            {
                Project = _project,
                ProjectTasks = _tasks,
                ProjectDocuments = _documents,
                ProjectComments = _comments,
                Users=_users
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
            ProjectVM.Id = Project.Id;
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

            string webRootPath = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            if (files.Count() > 0)
            {
                //string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, webRootPath+LocFilePaths.DIR_Projects_Doc + ProjectVM.Name);

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                /*
                #region Check Customer Directories
                if (!(Directory.Exists(LocFilePaths.RootAsset)))
                    Directory.CreateDirectory(LocFilePaths.RootAsset);
                if (!(Directory.Exists(LocFilePaths.DIR_Projects_Main)))
                    Directory.CreateDirectory(LocFilePaths.DIR_Projects_Main);
                if (!(Directory.Exists(LocFilePaths.DIR_Projects_Doc)))
                    Directory.CreateDirectory(LocFilePaths.DIR_Projects_Doc);
                if (!(Directory.Exists(LocFilePaths.DIR_Projects_Doc + ProjectVM.Name)))
                    Directory.CreateDirectory(LocFilePaths.DIR_Projects_Doc + ProjectVM.Name);
                #endregion Check Customer Directories
                */

                foreach (var item in files)
                {
                    string extension = "";
                    var _filename = item.FileName.Split('.');
                    if (_filename.Count() == 1)
                        extension = Path.GetExtension(item.FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, item.FileName + extension), FileMode.Create))
                    {
                        item.CopyTo(fileStream);
                        var Document = new ProjectDocuments()
                        {
                            ProjectsId = ProjectVM.Id,
                            FileName = item.FileName + extension,
                        };
                        _uow.ProjectDocuments.Add(Document);
                    }
                }
            }
            _uow.Save();
            WebSocketActionExtensions WebSocAct = new WebSocketActionExtensions(_context, _uow);
            await WebSocAct.ListProjects_WebSocket();
            return Redirect("/");
        }
        [HttpPost("proje-detaylari/dokuman-guncelle")]
        public IActionResult UpdateDocuments(ProjectDetailVM projectDetailVM)
        {
            string webRootPath = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var Project = _uow.Project.GetFirstOrDefault(i => i.Id == projectDetailVM.Project.Id);
            if (files.Count() > 0)
            {
                //string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, webRootPath + LocFilePaths.DIR_Projects_Doc + Project.Name);

                #region Check Customer Directories
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                #endregion Check Customer Directories


                foreach (var item in files)
                {
                    string extension = "";
                    var _filename = item.FileName.Split('.');
                    if (_filename.Count() == 1)
                        extension = Path.GetExtension(item.FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, item.FileName + extension), FileMode.Create))
                    {
                        item.CopyTo(fileStream);
                        var Document = new ProjectDocuments()
                        {
                            ProjectsId = Project.Id,
                            FileName = item.FileName + extension,
                        };
                        _uow.ProjectDocuments.Add(Document);
                    }
                }
                _uow.Save();
            }
            return Redirect("/proje-detaylari/" + projectDetailVM.Project.Id);
        }
        [HttpGet("proje-detaylari/dokuman-indir/{id}")]
        public IActionResult DownloadFile(string id)
        {
            string webRootPath = _hostEnvironment.WebRootPath;
            string contentPath = _hostEnvironment.ContentRootPath;
            var Document = _uow.ProjectDocuments.GetFirstOrDefault(i => i.Id == Guid.Parse(id), includeProperties: "Projects");

            //string fileName = Guid.NewGuid().ToString();
            var downloads = Path.Combine(webRootPath, LocFilePaths.DIR_Projects_Doc + Document.Projects.Name);
            
            using (var fileStream = new FileStream(Path.Combine(downloads, Document.FileName), FileMode.Open, System.IO.FileAccess.Read))
            {
                fileStream.Close();
                //Response.Headers.Add("content-disposition", "attachment; filename=" + Document.FileName);
                return File(fileStream, ContentTypes.Jpeg,Document.FileName); // or "application/x-rar-compressed"
                //return File(fileStream, "application/octet-stream"); // or "application/x-rar-compressed"
            }
            /*
            var response = HttpContext.Response;
            response.ContentType = ContentType;
            context.HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + FileDownloadName });
            using (var fileStream = new FileStream(FilePath, FileMode.Open))
            {
                await fileStream.CopyToAsync(context.HttpContext.Response.Body);
            }*/
            return null;
        }
        [HttpGet("proje-detaylari/dokuman-kaldir/{id}")]
        public IActionResult RemoveDocument(string id)
        {
            var Document = _uow.ProjectDocuments.GetFirstOrDefault(i => i.Id == Guid.Parse(id));
            var project = _uow.Project.GetFirstOrDefault(i => i.Id == Document.ProjectsId);
            var loc = LocFilePaths.DIR_Projects_Doc + project.Name + @"\" + Document.FileName;
            _uow.ProjectDocuments.Remove(Document);
            _uow.Save();
            if (System.IO.File.Exists(loc))
            {
                System.IO.File.Delete(loc);
            }
            /*
            if (Directory.Exists(LocFilePaths.DIR_Projects_Doc + project.Name+ @"\"+Document.FileName))
                Directory.Delete(LocFilePaths.DIR_Projects_Doc + project.Name + @"\" + Document.FileName);*/


            return Redirect("/proje-detaylari/" + project.Id);
        }
        [HttpGet("proje-detaylari/gorev-degistir/{id}")]
        public IActionResult ChangTask(ProjectDetailVM ProjectDetailVM)
        {

            //return Redirect("/proje-detaylari/" + project.Id);
            return Redirect("/");
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("jsonresult/updatedescription")]
        public JsonResult UpdateDesc(ProjectDetailVM _projectDetailVM)
        {
            string id = Convert.ToString(_projectDetailVM.Project.Id);
            string description = _projectDetailVM.Project.Description;
            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id), includeProperties: "Customers");
            _project.Description = description;
            _uow.Project.Update(_project);
            _uow.Save();
            //return Json("/proje-detaylari/"+id+ "&updated=" + true);
            return Json(_project.Description);
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
                    item.IsDelayed = true;
                    if(item.Status<3)
                        Delayeds++;
                }
                else
                    item.ProjectSequence = 2;
                _projects.Add(item);
            }
            var ProjectListVM = new ProjectListVM()
            {
                Projects = _projects,
                DelayedProjects = Delayeds
            };

            return Json(ProjectListVM);
        }
        [HttpGet("jsonresult/getprojectdetail/{id}")]
        public JsonResult GetProjectDetail(string id)
        {
            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id), includeProperties: "Customers");
            return Json(_project);
        }
        [HttpGet("jsonresult/deleteproject/{id}")]
        public async Task<JsonResult> DeleteProject(string id)
        {
            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id));
            _uow.Project.Remove(_project);
            _uow.Save();
            WebSocketActionExtensions WebSocAct = new WebSocketActionExtensions(_context, _uow);
            await WebSocAct.ListProjects_WebSocket();
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
        [HttpPost("jsonresult/updateTasks")]
        public JsonResult UpdateTasks([FromBody]List<ProjectTasks> projectTasks)
        {
            var ProjectId = _uow.ProjectTasks.GetFirstOrDefault(i=>i.Id== projectTasks[0].Id).ProjectsId;
            List<ProjectTasks> projectTasksList = new List<ProjectTasks>();
            foreach (var item in projectTasks)
            {
                var _projectTask = _uow.ProjectTasks.GetFirstOrDefault(i => i.Id == item.Id);
                _projectTask.Done = item.Done;
                projectTasksList.Add(_projectTask);
            }
            _uow.ProjectTasks.UpdateRange(projectTasksList);
            _uow.Save();

            return Json(projectTasksList);
        }
        [HttpGet("jsonresult/changeToNewState/{id}")]
        public async Task<JsonResult> ChangetoNewState(string id)
        {
            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id));
            _project.Status = 0;
            _uow.Project.Update(_project);
            _uow.Save();
            WebSocketActionExtensions WebSocAct = new WebSocketActionExtensions(_context, _uow);
            await WebSocAct.ListProjects_WebSocket();
            return Json(null);
        }
        [HttpGet("jsonresult/changeToConstructionState/{id}")]
        public async Task<JsonResult> ChangetoConstructionState(string id)
        {
            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id));
            _project.Status = 1;
            _uow.Project.Update(_project);
            _uow.Save();
            WebSocketActionExtensions WebSocAct = new WebSocketActionExtensions(_context, _uow);
            await WebSocAct.ListProjects_WebSocket();
            return Json(null);
        }
        [HttpGet("jsonresult/changeToCustomerApproveState/{id}")]
        public async Task<JsonResult> ChangetoCustomerApprove(string id)
        {
            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id));
            _project.Status = 2;
            _uow.Project.Update(_project);
            _uow.Save();
            WebSocketActionExtensions WebSocAct = new WebSocketActionExtensions(_context, _uow);
            await WebSocAct.ListProjects_WebSocket();
            return Json(null);
        }
        [HttpGet("jsonresult/changeToDoneState/{id}")]
        public async Task<JsonResult> ChangetoDone(string id)
        {
            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id));
            _project.Status = 3;
            _uow.Project.Update(_project);
            _uow.Save();
            WebSocketActionExtensions WebSocAct = new WebSocketActionExtensions(_context, _uow);
            await WebSocAct.ListProjects_WebSocket();
            return Json(null);
        }
        #endregion API

    }
}
