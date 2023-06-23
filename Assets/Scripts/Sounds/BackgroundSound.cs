using Agava.WebUtility;
using UnityEngine;

namespace Assets.Scripts.Sounds
{
    public class BackgroundSound : MonoBehaviour
    {
        private void Awake()
        {
            WebApplication.InBackgroundChangeEvent += ChangeBackgroundSounds;
        }

        private void OnDestroy()
        {
            WebApplication.InBackgroundChangeEvent -= ChangeBackgroundSounds;
        }

        private void ChangeBackgroundSounds(bool hidden)
        {
            if (hidden)
                Sound.Instance.UpPause();
            else
                Sound.Instance.Pause();
        }
    }
}