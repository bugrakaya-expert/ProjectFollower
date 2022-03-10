using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ProjectFollower.DataAcces.IMainRepository;
using ProjectFollower.Extensions;
using ProjectFollower.Hubs;
using ProjectFollower.Models.DbModels;
using ProjectFollower.Models.ViewModels;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static ProjectFollower.Utility.ProjectConstant;

namespace ProjectFollower.Controllers
{

    public class SchedulerController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IUnitOfWork _uow;
        protected IHubContext<HomeHub> _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        public WebSocketActionExtensions WebSocket;

        public SchedulerController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ILogger<AccountController> logger,
        IHostingEnvironment hostingEnvironment,
                IHubContext<HomeHub> context,
        IUnitOfWork uow
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _uow = uow;
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [HttpGet]
        public IActionResult Index(Customers _customer)
        {

            #region Authentication Index
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                var customerid = _customer.Id.ToString();
                var ApplicationUser = _uow.ApplicationUser.GetFirstOrDefault(i => i.Id == Claims.Value);
                var customer = _uow.Customers.GetFirstOrDefault(i => i.Id == _customer.Id);
                return View(customer);//Go Dashboard
            }

            #endregion Authentication Index

            return RedirectToAction("Index", "SignIn");
        }
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [HttpPost("export-pdf")]
        public IActionResult DownloadPDF(string htmlstring,string customerId)
        {


            string baseUrl = "https://localhost:5001/Scheduler/schedulerpdf";


            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            WebKitConverterSettings webkitConverterSettings = new WebKitConverterSettings();
            webkitConverterSettings.WebKitPath = Path.Combine(_hostingEnvironment.ContentRootPath, "ExtensionLibs/QtBinariesDotNetCore");

            webkitConverterSettings.Orientation = PdfPageOrientation.Landscape;
            webkitConverterSettings.EnableRepeatTableHeader = true;
            webkitConverterSettings.MediaType = MediaType.Print;
            //webkitConverterSettings.Margin.Bottom = 40;
            webkitConverterSettings.Margin.Bottom = 30;
            webkitConverterSettings.Margin.Top = 20;

            htmlConverter.ConverterSettings = webkitConverterSettings;
            PdfDocument document = new PdfDocument();
            document = htmlConverter.Convert(SchedulerHtml.Before+htmlstring+SchedulerHtml.After, baseUrl);
            MemoryStream ms = new MemoryStream();

            document.Save(ms);
            document.Close(true);
            ms.Position = 0;

            Console.WriteLine("Pdf Converted.");
            /*
            var output = new FileContentResult(ms.ToArray(), "application/pdf");
            output.FileDownloadName = projectCode + ".pdf";

            return output;*/

            //return File(ms, "application/pdf", projectCode + ".pdf", true);// Download Directly Pdf

            return File(ms, "application/octet-stream", "pdfcode" + ".pdf", true);// Download Directly Pdf

            #region Authentication Index
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                return View(null);//Go Dashboard
            }

            #endregion Authentication Index

            return RedirectToAction("Index", "SignIn");
        }

        public IActionResult schedulerpdf()
        {
            return View();
        }

        #region API
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [HttpGet("jsonresult/trigger_WebSocket/{id}")]
        public async Task<JsonResult> WEB_SOCKET_REFRESH_PAGE(string id)
        {

            WebSocketActionExtensions WebSocAct = new WebSocketActionExtensions(_context, _uow);
            await WebSocAct.SchedulerQuery_WebSocket(GetClaim(), id);
            return Json(null);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [HttpGet("getscheduler/{id}")]
        public JsonResult GetScheduler(string id)
        {
            List<SchedulerCustomerVM> schedulerCustomer = new List<SchedulerCustomerVM>();
            var companies = _uow.Customers.GetFirstOrDefault(i => i.Id == Guid.Parse(id));
            var scheduler = _uow.Scheduler.GetAll(i=>i.CustomersId==Guid.Parse(id));
            var schedulerpriorities = _uow.SchedulerPriority.GetAll();

            /*
            foreach (var item in companies)
            {
                var _schedulerCustomer = new SchedulerCustomerVM()
                {
                    Id = item.Id.ToString(),
                    Text = item.Name
                };
                schedulerCustomer.Add(_schedulerCustomer);
            }*/
            var _schedulerCustomerItem = new  SchedulerCustomerVM()
            {
                Id = companies.Id.ToString(),
                Text = companies.Name
            };
            var _schedulerCustomer = new List<SchedulerCustomerVM>();
            _schedulerCustomer.Add(_schedulerCustomerItem);
            var _schedulerVM = new SchedulerVM()
            {
                Scheduler = scheduler,
                SchedulerPriority = schedulerpriorities,
                Customers = _schedulerCustomer
            };

            return Json(_schedulerVM);

        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager + "," + UserRoles.Personel)]
        [HttpGet("getCustomersforScheduler/")]
        public JsonResult GetCustomers()
        {
            var customers = _uow.Customers.GetAll(s => s.SchedulerEnabled).OrderBy(i => i.Name);
            return Json(customers);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("postscheduler")]
        public JsonResult SetScheduler([FromBody] Scheduler scheduler)
        {
            _uow.Scheduler.Add(scheduler);
            _uow.Save();
            return Json(scheduler);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("updatescheduler")]
        public JsonResult UpdateScheduler([FromBody] Scheduler scheduler)
        {
            _uow.Scheduler.Update(scheduler);
            _uow.Save();
            return Json(scheduler);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("deletescheduler/{id}")]
        public JsonResult DeleteScheduler(int id)
        {
            var scheduler = _uow.Scheduler.GetFirstOrDefault(i => i.Id == id);
            _uow.Scheduler.Remove(scheduler);
            _uow.Save();
            return Json(null);
        }
        #endregion API
        public Claim GetClaim()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claims != null)
            {
                return Claims;
            }
            return null;
        }
    }
}
