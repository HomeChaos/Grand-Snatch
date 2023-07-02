using Assets.Scripts.Data;
using Assets.Scripts.MainCore.HumanScripts;
using Assets.Scripts.MainCore.MinionScripts;
using Assets.Scripts.Sounds;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Localization;
using UnityEngine;

namespace Assets.Scripts.MainCore
{
    public class GameStart : MonoBehaviour
    {
        private readonly float _timeToStartSpawn = 1f;
        
        [SerializeField] private GarageOfCars _garage;
        [SerializeField] private HumansSpawner _humansSpawner;
        [SerializeField] private GameSession _gameSession;
        [SerializeField] private PaymentSystem _paymentSystem;
        [SerializeField] private GameUI _gameUi;
        [SerializeField] private Localizer _localizer;
        [SerializeField] private MinionSpawner _minionSpawner;
        [SerializeField] private ItemManager _itemManager;

        private void Awake()
        {
            ApplyGameSettings();
        }

        private void ApplyGameSettings()
        {
            _garage.Init();
            _gameSession.Init();
            _humansSpawner.Init();
            _itemManager.Init();
            _paymentSystem.Init(_gameSession);
            _gameUi.Init(_paymentSystem);
            Sound.Instance.PlayBackgroundMusic(CollectionOfSounds.Game, CollectionOfSounds.Bird);
            _localizer.Init();
            Invoke(nameof(StartSpawn), _timeToStartSpawn);
        }

        private void StartSpawn()
        {
            _minionSpawner.AddMinion();
            Sound.Instance.PlaySFX(CollectionOfSounds.Car);
        }
    }
}