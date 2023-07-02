using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class PaymentInformant : MonoBehaviour
    {
        [SerializeField] private float _timeToAnimation;
        [SerializeField] private float _timeToDelay;
        [SerializeField] private InfoElement _enoughMinions;
        [SerializeField] private InfoElement _notMoneyForMinions;
        [SerializeField] private InfoElement _notMoneyForSpeed;
        [SerializeField] private InfoElement _notMoneyForIncome;

        private Coroutine _coroutine;
        
        private void Awake()
        {
            var items = new List<InfoElement> {_enoughMinions, _notMoneyForMinions, _notMoneyForSpeed, _notMoneyForIncome};

            foreach (var item in items)
            {
                item.Text.DOFade(0, 0);
                item.Image.DOFade(0, 0);
            }
        }

        public void OverflowMinions()
        {
            StartAnimation(_enoughMinions);
        }
        
        public void ShowInfoNotEnoughMoney(LackOfMoney lackOfMoney)
        {
            switch (lackOfMoney)
            {
                case LackOfMoney.Minions:
                    StartAnimation(_notMoneyForMinions);
                    break;
                case LackOfMoney.Speed:
                    StartAnimation(_notMoneyForSpeed);
                    break;
                case LackOfMoney.Income:
                    StartAnimation(_notMoneyForIncome);
                    break;
            }
        }

        private void StartAnimation(InfoElement element)
        {
            if (_coroutine == null)
                _coroutine = StartCoroutine(ApplyAnimation(element));
        }

        private IEnumerator ApplyAnimation(InfoElement element)
        {
            element.Text.DOFade(1, _timeToAnimation);
            element.Image.DOFade(1, _timeToAnimation);
            
            yield return new WaitForSeconds(_timeToDelay);
            
            element.Text.DOFade(0, _timeToAnimation);
            element.Image.DOFade(0, _timeToAnimation);
            _coroutine = null;
        }
    }

    [System.Serializable]
    public class InfoElement
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _image;

        public TMP_Text Text => _text;
        public Image Image => _image;
    }
}