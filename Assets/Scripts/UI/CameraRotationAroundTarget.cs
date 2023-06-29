using UnityEngine;

namespace Assets.Scripts.UI
{
    public class CameraRotationAroundTarget : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed = 10f;

        private void Update()
        {
            transform.RotateAround(_target.position, Vector3.up, _speed * Time.deltaTime);
        }
    }
}