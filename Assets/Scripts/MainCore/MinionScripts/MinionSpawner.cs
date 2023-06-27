using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MainCore.MinionScripts
{
    public class MinionSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _car;
        [SerializeField] private Booster _minionBooster;
        [SerializeField] private ItemManager _itemManager;
        [SerializeField] private GameObject _minionTemlate;

        private List<Minion> _minions = new List<Minion>();

        public int CountOfMinions => _minions.Count;
        
        private void OnDisable()
        {
            foreach (Minion minion in _minions)
            {
                minion.OnSellItem -= OnBroughtItem;
            }
        }

        public void AddMinion()
        {
            if (_itemManager.CountOfItems == 0)
                return;

            Minion minion = Instantiate(_minionTemlate, _car.position, Quaternion.identity, transform).GetComponent<Minion>();
            minion.Init(_car.position, _minionBooster);
            minion.OnSellItem += OnBroughtItem;
            minion.SetNewItem(_itemManager.GetNextItem());
            _minions.Add(minion);
        }

        public void AddSpeed()
        {
            foreach (var minion in _minions)
            {
                minion.UpdateSpeed();
            }
        }

        private void OnBroughtItem(Minion minion)
        {
            if (_itemManager.CountOfItems == 0)
            {
                DestroyMinion(minion);
            }
            else
            {
                minion.SetNewItem(_itemManager.GetNextItem());
            }
        }

        private void DestroyMinion(Minion minion)
        {
            minion.OnSellItem -= OnBroughtItem;
            _minions.Remove(minion);
            Destroy(minion.gameObject);
        }
    }
}
