using Characters;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainCharacterThrowProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _timerFillImage;
        
        private MainCharacter _character;

        private void Awake()
        {
            _character = GetComponent<MainCharacter>();
        }

        private void Update()
        {
            _timerFillImage.fillAmount = _character.TimePercent;
        }
    }
}
