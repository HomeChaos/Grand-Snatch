using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.Shop;
using UnityEngine;

namespace Assets.Scripts.MainCore
{
    public class GarageOfCars : MonoBehaviour
    {
        [SerializeField] private List<Car> _cars;

        public void Init()
        {
            CarType carType = (CarType) PlayerData.Instance.SelectedCar;

            foreach (var car in _cars)
            {
                if (car.CarType == carType)
                {
                    car.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}