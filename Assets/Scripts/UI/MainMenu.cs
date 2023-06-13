﻿using UnityEngine;
using UnityEngine.UI;
using IJunior.TypedScenes;

namespace Assets.Scripts.UI
{
    public class MainMenu: MonoBehaviour
    {
        [SerializeField] private Button _start;
        [SerializeField] private Button _shop;
        [SerializeField] private Button _creators;
        [SerializeField] private Button _settings;

        private void Awake()
        {
            _start.onClick.AddListener(StartGame);
            _shop.onClick.AddListener(ShowShop);
            _creators.onClick.AddListener(ShowCreators);
            _settings.onClick.AddListener(ShowSettings);
        }

        private void OnDestroy()
        {
            _start.onClick.RemoveListener(StartGame);
            _shop.onClick.RemoveListener(ShowShop);
            _creators.onClick.RemoveListener(ShowCreators);
            _settings.onClick.RemoveListener(ShowSettings);
        }

        private void StartGame()
        {
            Level_1.Load();
        }

        private void ShowShop()
        {
            Debug.Log("Shop");
        }

        private void ShowCreators()
        {
            Debug.Log("Creators");
        }

        private void ShowSettings()
        {
            Debug.Log("Settings");
        }
    }
}