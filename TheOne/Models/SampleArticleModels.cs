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
    public class Sample
    {
        public static List<ArticleType> SampleList()
        {
            string path = "C:\\Users\\Kgosi\\Documents\\Visual Studio 2012\\Projects\\TheOne\\TheOne\\App_Data\\Articles.xml";
            WebRequest request = WebRequest.Create(path);
            WebResponse response = request.GetResponse();
            StringBuilder sb = new StringBuilder("");
            Stream rssStream = response.GetResponseStream();
            XmlDocument rssDoc = new XmlDocument();
            rssDoc.Load(path);

            XmlNodeList rssItems = rssDoc.SelectNodes("Articles/Article");

            var itemList = new List<ArticleType>();

            for (int i = 0; i < rssItems.Count; i++)
            {
                var tempItem = new ArticleType();
                tempItem.imageUrl = rssItems[i]["ImageURL"].InnerText;
                tempItem.imageCaption = rssItems[i]["ImageCaption"].InnerText;
                tempItem.heading = rssItems[i]["Heading"].InnerText;
                tempItem.source = rssItems[i]["Source"].InnerText;
                tempItem.genre = rssItems[i]["Genre"].InnerText;
                tempItem.content = rssItems[i]["Content"].InnerText;
                tempItem.link = rssItems[i]["Link"].InnerText;

                if (Validators.ValidateArticle(tempItem))
                {
                    itemList.Add(tempItem);
                }
            }

            return itemList;
        }
    }
}
