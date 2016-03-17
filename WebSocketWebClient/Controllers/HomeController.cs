using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;
using WebSocketWebClient.Models;

namespace WebSocketWebClient.Controllers
{
    public class HomeController : Controller
    {
        private IOptions<Config> _webSocketServerAddress;

        public HomeController(IOptions<Config> webSocketServerAddress)
        {
            _webSocketServerAddress = webSocketServerAddress;
        }

        public IActionResult Index()
        {
            return View(new IndexVM
            {
                WebSocketWCFServerAddress = _webSocketServerAddress.Options.WebSocketWCFServerAddress,
                WebSocketServerAddress = _webSocketServerAddress.Options.WebSocketServerAddress
            });
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
