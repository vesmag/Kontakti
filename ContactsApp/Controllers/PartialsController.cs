using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactsApp.Controllers
{
    public class PartialsController : Controller
    {
        // GET: Partials
        public ActionResult All()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}