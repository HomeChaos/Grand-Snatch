using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Assets.Scripts.MainCore.MinionScripts
{
    
    public class MinionMoving : MonoBehaviour
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

        public event UnityAction OnSellItem;

        public void Init(NavMeshAgent agent, Vector3 carPosition)
        {
            _agent = agent;
            _carPositon = carPosition;
        }

        public void GoToItem(Item item)
        {
            _targetItem = item;

            var itemPositon = _targetItem.gameObject.transform.position;
            MinionAction stopNearItem = WaitToPickupItem;

            StartState(GoToTarget(itemPositon, stopNearItem));
        }

        private void WaitToPickupItem()
        {
            _agent.isStopped = true;

            _targetItem.OnAnimationComplete += TakeItem;
            _targetItem.PlayLiftingAnimation(_pointToObject.position);

            _animator.SetTrigger(_pickupKey);
        }

        private IEnumerator GoToTarget(Vector3 targetPosition, MinionAction nextMinionAction)
        {
            _agent.SetDestination(targetPosition);

            var distane = Vector3.Distance(transform.position, targetPosition);
            var waitForEndOfFrame = new WaitForEndOfFrame();

            while (distane > _minDistance)
            {
                yield return waitForEndOfFrame;
                distane = Vector3.Distance(transform.position, targetPosition);
            }

            nextMinionAction();
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

            OnSellItem?.Invoke();
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
