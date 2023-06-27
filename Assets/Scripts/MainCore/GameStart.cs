using Assets.Scripts.Data;
using Assets.Scripts.MainCore.HumanScripts;
using Assets.Scripts.MainCore.MinionScripts;
using Assets.Scripts.Sounds;
using Assets.Scripts.UI;
using UI.Localization;
using UnityEngine;

namespace Assets.Scripts.MainCore
{
    public class GameStart : MonoBehaviour
    {
        [SerializeField] private GarageOfCars _garage;
        [SerializeField] private HumansSpawner _humansSpawner;
        [SerializeField] private GameSession _gameSession;
        [SerializeField] private PaymentSystem _paymentSystem;
        [SerializeField] private GameUI _gameUi;
        [SerializeField] private Sound _sound;
        [SerializeField] private Localizer _localizer;
        [SerializeField] private MinionSpawner _minionSpawner;
        [SerializeField] private ItemManager _itemManager;

        private void Awake()
        {
            ApplyGameSettings();
        }

        private void OnDestroy()
        {
            PlayerData.Instance.SaveData();
        }

        private void ApplyGameSettings()
        {
            _garage.Init();
            _gameSession.Init();
            _humansSpawner.Init();
            _itemManager.Init();
            _paymentSystem.Init(_gameSession);
            _gameUi.Init(_paymentSystem);
            _sound.Init();
            _sound.PlayBackgroundMusic(CollectionOfSounds.Game);
            _sound.PlayBackgroundSounds(CollectionOfSounds.Bird);
            _localizer.Init();
            Invoke(nameof(StartSpawn), 1f);
        }

        private void StartSpawn()
        {
            _minionSpawner.AddMinion();
            _sound.PlaySFX(CollectionOfSounds.Car);
        }
    }
}