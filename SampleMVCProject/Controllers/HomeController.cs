using Microsoft.AspNetCore.Mvc;
using SampleMVCProject.Models;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;


namespace SampleMVCProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly string xmlFilePath = "App_Data/Users.xml";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Save to XML file
                SaveUserToXml(model);
                // Normally save to database here
                TempData["SuccessMessage"] = "Registration successful! User saved in XML.";
                return RedirectToAction("Login");
            }

            return View(model);
        }
        private void SaveUserToXml(RegisterViewModel user)
        {
            // Ensure App_Data folder exists
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, "Users.xml");

            List<RegisterViewModel> users = new List<RegisterViewModel>();

            // Load existing users if file exists
            if (System.IO.File.Exists(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<RegisterViewModel>));
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    users = (List<RegisterViewModel>)serializer.Deserialize(fs);
                }
            }

            // Add new user
            users.Add(user);

            // Save updated list back to XML
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<RegisterViewModel>));
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                xmlSerializer.Serialize(fs, users);
            }
        }

        public IActionResult Login()
        {
            ViewBag.Message = TempData["SuccessMessage"];
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
