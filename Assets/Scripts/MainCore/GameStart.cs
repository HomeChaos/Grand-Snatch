using System.Collections;
using Assets.Scripts.Data;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.MainCore
{
    public class GameStart : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private GameSession _gameSession;
        [SerializeField] private PaymentSystem _paymentSystem;
        [SerializeField] private GameUI _gameUi;
        [SerializeField] private MinionSpawner _minionSpawner;
        [SerializeField] private ItemManager _itemManager;

        private void Awake()
        {
            _playerData.Init();
            StartCoroutine(WaitForLoadPlayerData());
        }

        private IEnumerator WaitForLoadPlayerData()
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();
            
            while (_playerData.IsDataLoaded == false)
            {
                yield return waitForEndOfFrame;
            }
            
            ApplyGameSettings();
        }

        private void ApplyGameSettings()
        {
            _gameSession.Init();
            _itemManager.Init();
            _paymentSystem.Init(_gameSession);
            _gameUi.Init(_paymentSystem);
            Invoke(nameof(StartSpawn), 1f);
        }

        private void StartSpawn()
        {
            _minionSpawner.AddMinion();
        }
        
#if UNITY_EDITOR
        [ContextMenu("Reset Data")]
        private void ResetPlayerData()
        {
            _playerData.ResetData();
        }
#endif
    }
}