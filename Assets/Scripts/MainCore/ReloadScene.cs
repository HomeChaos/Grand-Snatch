using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.MainCore
{
    public class ReloadScene: MonoBehaviour
    {
        public void Reload()
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
