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

namespace ProjectFollower.Controllers
{
    //[Authorize(Roles = UserRoles.Admin)]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UsersController> _logger;
        private readonly IUnitOfWork _uow;

        public UsersController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<UsersController> logger,
        IUnitOfWork uow
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _uow = uow;
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
        public IActionResult NewUser()
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
            return View();
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

            bool posted = true;
            //Input.IsCompany=Convert.ToBoolean(RegCheck) ;
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                //var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    Lastname = Input.Lastname,
                    DepartmentId=Input.DepartmentId,
                    EmailConfirmed=true

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

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        //return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = "~/" });
                        return RedirectToAction("SuccessResult", posted);
                        //return Redirect("~/");
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    
                }


                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }

            // If we got this far, something failed, redisplay form

            return LocalRedirect(returnUrl);


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

        
        [HttpGet("jsonresult/getalluserjson")]
        public JsonResult GetAllUser()
        {
            List<Users> _Users = new List<Users>();

            
            #region Authentication Index
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                var users = _uow.ApplicationUser.GetAll(includeProperties: "Department");
                foreach (var item in users)
                {
                    Users Useritem = new Users()
                    {
                        AppUserName = item.AppUserName,
                        DepartmentId = item.DepartmentId,
                        Department = item.Department,
                        FirstName = item.FirstName,
                        Lastname = item.Lastname,
                        IdentityNumber = item.IdentityNumber,
                        ImageUrl = item.ImageUrl
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
