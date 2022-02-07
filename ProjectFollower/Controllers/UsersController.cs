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
using ProjectFollower.Utility;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Http;

namespace ProjectFollower.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UsersController> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UsersController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<UsersController> logger,
        IUnitOfWork uow,
        IWebHostEnvironment hostEnvironment
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _uow = uow;
            _hostEnvironment = hostEnvironment;
        }
        [BindProperty]
        public UserRegisterVM Input { get; set; }

        [Route("users")]
        public IActionResult Index()
        {
            
            #region Authentication Index
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
            }
            else
                return View("SignIn");
            #endregion Authentication Index
            
            return View();
        }
        [Route("new-user")]
        public IActionResult NewUser(ModalMessageVM ModalMessage)
        {
            /*
            #region Authentication Index
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
            }
            else
                return View("SignIn");
            #endregion Authentication Index
            */
            if (ModalMessage.Status==true)
            {
                var addUser = new UserRegisterVM()
                {
                    ModalMessage = ModalMessage
                };

                return View(addUser);
            }

            
            var _ModalMessage = new ModalMessageVM()
            {
                Message = "",
                Icon = "",
                Status = false
            };
            var addUserStatus = new UserRegisterVM()
            {
                ModalMessage = _ModalMessage
            };
            return View(addUserStatus);

        }

        [HttpPost("new-user")]
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.Manager))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.Personel))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Personel));
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                var userpath = webRootPath + LocFileForWeb.DIR_Users_Main + Input.Email;

                Console.WriteLine(files.Count.ToString());
                //System.Diagnostics.Debug.WriteLine(files.ToString());
                
                if (files.Count() > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(userpath+@"\"+ LocFileForWeb.Img);


                    #region Check Users Directories
                    if (!(Directory.Exists(uploads)))
                        Directory.CreateDirectory(uploads);
                    /*
                    if (!(Directory.Exists(WebRootPaths.DIR_Users_Main)))
                        Directory.CreateDirectory(WebRootPaths.DIR_Users_Main);
                    if (!(Directory.Exists(WebRootPaths.DIR_Users_Img)))
                        Directory.CreateDirectory(WebRootPaths.DIR_Users_Img);*/
                    #endregion Check Users Directories


                    //var imageUrl = productVM.Product.ImageUrl;

                    var extension = Path.GetExtension(files[0].FileName);

                    /*
                    if (productVM.Product.ImageUrl != null)
                    {
                        var imagePath = Path.Combine(webRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }*/

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        var stream=files[0].OpenReadStream();
                        var image = Image.FromStream(stream);
                        var size = image.Size;
                        var width = (float)size.Width;
                        var height = (float)size.Height;

                        float rate = width / height;

                        if (width > 200 || height > 200)
                        {
                            ModelState.AddModelError(string.Empty, "Kullanıcı oluşturulamadı! Profil resmi 200px den fazla olamaz.");
                            ModalMessageVM ModalMessage = new ModalMessageVM()
                            {
                                Message = "Kullanıcı oluşturulamadı! Profil resmi 200px den fazla olamaz.",
                                Icon="warning",
                                Status = true
                            };
                            return RedirectToAction("NewUser",ModalMessage);
                        }
                        if (rate !=1)
                        {
                            ModelState.AddModelError(string.Empty, "Kullanıcı oluşturulamadı! Profil resmi 1:1 oranında olmalıdır. ");
                            ModalMessageVM ModalMessage = new ModalMessageVM()
                            {
                                Message = "Kullanıcı oluşturulamadı! Profil resmi 1:1 oranında olmalıdır. ",
                                Icon = "warning",
                                Status = true
                            };
                            return RedirectToAction("NewUser", ModalMessage);
                        }

                        files[0].CopyTo(fileStreams);
                    }
                    Input.ImageUrl = fileName+ extension;
                }
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    Lastname = Input.Lastname,
                    DepartmentId=Input.DepartmentId,
                    EmailConfirmed=true,
                    ImageUrl= Input.ImageUrl,
                    UserRole=UserRoles.Personel
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Kayıt işlemi yapıldı.");

                    if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                    }
                    if (!await _roleManager.RoleExistsAsync(UserRoles.Manager))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));
                    }
                    if (!await _roleManager.RoleExistsAsync(UserRoles.Personel))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.Personel));
                    }

                    if (User.IsInRole(UserRoles.Admin))
                    {
                        await _userManager.AddToRoleAsync(user, UserRoles.Personel);
                    }
                    /*
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {

                        var ModalMessage = new ModalMessageVM()
                        {
                            Message="Kullanıcı başarılı bir şekilde eklendi.",
                            Icon= "success",
                            Status=true
                        };
                        var addUserStatus = new UserRegisterVM()
                        {
                            ModalMessage=ModalMessage
                        };
                        return RedirectToAction("NewUser",addUserStatus);
                    }
                    else
                    {
                        return View();
                    }
                    */
                    var ModalMessage = new ModalMessageVM()
                    {
                        Message = "Kullanıcı başarılı bir şekilde eklendi.\n"+ "'"+user.UserName+"'",
                        Icon = "success",
                        Status = true
                    };
                    return RedirectToAction("NewUser", ModalMessage);

                }


                foreach (var error in result.Errors)
                {
                    if(error.Code== "DuplicateUserName")
                    {
                        var ModalMessage = new ModalMessageVM()
                        {
                            Message = "Doğrulama hatası",
                            Icon = "error",
                            Status = true
                        };
                        var addUserStatus = new UserRegisterVM()
                        {
                            ModalMessage = ModalMessage
                        };
                        return RedirectToAction("NewUser", addUserStatus);
                    }
                    //


                    //ModelState.AddModelError(string.Empty, error.Description);

                    return RedirectToAction("NewUser");

                }

            }
            // If we got this far, something failed, redisplay form
            foreach (var item in ModelState.Values)
            {
                if (item.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                {
                    ModelState.AddModelError(string.Empty, item.Errors[0].ErrorMessage);
                    var ModalMessage = new ModalMessageVM()
                    {
                        Message = item.Errors[0].ErrorMessage,
                        Icon = "warning",
                        Status = true
                    };
                    var addUserStatus = new UserRegisterVM()
                    {
                        ModalMessage = ModalMessage
                    };
                    return RedirectToAction("NewUser", addUserStatus);
                }
            }

            return RedirectToAction("NewUser");


        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var AppUser = _uow.ApplicationUser.GetFirstOrDefault(i=>i.Id == id);
            _uow.ApplicationUser.Remove(AppUser);
            _uow.Save();
            return RedirectToAction("NewUser");
        }


        [HttpGet("kullanici/rol-yukselt/{id}")]
        public async Task<IActionResult> UpsertUser(string id)
        {
            var appUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == id);
            appUser.UserRole = UserRoles.Manager;
            _uow.ApplicationUser.Update(appUser);
            var currentRoles = await _userManager.GetRolesAsync(appUser);
            await _userManager.RemoveFromRoleAsync(appUser, UserRoles.Personel);
            await _userManager.AddToRoleAsync(appUser, UserRoles.Manager);
            _uow.Save();
            return Redirect("/users");

        }
        #region API
        [HttpGet("jsonresult/getdepartmentsjson")]
        public JsonResult GetDepartments()
        {
            /*
            #region Authentication Index
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
            }
            else
                return Json(StatusCode(404));
            #endregion Authentication Index
            */
            var Departments = _uow.Department.GetAll();
            return Json(Departments);
        }

        
        [HttpGet("jsonresult/getpersoneluserjson")]
        public JsonResult GetPersonelUser()
        {
            List<Users> _Users = new List<Users>();

            
            #region Authentication Index
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                var users = _uow.ApplicationUser.GetAll(r=>r.UserRole==UserRoles.Personel,includeProperties: "Department");
                foreach (var item in users)
                {

                    if (item.UserRole == UserRoles.Personel)
                    {
                        Users Useritem = new Users()
                        {
                            Id=item.Id,
                            AppUserName = item.AppUserName,
                            DepartmentId = item.DepartmentId,
                            Department = item.Department,
                            FullName = item.FirstName + " " + item.Lastname,
                            IdentityNumber = item.IdentityNumber,
                            Email = item.Email,
                            ImageUrl = item.ImageUrl,
                            Role = item.Role
                        };

                        _Users.Add(Useritem);
                    }
                }
            }
            else
                return Json(StatusCode(404));
            #endregion Authentication Index
            


            //_Users.AddRange((IEnumerable<Users>)(Users)users);


            return Json(_Users);
        }

        [HttpGet("jsonresult/getmanageruserjson")]
        public JsonResult GetManagerUser()
        {
            List<Users> _Users = new List<Users>();


            #region Authentication Index
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                var AdminUsers = _uow.ApplicationUser.GetAll(r => r.UserRole == UserRoles.Admin,includeProperties: "Department");
                var ManagerUsers = _uow.ApplicationUser.GetAll(r => r.UserRole == UserRoles.Manager, includeProperties: "Department");
                foreach (var item in AdminUsers)
                {
                    Users Useritem = new Users()
                    {
                        AppUserName = item.AppUserName,
                        DepartmentId = item.DepartmentId,
                        Department = item.Department,
                        FullName = item.FirstName + " " + item.Lastname,
                        IdentityNumber = item.IdentityNumber,
                        Email = item.Email,
                        ImageUrl = item.ImageUrl,
                        Role = item.Role
                    };
                    _Users.Add(Useritem);
                }
                foreach (var item in ManagerUsers)
                {
                    Users Useritem = new Users()
                    {
                        AppUserName = item.AppUserName,
                        DepartmentId = item.DepartmentId,
                        Department = item.Department,
                        FullName = item.FirstName + " " + item.Lastname,
                        IdentityNumber = item.IdentityNumber,
                        Email = item.Email,
                        ImageUrl = item.ImageUrl,
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

        #endregion API
    }
}
