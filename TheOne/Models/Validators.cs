using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheOne.Models
{
    public class Validators
    {
        public static bool ValidateArticle(ArticleType article)
        {
            bool isValid = true;
            if (String.IsNullOrEmpty(article.heading) || String.IsNullOrEmpty(article.content))
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
