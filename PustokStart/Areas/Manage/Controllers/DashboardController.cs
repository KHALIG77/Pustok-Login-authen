using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PustokStart.Areas.Manage.Controllers
{
    [Authorize]
    [Area("manage")]
    public class DashboardController:Controller
    {

        public IActionResult Index() 
        {
            return View();
        }

    }
}
