using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Assets.Scripts.MainCore
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Minion : MonoBehaviour
    {
        private readonly int _isCarryKey = Animator.StringToHash("IsCarry");
        private readonly int _pickupKey = Animator.StringToHash("Pickup");

        [SerializeField] private float _minDistance = 2f;
        [SerializeField] private Transform _pointToObject;
        [SerializeField] private Animator _animator;

        private NavMeshAgent _agent;
        private Vector3 _carPositon;
        private Item _targetItem;
        private IEnumerator _currentState;

        private delegate void MinionAction();

        public event UnityAction<Minion> OnSellItem;

        public void Init(Vector3 carPositon)
        {
            _carPositon = carPositon;
            _agent = GetComponent<NavMeshAgent>();
        }

        public void SetNewItem(Item item)
        {
            _targetItem = item;
            GoToItem();            
        }

        public void AddSpeed()
        {
            _agent.speed += 1;
        }

        private void GoToItem()
        {
            var itemPositon = _targetItem.gameObject.transform.position;
            MinionAction stopNearItem = StopNearItem;
            
            StartState(GoToTarget(itemPositon, stopNearItem));
        }

        private IEnumerator GoToTarget(Vector3 targetPosition, MinionAction minionAction)
        {
            _agent.SetDestination(targetPosition);

            var distane = Vector3.Distance(transform.position, targetPosition);            
            var waitForEndOfFrame = new WaitForEndOfFrame();

            while (distane > _minDistance)
            {
                yield return waitForEndOfFrame;
                distane = Vector3.Distance(transform.position, targetPosition);                
            }

            minionAction();
        }

        private void StopNearItem()
        {
            _agent.isStopped = true;

            _targetItem.OnAnimationComplete += TakeItem;
            _targetItem.PlayLiftingAnimation(_pointToObject.position);

            _animator.SetTrigger(_pickupKey);
        }

        private void TakeItem()
        {
            _targetItem.OnAnimationComplete -= TakeItem;
            _targetItem.gameObject.transform.SetParent(transform);

            _animator.SetBool(_isCarryKey, true);
            _agent.isStopped = false;            

            MinionAction sellItem = SellItem;
            StartState(GoToTarget(_carPositon, sellItem));
        }

        private void SellItem()
        {
            _targetItem.Sell();
            _animator.SetBool(_isCarryKey, false);
            Stop();
        }

        private void Stop()
        {
            StopCoroutine(_currentState);
            _currentState = null;

            OnSellItem?.Invoke(this);
        }

        private void StartState(IEnumerator coroutine)
        {
            if (_currentState != null)
                StopCoroutine(_currentState);

            _currentState = coroutine;
            StartCoroutine(coroutine);
        }
    }
}
