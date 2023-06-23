using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.Sounds;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.MainCore.HumanScripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Human : MonoBehaviour
    {
        private readonly int _isWalkKey = Animator.StringToHash("IsWalk");
        private readonly int _runKey = Animator.StringToHash("Run");
        
        [SerializeField] private Animator _animator;
        [SerializeField] private float _delayBeforeWalk;
        [SerializeField] private float _minDistanceToPoint = 1f;
        [SerializeField] private ParticleSystem _emoji;

        private NavMeshAgent _agent;
        private IEnumerator _currentState;
        private Transform _exitPoint;
        private List<Transform> _pointToWalk;
        private bool _isRunningToExit;

        public void Init(Transform pointToExit, List<Transform> pointToWalk)
        {
            _exitPoint = pointToExit;
            _pointToWalk = pointToWalk;
            
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = PlayerData.Instance.Config.HumanSpeed;

            _animator.SetBool(_isWalkKey, true);
            StartState(GoToPoint(GetRandomPoint()));
        }

        public void StartRunningToExit()
        {
            if (_isRunningToExit == false)
            {
                Sound.Instance.PlaySFX(CollectionOfSounds.ScreamMan);
                _emoji.Play();
                _isRunningToExit = true;
                _animator.SetTrigger(_runKey);
                _agent.speed = 4;
                StartState(GoToPoint(_exitPoint.position));
            }
        }

        private IEnumerator GoToPoint(Vector3 point)
        {
            _agent.isStopped = false;
            _agent.SetDestination(point);
            var distance = Vector3.Distance(transform.position, point);
            var waitForEndOfFrame = new WaitForEndOfFrame();
            
            while (distance > _minDistanceToPoint)
            { 
                yield return waitForEndOfFrame;
                distance = Vector3.Distance(transform.position, point);
            }

            if (_isRunningToExit)
            {
                ExitScene();
            }
            else
            {
                
                StartState(WaitNextPoint());
            }            
        }

        private IEnumerator WaitNextPoint()
        {
            _animator.SetBool(_isWalkKey, false);
            _agent.isStopped = true;
            yield return new WaitForSeconds(_delayBeforeWalk);
            _animator.SetBool(_isWalkKey, true);
            StartState(GoToPoint(GetRandomPoint()));
        }

        private Vector3 GetRandomPoint()
        {
            return _pointToWalk[Random.Range(0, _pointToWalk.Count)].position;
        }

        private void ExitScene()
        {
            if (_currentState != null)
                StopCoroutine(_currentState);
            
            Destroy(gameObject);
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