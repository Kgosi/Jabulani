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
    public class TimesLive
    {
        public static List<ArticleType> TimesLiveList(string category)
        {
         
            var itemList = new List<ArticleType>();
            var url = String.Empty;

            switch (category)
            {
                case "health":
                    url = "http://www.timeslive.co.za/lifestyle/?service=rss";
                    break;
                case "tech":
                    url = "http://www.timeslive.co.za/scitech/?service=rss";
                    break;
                case "business":
                    return null;
                case "entertainment":
                    url = "http://www.timeslive.co.za/entertainment/?service=rss";
                    break;
                case "sports":
                    url = "http://www.timeslive.co.za/sport/?service=rss";
                    break;
                default:
                    url = "http://www.timeslive.co.za/?service=rss";
                    break;
            }

            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                StringBuilder sb = new StringBuilder("");
                Stream rssStream = response.GetResponseStream();
                XmlDocument rssDoc = new XmlDocument();
                rssDoc.Load(rssStream);

                XmlNodeList rssItems = rssDoc.SelectNodes("rss/channel/item");


                for (int i = 0; i < rssItems.Count; i++)
                {
                    var tempItem = new ArticleType();

                    //tempItem.imageUrl = Regex.Match(rssItems[i]["description"].InnerText, "<img.*?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                    tempItem.imageUrl = (rssItems[i]["enclosure"] != null) ? rssItems[i]["enclosure"].Attributes["url"].Value : String.Empty; ;
                    tempItem.heading = HttpUtility.HtmlDecode((rssItems[i]["title"].InnerText));
                    tempItem.content = HttpUtility.HtmlDecode(rssItems[i]["description"].InnerText);
                    tempItem.link = rssItems[i]["link"].InnerText;
                    //Regex regex = new Regex(@"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", RegexOptions.Singleline);
                    //tempItem.content = Regex.Replace(tempItem.content, regex.ToString(), String.Empty);
                    if (!String.IsNullOrEmpty(rssItems[i]["pubDate"].InnerText))
                    {
                        var pubDate = DateTime.Now.Subtract(DateTime.Parse(rssItems[i]["pubDate"].InnerText.Replace("\n", "").Trim())).ToString();
                        tempItem.pubDate = Helpers.PublishDateTime(pubDate);
                    }
                    tempItem.source = "Times LIVE";
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
