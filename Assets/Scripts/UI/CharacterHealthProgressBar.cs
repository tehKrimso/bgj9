using Characters;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CharacterHealthProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _healthFillImage;
        
        private BattleCharacter _character;

        private void Awake()
        {
            _character = GetComponent<BattleCharacter>();
        }

        private void Update()
        {
            _healthFillImage.fillAmount = _character.GetStats().HealthPercent;
        }
    }
}