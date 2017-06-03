using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Xml;

namespace ANN.Models
{
    public static class FillCourses
    {

        private static ANNDBEntities1 db = new ANNDBEntities1();

        static string URL = "http://www.cbr.ru/scripts/XML_daily.asp?date_req=";
        private static void Fill(DateTime date)
        {
            var client = new WebClient();
            string rdate = "";
            if(date.Day < 10)
            {
                rdate = rdate + "0" + date.Day.ToString();
            }
            else
            {
                rdate = rdate + date.Day.ToString();
            }
            if (date.Month < 10)
            {
                rdate = rdate + "/0" + date.Month.ToString();
            }
            else
            {
                rdate = rdate + "/" + date.Month.ToString();
            }
            rdate = rdate + "/" + date.Year.ToString();
            string requrl = URL + rdate;
            var request = client.DownloadString(requrl);
            var doc = new XmlDocument();
            doc.LoadXml(request);
            coursecurrency course = new coursecurrency();
            int nom = 1;
            course.datecourse = date;
            course.substractcourse = 0;
            foreach (XmlNode node in doc.SelectNodes("ValCurs"))
            {
                foreach (XmlNode cour in node.ChildNodes)
                {
                    foreach (XmlNode val in cour.ChildNodes)
                    {
                        switch (val.Name.ToLower())
                        {
                            case "charcode": course.namecurrency = val.InnerText; break;
                            case "nominal": nom = Convert.ToInt32(val.InnerText); break;
                            case "value": course.course = Convert.ToDecimal(val.InnerText) / nom; break;
                            default: break;
                        }

                    }
                    db.coursecurrency.Add(course);
                    db.SaveChanges();
                }
            }
        }

        public static void DownloadCorses()
        {
            int m = 2;
            DateTime first = new DateTime(2002, 1, 1);
            while (first.Day < DateTime.Today.Day || first.Month < DateTime.Today.Month || first.Year < DateTime.Today.Year)
            {
                m++;
                first = first.AddDays(1);
            }
            var corses = db.coursecurrency.ToArray();
            DateTime date = corses.Last().datecourse.Value; 
            while (date.Day < DateTime.Today.Day || date.Month < DateTime.Today.Month || date.Year < DateTime.Today.Year)
            {
                Fill(date);
                date = date.AddDays(1);
            }
            corses = db.coursecurrency.ToArray();
            Dictionary<string, int> dict = new Dictionary<string, int>();
            int a;
            for(int i = 0; i < corses.Length; i++)
            {
                if(dict.ContainsKey(corses[i].namecurrency))
                {
                    dict.TryGetValue(corses[i].namecurrency,out a);
                    dict.Remove(corses[i].namecurrency);
                    a++;
                    dict.Add(corses[i].namecurrency, a);
                }
                else
                {
                    dict.Add(corses[i].namecurrency, 1);
                }
            }
            ICollection<string> keys = dict.Keys;
            List<string> list = keys.ToList();
            foreach(var item in list)
            {
                dict.TryGetValue(item, out a);
                if(a < m)
                {
                    dict.Remove(item);
                    db.Database.ExecuteSqlCommand("DELETE FROM coursecurrency WHERE namecurrency = @name", new NpgsqlParameter("@name", item));
                }
            }
        }
    }
}