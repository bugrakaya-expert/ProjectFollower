using Microsoft.AspNetCore.Mvc;
using ProjectFollower.DataAcces.IMainRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectFollower.ViewComponents
{
    public class GetUser : ViewComponent
    {
        private readonly IUnitOfWork _uow;

        public GetUser(IUnitOfWork uow)
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
                var FullName = AppUser.FirstName + " " + AppUser.Lastname;
                return View("default",FullName);
            }
            return View("ERROR NAME");
        }
    }
}
