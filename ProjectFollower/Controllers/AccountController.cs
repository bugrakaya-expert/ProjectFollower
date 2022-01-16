using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using ProjectFollower.Models.ViewModels;
using ProjectFollower.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectFollower.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IUnitOfWork _uow;

        public AccountController(
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
        [BindProperty]
        public SignInInput SignInInput { get; set; }

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

            return View("SignIn");
        }

        /*
        [Route("giris")]
        public IActionResult SignIn()
        {
            #region Authentication Index
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                //Go Dashboard
            }
            #endregion Authentication Index

            return View();
        }
        */
        [HttpPost("registerUser")]
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            bool posted = true;
            //Input.IsCompany=Convert.ToBoolean(RegCheck) ;
            returnUrl = returnUrl ?? Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                //var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    Lastname = Input.Lastname,
                    AppUserName = Input.AppUserName,
                    IdentityNumber = Input.IdentityNumber


                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Kayıt işlemi yapıldı.");

                    /*Email Send*/



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
        /*
        [HttpPost("login")]
        public async Task<IActionResult> OnPostAsyncLogin(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(SignInInput.Email, SignInInput.Password, SignInInput.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = _uow.ApplicationUser.GetFirstOrDefault(u => u.Email == SignInInput.Email);
                    _logger.LogInformation("Kullanıcı giriş yaptı." + "Kullanıcı: " + user.Email);
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = SignInInput.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Kullanıcı hesabı kilitlendi!");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    _logger.LogInformation("Kullanıcı bilgisi yalnış. - Girilen Email :" + SignInInput.Email);
                    ModelState.AddModelError(string.Empty, "Sorry, Your e-mail address or password is incorrect. Please check your e-mail and password carefully..");
                    return View("Index");
                    //return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return View("SignIn", model: ModelState.Values);
        }
        */
        //[HttpPost("signup")]
        public IActionResult SuccessResult(bool posted)
        {
            if (posted)
                return View();
            return NotFound();
        }
        public IActionResult AddUser()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
