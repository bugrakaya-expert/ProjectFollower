using Microsoft.AspNetCore.Mvc;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectFollower.ViewComponents
{
    public class EditProfile : ViewComponent
    {
        private readonly IUnitOfWork _uow;
        public EditProfile(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public IViewComponentResult Invoke()
        {
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
                return View("default", _user);
            }

            return null;
        }
    }
}
