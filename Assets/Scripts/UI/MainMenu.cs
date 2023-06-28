using UnityEngine;
using UnityEngine.UI;
using IJunior.TypedScenes;
using TMPro;

namespace Assets.Scripts.UI
{
    public class MainMenu: MonoBehaviour
    {
        [SerializeField] private Button _start;
        [SerializeField] private Button _shop;
        [SerializeField] private TMP_Text _version;

        private void Awake()
        {
            _start.onClick.AddListener(StartGame);
            _shop.onClick.AddListener(ShowShop);
            _version.text = $"V {Application.version}";
        }

        private void OnDestroy()
        {
            _start.onClick.RemoveListener(StartGame);
            _shop.onClick.RemoveListener(ShowShop);
        }

        private void StartGame()
        {
            Level_1.Load();
        }

        private void ShowShop()
        {
            ShopLevel.Load();
        }
    }
}
