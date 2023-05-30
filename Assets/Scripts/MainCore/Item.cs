using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.MainCore
{
    public class Item: MonoBehaviour
    {
        [SerializeField] private ItemType _type;

        public event UnityAction OnAnimationComplete;
        public ItemType Type => _type;
        
        public void Sell()
        {
            Debug.Log("Меня продали");
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
                transform.position = Vector3.MoveTowards(transform.position, position, 1f * Time.deltaTime);
                yield return waitForEndOfFrame;
            }
  
            OnAnimationComplete?.Invoke();
        }
    }
}
