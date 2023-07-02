using Assets.Scripts.UI;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public class ImprovementButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _value;
        [SerializeField] private TMP_Text _cost;
        [SerializeField] private ParticleSystem _particle;

        public void SetValues(int value, int cost)
        {
            _value.text = value.ToString();
            _cost.text = $"{NumberSeparator.SplitNumber(cost)}$";
        }

        public void PlayBuyParticle()
        {
            _particle.Play();
        }
    }
}