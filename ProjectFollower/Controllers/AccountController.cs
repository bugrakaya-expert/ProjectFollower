using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.DbModels;
using ProjectFollower.Models.ViewModels;
using ProjectFollower.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using static ProjectFollower.Utility.ProjectConstant;
using Microsoft.AspNetCore.Authorization;

namespace ProjectFollower.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<AccountController> logger,
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
                    IdentityNumber = Input.IdentityNumber,
                    DepartmentId= Guid.Parse("c4c20233-e1b8-41f4-96e7-136aa1c31ad5")


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
        [Route("registerUser")]
        public IActionResult AddUser()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            var charsToRemove = new string[] { "@", ",", ".", ";", "/" };
            foreach (var c in charsToRemove)
            {
                returnUrl = returnUrl.Replace(c, string.Empty);
            }
            return Redirect("/signin?ReturnUrl=%2F" + returnUrl);
        }
        [HttpPost("jsonresult/edituserpassjson")]
        public async Task<JsonResult> EditUser(EditUserPass editUserPass)
        {
            //string currentPassword="",newPassword="";
            IdentityResult ChangeResult = new IdentityResult();
            bool CheckResult = false;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (Claims != null)
            {
                var AppUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);

                var _user = new Users()
                {
                    FullName = AppUser.FirstName + " " + AppUser.Lastname,
                    ImageUrl = AppUser.ImageUrl
                };
                CheckResult = editUserPass.newPassword.Equals(editUserPass.confirmnewPassword);
                if (CheckResult)
                    ChangeResult = await _userManager.ChangePasswordAsync(AppUser, editUserPass.currentPassword, editUserPass.newPassword);
                else
                    return Json("Şifre tekrarı ile uyuşmuyor.");
            }
            if (ChangeResult.Succeeded)
                return Json(ChangeResult.Errors);
            return Json(ChangeResult.Errors);
        }

        [HttpGet("jsonresult/getphotolinkjson")]
        public JsonResult GetPhotoLink()
        {
            string imglink = "";
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (Claims != null)
            {
                var AppUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                var userpath = WebRootPaths.DIR_Users_Main + AppUser.Email + "/" + WebRootPaths.Img + AppUser.ImageUrl;
                imglink = userpath;
            }
            return Json(imglink);
        }
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [HttpPost("profil/fotoguncelle")]
        public IActionResult UpdateUserPhoto()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (Claims != null)
            {
                var AppUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                string webRootPath = _hostEnvironment.WebRootPath;
                var userpath = LocFileForWeb.DIR_Users_Main + AppUser.Email + @"\" + LocFileForWeb.Img;
                var files = HttpContext.Request.Form.Files;
                string fileName;
                if (files.Count() < 1)
                    return NoContent();
                var extension = Path.GetExtension(files[0].FileName);
                if (AppUser.ImageUrl == null)
                {

                    fileName = Guid.NewGuid().ToString()+extension;
                    AppUser.ImageUrl = fileName;
                    _uow.ApplicationUser.Update(AppUser);
                    _uow.Save();
                }
                else
                {
                    fileName = AppUser.ImageUrl;
                }


                if (files.Count() > 0)
                {
                    var uploads = Path.Combine(webRootPath+userpath);
                    string fileLocation = uploads + fileName;

                    if (!(Directory.Exists(uploads)))
                        Directory.CreateDirectory(uploads);
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                        fileName = AppUser.ImageUrl;
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        var stream = files[0].OpenReadStream();
                        var image = Image.FromStream(stream);
                        var size = image.Size;
                        var width = (float)size.Width;
                        var height = (float)size.Height;

                        float rate = width / height;
                        if (rate != 1)
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
                        /*
                        if (width > 200 || height > 200)
                        {
                            ModelState.AddModelError(string.Empty, "Kullanıcı oluşturulamadı! Profil resmi 200px den fazla olamaz.");
                            ModalMessageVM ModalMessage = new ModalMessageVM()
                            {
                                Message = "Kullanıcı oluşturulamadı! Profil resmi 200px den fazla olamaz.",
                                Icon = "warning",
                                Status = true
                            };
                            return RedirectToAction("NewUser", ModalMessage);
                        }*/


                        files[0].CopyTo(fileStreams);
                    }
                }



            }

            return Redirect("/");
        }
        /*
        [HttpGet("jsonresult/updateUserPhoto")]
        public JsonResult UpdateUserPhoto()
        {

            return Json(null);
        }*/
    }
}
