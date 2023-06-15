using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Sounds
{
    [RequireComponent(typeof(Button))]
    public class ClickSound : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(PlaySoundClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(PlaySoundClick);
        }

        private void PlaySoundClick()
        {
            Sound.Instance.PlayUISFX(CollectionOfSounds.Button);
        }
    }
}