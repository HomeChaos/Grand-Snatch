using Assets.Scripts.Shop;
using UnityEngine;

namespace Assets.Scripts.MainCore
{
    public class Car : MonoBehaviour
    {
        [SerializeField] private CarType _carType;
        
        public CarType CarType => _carType;
    }
}