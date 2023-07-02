using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MainCore.Toturials
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private List<TutorialAction> _actions;
        [SerializeField] private float _delayBeforeStart = 2f;
        [SerializeField][Range(1f, 10f)] private float _delayAfterShowing = 5f;

        private int _currentNumberOfTutorial = 0;

        private void Start()
        {
            Invoke(nameof(ActivateTutorial), _delayBeforeStart);
        }

        private void OnDestroy()
        {
            foreach (var action in _actions)
            {
                action.OnTutorialComplete -= ChooseNextTutorial;
            }
        }
        
        private void ActivateTutorial()
        {
            _actions[_currentNumberOfTutorial].StartTutorial();
            _actions[_currentNumberOfTutorial].OnTutorialComplete += ChooseNextTutorial;
        }
        
        private void ChooseNextTutorial()
        {
            _actions[_currentNumberOfTutorial].OnTutorialComplete -= ChooseNextTutorial;
            _currentNumberOfTutorial++;
            
            if (_currentNumberOfTutorial == _actions.Count)
                return;
        
            StartCoroutine(WaitStartNextTutorial());
        }
        
        private IEnumerator WaitStartNextTutorial()
        {
            yield return new WaitForSeconds(_delayAfterShowing);
            ActivateTutorial();
        }
    }
}