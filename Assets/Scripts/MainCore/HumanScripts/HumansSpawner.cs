using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.MainCore.HumanScripts
{
    public class HumansSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _template;
        [SerializeField] private List<Transform> _pointToInstanse;
        [SerializeField] private List<Transform> _pointToWalk;
        [SerializeField] private Transform _pointToExit;

        public void Init()
        {
            var countToSpawn = PlayerData.Instance.Config.HumanCountToSpawn;

            for (int i = 0; i < countToSpawn; i++)
            {
                Human human = Instantiate(_template, _pointToInstanse[Random.Range(0, _pointToInstanse.Count)].position, Quaternion.identity, transform).GetComponent<Human>();
                human.Init(_pointToExit, _pointToWalk);
            }
        }
    }
}