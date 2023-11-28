using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DebuffType
{
    EXTRA_DAMAGE_RECEIVED,
    EXTRA_DAMAGE_ADDED,
    DAMAGE_POINT_RECEIVED_MULTIPLIER,
    DAMAGE_POINT_ADDED_MULTIPLIER,
    CLEAR_DAMAGE_RECEIVED,
    CLEAR_DAMAGE_ADDED,
}

public enum DebuffDecrementType
{
    DIRECT,
    ATTACK,
    TURN,
}

[Serializable]    
public class Debuff
{
    public DebuffType debuffType;
    public float value;
    public DebuffDecrementType debuffDecrementType;
    public int numberIteration;
}

public class DebuffCardEffect : CardEffect
{
    [SerializeField] private List<Debuff> debuffList;
    
    public override void Use(Target targetController)
    {
        targetController.AddDebuffToList(debuffList);
    }
}
