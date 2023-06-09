using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Assets.Scripts.MainCore.MinionScripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(MinionMoving))]
    [RequireComponent(typeof(MinionBoost))]
    public class Minion : MonoBehaviour
    {
        private MinionMoving _minionMoving;
        private MinionBoost _minionBoost;        

        public event UnityAction<Minion> OnSellItem;

        private void OnDestroy()
        {
            _minionMoving.OnSellItem -= SellItem;
        }

        public void Init(Vector3 carPosition, Clicker clicker)
        {
            var agent = GetComponent<NavMeshAgent>();
            _minionMoving = GetComponent<MinionMoving>();
            _minionBoost = GetComponent<MinionBoost>();

            _minionMoving.Init(agent, carPosition);
            _minionBoost.Init(agent, clicker);

            _minionMoving.OnSellItem += SellItem;
        }

        public void SetNewItem(Item item)
        {
            _minionMoving.GoToItem(item);       
        }

        public void UpdateSpeed()
        {
            _minionBoost.UpdateSpeed();
        }

        private void SellItem()
        {
            OnSellItem?.Invoke(this);
        }
    }
}
