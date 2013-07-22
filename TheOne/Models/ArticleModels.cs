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
    public class Lists
    {
        public static List<ArticleType> CombinedList(string category) {

            //var sample = Sample.SampleList();
            var yahoo = Yahoo.YahooList(category);
            var bbc = BBC.BBCList(category);
            var times = TimesLive.TimesLiveList(category);
            var reuters = Reuters.ReutersList(category);
            var twentyfour = TwentyFour.TwentyFourDotcom(category);

            var main = new List<ArticleType>();

            //main.AddRange(sample);
            main.AddRange(yahoo);
            main.AddRange(bbc);
            if (times != null)
                main.AddRange(times);
            main.AddRange(reuters);
            main.AddRange(twentyfour);

            Helpers.RemoveRestrictedArticles(main);

            return main.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}
