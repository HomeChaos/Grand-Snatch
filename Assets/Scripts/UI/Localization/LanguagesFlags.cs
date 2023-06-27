using System;
using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;

namespace UI.Localization
{
    public class LanguagesFlags : MonoBehaviour
    {
        [SerializeField] private List<Flag> _flags;

        private void Awake()
        {
            foreach (var flag in _flags)
            {
                flag.FlagClicked += OnFlagClick;
                flag.SetIconActive(false);
            }
        }

        private void OnEnable()
        {
            Debug.Log("Start find language");
            foreach (var flag in _flags)
            {
                Debug.Log($"{flag.Language} & {PlayerData.Instance.CurrentLocalization} = {flag.Language == PlayerData.Instance.CurrentLocalization}");
                if (flag.Language == PlayerData.Instance.CurrentLocalization)
                {
                    flag.SetIconActive(true);
                    break;
                }
            }
        }

        private void OnDisable()
        {
            foreach (var flag in _flags)
            {
                flag.FlagClicked += OnFlagClick;
                flag.SetIconActive(false);
            }
        }

        private void OnFlagClick(Flag clickedFlag, string selectedLanguage)
        {
            foreach (var flag in _flags)
            {
                flag.SetIconActive(false);
            }
            
            clickedFlag.SetIconActive(true);
            PlayerData.Instance.CurrentLocalization = selectedLanguage;
        }
    }
}