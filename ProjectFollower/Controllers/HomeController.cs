﻿using Microsoft.AspNetCore.Authentication;
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
using System.Threading;

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
        private ProjectTasks ProjectTaskItem = new ProjectTasks();
        public List<NotificationVM> NotificationVMs = new List<NotificationVM>();
        public IEnumerable<NotificationVM> INotifications;
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
        public string Nullorempty { get; private set; }

        public int Sequence = 0;
        public int Delayeds = 0;
        public IActionResult Index()
        {
            #region Authentication Index
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                bool archived = Convert.ToBoolean(Request.Cookies["_kj6ght"]);

                if (archived == null)
                {
                    Response.Cookies.Append("_kj6ght", "False");
                }
                Response.Cookies.Append("_nb59x45k9", GetClaim().Value);
                return RedirectToAction("Dashboard", "Home");//Go Dashboard
            }

            #endregion Authentication Index

            return RedirectToAction("Index", "SignIn");
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            var AppUser = _uow.ApplicationUser.GetFirstOrDefault(id => id.Id == GetClaim().Value);
            Response.Cookies.Append("_nb59x45k9", GetClaim().Value);
            List<DepartmentsVM> departmentsVM = new List<DepartmentsVM>();
            var GetDepartments = _uow.Department.GetAll();
            foreach (var item in GetDepartments)
            {
                var UserWidtDep = _uow.ApplicationUser.GetAll(i => i.DepartmentId == item.Id).Where(a => a.Active);
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
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [Route("proje-detaylari/{id}")]
        public IActionResult Details(string id)
        {
            List<Users> _users = new List<Users>();
            List<CommentVM> _commentList = new List<CommentVM>();
            List<DepartmentsVM> _departmentsVMs = new List<DepartmentsVM>();
            List<DocumentVM> documentVMs = new List<DocumentVM>();
            List<ProjectTasks> _projectTasks = new List<ProjectTasks>();

            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id), includeProperties: "Customers");
            var _tasks = _uow.ProjectTasks.GetAll(i => i.ProjectsId == Guid.Parse(id));
            var _documents = _uow.ProjectDocuments.GetAll(i => i.ProjectsId == Guid.Parse(id));
            var _comments = _uow.ProjectComments.GetAll(i => i.ProjectsId == Guid.Parse(id));
            var _responsibles = _uow.ResponsibleUsers.GetAll(i => i.ProjectId == Guid.Parse(id));
            foreach (var item in _documents)
            {
                var _documentVM = new DocumentVM()
                {
                    Id = item.Id,
                    FileName = item.FileName,
                    ProjectsId = item.ProjectsId,
                    Length = item.Length,
                    Length_MB = (item.Length / 1024) / 1024
                };
                documentVMs.Add(_documentVM);
            }
            foreach (var item in _responsibles)
            {
                var AppUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == item.UserId.ToString(), includeProperties: "Department");
                var ImageUrl = WebRootPaths.DIR_Users_Main + AppUser.Id + "/" + WebRootPaths.Img + AppUser.ImageUrl;
                if (AppUser.ImageUrl == "")
                    ImageUrl = WebRootPaths.EmptyAvatar;
                var User = new Users()
                {
                    Id = AppUser.Id,
                    FirstName = AppUser.FirstName,
                    LastName = AppUser.Lastname,
                    FullName = AppUser.FirstName + " " + AppUser.Lastname,
                    Email = AppUser.Email,
                    Department = AppUser.Department,
                    DepartmentId = AppUser.DepartmentId,
                    AppUserName = AppUser.AppUserName,
                    ImageUrl = ImageUrl
                };
                _users.Add(User);
            }
            foreach (var item in _comments)
            {
                var UserItem = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == item.UserId);
                var ImageUrl = WebRootPaths.DIR_Users_Main + UserItem.Id + "/" + WebRootPaths.Img + UserItem.ImageUrl;
                if (UserItem.ImageUrl == "")
                    ImageUrl = WebRootPaths.EmptyAvatar;
                var commentItem = new CommentVM()
                {
                    Id = item.Id,
                    Comment = item.Comment,
                    CommentTime = item.CommentTime.ToString("f"),
                    FullName = UserItem.FirstName + " " + UserItem.Lastname,
                    Img = ImageUrl,
                    UserEmail = UserItem.Email
                };
                _commentList.Add(commentItem);
            }
            var _responsibleUsersSelected = _uow.ResponsibleUsers.GetAll(i => i.ProjectId == Guid.Parse(id));
            var _alldepartments = _uow.Department.GetAll();

            foreach (var item in _alldepartments)
            {
                var DepartmentUsers = _uow.ApplicationUser.GetAll(i => i.DepartmentId == item.Id);
                List<Users> UserList = new List<Users>();
                foreach (var useritem in DepartmentUsers)
                {
                    var _responsible = _uow.ResponsibleUsers.GetAll(i => i.ProjectId == Guid.Parse(id)).Where(i => i.UserId == Guid.Parse(useritem.Id));
                    if (_responsible.Count() > 0)
                    {
                        var _user = new Users()
                        {
                            Id = useritem.Id,
                            FirstName = useritem.FirstName,
                            LastName = useritem.Lastname,
                            IsResponsible = true

                        };
                        UserList.Add(_user);
                    }
                    else
                    {
                        var _user = new Users()
                        {
                            Id = useritem.Id,
                            FirstName = useritem.FirstName,
                            LastName = useritem.Lastname,
                            IsResponsible = false

                        };
                        UserList.Add(_user);
                    }


                }
                var _appUser = _uow.ApplicationUser.GetAll(i => i.DepartmentId == item.Id).Where(s => s.Active);
                DepartmentsVM departmentsVM = new DepartmentsVM()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Users = UserList,
                    ApplicationUser = _appUser

                };
                _departmentsVMs.Add(departmentsVM);
            }
            foreach (var item in _tasks)
            {
                var _players = _uow.TaskPlayers.GetAll(i => i.ProjectTaskId == item.Id.ToString()).ToList();
                List<TaskPlayers> _taskplayers = new List<TaskPlayers>();
                foreach (var playerItem in _players)
                {
                    var username = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == playerItem.UserId);
                    var taskPlayerItem = new TaskPlayers()
                    {
                        Id = playerItem.Id,
                        FirstName = username.FirstName,
                        LastName = username.Lastname,
                        ProjectTaskId = playerItem.ProjectTaskId,
                        UserId = playerItem.UserId
                    };
                    _taskplayers.Add(taskPlayerItem);
                }
                item.TaskPlayers = _taskplayers;
                _projectTasks.Add(item);
            }
            var _projectDetailVM = new ProjectDetailVM()
            {
                Project = _project,
                ProjectTasks = _projectTasks,
                ProjectDocuments = _documents,
                CommentVM = _commentList,
                Users = _users,
                DepartmentsVMs = _departmentsVMs,
                ResponsibleUsers = _responsibleUsersSelected
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
                var UserWidtDep = _uow.ApplicationUser.GetAll(i => i.DepartmentId == item.Id).Where(a => a.Active);
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
                var _notify = new NotificationVM()
                {
                    UserId = User.Id,
                    Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                    Title = "Yeni Bir Proje Eklendi",
                    Message = "Adınıza yeni bir proje açıldı. Detaylar için tıklayınız.",
                    ProjectId = ProjectVM.Id.ToString(),
                    Url = "/proje-detaylari/" + ProjectVM.Id,
                };
                NotificationVMs.Add(_notify);
                _uow.ResponsibleUsers.Add(ResponsibleUser);
            }
            INotifications = NotificationVMs;
            if (ProjectVM.TaskDesc == null)
                return NoContent();
            int t = 0;
            foreach (var item in ProjectVM.TaskDesc)
            {

                if (t % 2 == 0)
                {
                    var ProjectTask = new ProjectTasks()
                    {
                        Description = item,
                        ProjectsId = Project.Id
                    };
                    _uow.ProjectTasks.Add(ProjectTask);
                    //_uow.Save();
                    ProjectTaskItem = ProjectTask;
                }
                else
                {
                    string[] ids = item.Split(',');
                    foreach (var itemm in ids)
                    {
                        var TaskPlayer = new TaskPlayers()
                        {
                            ProjectTaskId = ProjectTaskItem.Id.ToString(),
                            UserId = itemm
                        };
                        _uow.TaskPlayers.Add(TaskPlayer);
                    }
                }
                t++;
            }



            string webRootPath = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            if (files.Count() > 0)
            {
                //string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, webRootPath + LocFilePaths.DIR_Projects_Doc + Project.Id);

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
                            Length = item.Length
                        };
                        _uow.ProjectDocuments.Add(Document);
                    }
                }
            }
            foreach (var item in NotificationVMs)
            {
                var _notify = new Notifications()
                {
                    Date = item.Date,
                    Message = "Adınıza " + ProjectVM.Customers.Name + " Firmasına ait " + ProjectVM.Name + " adında bir proje açıldı. <a href='/proje-detaylari/" + Project.Id.ToString() + "'>Projeye git</a>",
                    Title = item.Title,
                    UserId = item.UserId,
                    ProjectId = Project.Id.ToString(),
                    Url = "/proje-detaylari/" + ProjectVM.Id,
                };
                _uow.Notifications.Add(_notify);
            }
            //_uow.Notifications.AddRange((IEnumerable<Notifications>)NotificationVMs); TEST
            _uow.Save();


            WebSocketActionExtensions WebSocAct = new WebSocketActionExtensions(_context, _uow);
            await WebSocAct.SendNotification_WebSocket(GetClaim(), INotifications);

            //return NoContent();

            return Redirect("/dashboard?status=true");
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [HttpPost("proje-detaylari/dokuman-guncelle")]
        public IActionResult UpdateDocuments(ProjectDetailVM projectDetailVM)
        {
            string webRootPath = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var Project = _uow.Project.GetFirstOrDefault(i => i.Id == projectDetailVM.Project.Id);
            if (files.Count() > 0)
            {
                //string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, webRootPath + LocFilePaths.DIR_Projects_Doc + Project.Id);

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
                    long length = 0;
                    using (var fileStream = new FileStream(Path.Combine(uploads, item.FileName + extension), FileMode.Create))
                    {

                        item.CopyTo(fileStream);
                        var Document = new ProjectDocuments()
                        {
                            ProjectsId = Project.Id,
                            FileName = item.FileName + extension,
                            Length = item.Length
                        };
                        _uow.ProjectDocuments.Add(Document);
                    }
                }
                _uow.Save();
            }
            return Redirect("/proje-detaylari/" + projectDetailVM.Project.Id);
        }


        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [HttpGet("proje-detaylari/dokuman-indir/{id}")]
        public IActionResult DownloadFile(string id)
        {
            string webRootPath = _hostEnvironment.WebRootPath;
            string contentPath = _hostEnvironment.ContentRootPath;
            var Document = _uow.ProjectDocuments.GetFirstOrDefault(i => i.Id == Guid.Parse(id), includeProperties: "Projects");

            //string fileName = Guid.NewGuid().ToString();
            var downloads = Path.Combine(webRootPath, LocFilePaths.DIR_Projects_Doc + Document.Projects.Id);

            using (var fileStream = new FileStream(Path.Combine(downloads, Document.FileName), FileMode.Open, System.IO.FileAccess.Read))
            {
                fileStream.Close();
                //Response.Headers.Add("content-disposition", "attachment; filename=" + Document.FileName);
                return File(fileStream, ContentTypes.Jpeg, Document.FileName); // or "application/x-rar-compressed"
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
        }


        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
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


        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [HttpPost]
        public async Task<IActionResult> AddTask(ProjectDetailVM projectDetailVM)
        {
            List<NotificationVM> notificationVMs = new List<NotificationVM>();
            var _appUserId = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == GetClaim().Value).Id;
            /*
            if (!(User.IsInRole(UserRoles.Admin)))
            {
                var player = new TaskPlayers()
                {
                    ProjectTaskId = projectTask.Id.ToString(),
                    UserId = item
                };
                _uow.TaskPlayers.Add(player);
            }*/
            var projectTask = new ProjectTasks()
            {
                ProjectsId = projectDetailVM.ProjectsId,
                Description = projectDetailVM.ProjectTaskItem.Description
            };
            _uow.ProjectTasks.Add(projectTask);
            if (!(User.IsInRole(UserRoles.Admin)))
            {
                var player = new TaskPlayers()
                {
                    ProjectTaskId = projectTask.Id.ToString(),
                    UserId = _appUserId,
                };
                _uow.TaskPlayers.Add(player);
                _uow.Save();
            }
            else
            {
                foreach (var item in projectDetailVM.UserId)
                {
                    var player = new TaskPlayers()
                    {
                        ProjectTaskId = projectTask.Id.ToString(),
                        UserId = item
                    };
                    var _notify = new Notifications()
                    {
                        UserId = player.UserId,
                        Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                        Title = "Yeni Bir Görev Eklendi",
                        Message = "Adınıza yeni bir görev eklendi. Detaylar için tıklayınız.",
                        ProjectId = projectTask.ProjectsId.ToString(),
                        Url = "/proje-detaylari/" + projectDetailVM.ProjectsId.ToString()
                    };
                    var _notifyVM = new NotificationVM()
                    {
                        UserId = _notify.UserId,
                        Date = _notify.Date,
                        Title = _notify.Title,
                        Message = _notify.Message,
                        ProjectId = _notify.ProjectId,
                        Url = "/proje-detaylari/" + projectDetailVM.ProjectsId.ToString()

                    };
                    _uow.Notifications.Add(_notify);
                    _uow.TaskPlayers.Add(player);
                    notificationVMs.Add(_notifyVM);
                }
                _uow.Save();
                WebSocketActionExtensions WebSocAct = new WebSocketActionExtensions(_context, _uow);
                await WebSocAct.SendNotification_WebSocket(GetClaim(), notificationVMs);
            }
            return Redirect("/proje-detaylari/" + projectDetailVM.ProjectsId);
            //return NoContent();
        }
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [HttpGet("proje-detaylari/gorev-kaldir/{id}")]
        public IActionResult RemoveTask(string id)
        {
            var authorized = false;
            var task = _uow.ProjectTasks.GetFirstOrDefault(i => i.Id == Guid.Parse(id));
            var players = _uow.TaskPlayers.GetAll(i => i.ProjectTaskId == task.Id.ToString());
            var project = _uow.Project.GetFirstOrDefault(i => i.Id == task.ProjectsId);
            var _user = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == GetClaim().Value);
            foreach (var item in players)
            {
                if (item.UserId == _user.Id || _user.UserRole == "Yönetici")
                    authorized = true;
            }
            if (authorized)
            {
                _uow.TaskPlayers.RemoveRange(players);
                _uow.ProjectTasks.Remove(task);
                _uow.Save();
            }

            return Redirect("/proje-detaylari/" + project.Id);
        }


        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("proje-detaylari/gorev-degistir/{id}")]
        public IActionResult ChangTask(ProjectDetailVM ProjectDetailVM)
        {

            //return Redirect("/proje-detaylari/" + project.Id);
            return Redirect("/");
        }


        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [HttpPost("jsonresult/deletecomment/")]
        public IActionResult DeleteComment(string id)
        {
            var appUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == GetClaim().Value);
            var comment = _uow.ProjectComments.GetFirstOrDefault(i => i.Id == Guid.Parse(id));
            if (comment.UserId == appUser.Id || User.IsInRole(UserRoles.Admin))
            {
                var project = _uow.Project.GetFirstOrDefault(i => i.Id == comment.ProjectsId);
                _uow.ProjectComments.Remove(comment);
                _uow.Save();
                return Redirect("/proje-detaylari/" + project.Id.ToString());
            }
            return NoContent();

        }
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [Route("notification")]
        public IActionResult Notification()
        {
            return View();
        }
        #region API
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager)]
        [HttpPost("jsonresult/updatedescription")]
        public JsonResult UpdateDesc(ProjectDetailVM _projectDetailVM)
        {
            string id = Convert.ToString(_projectDetailVM.Project.Id);
            string description = _projectDetailVM.Project.Description;
            string endingDate = _projectDetailVM.Project.EndingDate;
            string projectName = _projectDetailVM.Project.Name;
            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id), includeProperties: "Customers");
            _project.Description = description;
            _project.EndingDate = endingDate;
            _project.Name = projectName;
            var responsibles = _uow.ResponsibleUsers.GetAll(i => i.ProjectId == _project.Id);
            _uow.ResponsibleUsers.RemoveRange(responsibles);
            foreach (var item in _projectDetailVM.UserId)
            {
                var _responsibles = new ResponsibleUsers()
                {
                    ProjectId = _project.Id,
                    UserId = Guid.Parse(item)
                };
                _uow.ResponsibleUsers.Add(_responsibles);
            }

            _uow.Project.Update(_project);
            _uow.Save();
            //return Json("/proje-detaylari/"+id+ "&updated=" + true);
            return Json(_project.Description);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [HttpGet("jsonresult/getprojectlist/{ArchiveStatus}")]
        public JsonResult ListJson(bool ArchiveStatus)
        {
            IEnumerable<Projects> Projects;
            List<Projects> _projects = new List<Projects>();
            Projects _projectItem = new Projects();
            List<ProjectListVM> ProjectListVMs = new List<ProjectListVM>();
            ApplicationUser AppUser = new ApplicationUser();

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                AppUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
            }
            else
                return Json(null);
            if (ArchiveStatus == false)
            {
                Projects = _uow.Project.GetAll(i => i.Archived == false, includeProperties: "Customers");
            }
            else if (ArchiveStatus)
            {
                Projects = _uow.Project.GetAll(includeProperties: "Customers");
            }
            else
                return Json(null);


            if (User.IsInRole(UserRoles.Personel))
            {
                var Responsibles = _uow.ResponsibleUsers.GetAll(i => i.UserId == Guid.Parse(AppUser.Id));
                foreach (var item in Responsibles)
                {
                    _projectItem = _uow.Project.GetFirstOrDefault(i => i.Id == item.ProjectId);
                    if (ArchiveStatus == false && _projectItem.Archived == false)
                    {
                        _projectItem.SequanceDate = Sequence++;
                        if (DateTime.Now.Date > Convert.ToDateTime(_projectItem.EndingDate))
                        {
                            _projectItem.ProjectSequence = 1;
                            _projectItem.IsDelayed = true;
                            if (_projectItem.Status < 3)
                                Delayeds++;
                        }
                        else
                            _projectItem.ProjectSequence = 2;

                        _projects.Add(_projectItem);
                    }
                    else if (_projectItem.Archived == false || (_projectItem.Archived == true && ArchiveStatus == true))
                    {
                        _projectItem.SequanceDate = Sequence++;
                        if (DateTime.Now.Date > Convert.ToDateTime(_projectItem.EndingDate))
                        {
                            _projectItem.ProjectSequence = 1;
                            _projectItem.IsDelayed = true;
                            if (_projectItem.Archived)
                                _projectItem.Status = 4;
                            else
                            {
                                if (_projectItem.Status < 3)
                                    Delayeds++;
                            }

                        }
                        if (_projectItem.Archived)
                            _projectItem.Status = 4;
                        else
                            _projectItem.ProjectSequence = 2;

                        _projects.Add(_projectItem);
                    }
                }
                var FilteredProject = _projects.OrderBy(d => Convert.ToDateTime(d.EndingDate));
                /*var ProjectList = new List<Projects>();
                ProjectList.AddRange(FilteredProject);*/
                Thread.Sleep(150);
                var _ProjectListVM = new ProjectListVM()
                {
                    Projects = FilteredProject.ToList(),
                    DelayedProjects = Delayeds
                };

                return Json(_ProjectListVM);
            }
            else
            {
                var FilteredProject = Projects.OrderBy(d => Convert.ToDateTime(d.EndingDate));
                foreach (var item in FilteredProject)
                {
                    item.SequanceDate = Sequence++;
                    if (DateTime.Now.Date > Convert.ToDateTime(item.EndingDate))
                    {
                        item.ProjectSequence = 1;
                        item.IsDelayed = true;
                        if (item.Archived)
                            item.Status = 4;
                        else
                        {
                            if (_projectItem.Status < 3)
                                Delayeds++;
                        }
                    }
                    else
                        item.ProjectSequence = 2;

                    if (item.Archived)
                        item.Status = 4;
                    _projects.Add(item);
                }
            }

            var ProjectListVM = new ProjectListVM()
            {
                Projects = _projects,
                DelayedProjects = Delayeds
            };

            return Json(ProjectListVM);
        }
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [HttpGet("jsonresult/getprojectdetail/{id}")]
        public JsonResult GetProjectDetail(string id)
        {
            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id), includeProperties: "Customers");
            return Json(_project);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("jsonresult/deleteproject/{id}")]
        public JsonResult DeleteProject(string id)
        {
            List<TaskPlayers> _taskplayers = new List<TaskPlayers>();
            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id));
            var _responsibles = _uow.ResponsibleUsers.GetAll(i => i.ProjectId == Guid.Parse(id));
            var tasks = _uow.ProjectTasks.GetAll(i => i.ProjectsId == _project.Id);

            foreach (var item in tasks)
            {
                var players = _uow.TaskPlayers.GetAll(i => i.ProjectTaskId == item.Id.ToString());
                _taskplayers.AddRange(players);
            }
            _uow.TaskPlayers.RemoveRange(_taskplayers);
            _uow.Project.Remove(_project);
            _uow.ResponsibleUsers.RemoveRange(_responsibles);
            _uow.Save();
            return Json(null);
        }
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
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
        public JsonResult UpdateTasks([FromBody] List<ProjectTasks> projectTasks)
        {
            bool authorized = false;
            bool authorizedByOne = false;
            var user = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == GetClaim().Value);
            var ProjectId = _uow.ProjectTasks.GetFirstOrDefault(i => i.Id == projectTasks[0].Id).ProjectsId;
            List<ProjectTasks> projectTasksList = new List<ProjectTasks>();
            foreach (var item in projectTasks)
            {
                var _projectTask = _uow.ProjectTasks.GetFirstOrDefault(i => i.Id == item.Id);
                var _taskplayers = _uow.TaskPlayers.GetAll(i => i.ProjectTaskId == item.Id.ToString());
                authorized = false;
                foreach (var _taskplayer in _taskplayers)
                {
                    if (_taskplayer.UserId == GetClaim().Value || user.UserRole == "Yönetici")
                    {
                        authorizedByOne = true;
                        authorized = true;
                    }

                }
                if (authorized)
                {
                    _projectTask.Done = item.Done;
                    projectTasksList.Add(_projectTask);
                }
            }
            if (authorizedByOne)
            {
                _uow.ProjectTasks.UpdateRange(projectTasksList);
                _uow.Save();
            }
            else
            {
                return Json("Görevler üzerinde değişiklik yapma yetkiniz bulunmamaktadır!");
            }


            return Json(projectTasksList);
        }
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [HttpGet("jsonresult/changearcstatus/{archived}")]
        public JsonResult ChangeArciveStatus(bool archived)
        {
            Response.Cookies.Append("_kj6ght", archived.ToString());
            return Json(archived);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("jsonresult/changeToArchiveState/{id}")]
        public JsonResult ChangetoArchiveState(string id)
        {
            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id));
            if (_project.Status == 3 && _project.Archived == false)
            {
                _project.Status = 4;
                _project.Archived = true;
                _uow.Project.Update(_project);
                _uow.Save();
                var _ModalMessage = new ModalMessageVM()
                {
                    Message = "Başarıyla güncellendi.",
                    Icon = "success",
                    Status = true
                };
                return Json(_ModalMessage);
            }
            else
            {
                var _ModalMessage = new ModalMessageVM()
                {
                    Message = "Proje arşivlenmeye uygun değildir.",
                    Icon = "warning",
                    Status = true
                };
                return Json(_ModalMessage);
            }
        }
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [HttpGet("jsonresult/changeToNewState/{id}")]
        public JsonResult ChangetoNewState(string id)
        {
            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id));
            _project.Status = 0;
            _project.Archived = false;
            _uow.Project.Update(_project);
            _uow.Save();
            return Json(null);
        }
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [HttpGet("jsonresult/changeToConstructionState/{id}")]
        public JsonResult ChangetoConstructionState(string id)
        {
            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id));
            if (User.IsInRole(UserRoles.Admin))
            {
                _project.Status = 1;
                _project.Archived = false;
                _uow.Project.Update(_project);
                _uow.Save();
            }
            if (_project.Status == 0 && User.IsInRole(UserRoles.Personel))
            {
                _project.Status = 1;
                _project.Archived = false;
                _uow.Project.Update(_project);
                _uow.Save();
            }
            return Json(null);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("jsonresult/changeToCustomerApproveState/{id}")]
        public JsonResult ChangetoCustomerApprove(string id)
        {
            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id));
            _project.Status = 2;
            _project.Archived = false;
            _uow.Project.Update(_project);
            _uow.Save();
            return Json(null);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("jsonresult/changeToDoneState/{id}")]
        public JsonResult ChangetoDone(string id)
        {
            int s = 0;
            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id));
            var _tasks = _uow.ProjectTasks.GetAll(i => i.ProjectsId == _project.Id);
            foreach (var item in _tasks)
            {
                if (item.Done)
                    s++;
            }
            if (_tasks.Count() == s)
            {

                _project.Status = 3;
                _project.Archived = false;
                _uow.Project.Update(_project);
                _uow.Save();
                var _message = new ModalMessageVM()
                {
                    Icon = "success",
                    Status = true,
                    Message = "Proje başarıyla güncellendi."
                };
                return Json(_message);
            }
            else
            {
                var _message = new ModalMessageVM()
                {
                    Icon = "warning",
                    Status = false,
                    Message = "Görevler tamamlanmadan proje tamamlandı statüsüne alınamaz!"
                };
                return Json(_message);
            }


        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("jsonresult/changeToAwait/{id}")]
        public JsonResult ChangetoAwait(string id)
        {
            var _project = _uow.Project.GetFirstOrDefault(i => i.Id == Guid.Parse(id));
            _project.Status = 6;
            _project.Archived = false;
            _uow.Project.Update(_project);
            _uow.Save();
            return Json(null);
        }
        [HttpPost("jsonresult/addcomment/")]
        public async Task<JsonResult> AddComments(ProjectComments projectComments)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //CommentVM commentVM = new CommentVM();
            if (String.IsNullOrEmpty(projectComments.Comment))
            {
                return Json(null);
            }
            if (Claims != null)
            {
                var AppUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                projectComments.UserId = AppUser.Id;
                projectComments.CommentTime = DateTime.Now;
                _uow.ProjectComments.Add(projectComments);
                var userpath = WebRootPaths.DIR_Users_Main + AppUser.Id + "/" + WebRootPaths.Img + AppUser.ImageUrl;
                if (AppUser.ImageUrl == "")
                    userpath = WebRootPaths.EmptyAvatar;

                var Comment = new CommentVM()
                {
                    UserId = AppUser.Id,
                    Comment = projectComments.Comment,
                    CommentTime = projectComments.CommentTime.ToString("F"),
                    Img = userpath,
                    FullName = AppUser.FirstName + " " + AppUser.Lastname,
                    UserEmail = AppUser.Email
                };
                var respUsers = _uow.ResponsibleUsers.GetAll(i => i.ProjectId == projectComments.ProjectsId);
                List<NotificationVM> _notificationList = new List<NotificationVM>();
                foreach (var item in respUsers)
                {
                    if (AppUser.Id != item.UserId.ToString())
                    {
                        var _notify = new NotificationVM()
                        {
                            UserId = item.UserId.ToString(),
                            Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                            ProjectId = projectComments.ProjectsId.ToString(),
                            Title = "Yeni Bir Yorum Eklendi",
                            Message = "Bulunduğunuz bir projeye birisi yorum yaptı."
                        };
                        _notificationList.Add(_notify);
                    }
                }
                WebSocketActionExtensions WebSocAct = new WebSocketActionExtensions(_context, _uow);
                await WebSocAct.SendNotification_WebSocket(GetClaim(), _notificationList);
                _uow.Save();

                return Json(Comment);

                //_uow.ProjectComments.Add(ProjectComments);
            }
            return Json(null);
            //var project = new Projects();


        }
        [HttpGet("jsonresult/getallusers")]
        public JsonResult GetAllUsers()
        {
            List<Users> _Users = new List<Users>();


            #region Authentication Index
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                var AllUsers = _uow.ApplicationUser.GetAll(a => a.Active, includeProperties: "Department");
                foreach (var item in AllUsers)
                {
                    var ImageUrl = WebRootPaths.DIR_Users_Main + item.Id + "/" + WebRootPaths.Img + item.ImageUrl;
                    Users Useritem = new Users()
                    {
                        Id = item.Id,
                        AppUserName = item.AppUserName,
                        DepartmentId = item.DepartmentId,
                        Department = item.Department,
                        FullName = item.FirstName + " " + item.Lastname,
                        IdentityNumber = item.IdentityNumber,
                        Email = item.Email,
                        ImageUrl = ImageUrl,
                        Role = item.Role
                    };
                    _Users.Add(Useritem);
                }
            }
            else
                return Json(StatusCode(404));
            #endregion Authentication Index



            //_Users.AddRange((IEnumerable<Users>)(Users)users);


            return Json(_Users);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("jsonresult/getuserbyfilter/{id}/{arcstatus}")]
        public JsonResult GetUserByFilter(string id, bool ArchiveStatus)
        {
            List<Projects> _projects = new List<Projects>();
            Projects _projectItem = new Projects();
            List<ProjectListVM> ProjectListVMs = new List<ProjectListVM>();
            ApplicationUser AppUser = new ApplicationUser();
            try
            {
                var Responsibles = _uow.ResponsibleUsers.GetAll(i => i.UserId == Guid.Parse(id));
                foreach (var item in Responsibles)
                {
                    _projectItem = _uow.Project.GetFirstOrDefault(i => i.Id == item.ProjectId, includeProperties: "Customers");
                    if (_projectItem != null)
                    {
                        if (ArchiveStatus == false && _projectItem.Archived == false)
                        {
                            _projectItem.SequanceDate = Sequence++;
                            if (DateTime.Now.Date > Convert.ToDateTime(_projectItem.EndingDate))
                            {
                                _projectItem.ProjectSequence = 1;
                                _projectItem.IsDelayed = true;
                                if (_projectItem.Status < 3)
                                    Delayeds++;
                            }
                            else
                                _projectItem.ProjectSequence = 2;

                            _projects.Add(_projectItem);
                        }
                        else if (_projectItem.Archived == false || (_projectItem.Archived == true && ArchiveStatus == true))
                        {
                            _projectItem.SequanceDate = Sequence++;
                            if (DateTime.Now.Date > Convert.ToDateTime(_projectItem.EndingDate))
                            {
                                _projectItem.ProjectSequence = 1;
                                _projectItem.IsDelayed = true;
                                if (_projectItem.Archived)
                                    _projectItem.Status = 4;
                                else
                                {
                                    if (_projectItem.Status < 3)
                                        Delayeds++;
                                }

                            }
                            if (_projectItem.Archived)
                                _projectItem.Status = 4;
                            else
                                _projectItem.ProjectSequence = 2;

                            _projects.Add(_projectItem);
                        }
                    }

                }

                /*var ProjectList = new List<Projects>();
                ProjectList.AddRange(FilteredProject);*/
                Thread.Sleep(150);

            }
            catch (Exception)
            {

                throw;
            }
            var FilteredProject = _projects.OrderBy(d => Convert.ToDateTime(d.EndingDate));
            var _ProjectListVM = new ProjectListVM()
            {
                Projects = FilteredProject.ToList(),
                DelayedProjects = Delayeds
            };
            return Json(_ProjectListVM);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("jsonresult/getstatusbyfilter/{id}")]
        public JsonResult GetStatusByFilter(int id)
        {
            var _FilteredProjects = new List<Projects>();
            var _projects = _uow.Project.GetAll(i => i.Status == id, includeProperties: "Customers");
            //var FilteredProject = _projects.OrderBy(d => Convert.ToDateTime(d.EndingDate));
            foreach (var item in _projects)
            {
                item.SequanceDate = Sequence++;
                if (DateTime.Now.Date > Convert.ToDateTime(item.EndingDate))
                {
                    item.ProjectSequence = 1;
                    item.IsDelayed = true;
                    if (item.Status < 3)
                        Delayeds++;
                }
                else
                    item.ProjectSequence = 2;
                var _project = new Projects()
                {
                    Id = item.Id,
                    Archived = item.Archived,
                    IsDelayed = item.IsDelayed,
                    CustomersId = item.CustomersId,
                    Customers = item.Customers,
                    Name = item.Name,
                    CreationDate = item.CreationDate,
                    EndingDate = item.EndingDate,
                    SequanceDate = item.SequanceDate,
                    Status = item.Status

                };
                _FilteredProjects.Add(_project);
            }
            var _ProjectListVM = new ProjectListVM()
            {
                Projects = _FilteredProjects,
                DelayedProjects = Delayeds
            };
            return Json(_ProjectListVM);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [HttpGet("jsonresult/refreshpage_socket/")]
        public async Task<JsonResult> WEB_SOCKET_REFRESH_PAGE(string id)
        {

            WebSocketActionExtensions WebSocAct = new WebSocketActionExtensions(_context, _uow);
            await WebSocAct.ListProjects_WebSocket(GetClaim());
            return Json(null);
        }
        //[Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [HttpGet("api/getnotify")]
        public async Task<IEnumerable<Notifications>> GetNotifications()
        {
            await Task.Delay(1);
            return _uow.Notifications.GetAll(i => i.UserId == GetClaim().Value).OrderByDescending(d => DateTime.Parse(d.Date)).OrderBy(o => o.Readed);

        }
        [HttpPost("api/updatenotify")]
        public JsonResult UpdateNotifications([FromBody] IEnumerable<Notifications> notification)
        {
            foreach (var item in notification)
            {
                _uow.Notifications.Update(item);
            }
            _uow.Save();
            return Json(null);

        }
        [HttpPost("api/removenotify")]
        public JsonResult RemoveNotifications([FromBody] IEnumerable<Notifications> notification)
        {
            _uow.Notifications.RemoveRange(notification);
            _uow.Save();
            return Json(null);

        }
        [HttpGet("api/checknotify")]
        public JsonResult CheckNotification()
        {
            _uow.Notifications.RemoveRange(_uow.Notifications.GetAll()
                .Where(d => DateTime.Parse(d.Date) <
                (DateTime.Now.AddDays(-4))).ToList());

            _uow.Save();
            return Json(null);
        }
        [HttpGet("api/release-note-readed")]
        public JsonResult VersionInformation()
        {
            var _user = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == GetClaim().Value);
            if (_user.InfoVerModal)
                return Json(_user.InfoVerModal);

            _user.InfoVerModal = true;
            _uow.ApplicationUser.Update(_user);
            _uow.Save();
            return Json(false);
        }
        #endregion API

        public Claim GetClaim()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                return Claims;
            }
            return null;
        }

    }
}
