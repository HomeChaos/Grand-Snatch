using Assets.Scripts.UI;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public class Product : MonoBehaviour
    {
        [SerializeField] private TMP_Text _value;
        [SerializeField] private TMP_Text _cost;

        public void SetValues(int value, int cost)
        {
            _value.text = value.ToString();
            _cost.text = $"{NumberSeparator.SplitNumber(cost)}$"; 
        }
    }
}
