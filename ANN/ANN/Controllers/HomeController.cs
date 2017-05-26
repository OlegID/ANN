using ANN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ANN.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var n = new NeuronNetwork(1);
            //FillCourses.DownloadCorses();
            DateTime date = new DateTime(2002, 1, 1);
            int i = 1;
            while (date.Day != DateTime.Today.Day || date.Month != DateTime.Today.Month || date.Year != DateTime.Today.Year)
            {
                i++;
                date = date.AddDays(1);
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}