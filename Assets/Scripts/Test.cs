using System;
using Assets.Scripts.UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed = 1f;

        private void Start()
        {
            
            
        }

        private void Update()
        {
            transform.RotateAround (_target.position, Vector3.up, _speed * Time.deltaTime);
        }
    }
}