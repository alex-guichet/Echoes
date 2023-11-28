using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCardEffect : CardEffect
{
    [SerializeField] private float damagePoint;
    [SerializeField] private bool strike = false;
    
    public override void Use(Target target)
    {
		float damageFinal = damagePoint + PlayerTarget.Instance.GetExtraDamageAdded();
        target.ReceiveDamage(damageFinal > 0f ? damageFinal : 0f);
    }

    public bool HasStrike()
    {
        return strike;
    }
}
