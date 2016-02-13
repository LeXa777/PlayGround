namespace TheWorld.Controllers.Web
{
    using System;
    using Microsoft.AspNet.Mvc;
    using Microsoft.AspNet.Authorization;
    using Models;
    using Services.MailService;
    using ViewModels;

    public class AppController : Controller
    {
        private IMailService mailService;
        private IWorldRepository repository;

        public AppController(IMailService mailService, IWorldRepository repository)
        {
            this.mailService = mailService;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Trips()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                var email = Startup.Configuration["AppSettings:SiteEmailAddress"];

                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new Exception("Email is null");
                }

                if (this.mailService.SendMail(email, email, $"Contact from {viewModel.Email}", viewModel.Message))
                {
                    this.ModelState.Clear();

                    this.ViewBag.Message = "Meesage sent, thanks!";
                }
            }
            return View();
        }
    }
}