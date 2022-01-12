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
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly ILogger<UsersController> _logger;
        private readonly IUnitOfWork _uow;

        public UsersController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ILogger<UsersController> logger,
        IUnitOfWork uow
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _uow = uow;
        }
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
    }
}
