using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MainCore
{
    public class MinionSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _car;
        [SerializeField] private ItemManager _itemManager;
        [SerializeField] private GameObject _minionTemlate;

        private List<Minion> _minions = new List<Minion>();

        private void Start()
        {
            
        }

        [ContextMenu("Add Minion")]
        public void AddMinion()
        {
            if (_itemManager.IsThereItems == false)
                return;

            Minion minion = Instantiate(_minionTemlate, _car.position, Quaternion.identity, transform).GetComponent<Minion>();
            minion.Init(_car.position);
            minion.OnBroughtItem += OnBroughtItem;
            minion.SetNewItemPosition(_itemManager.GetNextItem());
            _minions.Add(minion);
        }

        public void AddSpeed()
        {
            foreach (Minion minion in _minions)
            {
                minion.AddSpeed();
            }
        }

        private void OnBroughtItem(Minion minion)
        {
            if (_itemManager.IsThereItems == false)
            {
                minion.OnBroughtItem -= OnBroughtItem;
                return;
            }                
            
            minion.SetNewItemPosition(_itemManager.GetNextItem());
        }

        private void OnDisable()
        {
            foreach (Minion minion in _minions)
            {
                minion.OnBroughtItem -= OnBroughtItem;
            }
        }
    }
}
