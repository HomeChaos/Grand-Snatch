using System;
using Agava.YandexGames;
using Assets.Scripts.Sounds;
using UnityEngine;

namespace Assets.Scripts.YandexSDK
{
    public class YandexAd : MonoBehaviour
    {
        public void OnShowVideoButtonClick(Action reward)
        {
            if (Application.isEditor)
            {
                reward?.Invoke();
                return;
            }
            
            VideoAd.Show(
                onOpenCallback: Sound.Instance.Pause,
                onRewardedCallback: reward,
                onCloseCallback: Sound.Instance.UpPause,
                onErrorCallback: (error) => Sound.Instance.UpPause()
                );
        }
        
        public void OnShowInterstitial(Action loadNextLevel)
        {
            if (Application.isEditor)
            {
                loadNextLevel?.Invoke();
                return;
            }
            
            InterstitialAd.Show(
                onOpenCallback:Sound.Instance.Pause,
                onCloseCallback: (status) =>
                {
                    Sound.Instance.UpPause(); 
                    loadNextLevel?.Invoke();
                    
                });
        }
    }
}