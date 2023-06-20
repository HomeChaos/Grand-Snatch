using System;
using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Shop
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private List<Product> _products;

        private void Start()
        {
            Invoke(nameof(Init), 0.1f);
        }

        public void Init()
        {
            var data = PlayerData.Instance.Money;

            foreach (var car in _products)
            {
                if (((int) car.Type) == 2)
                {
                    car.Select();
                    break;
                }
            }
        }
    }
}