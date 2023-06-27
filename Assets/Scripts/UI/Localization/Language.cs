namespace UI.Localization
{
    public static class Language
    {
        public const string ENG = "English";
        public const string RUS = "Russian";
        public const string TUR = "Turkish";

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
    }
}