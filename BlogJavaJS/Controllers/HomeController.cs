using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlogJavaJS.Models;
using BlogJavaJS.Data;   // để dùng BlogDbContext

namespace BlogJavaJS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogDbContext _context;

        public HomeController(BlogDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index() => View();
        public IActionResult Privacy() => View();
        public IActionResult About() => View();
        public IActionResult Profile() => View();

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var contact = new Contact
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Message = model.Message
                };

                // ✅ Lưu vào DB
                _context.Contacts.Add(contact);
                _context.SaveChanges();

                _logger.LogInformation(
                    "New contact from {First} {Last}, Email: {Email}, Phone: {Phone}, Msg: {Msg}",
                    model.FirstName, model.LastName, model.Email, model.Phone, model.Message
                );

                // ✅ báo hiệu để hiển thị popup cảm ơn
                TempData["ShowThankYouModal"] = true;

                ModelState.Clear(); // reset form
                return RedirectToAction("Contact");
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
