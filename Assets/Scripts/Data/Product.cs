using System;
using Assets.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Data
{
    public class Product : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _value;
        [SerializeField] private TMP_Text _cost;
        [SerializeField] private string _template;

        public void SetValues(int value, int cost)
        {
            _value.text = string.Format(_template, value);
            _cost.text = $"{NumberSeparator.SplitNumber(cost)}$"; 
        }
    }
}
