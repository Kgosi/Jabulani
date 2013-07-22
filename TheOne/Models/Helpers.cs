using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace TheOne.Models
{
    public class Helpers
    {
        public static void RemoveRestrictedArticles(List<ArticleType> article)
        {
            var restricted = RestrictedList();

            for (int i = 0; i < article.Count; i++)
            {
                if (restricted.Any(article[i].heading.ToUpper().Contains) || restricted.Any(article[i].content.ToUpper().Contains))
                {
                    article.Remove(article[i]);
                }
            }

            if (article.Count > 0)
            {
                article[0].isMain = true;
                article[0].imageUrl = String.Concat("http", Regex.Split(article[0].imageUrl, "http")[2]);
            }
        }

        public static List<string> RestrictedList()
        {
            string path = "C:\\Users\\Kgosi\\Documents\\Visual Studio 2012\\Projects\\TheOne\\TheOne\\App_Data\\RestrictedWords.xml";
            WebRequest request = WebRequest.Create(path);
            WebResponse response = request.GetResponse();
            StringBuilder sb = new StringBuilder("");
            Stream rssStream = response.GetResponseStream();
            XmlDocument rssDoc = new XmlDocument();

            rssDoc.Load(path);

            XmlNodeList rssItems = rssDoc.SelectNodes("RestrictedList/Restricted");

            var itemList = new List<string>();

            for (int i = 0; i < rssItems.Count; i++)
            {
                itemList.Add(rssItems[i].InnerText);
            }

            return itemList;
        }

        public static string PublishDateTime(string pubDate)
        {
            string timeElapsed = string.Empty;
            string[] units = pubDate.Split(':');

            if (units[0] != "00")
            {
                var hours = (long)double.Parse(units[0]);
                timeElapsed = String.Concat(hours, " hours ago");
            }
            else if (units[1] != "00")
            {
                var mins = (long)double.Parse(units[1]);
                timeElapsed = String.Concat(mins, " minutes ago");
            }
            else if (units[2] != "00")
            {
                timeElapsed = "seconds ago";
            }

            return timeElapsed;
        }
    }
}
