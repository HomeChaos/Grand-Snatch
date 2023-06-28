using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Sounds
{
    [RequireComponent(typeof(BackgroundSound))]
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
        private bool _isHidden;
        private CollectionOfSounds _currentBackgroundMusic = CollectionOfSounds.Win;

        private void Awake()
        {
            CheckingForDuplicates();
            
            if (Instance == null)
                Instance = this;
            
            DontDestroyOnLoad(this);

            _backgroundMusic.loop = true;
            _backgroundSounds.loop = true;
            _isHidden = false;
            
            _isMusicOn = PlayerData.Instance.IsMusicOn;
            _isSFXOn = PlayerData.Instance.IsSFXOn;
            
            PlayerData.Instance.MusicStatusChange += OnMusicStatusChange;
            PlayerData.Instance.SFXStatusChange += OnSFXStatusChange;
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

        public void PlayBackgroundMusic(CollectionOfSounds type)
        {
            if (_currentBackgroundMusic != type)
            {
                _backgroundSounds.Stop();
                _currentBackgroundMusic = type;
                PlayBackground(_backgroundMusic, type, _isMusicOn);
            }            
        }
        public void PlayBackgroundMusic(CollectionOfSounds typeFirst, CollectionOfSounds typeSecond)
        {
            if (_currentBackgroundMusic != typeFirst)
            {
                _currentBackgroundMusic = typeFirst;
                PlayBackground(_backgroundMusic, typeFirst, _isMusicOn);
                PlayBackground(_backgroundSounds, typeSecond, _isSFXOn);
            }
        }

        public void PlayUISFX(CollectionOfSounds type)
        {
            if (_isSFXOn && _isHidden == false)
                Play(_UISfx, type);
        }

        public void PlaySFX(CollectionOfSounds type)
        {
            if (_isSFXOn && _sfx.isPlaying == false && _isHidden == false)
                Play(_sfx, type);
        }

        public void Pause()
        {
            _isHidden = true;
            _backgroundMusic.Pause();
            _backgroundSounds.Pause();
            _UISfx.Pause();
            _sfx.Pause();
        }

        public void UpPause()
        {
            _isHidden = false;
            _backgroundMusic.UnPause();
            _backgroundSounds.UnPause();
            _UISfx.UnPause();
            _sfx.UnPause();
        }

        private void CheckingForDuplicates()
        {
            var sounds = FindObjectsOfType<Sound>();

            foreach (var sound in sounds)
            {
                if (sound != this)
                    Destroy(gameObject);
            }
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