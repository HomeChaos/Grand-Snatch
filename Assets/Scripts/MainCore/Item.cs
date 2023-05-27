using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.MainCore
{
    public class Item: MonoBehaviour
    {
        public event UnityAction OnItemTaken;
        
        public void Sell()
        {
            Debug.Log("Меня продали :3");
            Destroy(gameObject);
        }

        public void GetUp(Vector3 position)
        {
            StartCoroutine(Pickup(position));
        }

        private IEnumerator Pickup(Vector3 position)
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();

            while (transform.position != position) 
            {
                transform.position = Vector3.MoveTowards(transform.position, position, 1f * Time.deltaTime);
                yield return waitForEndOfFrame;
            }
  
            OnItemTaken?.Invoke();
        }
    }
}
