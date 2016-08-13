using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private IHomeModel _model;

        public HomeController(IHomeModel model)
        {
            _model = model;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.Title = "Home";

            return View(_model.GetHomeVM());
        }
    }
}
