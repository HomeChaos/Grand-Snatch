using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Localization
{
    [RequireComponent(typeof(Button))]
    public class Flag : MonoBehaviour
    {
        [SerializeField] private GameObject _icon_check;
        [SerializeField] private string _language;

        private Button _button;

        public string Language => _language;
        
        public event UnityAction<Flag, string> FlagClicked; 
        
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClickFlag);
        }

        private void OnClickFlag()
        {
            FlagClicked?.Invoke(this, _language);
        }

        public void SetIconActive(bool iconEnabled)
        {
            _icon_check.SetActive(iconEnabled);
        }
    }
}