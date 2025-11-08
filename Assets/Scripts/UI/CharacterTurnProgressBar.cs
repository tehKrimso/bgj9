using System;
using Characters;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class CharacterTurnProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _timerFillImage;
        
        private BattleCharacter _character;

        private void Awake()
        {
            _character = GetComponent<BattleCharacter>();
        }

        private void Update()
        {
            _timerFillImage.fillAmount = _character.TimePercent;
        }
    }
}
