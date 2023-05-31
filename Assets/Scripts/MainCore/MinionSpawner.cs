using Assets.Scripts.MainCore.MinionScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MainCore
{
    public class MinionSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _car;
        [SerializeField] private MinionAccelerator _minionAccelerator;
        [SerializeField] private ItemManager _itemManager;
        [SerializeField] private GameObject _minionTemlate;

        private List<Minion> _minions = new List<Minion>();
        
        private void OnDisable()
        {
            foreach (Minion minion in _minions)
            {
                minion.OnSellItem -= OnBroughtItem;
            }
        }

        public void AddMinion()
        {
            if (_itemManager.IsThereItems == false)
                return;

            Minion minion = Instantiate(_minionTemlate, _car.position, Quaternion.identity, transform).GetComponent<Minion>();
            minion.Init(_car.position, _minionAccelerator);
            minion.OnSellItem += OnBroughtItem;
            minion.SetNewItem(_itemManager.GetNextItem());
            _minions.Add(minion);
        }

        public void AddSpeed()
        {
            MinionSpecifications.Instance.AddSpeed();

            foreach (var minion in _minions)
            {
                minion.UpdateSpeed();
            }
        }

        private void OnBroughtItem(Minion minion)
        {
            if (_itemManager.IsThereItems == false)
            {
                minion.OnSellItem -= OnBroughtItem;
                return;
            }                
            
            minion.SetNewItem(_itemManager.GetNextItem());
        }
    }
}
