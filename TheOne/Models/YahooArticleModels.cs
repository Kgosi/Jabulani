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
    public class Yahoo
    {
        public static List<ArticleType> YahooList(string category)
        {
            var itemList = new List<ArticleType>();

            try
            {
                WebRequest request = WebRequest.Create("http://news.yahoo.com/rss/" + category);
                WebResponse response = request.GetResponse();
                StringBuilder sb = new StringBuilder("");
                Stream rssStream = response.GetResponseStream();
                XmlDocument rssDoc = new XmlDocument();
                rssDoc.Load(rssStream);

                XmlNodeList rssItems = rssDoc.SelectNodes("rss/channel/item");


                for (int i = 0; i < rssItems.Count; i++)
                {
                    var tempItem = new ArticleType();

                    //tempItem.isMain = (i == 0);

                    if (rssItems[i]["media:content"] != null)
                    {
                        var imageUrl = rssItems[i]["media:content"].Attributes[0].InnerText;
                        tempItem.imageUrl = imageUrl;//(tempItem.isMain) ? String.Concat("http", Regex.Split(imageUrl, "http")[2]) : imageUrl;
                    }
                    tempItem.heading = HttpUtility.HtmlDecode(rssItems[i]["title"].InnerText);
                    tempItem.link = rssItems[i]["link"].InnerText;
                    tempItem.content = HttpUtility.HtmlDecode(rssItems[i]["description"].InnerText);
                    tempItem.source = (rssItems[i]["source"] != null) ? String.Concat("Source : ", HttpUtility.HtmlDecode(rssItems[i]["source"].InnerText)) : String.Empty;
                    if (!String.IsNullOrEmpty(rssItems[i]["pubDate"].InnerText))
                    {
                        var pubDate = DateTime.Now.Subtract(DateTime.Parse(rssItems[i]["pubDate"].InnerText)).ToString();
                        tempItem.pubDate = Helpers.PublishDateTime(pubDate);
                    }

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
