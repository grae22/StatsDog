using System.Web.Mvc;
using StatsDog.Models;

namespace StatsDog.Controllers
{
  public class StatsController : Controller
  {
    //-------------------------------------------------------------------------

    private readonly ApplicationDbContext _applicationDbContext = new ApplicationDbContext();

    //-------------------------------------------------------------------------

    protected override void Dispose(bool disposing)
    {
      _applicationDbContext.Dispose();
    }

    //-------------------------------------------------------------------------
    // GET: Stats

    public ViewResult Index()
    {
      return View(_applicationDbContext.Stats);
    }

    //-------------------------------------------------------------------------
  }
}