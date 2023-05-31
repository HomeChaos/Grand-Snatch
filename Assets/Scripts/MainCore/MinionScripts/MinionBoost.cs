using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.MainCore.MinionScripts
{
    public class MinionBoost: MonoBehaviour
    {
        [SerializeField] private float _boostTime = 1f;
        

        private NavMeshAgent _agent;
        private MinionAccelerator _accelerator;
        
        private Coroutine _boost;

        private void OnDisable()
        {
            if (_accelerator != null)
                _accelerator.OnClick -= EnableBoost;
        }

        public void Init(NavMeshAgent agent, MinionAccelerator accelerator)
        {
            _agent = agent;
            _agent.speed = MinionSpecifications.Instance.MinSpeed;
            _accelerator = accelerator;
            _accelerator.OnClick += EnableBoost;            
        }

        public void UpdateSpeed()
        {
            _agent.speed = MinionSpecifications.Instance.MinSpeed;
        }

        private void EnableBoost()
        {
            if (_boost != null)
                StopCoroutine(_boost);

            _boost = StartCoroutine(ProduceBoost());
        }

        private IEnumerator ProduceBoost()
        {
            _agent.speed = MinionSpecifications.Instance.MaxSpeed;
            yield return new WaitForSeconds(_boostTime);
            _agent.speed = MinionSpecifications.Instance.MinSpeed;
            _boost = null;
        }
    }
}
