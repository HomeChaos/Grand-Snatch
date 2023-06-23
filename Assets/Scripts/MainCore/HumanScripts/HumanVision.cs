using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.MainCore.HumanScripts
{
    public class HumanVision : MonoBehaviour
    {
        [SerializeField] private LayerMask _mask;
        [SerializeField] private float _radius = 1f;
        [SerializeField] private UnityEvent _action;

        private bool _isObjectSeen = false;

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireArc(transform.position, transform.up, transform.right, 360, _radius);
        }
#endif
        private void Update()
        {
            if (_isObjectSeen)
                return;

            if (Physics.OverlapSphere(transform.position, _radius, _mask).Length > 0)
                OnObjectSeen();
        }

        void OnObjectSeen()
        {
            _action?.Invoke();
            _isObjectSeen = true;
        }
    }
}