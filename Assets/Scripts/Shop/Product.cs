using Assets.Scripts.Data;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Shop
{
    public class Product : MonoBehaviour
    {
        [SerializeField] private CarType _type;
        [SerializeField] private GameObject _template;
        [SerializeField] private GameObject _point;
        [SerializeField] private GameObject _iconCheck;
        [SerializeField] private GameObject _iconLock;
        [SerializeField] private TMP_Text _cost;

        public CarType Type => _type;

        private void Start()
        {
            GameObject car = Instantiate(_template, _point.transform);
            car.transform.localScale = new Vector3(1, 1, 1);
            car.transform.position = new Vector3(0, 0, 0);
            car.transform.localPosition = new Vector3(0, 0, 0);
            _iconCheck.SetActive(false);
        }

        public void Unlock()
        {
            _iconLock.SetActive(false);
            _cost.text = "";
        }

        public void Select()
        {
            _iconCheck.SetActive(true);
        }
    }
}