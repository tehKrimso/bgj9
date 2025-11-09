using UnityEngine;

namespace Characters
{
    public class BattleCharacterAnimatorControls : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        public float DeathAnimTime;
        
        public float AttackWaitingTime = 1.5f;

        private readonly int AttackAnimParamHash = Animator.StringToHash("Attack");
        private readonly int IsDeadAnimParamHash = Animator.StringToHash("IsDead");

        public void SetAttackParam(bool isAttacking)
        {
            _animator.SetBool(AttackAnimParamHash, isAttacking);
        }
        
        public void SetIsDeadParam() => _animator.SetBool(IsDeadAnimParamHash, true);
    }
}
