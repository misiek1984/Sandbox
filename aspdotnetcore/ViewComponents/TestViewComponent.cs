using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace Test.ViewComponents
{
    public class TestViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(DateTime.Now.Year);
        }
    }
}
