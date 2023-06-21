using UnityEngine;
using UnityEngine.UI;
using IJunior.TypedScenes;

namespace Assets.Scripts.UI
{
    public class MainMenu: MonoBehaviour
    {
        [SerializeField] private Button _start;
        [SerializeField] private Button _shop;
        [SerializeField] private Button _creators;

        private void Awake()
        {
            _start.onClick.AddListener(StartGame);
            _shop.onClick.AddListener(ShowShop);
            _creators.onClick.AddListener(ShowCreators);
        }

        private void OnDestroy()
        {
            _start.onClick.RemoveListener(StartGame);
            _shop.onClick.RemoveListener(ShowShop);
            _creators.onClick.RemoveListener(ShowCreators);
        }

        private void StartGame()
        {
            Level_1.Load();
        }

        private void ShowShop()
        {
            ShopLevel.Load();
        }

        private void ShowCreators()
        {
            Debug.Log("Creators");
        }
    }
}
