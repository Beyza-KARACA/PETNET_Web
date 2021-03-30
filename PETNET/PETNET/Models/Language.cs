using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace PETNET.Models
{
    public class SiteLanguage
    {
        public static List<Language> AvailableLanguage => new List<Language>
    {
        new Language { LangFullName="tr-TR", LangCultureName="tr-TR"},
        new Language { LangFullName="en-US" , LangCultureName="en-US"},
                               new Language { LangFullName="fr-FR" , LangCultureName="fr-FR"}
    };
        public static bool IsLanguageAvailable(string lang)
        {
            return AvailableLanguage.Where(x => x.LangFullName.Equals(lang)).FirstOrDefault() != null ? true : false;
        }
        public static string GetDefaultLanguage()
        {
            return AvailableLanguage[0].LangCultureName;
        }

        public void SetLanguage(string lang)
        {
            try
            {
                if (!IsLanguageAvailable(lang))
                {
                    lang = GetDefaultLanguage();
                }
                else
                {
                    var cultureInfo = new CultureInfo(lang);
                    Thread.CurrentThread.CurrentUICulture = cultureInfo;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
                    HttpCookie langCookie = new HttpCookie("culture", lang);
                    langCookie.Expires = DateTime.Now.AddYears(1);
                    HttpContext.Current.Response.Cookies.Add(langCookie);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public class Language
        {
            public string LangFullName { get; set; }
            public string LangCultureName { get; set; }
        }
    }
}