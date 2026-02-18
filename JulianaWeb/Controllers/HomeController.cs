using Newtonsoft.Json;
using System.Web.Mvc;
using System.Web.UI;

namespace JulianaWeb.Controllers
{
  public class HomeController : Controller
  {
    
    // GET: Home
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public ActionResult Index()
    {
      return View();
    }
  }
}
