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
        double[] inp = new double[5500];
        int state = 0;

        public void Prepare()
        {
            DateTime date = new DateTime(2002, 1, 1);
            for (int j = 0; j < 5500; j++)
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
        }

        public ActionResult Index()
        {
            FillCourses.DownloadCorses();
            Prepare();
            var corses = db.coursecurrency.ToArray();
            List<string> list = new List<string>();
            for(int i = 0; i < corses.Length; i++)
            {
                if (!list.Contains(corses[i].namecurrency))
                {
                    list.Add(corses[i].namecurrency);
                }
            }
            ViewBag.CourseList = new SelectList(list);
            return View();
        }

        public ActionResult Result(string courseName)
        {
            double[] inp = new double[125];
            DateTime date = DateTime.Today;
            for(int i = 0; i < 125; i++)
            {
                inp[i] = (double)db.coursecurrency.Where(c => c.namecurrency.ToLower().Equals(courseName.ToLower()) && c.datecourse == date).ToArray().First().course;
                date = date.AddDays(-1);
            }
            n.Calculate(inp);
            ViewBag.Result = n.Output;
            ViewBag.Day = inp[124];
            return View();
        }

        public ActionResult Learn()
        {
            LearnNetwork();
            return View();
        }

        public void LearnNetwork()
        {
            Random rand = new Random();
            for (int i = 0; i < 1000; i++)
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