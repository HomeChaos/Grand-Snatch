using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.MainCore
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] private Item[] _items;

        private Queue<Item> _queueBigItems = new Queue<Item>();
        private Queue<Item> _queueSmallItems = new Queue<Item>();
        private int _maxCountOfItems;
        
        public event UnityAction AllDone;
        
        public int CountOfItems => _queueBigItems.Count + _queueSmallItems.Count;

        public int MaxCountOfItems => _maxCountOfItems;

        public void Init()
        {
            Shuffle(_items);

            _maxCountOfItems = _items.Length;

            foreach (var item in _items)
            {
                if (item.Type == ItemType.Big)
                {
                    _queueBigItems.Enqueue(item);
                }
                else
                {
                    _queueSmallItems.Enqueue(item);
                }                
            }
        }

        public Item GetNextItem()
        {
            if (CountOfItems == 0)
            {
                AllDone?.Invoke();
                return null;
            }                

            return _queueSmallItems.Count > 0 ? _queueSmallItems.Dequeue() : _queueBigItems.Dequeue();
        }

        private void Shuffle(Item[] array)
        {
            int upperIndex = array.Length - 1;
            int lowerIndex = 0;
            int coefficientOfMixing = 5;
            int numberOfIterations = coefficientOfMixing * array.Length;

            int oldIndex;
            int newIndex;

            for (int i = 0; i < numberOfIterations; i++)
            {
                oldIndex = Random.Range(lowerIndex, upperIndex + 1);
                newIndex = Random.Range(lowerIndex, upperIndex + 1);

                (array[oldIndex], array[newIndex]) = (array[newIndex], array[oldIndex]);
            }
        }
    }
}

