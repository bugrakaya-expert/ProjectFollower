using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static ProjectFollower.Utility.ProjectConstant;

namespace ProjectFollower.ViewComponents
{
    public class GetUser : ViewComponent
    {
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _hostEnvironment;

        public GetUser(IUnitOfWork uow, IWebHostEnvironment hostEnvironment)
        {
            _uow = uow;
            _hostEnvironment = hostEnvironment;
        }
        public IViewComponentResult Invoke()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (Claims != null)
            {
                var AppUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                string webRootPath = _hostEnvironment.WebRootPath;
                var userpath = WebRootPaths.DIR_Users_Main + AppUser.Id + "/" + WebRootPaths.Img + AppUser.ImageUrl;
                var _user = new Users()
                {
                    FullName = AppUser.FirstName + " " + AppUser.Lastname,
                    ImageUrl = userpath,
                };
                return View("default", _user);
            }
            return View("ERROR NAME");
        }
    }
}
