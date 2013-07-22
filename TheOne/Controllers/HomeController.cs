using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using TheOne.Models;

namespace TheOne.Controllers
{
    public class HomeController : Controller
    {
        //[OutputCache(CacheProfile = "Cache10")]
        public ActionResult Index()
        {
            ViewBag.Title = "Jabulani - News Page";
            return View(Lists.CombinedList(String.Empty));
        }

        //[OutputCache(CacheProfile = "Cache10")]
        public ActionResult Sports()
        {
            ViewBag.Title = "Jabulani - Sports Page";
            return PartialView("IndexPartial", Lists.CombinedList("sports"));
        }

        //[OutputCache(CacheProfile = "Cache10")]
        public ActionResult Business()
        {
            ViewBag.Title = "Jabulani - Business Page";
            return PartialView("IndexPartial", Lists.CombinedList("business"));
        }

        //[OutputCache(CacheProfile = "Cache10")]
        public ActionResult Entertainment()
        {
            ViewBag.Title = "Jabulani - Entertainment Page";
            return PartialView("IndexPartial", Lists.CombinedList("entertainment"));
        }

        //[OutputCache(CacheProfile = "Cache10")]
        public ActionResult Technology()
        {
            ViewBag.Title = "Jabulani - Tech Page";
            return PartialView("IndexPartial", Lists.CombinedList("tech"));
        }

        //[OutputCache(CacheProfile = "Cache10")]
        public ActionResult Lifestyle()
        {
            ViewBag.Title = "Jabulani - Lifestyle Page";
            return PartialView("IndexPartial", Lists.CombinedList("health"));
        }
    }
}

