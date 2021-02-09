using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using StorageWebsite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StorageWebsite.Controllers
{
    public class DisplayDataController : Controller
    {
        const string SessionName = "_Name";
        private readonly IWebHostEnvironment _webhost;
        public DisplayDataController(IWebHostEnvironment webhost)
        {
            _webhost = webhost;
        }
        public IEnumerable<Item> GetData()
        {
            string webRoothPath = _webhost.WebRootPath;
            var JsonData = System.IO.File.ReadAllText(Path.Combine(webRoothPath, "data", "Products.json"));
            var jsonObj = JsonConvert.DeserializeObject<IEnumerable<Item>>(JsonData);
            return jsonObj;
        }
        [HttpGet("/Lager")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(SessionName) != null)
            {
                ViewBag.Name = HttpContext.Session.GetString(SessionName);
                ViewBag.loggedIn = true;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            if(GetData() == null || GetData().Count() < 1)
            {
                ViewBag.CheckifEmpty = false;
            }
            else
            {
                ViewBag.CheckifEmpty = true;
            }

            return View(GetData());
        }
        //Method for storing data with streamwriter
        private void StoreData(List<Item> Data)
        {
            string webRoothPath = _webhost.WebRootPath;
            string SaveData = JsonConvert.SerializeObject(Data, Formatting.Indented);
            using (StreamWriter saving =
                new StreamWriter(Path.Combine(webRoothPath, "data", "Products.json"), false))
            {
                saving.WriteLine(SaveData);
            }
        }
        //Get the categories from the json file and place them into a selectlistItem and return
        private List<SelectListItem> GetCategories()
        {
            string webRoothPath = _webhost.WebRootPath;
            var JsonData = System.IO.File.ReadAllText(Path.Combine(webRoothPath, "data", "Categories.json"));
            IEnumerable<Categories> JsonObj = JsonConvert.DeserializeObject<IEnumerable<Categories>>(JsonData);

            var list = JsonObj.Select(x => new SelectListItem { Text = x.CategoryName, Value = x.CategoryName }).ToList();

            return list;
        }
        [HttpGet("/Nysak")]
        public IActionResult AddItem()
        {
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.loggedIn = true;

            ViewData["Categories"] = GetCategories();

            return View();
        }
        [HttpPost("/Nysak")]
        public IActionResult AddItem(Item NewItem)
        {
            if (!ModelState.IsValid)
            {
                return AddItem();
            }

            if(GetData() != null)
            {
                string webRoothPath = _webhost.WebRootPath;
                var JsonData = System.IO.File.ReadAllText(Path.Combine(webRoothPath, "data", "Products.json"));
                List<Item> JsonObj = JsonConvert.DeserializeObject<List<Item>>(JsonData);
                NewItem.ID = (JsonObj.Count);
                JsonObj.Add(NewItem);
                StoreData(JsonObj);
            }
            else
            {
                NewItem.ID = 0;
                List<Item> Data = new List<Item> { NewItem };
                StoreData(Data);

            }

            return RedirectToAction("Index", "DisplayData");
        }

        [HttpGet("/Lager/Redigera")]
        public IActionResult Edit(int? id)
        {
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.loggedIn = true;

            var result = GetData().Where(x => x.ID == id).ToList();
            if (result.Count < 1)
            {
                return NotFound();
            }
            ViewData["Categories"] = GetCategories();
            return View(result[0]);
        }
        [HttpGet("/Lager/Radera")]
        public IActionResult Remove(int id)
        {
            if (HttpContext.Session.GetString(SessionName) == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.loggedIn = true;

            var result = GetData().Where(x => x.ID == id).ToList();
            if(result.Count < 1)
            {
                return NotFound();
            }
             ViewData["Föremål"] = result;
             ViewBag.ID = result[0].ID;

             return View();
        }
        [HttpPost("/Lager/Radera")]
        public IActionResult Removed(int ID)
        {
            string webRoothPath = _webhost.WebRootPath;
            var JsonData = System.IO.File.ReadAllText(Path.Combine(webRoothPath, "data", "Products.json"));
            List<Item> JsonObj = JsonConvert.DeserializeObject<List<Item>>(JsonData);

            for(int i = 0;i < JsonObj.Count; i++)
            {
                if(JsonObj[i].ID == ID)
                {
                    JsonObj.RemoveAt(i);
                    i = JsonObj.Count+1;
                }
            }
            StoreData(JsonObj);

            return RedirectToAction("Index", "DisplayData");
        }
        [HttpPost("/Lager/Redigera")]
        public IActionResult EditProduct(Item Data, int ID)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "DisplayData");
            }
            string webRoothPath = _webhost.WebRootPath;
            var JsonData = System.IO.File.ReadAllText(Path.Combine(webRoothPath, "data", "Products.json"));
            List<Item> JsonObj = JsonConvert.DeserializeObject<List<Item>>(JsonData);
            JsonObj[Data.ID] = Data;
            StoreData(JsonObj);

            return RedirectToAction("Index", "DisplayData");
        }
    }
}
