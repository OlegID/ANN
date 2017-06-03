using ANN.Models;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ANN.Controllers
{
    public class HomeController : Controller
    {
        private static ANNDBEntities1 db = new ANNDBEntities1();


        NeuronNetwork n = new NeuronNetwork(0.001);

        public ActionResult Index()
        {
            FillCourses.DownloadCorses();

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

        public void LearnNetwork()
        {
            Random rand = new Random();
            double[] inp = new double[5126];
            DateTime date = new DateTime(2002, 1, 1);
            for (int j = 0; j < 5126; j++)
            {
                inp[j] = (double)db.Database.SqlQuery<decimal>("SELECT course FROM coursecurrency WHERE coursecurrency.namecurrency = @name AND coursecurrency.datecourse = @date", new object[] {
                                                    new NpgsqlParameter("name","USD"),
                                                    new NpgsqlParameter
                                                    {
                                                        ParameterName = "date",
                                                        NpgsqlDbType = NpgsqlDbType.Date,
                                                        Value = date.ToShortDateString()
                                                    }}).ToArray().First();
                date = date.AddDays(1);
            }
            for (int i = 0; i < 100000; i++)
            {
                int m = rand.Next(0, 5000);
                double[] inpu = new double[125];
                for (int j = 0; j < 125; j++)
                {
                    inpu[j] = inp[m + j];
                }
                n.Calculate(inpu);
                double outp = n.Output;
                if (double.IsNaN(outp))
                {
                    new object();
                }
                double tr = inp[m + 126];
                n.Learning(tr);
            }
            n.SaveWeights();
        }
    }
}