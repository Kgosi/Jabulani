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
    public class BBC
    {
        public static List<ArticleType> BBCList(string category)
        {
            var itemList = new List<ArticleType>();
            var url = String.Empty;

            switch (category)
            {
                case "health":
                    url = "http://feeds.bbci.co.uk/health/rss.xml";
                    break;
                case "tech":
                    url = "http://feeds.bbci.co.uk/news/technology/rss.xml";
                    break;
                case "business":
                    url = "http://feeds.bbci.co.uk/news/business/rss.xml";
                    break;
                case "entertainment":
                    url = "http://feeds.bbci.co.uk/news/entertainment_and_arts/rss.xml";
                    break;
                case "sports":
                    url = "http://feeds.bbci.co.uk/sport/rss.xml";
                    break;
                default:
                    url = "http://feeds.bbci.co.uk/news/rss.xml";
                    break;
            }

            try
            {
                WebRequest request = WebRequest.Create("http://feeds.bbci.co.uk/news/rss.xml");//WebRequest.Create(path);
                WebResponse response = request.GetResponse();
                StringBuilder sb = new StringBuilder("");
                Stream rssStream = response.GetResponseStream();
                XmlDocument rssDoc = new XmlDocument();
                rssDoc.Load(rssStream);

                XmlNodeList rssItems = rssDoc.SelectNodes("rss/channel/item");


                for (int i = 0; i < rssItems.Count; i++)
                {
                    var tempItem = new ArticleType();
                    if (rssItems[i]["media:thumbnail"] != null)
                    {
                        if (rssItems[i]["media:thumbnail"].NextSibling != null)
                        {
                            var urls = rssItems[i]["media:thumbnail"].NextSibling;
                            tempItem.imageUrl = urls.Attributes["url"].InnerText;
                        }
                    }
                    tempItem.link = rssItems[i]["link"].InnerText;
                    tempItem.heading = HttpUtility.HtmlDecode((rssItems[i]["title"].InnerText));
                    tempItem.content = HttpUtility.HtmlDecode(rssItems[i]["description"].InnerText);
                    if (!String.IsNullOrEmpty(rssItems[i]["pubDate"].InnerText))
                    {
                        var pubDate = DateTime.Now.Subtract(DateTime.Parse(rssItems[i]["pubDate"].InnerText)).ToString();
                        tempItem.pubDate = Helpers.PublishDateTime(pubDate);
                    }
                    tempItem.source = "BBC News";

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
