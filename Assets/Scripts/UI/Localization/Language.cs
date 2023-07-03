using System.Collections.Generic;

namespace Assets.Scripts.UI.Localization
{
    public static class Language
    {
        public const string ENG = "English";
        public const string RUS = "Russian";
        public const string TUR = "Turkish";
        
        public static readonly string[] ListOfAllLanguage = {ENG, RUS, TUR};
        
        public static string DefineLanguage(string lang)
        {
            switch (lang)
            {
                case "ru": case "be": case "kk" : case "uk" : case "uz":
                    return RUS;
                case "tr":
                    return TUR;
                default:
                    return ENG;
            }
        }

        public static string GetAnonymous(string lang)
        {
            switch (lang)
            {
                case ENG:
                    return "Anonymous";
                case RUS:
                    return "Аноним";
                case TUR:
                    return "Anonim";
                default:
                    return "Anonymous";
            }
        }
    }
}