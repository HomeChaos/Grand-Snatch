﻿using System;
using Assets.Scripts.Data;
using Assets.Scripts.MainCore;
using Assets.Scripts.Sounds;
using Assets.Scripts.YandexSDK;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GameOverWindow : MonoBehaviour
    {
        [FormerlySerializedAs("_particle")] [SerializeField] private ParticleSystem _levelCompletionEffect;
        [SerializeField] private TMP_Text _textLevel;
        [SerializeField] private TMP_Text _textRevard;
        [SerializeField] private Button _buttonNextLevel;
        [SerializeField] private Button _buttonIncreaseRevenue;
        [SerializeField] private Image _spriteIncreaseRevenue;
        [SerializeField] private Sprite _disableSpriteForButton;
        [SerializeField] private YandexAd _yandexAd;
        [SerializeField] private CameraMovement _cameraMovement;
        [SerializeField] private LevelManager _levelManager;

        private void OnEnable()
        {
            _cameraMovement.enabled = false;
            Sound.Instance.PlayUISFX(CollectionOfSounds.Win);
            _levelCompletionEffect.Play();
            
            _buttonNextLevel.onClick.AddListener(LoadNextLevel);
            _buttonIncreaseRevenue.onClick.AddListener(WatchAdForMultiplicationOfReward);
            
            _textRevard.text = $"+{NumberSeparator.SplitNumber(PaymentSystem.Instance.EarningsPerLevel)} $";
            UpdateLevel();
        }

        private void OnDisable()
        {
            _buttonNextLevel.onClick.RemoveListener(LoadNextLevel);
            _buttonIncreaseRevenue.onClick.RemoveListener(WatchAdForMultiplicationOfReward);
        }

        private void WatchAdForMultiplicationOfReward()
        {
            _spriteIncreaseRevenue.sprite = _disableSpriteForButton;
            _buttonIncreaseRevenue.onClick.RemoveListener(WatchAdForMultiplicationOfReward);
            _buttonIncreaseRevenue.enabled = false;
            _yandexAd.OnShowVideoButtonClick(OnRewarded);
        }
        
        private void OnRewarded()
        {
            int additionalMoney = PaymentSystem.Instance.EarningsPerLevel;
            _textRevard.text = $"+{NumberSeparator.SplitNumber(PaymentSystem.Instance.EarningsPerLevel * 2)} $";

            if (PlayerData.Instance.Money + additionalMoney < 0)
                PlayerData.Instance.Money = Int32.MaxValue;
            else
                PlayerData.Instance.Money += additionalMoney;
            
            PlayerData.Instance.SaveData();
        }

        private void UpdateLevel()
        {
            PlayerData.Instance.Level += 1;
            _textLevel.text = PlayerData.Instance.Level.ToString();
            PlayerData.Instance.SaveData();
        }

        private void LoadNextLevel()
        {
            PlayerData.Instance.SaveData();
            _yandexAd.OnShowInterstitial(() => _levelManager.LoadNextLevel());
        }
    }
}