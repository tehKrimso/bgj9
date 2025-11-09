using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _flightTime;
    [SerializeField] private float _lifeTime = 3f;

    private float _gravity;
    private float _lifeTimeTimer;
    
    private IngredientType[] _potionContent;
    private BattleCharacter _target;
    
    public void Init(IngredientType[] potionContent ,BattleCharacter target, Color color)
    {
        _potionContent = new IngredientType[potionContent.Length];

        for (int i = 0; i < potionContent.Length; i++)
        {
            _potionContent[i] = potionContent[i];
        }
        
        _target = target;
        _spriteRenderer.color = color;
        
        _gravity = Physics2D.gravity.y;
    }

    public void LaunchPotion()
    {
        Vector2 targetPos = _target.transform.position;
        
        float Vx = (targetPos.x - transform.position.x) / _flightTime;
        float Vy = (targetPos.y - transform.position.y - 0.5f * _gravity * _flightTime * _flightTime) / _flightTime;
        
        Vector2 velocity = new Vector2(Vx, Vy);
        
        _rb.velocity = velocity;
    }

    private void Update()
    {
        _lifeTimeTimer += Time.deltaTime;
        if (_lifeTimeTimer >= _lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out BattleCharacter battleCharacter))
        {
            if (battleCharacter == _target)
            {
                battleCharacter.AddModifiers(_potionContent);
                
                //play effect on potion destroy
                Destroy(gameObject);
            }
        }
    }
}
