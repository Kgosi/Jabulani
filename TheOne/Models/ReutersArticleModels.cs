using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Xml;

namespace TheOne.Models
{
    public class Reuters
    {
        public static List<ArticleType> ReutersList(string category)
        {
            var itemList = new List<ArticleType>();
            var url = String.Empty;

            switch (category)
            {
                case "health":
                    url = "http://feeds.reuters.com/reuters/lifestyle";
                    break;
                case "tech":
                    url = "http://feeds.reuters.com/reuters/technologyNews";
                    break;
                case "business":
                    url = "http://feeds.reuters.com/reuters/businessNews";
                    break;
                case "entertainment":
                    url = "http://feeds.reuters.com/reuters/entertainment";
                    break;
                case "sports":
                    url = "http://feeds.reuters.com/reuters/sportsNews";
                    break;
                default:
                    url = "http://feeds.reuters.com/reuters/topNews";
                    break;
            }

            try
            {
                WebRequest request = WebRequest.Create("http://feeds.reuters.com/Reuters/worldNews");//WebRequest.Create(path);
                WebResponse response = request.GetResponse();
                StringBuilder sb = new StringBuilder("");
                Stream rssStream = response.GetResponseStream();
                XmlDocument rssDoc = new XmlDocument();
                rssDoc.Load(rssStream);

                XmlNodeList rssItems = rssDoc.SelectNodes("rss/channel/item");


                for (int i = 0; i < rssItems.Count; i++)
                {
                    var tempItem = new ArticleType();

                    tempItem.imageUrl = Regex.Match(rssItems[i].InnerXml, "<img.*?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;

                    tempItem.heading = HttpUtility.HtmlDecode(rssItems[i]["title"].InnerText);
                    tempItem.content = HttpUtility.HtmlDecode(rssItems[i]["description"].InnerText);
                    tempItem.link = rssItems[i]["link"].InnerText;

                    Regex regex = new Regex(@"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", RegexOptions.Singleline);

                    tempItem.content = Regex.Replace(tempItem.content, regex.ToString(), String.Empty);

                    if (Validators.ValidateArticle(tempItem))
                    {
                        itemList.Add(tempItem);
                    }
                }
            }
            catch (Exception)
            {

                //throw;
            }

            return itemList;
        }
    }
}
