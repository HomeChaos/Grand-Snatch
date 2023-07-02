using Assets.Scripts.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.MainCore
{
    public class Item: MonoBehaviour
    {
        [SerializeField] private ItemType _type;
        [SerializeField] private float _aminationTime = 1f;

        public event UnityAction OnAnimationComplete;
        
        public ItemType Type => _type;
        
        public void Sell()
        {
            PaymentSystem.Instance.SellItem();
            Destroy(gameObject);
        }

        public void PlayLiftingAnimation(Vector3 position)
        {
            StartCoroutine(StartAnimation(position));
        }

        private IEnumerator StartAnimation(Vector3 position)
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();

            while (transform.position != position) 
            {
                transform.position = Vector3.MoveTowards(transform.position, position, _aminationTime * Time.deltaTime);
                yield return waitForEndOfFrame;
            }
  
            OnAnimationComplete?.Invoke();
        }
    }
}
