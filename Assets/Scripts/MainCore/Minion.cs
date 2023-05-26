using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.MainCore
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Minion : MonoBehaviour
    {
        [SerializeField] private Transform[] _items;
        [SerializeField] private float _minDistance = 2f;

        private NavMeshAgent _agent;
        private NavMeshPath _navMeshPath;

        private int _itemNumber = 0;


        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _navMeshPath = new NavMeshPath();
            _agent.SetDestination(_items[_itemNumber].position);
        }

        private void Update()
        {
            var distane = Vector3.Distance(transform.position, _items[_itemNumber].position);

            if (Vector3.Distance(transform.position, _items[_itemNumber].position) <= _minDistance)
            {
                _itemNumber = (_itemNumber + 1) % _items.Length;
                _agent.SetDestination(_items[_itemNumber].position);
            }
            
            //if (_item)
            //{
            //    _agent.SetDestination(_item.position);

            //    _agent.CalculatePath(_item.position, _navMeshPath);

            //    if (_navMeshPath.status == NavMeshPathStatus.PathComplete)
            //        Debug.DrawLine(transform.position, _item.position, Color.green);
            //    else
            //        Debug.DrawLine(transform.position, _item.position, Color.red);
            //}
        }
    }
}
