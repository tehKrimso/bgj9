using Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CharacterHealthProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _healthFillImage;
        [SerializeField] private TextMeshProUGUI _healthText;
        
        private BattleCharacter _character;

        private void Awake()
        {
            _character = GetComponent<BattleCharacter>();
        }

        private void Update()
        {
            _healthFillImage.fillAmount = _character.GetStats().HealthPercent;
            _healthText.text = _character.GetStats().Health.ToString();
        }
    }
}