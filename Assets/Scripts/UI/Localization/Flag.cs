using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Localization
{
    [RequireComponent(typeof(Button))]
    public class Flag : MonoBehaviour
    {
        [SerializeField] private GameObject _iconCheck;
        [SerializeField] private string _language;

        private Button _button;

        public string Language => _language;
        
        public event UnityAction<Flag, string> FlagClicked;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClickFlag);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClickFlag);
        }

        public void SetIconActive(bool iconEnabled)
        {
            _iconCheck.SetActive(iconEnabled);
        }

        private void OnClickFlag()
        {
            FlagClicked?.Invoke(this, _language);
        }
    }
}