using Microsoft.AspNetCore.Mvc;

namespace SchedulerService.Controllers.Room
{
    public class RoomController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
