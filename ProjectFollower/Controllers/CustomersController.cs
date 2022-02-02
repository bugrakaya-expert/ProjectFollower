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

        [Route("musteriler")]
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
        [Route("musteriler/ekle")]
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

        [HttpPost("musteriler/ekle")]
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


            //var GetCustomer = _uow.Customers.GetFirstOrDefault(i => i.Email==customervm.Email);

            string webRootPath = _hostEnvironment.WebRootPath;
            var customerpath = LocFileForWeb.DIR_Customer_Main + customervm.Email+ @"\" + LocFileForWeb.Img;
            var files = HttpContext.Request.Form.Files;
            string FileName="";
            if (files.Count() > 0)
            {
                //string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath+ customerpath);

                if (!(Directory.Exists(uploads)))
                    Directory.CreateDirectory(uploads);


                foreach (var item in files)
                {
                    var extension = Path.GetExtension(item.FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, item.FileName), FileMode.Create))
                    {
                        item.CopyTo(fileStream);
                        /*
                        var Document = new CompanyDocuments()
                        {
                            CustomerId = customer.Id,
                            FileName = item.FileName + extension,
                        };

                        _uow.CompanyDocuments.Add(Document);*/
                    }
                    FileName = item.FileName;
                }
            }
            var customer = new Customers()
            {
                AuthorizedName = customervm.AuthorizedName,
                CompanyType = _CompanyType,
                CompanyTypeId = customervm.CompanyTypeId,
                Description = customervm.Description,
                Email = customervm.Email,
                ImageUrl = FileName,
                Name = customervm.Name,
                Phone = customervm.Phone,

            };
            _uow.Customers.Add(customer);
            _uow.Save();
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
            //_uow.Save();
            return RedirectToAction("Index", "Customers");
        }

        [Route("musteriler/{id}")]
        public IActionResult Details(Guid id)
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

            var getCustomer = _uow.Customers.GetFirstOrDefault(i => i.Id == id);
            var getDocuments = _uow.CompanyDocuments.GetAll(i => i.CustomerId == id);
            var Customer = new CustomerVM()
            {
                AuthorizedName = getCustomer.AuthorizedName,
                Description = getCustomer.Description,
                ImageUrl = getCustomer.ImageUrl,
                Name = getCustomer.Name,
                Email = getCustomer.Email,
                Phone = getCustomer.Phone,
                Documents = getDocuments
            };

            return View(Customer);
        }
        [HttpGet("musteriler/sil/{id}")]
        public IActionResult Remove(Guid id, bool status)
        {

            if (status)
            {
                var getCustomer = _uow.Customers.GetFirstOrDefault(i => i.Id == id);
                var getDocuments = _uow.CompanyDocuments.GetAll(i => i.CustomerId == id);

                if (getDocuments.Count() > 0)
                    if ((Directory.Exists(LocFilePaths.DIR_Customer_Doc + getCustomer.Name)))
                        Directory.Delete(LocFilePaths.DIR_Customer_Doc + getCustomer.Name,true);

                _uow.Customers.Remove(getCustomer);
                _uow.CompanyDocuments.RemoveRange(getDocuments);
                _uow.Save();
                
            }

            return RedirectToAction("Index","Customers");
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
