using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Sounds
{
    public class Sound : MonoBehaviour
    {
        public static Sound Instance { get; private set; }

        [SerializeField] private AudioSource _backgroundMusic;
        [SerializeField] private AudioSource _backgroundSounds;
        [SerializeField] private AudioSource _UISfx;
        [SerializeField] private AudioSource _sfx;
        [Space] [SerializeField] private List<AudioItem> _soundItems;

        private bool _isMusicOn;
        private bool _isSFXOn;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            _backgroundMusic.loop = true;
            _backgroundSounds.loop = true;
        }

        private void OnDisable()
        {
            PlayerData.Instance.MusicStatusChange -= OnMusicStatusChange;
            PlayerData.Instance.SFXStatusChange -= OnSFXStatusChange;
        }

        private void OnValidate()
        {
            var duplicates = _soundItems.GroupBy(sound => sound.Type).SelectMany(item => item.Skip(1)).ToArray();

            if (duplicates.Length > 0)
                throw new System.ArgumentException("Warning! There should be no duplicates in the list of sounds when specifying types!");
        }

        public void Init()
        {
            _isMusicOn = PlayerData.Instance.IsMusicOn;
            _isSFXOn = PlayerData.Instance.IsSFXOn;
            
            PlayerData.Instance.MusicStatusChange += OnMusicStatusChange;
            PlayerData.Instance.SFXStatusChange += OnSFXStatusChange;
        }

        public void PlayBackgroundMusic(CollectionOfSounds type)
        {
            PlayBackground(_backgroundMusic, type, _isMusicOn);
        }

        public void PlayBackgroundSounds(CollectionOfSounds type)
        {
            PlayBackground(_backgroundSounds, type, _isSFXOn);
        }

        public void PlayUISFX(CollectionOfSounds type)
        {
            if (_isSFXOn)
                Play(_UISfx, type);
        }

        public void PlaySFX(CollectionOfSounds type)
        {
            if (_isSFXOn)
                Play(_sfx, type);
        }

        private void PlayBackground(AudioSource source, CollectionOfSounds type, bool isPlay)
        {
            var sound = _soundItems.FirstOrDefault(item => item.Type == type);
            source.volume = sound.Volume;
            source.clip = sound.Clip;
            
            if (isPlay)
                source.Play();
        }

        private void Play(AudioSource source, CollectionOfSounds type)
        {
            var sound = _soundItems.FirstOrDefault(item => item.Type == type);
            source.volume = sound.Volume;
            source.clip = sound.Clip;
            source.Play();
        }

        private void OnSFXStatusChange()
        {
            _isSFXOn = PlayerData.Instance.IsSFXOn;

            if (_isSFXOn)
                _backgroundSounds.Play();
            else
                _backgroundSounds.Stop();
        }

        private void OnMusicStatusChange()
        {
            _isMusicOn = PlayerData.Instance.IsMusicOn;
            
            if (_isMusicOn)
                _backgroundMusic.Play();
            else
                _backgroundMusic.Stop();
        }
    }

    [System.Serializable]
    public class AudioItem
    {
        [SerializeField] private CollectionOfSounds _type;
        [SerializeField] private AudioClip _clip;
        [SerializeField] [Range(0, 1)] private float _volume;

        public CollectionOfSounds Type => _type;
        public AudioClip Clip => _clip;
        public float Volume => _volume;
    }
}