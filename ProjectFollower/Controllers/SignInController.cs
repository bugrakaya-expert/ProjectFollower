﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
    public class SignInController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IUnitOfWork _uow;

        public SignInController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<AccountController> logger,
        IUnitOfWork uow
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _uow = uow;
        }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        [BindProperty]
        public SignInInput Input { get; set; }
        [Route("signin")]
        public IActionResult Index(string returnUrl)
        {
            /*
            string[]? _slashdetails = returnUrl.Split("proje-detaylari");
            if(_slashdetails.Count() > 0)
            {
                returnUrl = "proje-detaylari/" + _slashdetails[1];
            }*/
            #region Authentication Index
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                if (ApplicationUser != null)
                    return RedirectToAction("Index", "Home");//return dashboard;
            }
            #endregion Authentication Index

            return View();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = _uow.ApplicationUser.GetFirstOrDefault(u => u.Email == Input.Email);
                    _logger.LogInformation("Kullanıcı giriş yaptı." + "Kullanıcı: " + user.Email);

                    return LocalRedirect(returnUrl);
                }
                if (result.ToString() == "Failed")
                {
                    _logger.LogInformation("Kullanıcı bilgisi doğru değil. - Girilen Email :" + Input.Email);
                    //ModelState.AddModelError(string.Empty, "Kullanıcı bilgisi yalnış. Lütfen bilgilerinizi kontrol ediniz.");
                    ModelState.AddModelError(string.Empty, "Giriş bilgilerinden E-posta veya Şifre doğru değil. Lütfen bilgilerinizi kontrol ediniz.");
                    return View("Index");
                }
                if (result.IsNotAllowed)
                {
                    _logger.LogInformation("Onaylanmamış Hesaba giriş denemesi:" + Input.Email);
                    ModelState.AddModelError(string.Empty, "Hesabınız pasif durumunda.");
                    return View("Index");
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Kullanıcı hesabı kilitlendi!");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    _logger.LogInformation("Kullanıcıgirişinde bilinmeyen bir hata:" + Input.Email);
                    ModelState.AddModelError(string.Empty, "Kullanıcı girişinde bilinmeyen bir hata. ");
                    return View("Index");
                    //return Page();
                }
            }

            _logger.LogInformation("Error: 'ModelState Invalid' - Girilen Email :" + Input.Email);
            //ModelState.AddModelError(string.Empty, "Kullanıcı bilgisi yalnış. Lütfen bilgilerinizi kontrol ediniz.");
            ModelState.AddModelError(string.Empty, "Giriş bilgilerinden E-posta veya Şifre doğru değil. Lütfen bilgilerinizi kontrol ediniz.");
            return View("Index");
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
