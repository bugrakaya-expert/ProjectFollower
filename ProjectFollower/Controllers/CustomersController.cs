using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private readonly IWebHostEnvironment _hostEnvironment;

        public CustomersController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<CustomersController> logger,
            IUnitOfWork uow,
            IWebHostEnvironment hostEnvironment
    )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _uow = uow;
            _hostEnvironment = hostEnvironment;
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
        public IActionResult AddCustomerAction(CustomerVM customervm, ICollection<IFormFile> filess)
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

            var _CompanyType = _uow.CompanyType.GetFirstOrDefault(i => i.Id == customervm.CompanyTypeId);

            var DocumentList = new List<CompanyDocuments>();
            var customer = new Customers()
            {
                AuthorizedName = customervm.AuthorizedName,
                CompanyType = _CompanyType,
                CompanyTypeId = customervm.CompanyTypeId,
                Description = customervm.Description,
                Email = customervm.Email,
                ImageUrl = customervm.ImageUrl,
                Name = customervm.Name,
                Phone = customervm.Phone,

            };
            _uow.Customers.Add(customer);
            _uow.Save();

            //var GetCustomer = _uow.Customers.GetFirstOrDefault(i => i.Email==customervm.Email);

            string webRootPath = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            if (files.Count() > 0)
            {
                //string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, LocFilePaths.DIR_Customer_Doc + customervm.Name);


                #region Check Customer Directories
                if (!(Directory.Exists(LocFilePaths.RootAsset)))
                    Directory.CreateDirectory(LocFilePaths.RootAsset);
                if (!(Directory.Exists(LocFilePaths.DIR_Customer_Main)))
                    Directory.CreateDirectory(LocFilePaths.DIR_Customer_Main);
                if (!(Directory.Exists(LocFilePaths.DIR_Customer_Doc)))
                    Directory.CreateDirectory(LocFilePaths.DIR_Customer_Doc);
                if (!(Directory.Exists(LocFilePaths.DIR_Customer_Doc + customervm.Name)))
                    Directory.CreateDirectory(LocFilePaths.DIR_Customer_Doc + customervm.Name);
                #endregion Check Customer Directories


                foreach (var item in files)
                {
                    var extension = Path.GetExtension(item.FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, item.FileName + extension), FileMode.Create))
                    {
                        item.CopyTo(fileStream);
                        var Document = new CompanyDocuments()
                        {
                            CustomerId = customer.Id,
                            FileName = item.FileName+extension,
                        };
                        _uow.CompanyDocuments.Add(Document);
                    }
                }
            }
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
            //_uow.Customers.Add(customer);
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
        public async Task<JsonResult> DropzoneActionHandler(ICollection<IFormFile> files)
        {
            return Json(null);
        }
        #region API
        [HttpGet("jsonresult/getallcustomers")]
        public JsonResult GetCustomers()
        {
            var customers = _uow.Customers.GetAll(includeProperties: "CompanyType");
            /*
            foreach (var item in customers)
            {
                var custorvm = new CustomerVM()
                {
                    Name = cu
                };
            }*/

            return Json(customers);
        }
        [HttpGet("jsonresult/getcompanytypesjson")]
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
            var CompanyTypes = _uow.CompanyType.GetAll();
            return Json(CompanyTypes);
        }
        #endregion API

    }
}
