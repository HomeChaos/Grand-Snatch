using System.Collections.Generic;
using Lean.Localization;
using UnityEngine;

namespace Assets.Scripts.Shop
{
    [CreateAssetMenu(fileName = "PriceList", menuName = "Price List", order = 52)]
    public class PriceList : ScriptableObject
    {
        [SerializeField] private List<Price> _prices;

        public List<Price> Prices => _prices;
    }

    [System.Serializable]
    public class Price
    {
        [SerializeField] private CarType _carType;
        [SerializeField] private string _nameKey;
        [SerializeField] private int cost;
        [SerializeField] private bool _isBuyForAd;
        [SerializeField] private Sprite _image;

        public CarType CarType => _carType;
        public string NameKey => _nameKey;
        public int Cost => cost;
        public bool IsBuyForAd => _isBuyForAd;
        public Sprite Image => _image;
    }
}