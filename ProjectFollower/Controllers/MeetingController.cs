using Microsoft.AspNetCore.Mvc;

namespace ProjectFollower.Controllers
{
    public class MeetingController : Controller
    {
        [Route("meeting")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
