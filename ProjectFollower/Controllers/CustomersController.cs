using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
using static ProjectFollower.Utility.ProjectConstant;

namespace ProjectFollower.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class CustomersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly ILogger<CustomersController> _logger;
        private readonly IUnitOfWork _uow;

        public CustomersController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<CustomersController> logger,
            IUnitOfWork uow
    )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _uow = uow;
        }

        [Route("customers")]
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
        [Route("customers-add")]
        public IActionResult AddCustomer()
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

        [HttpPost("customers-add")]
        public IActionResult AddCustomerAction(CustomerVM customervm)
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


            customervm.CompanyTypeId = Guid.Parse("6abbeaf7-a6bb-46f7-a075-b640e522841c");


            var DocumentList = new List<CompanyDocuments>();
            var customer = new Customers()
            {
                AuthorizedName=customervm.AuthorizedName,
                CompanyTypeId = customervm.CompanyTypeId,
                Description = customervm.Description,
                Email = customervm.Email,
                ImageUrl = customervm.ImageUrl,
                Name = customervm.Name,
                Phone = customervm.Phone,
                
            };
            /*
            foreach (var document_item in customervm.Documents)
            {
                var Document = new CompanyDocuments()
                {
                    CustomerId = document_item.CustomerId,
                    DocumentUrl = document_item.DocumentUrl,
                };
                _uow.CompanyDocuments.Add(Document);
            }
            */
            _uow.Customers.Add(customer);
            _uow.Save();
            return View("Index");
        }

        [Route("customers-details")]
        public IActionResult Details()
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
