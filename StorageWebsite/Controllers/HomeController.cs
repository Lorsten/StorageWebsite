using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Newtonsoft.Json;
using StorageWebsite.Models;
using Microsoft.AspNetCore.Http;

namespace StorageWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webhost;
        const string SessionName = "_Name";
        public HomeController(IWebHostEnvironment webhost)
        {
            _webhost = webhost;
        }
        public IActionResult Index()
        {
            ViewBag.loggedIn = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(User UserLogin)
        {
            // If the data is not valid send back to index
            if (!ModelState.IsValid)
            {
                return Index();
            }

            var PathTofile = _webhost.WebRootPath;
            var PathtoData = System.IO.File.ReadAllText(Path.Combine(PathTofile, "data", "Users.json"));
            var Json = JsonConvert.DeserializeObject<List<User>>(PathtoData);

            for (int i = 0; i < Json.Count; i++ )
            {
                if(Json[i].Name == UserLogin.Name && Json[i].Password == UserLogin.Password)
                {
                    HttpContext.Session.SetString(SessionName, UserLogin.Name);
                    Response.Redirect("/Lager");
                }
            }
            
            ViewBag.Error = "Ditt användarnamn eller lösenord matchar inte med ett existerande konto";
            return Index();
        }

        [HttpGet("/OmSidan")]
        public IActionResult About()
        {
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.loggedIn = true;
            return View();
        }
        //Clear the sesssion 
        public void Logut()
        {
            HttpContext.Session.Clear();
            Response.Redirect("Index");
        }
    }
}
