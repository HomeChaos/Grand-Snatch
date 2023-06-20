using System.Collections;
using Assets.Scripts.Data;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.MainCore.MinionScripts
{
    public class MinionBoost: MonoBehaviour
    {
        [SerializeField] private float _boostTime = 1f;
        [SerializeField] private ParticleSystem _particleSystem;

        private NavMeshAgent _agent;
        private Booster _booster;
        
        private Coroutine _boost;

        private void OnDisable()
        {
            if (_booster != null)
                _booster.OnClick -= EnableBoost;
        }

        public void Init(NavMeshAgent agent, Booster booster)
        {
            _agent = agent;
            _agent.speed = GameSession.Instance.MinionSpecifications.MinSpeed;
            _booster = booster;
            _booster.OnClick += EnableBoost;            
        }

        public void UpdateSpeed()
        {
            _agent.speed = GameSession.Instance.MinionSpecifications.MinSpeed;
        }

        private void EnableBoost()
        {
            if (_boost != null)
                StopCoroutine(_boost);

            _boost = StartCoroutine(ProduceBoost());
        }

        private IEnumerator ProduceBoost()
        {
            _agent.speed = GameSession.Instance.MinionSpecifications.MaxSpeed;
            
            if (_particleSystem.isStopped)
                _particleSystem.Play();

            yield return new WaitForSeconds(_boostTime);
            
            _agent.speed = GameSession.Instance.MinionSpecifications.MinSpeed;
            _particleSystem.Stop();
            _boost = null;
        }
    }
}
