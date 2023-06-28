using System;
using Assets.Scripts.Data;
using Lean.Localization;
using UnityEngine;

namespace Assets.Scripts.UI.Localization
{
    [RequireComponent(typeof(LeanLocalization))]
    public class Localizer : MonoBehaviour
    {
        private LeanLocalization _localization;

        private void OnDestroy()
        {
            PlayerData.Instance.LanguageChange -= ChangeLanguage;
        }

        public void Init()
        {
            _localization = GetComponent<LeanLocalization>();
            _localization.SetCurrentLanguage(PlayerData.Instance.CurrentLocalization);

            PlayerData.Instance.LanguageChange += ChangeLanguage;
        }

        private void ChangeLanguage(string language)
        {
            _localization.SetCurrentLanguage(language);
        }
    }
}