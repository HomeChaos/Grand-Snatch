using Assets.Scripts.Data;
using Assets.Scripts.UI;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Shop
{
    [RequireComponent(typeof(Button))]
    public class Product : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _iconCheck;
        [SerializeField] private GameObject _iconLock;
        [SerializeField] private GameObject _adIcon;
        [SerializeField] private TMP_Text _cost;
        [SerializeField] private LeanLocalizedTextMeshProUGUI _localized;

        private Price _price;
        private Button _button;

        public event UnityAction<Product, Price> Clicked;

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        public void Init(Price price)
        {
            _price = price;

            _image.sprite = price.Image;

            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
            
            _localized.TranslationName = price.NameKey;
            _localized.UpdateLocalization();

            _iconCheck.SetActive(false);

            SetCorrectCost();

            if (PlayerData.Instance.SelectedCar == (int)price.CarType)
                Select();
        }

        public void Unlock()
        {
            _iconLock.SetActive(false);
            _adIcon.SetActive(false);
            _cost.text = "";
        }

        private void OnClick() => Clicked?.Invoke(this, _price);

        public void Select()
        {
            _iconCheck.SetActive(true);
        }

        public void UnSelect()
        {
            _iconCheck.SetActive(false);
        }

        public void UpdateCostText()
        {
            var numberOfOperationBeforeBuy = PlayerData.Instance.ConditionsForCars[_price.CarType];
            _cost.text = $"{numberOfOperationBeforeBuy} / {_price.Cost}";
        }

        private void SetCorrectCost()
        {
            var numberOfOperationBeforeBuy = PlayerData.Instance.ConditionsForCars[_price.CarType];
            
            if (numberOfOperationBeforeBuy == 0)
            {
                Unlock();
            }
            else if (_price.IsBuyForAd)
            {
                _adIcon.SetActive(true);
                _cost.text = $"{numberOfOperationBeforeBuy} / {_price.Cost}";
            }
            else
            {
                _cost.text = NumberSeparator.SplitNumber(_price.Cost) + " $";
            }
        }
    }
}