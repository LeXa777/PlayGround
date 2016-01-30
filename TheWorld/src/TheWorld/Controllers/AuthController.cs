namespace TheWorld.Controllers
{
    using Microsoft.AspNet.Mvc;

    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Trips", "App");
            }
            return View();
        }
    }
}
