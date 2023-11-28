using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyTarget : Target
{
    [SerializeField] private Vector2 baseDamage;
    [SerializeField] private bool canCopyAttack;
    
    private float _nextEnemyDamage;
    private bool _isResettingDamage;
    private float _resetTimer;

    protected override void Awake()
    {
        base.Awake();
        _nextEnemyDamage = (int)Random.Range(baseDamage.x, baseDamage.y);
    }
    
    public float GetNextEnemyDamage()
    {
        return _nextEnemyDamage + GetExtraDamageAdded();
    }
    
    public override void ReceiveDamage(float damagePoints)
    {
        if (canCopyAttack)
        {
            _nextEnemyDamage = damagePoints;
        }
        base.ReceiveDamage(damagePoints);
    }
    
    public void DamagePlayer()
    {
        float damageFinal = _nextEnemyDamage + GetExtraDamageAdded();
        OnAttackEvent();
        PlayerTarget.Instance.ReceiveDamage(damageFinal > 0f ? damageFinal : 0f);
        ResetDamage();
    }

    public void ResetDamage()
    {
        _nextEnemyDamage = (int)Random.Range(baseDamage.x, baseDamage.y);
        _isResettingDamage = true;
    }

    protected override void Update()
    {
        if (_isResettingDamage)
        {
            if (_resetTimer <= startAnimationTimerMax)
            {
                _resetTimer += Time.deltaTime;
            }
            else
            {
                _isResettingDamage = false;
                _resetTimer = 0f;
                OnNextDamageChangeEvent();
            }
        }
        
        base.Update();
    }
}