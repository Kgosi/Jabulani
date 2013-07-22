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
    public class TwentyFour
    {
        public static List<ArticleType> TwentyFourDotcom(string category)
        {
            var itemList = new List<ArticleType>();
            var url = String.Empty;

            switch (category)
            {
                case "health":
                    url = "http://feeds.health24.com/articles/health24/News/rss";
                    break;
                case "tech":
                    url = "http://feeds.news24.com/articles/News24/SciTech/rss";
                    break;
                case "business":
                    url = "http://feeds.24.com/articles/Fin24/News/rss";
                    break;
                case "entertainment":
                    url = "http://feeds.news24.com/articles/channel/news/rss";
                    break;
                case "sports":
                    url = "http://24.com.feedsportal.com/c/33816/f/607928/index.rss";
                    break;
                default:
                    url = "http://feeds.news24.com/articles/News24/TopStories/rss";
                    break;
            }

            try
            {
                WebRequest request = WebRequest.Create("http://mg.co.za/rss/");//WebRequest.Create(path);
                WebResponse response = request.GetResponse();
                StringBuilder sb = new StringBuilder("");
                Stream rssStream = response.GetResponseStream();
                XmlDocument rssDoc = new XmlDocument();
                rssDoc.Load(rssStream);

                XmlNodeList rssItems = rssDoc.SelectNodes("rss/channel/item");


                for (int i = 0; i < rssItems.Count; i++)
                {
                    var tempItem = new ArticleType();
                    if (rssItems[i]["media:content"] != null)
                    {
                        var imageUrl = rssItems[i]["media:content"].Attributes[0].InnerText;
                        tempItem.imageUrl = String.Concat("http", Regex.Split(imageUrl, "http")[2]);
                    }
                    tempItem.heading = HttpUtility.HtmlDecode(rssItems[i]["title"].InnerText);
                    tempItem.content = HttpUtility.HtmlDecode(rssItems[i]["description"].InnerText);
                    tempItem.source = (rssItems[i]["source"] != null) ? String.Concat("Source : ", HttpUtility.HtmlDecode(rssItems[i]["source"].InnerText)) : String.Empty;
                    tempItem.link = rssItems[i]["link"].InnerText;
                    //tempItem.pubDate = DateTime.Parse(rssItems[i]["pubdate"].InnerText);

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

               // throw;
            }

            return itemList;
        }
    }
}
