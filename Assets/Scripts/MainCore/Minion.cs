using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Assets.Scripts.MainCore
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Minion : MonoBehaviour
    {        
        [SerializeField] private float _minDistance = 2f;
        [SerializeField] private Transform _pointToObject;
        [SerializeField] private Animator _animator;

        private NavMeshAgent _agent;
        private Vector3 _carPositon;
        private Vector3 _targetPositon;
        private Item _targetItem;

        public event UnityAction<Minion> OnBroughtItem;

        private IEnumerator _currentState;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public void Init(Vector3 carPositon)
        {
            _carPositon = carPositon;
            _agent = GetComponent<NavMeshAgent>();
        }

        public void SetNewItemPosition(Item item)
        {
            _targetItem = item;
            _targetPositon = item.gameObject.transform.position;
            
            StartState(GoToItem());
        }

        public void AddSpeed()
        {
            _agent.speed += 1;
        }

        private IEnumerator GoToItem()
        {
            _agent.SetDestination(_targetPositon);
            _animator.SetTrigger("Runnig");
            var distane = Vector3.Distance(transform.position, _targetPositon);            
            var waitForEndOfFrame = new WaitForEndOfFrame();

            while (distane > _minDistance || IsItemTakenAway())
            {
                yield return waitForEndOfFrame;
                distane = Vector3.Distance(transform.position, _targetPositon);                
            }

            if (IsItemTakenAway())
            {
                Stop();
                yield return null;
            }

            _agent.isStopped = true;
            _targetItem.OnItemTaken += OnItemTaken;

            PickupObject(_targetItem);
            _animator.SetTrigger("Pickup");
        }

        private void OnItemTaken()
        {
            _targetItem.OnItemTaken -= OnItemTaken;
            _targetItem.gameObject.transform.SetParent(transform);
            StartState(GoToCar());
        }

        private IEnumerator GoToCar()
        {
            _animator.SetBool("IsCarry", true);
            _agent.isStopped = false;
            _agent.SetDestination(_carPositon);
            
            var distane = Vector3.Distance(transform.position, _carPositon);
            var waitForEndOfFrame = new WaitForEndOfFrame();

            while (distane > _minDistance)
            {
                yield return waitForEndOfFrame;
                distane = Vector3.Distance(transform.position, _carPositon);
            }

            _targetItem.Sell();
            _animator.SetBool("IsCarry", false);
            Stop();         
        }

        private void Stop()
        {
            StopCoroutine(_currentState);
            _currentState = null;

            OnBroughtItem?.Invoke(this);
        }

        private bool IsItemTakenAway()
        {
            return _targetItem == null;
        }

        private void StartState(IEnumerator coroutine)
        {
            if (_currentState != null)
                StopCoroutine(_currentState);

            _currentState = coroutine;
            StartCoroutine(coroutine);
        }

        private void PickupObject(Item item)
        {            
            item.GetUp(_pointToObject.position);
        }
    }
}
