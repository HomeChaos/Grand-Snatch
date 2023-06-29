using UnityEngine.Events;

namespace Assets.Scripts.MainCore.Toturials
{
    public interface ITutorialAction
    {
        public event UnityAction OnTutorialComplete;
    }
}