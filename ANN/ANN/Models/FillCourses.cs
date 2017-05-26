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
            db.Database.ExecuteSqlCommand("DELETE FROM coursecurrency");
            DateTime date = new DateTime(2002, 1,1);
            while (date.Day != DateTime.Today.Day || date.Month != DateTime.Today.Month || date.Year != DateTime.Today.Year)
            {
                Fill(date);
                date = date.AddDays(1);
            }
        }
    }
}