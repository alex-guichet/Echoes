using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseCardEffect : CardEffect
{
    [SerializeField] public float armorPoints;
    
    public override void Use(Target target)
    {
        target.IncreaseArmor(armorPoints);
    }
}
