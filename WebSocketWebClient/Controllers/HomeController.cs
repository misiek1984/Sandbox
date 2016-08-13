using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
                WebSocketWCFServerAddress = _webSocketServerAddress.Value.WebSocketWCFServerAddress,
                WebSocketServerAddress = _webSocketServerAddress.Value.WebSocketServerAddress
            });
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
