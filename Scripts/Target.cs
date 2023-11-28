using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public abstract class Target : MonoBehaviour
{
    [SerializeField] private float healthPoints;
    [SerializeField] private float armorPoints;
    [SerializeField] private Button selectionButton;
    [SerializeField] protected float startAnimationTimerMax = 1f;
    
    [Header("Debuff Settings")]
    private List<Debuff> debuffList = new();
    [SerializeField] protected float extraDamageReceived;
    [SerializeField] protected float extraDamageAdded;

    public event EventHandler OnHealthPointUpdate;
    public event EventHandler OnArmorPointUpdate;
    public event EventHandler OnDebuffUpdate;
    public event EventHandler OnDamageReceive;
    public event EventHandler OnAttack;
    public event EventHandler OnNextDamageChange;
    public event EventHandler OnArmorIncrease;
    public event EventHandler OnMagic;
    
    public static event EventHandler OnAnyAttack;

    private float _currentHealthPoints;
    private float _currentArmorPoints;
    
    protected float _startAnimationTimer;
    private bool _isReceivingImpact;
    private bool _updateShield;


    protected virtual void Awake()
    {
        _currentHealthPoints = healthPoints;
        _currentArmorPoints = armorPoints;

        selectionButton.interactable = false;
        
        selectionButton.onClick.AddListener(UseCardOnTarget);
    }

    private void UseCardOnTarget()
    {
        if (CardManager.Instance.GetRemainingEnergy() >= CardManager.Instance.GetEnergyCurrentCard()){
            
            GameManager.Instance.SetCurrentTargetSelected(this);
            CardManager.Instance.ExecuteCurrentCard();
            CardManager.Instance.RemoveCardFromHandList();

            if (CardManager.Instance.GetCurrentCardSelected().HasAttackEffects())
            {
                PlayerTarget.Instance.OnAttackEvent();
            }
            else
            {
                OnMagic?.Invoke(this,EventArgs.Empty);
            }
            GameManager.Instance.DisableAllSelectionButtons();
        }
    }
    
    public void AddDebuffToList(List<Debuff> debuffList)
    {
        this.debuffList.AddRange(debuffList);
        CalculateDirectDebuff();
    }

    public float GetExtraDamageAdded()
    {
        float extra_damage_added = extraDamageAdded;
        
        foreach (var debuff in debuffList.Where(x => x.debuffType == DebuffType.EXTRA_DAMAGE_ADDED))
        {
            extra_damage_added += debuff.value;
        }
        
        foreach (var debuff in debuffList.Where(x => x.debuffType == DebuffType.DAMAGE_POINT_ADDED_MULTIPLIER))
        {
            extra_damage_added *= debuff.value;
        }

        return extra_damage_added;
    }
    
    public float GetExtraDamageReceived()
    {
        float extra_damage_received = extraDamageReceived;

        foreach (var debuff in debuffList.Where(x => x.debuffType == DebuffType.EXTRA_DAMAGE_RECEIVED))
        {
            extra_damage_received += debuff.value;
        }
        
        foreach (var debuff in debuffList.Where(x => x.debuffType == DebuffType.DAMAGE_POINT_RECEIVED_MULTIPLIER))
        {
            extra_damage_received *= debuff.value;
        }
        
        return extra_damage_received;
    }
    
    public void DecrementAttackDebuffList()
    {
        for (int i = debuffList.Count - 1; i >= 0; i--)
        {
            if (debuffList[i].debuffDecrementType != DebuffDecrementType.ATTACK)
                continue;
            
            debuffList[i].numberIteration--;
                
            if (debuffList[i].numberIteration <= 0)
            {
                debuffList.RemoveAt(i);
            }
        }
        OnDebuffUpdate?.Invoke(this, EventArgs.Empty);
    }
    
    public void DecrementTurnDebuffList()
    {
        for (int i = debuffList.Count - 1; i >= 0; i--)
        {
            if (debuffList[i].debuffDecrementType != DebuffDecrementType.TURN)
                continue;
            
            debuffList[i].numberIteration--;
                
            if (debuffList[i].numberIteration <= 0)
            {
                debuffList.RemoveAt(i);
            }
        }
        OnDebuffUpdate?.Invoke(this, EventArgs.Empty);
    }
    
    public void ClearDirectDebuffFromList()
    {
        for (int i = debuffList.Count - 1; i >= 0; i--)
        {
            if (debuffList[i].debuffDecrementType != DebuffDecrementType.DIRECT)
                continue;
            
            debuffList.RemoveAt(i);
        }
    }

    protected void OnAttackEvent()
    {
        OnAttack?.Invoke(this, EventArgs.Empty);
        DecrementAttackDebuffList();
    }
    
    protected void OnNextDamageChangeEvent()
    {
        OnNextDamageChange?.Invoke(this, EventArgs.Empty);
    }
    
    
    /*
    private void CalculateDebuff()
    {
        extraDamageAdded = 0f;
        extraDamageReceived = 0f;
        
        foreach (var debuff in debuffList.Where(x => x.debuffType == DebuffType.EXTRA_DAMAGE_ADDED || x.debuffType == DebuffType.EXTRA_DAMAGE_RECEIVED))
        {
            if (debuff.debuffType == DebuffType.EXTRA_DAMAGE_ADDED)
            {
                extraDamageAdded += debuff.value;
            }
            else if (debuff.debuffType == DebuffType.EXTRA_DAMAGE_RECEIVED)
            {
                extraDamageReceived += debuff.value;
            }
        }
        foreach (var debuff in debuffList.Where(x => x.debuffType == DebuffType.DAMAGE_POINT_RECEIVED_MULTIPLIER || x.debuffType == DebuffType.DAMAGE_POINT_ADDED_MULTIPLIER))
        {
            if (debuff.debuffType == DebuffType.DAMAGE_POINT_ADDED_MULTIPLIER)
            {
                extraDamageAdded *= debuff.value;
            }
            else if (debuff.debuffType == DebuffType.DAMAGE_POINT_RECEIVED_MULTIPLIER)
            {
                extraDamageReceived *= debuff.value;
            }
        }
        bool clearDamageAddedDebuff = false;
        bool clearDamageReceivedDebuff = false;
        foreach (var debuff in debuffList.Where(x => x.debuffType == DebuffType.CLEAR_DAMAGE_ADDED || x.debuffType == DebuffType.CLEAR_DAMAGE_RECEIVED))
        {
            if (debuff.debuffType == DebuffType.CLEAR_DAMAGE_ADDED)
            {
                extraDamageAdded = 0f;
                clearDamageAddedDebuff = true;
            }
            else if (debuff.debuffType == DebuffType.CLEAR_DAMAGE_RECEIVED)
            {
                extraDamageReceived = 0f;
                clearDamageReceivedDebuff = true;
            }
        }
        if (clearDamageAddedDebuff)
        {
            ClearDamageAddedDebuffFromDebuffList();
        }
        
        
        if (clearDamageReceivedDebuff)
        {
            ClearDamageReceivedDebuffFromDebuffList();
        }
        OnDebuffUpdate?.Invoke(this, EventArgs.Empty);
    }
    */
    
    
    private void CalculateDirectDebuff()
    {
        foreach (var debuff in debuffList.Where(x => (x.debuffType == DebuffType.EXTRA_DAMAGE_ADDED || x.debuffType == DebuffType.EXTRA_DAMAGE_RECEIVED) && x.debuffDecrementType == DebuffDecrementType.DIRECT))
        {
            if (debuff.debuffType == DebuffType.EXTRA_DAMAGE_ADDED)
            {
                extraDamageAdded += debuff.value;
            }
            else if (debuff.debuffType == DebuffType.EXTRA_DAMAGE_RECEIVED)
            {
                extraDamageReceived += debuff.value;
            }
        }
        foreach (var debuff in debuffList.Where(x => (x.debuffType == DebuffType.DAMAGE_POINT_RECEIVED_MULTIPLIER || x.debuffType == DebuffType.DAMAGE_POINT_ADDED_MULTIPLIER) && x.debuffDecrementType == DebuffDecrementType.DIRECT))
        {
            if (debuff.debuffType == DebuffType.DAMAGE_POINT_ADDED_MULTIPLIER)
            {
                extraDamageAdded *= debuff.value;
            }
            else if (debuff.debuffType == DebuffType.DAMAGE_POINT_RECEIVED_MULTIPLIER)
            {
                extraDamageReceived *= debuff.value;
            }
        }
        
        bool clearDamageAddedDebuff = false;
        bool clearDamageReceivedDebuff = false;
        
        foreach (var debuff in debuffList.Where(x => (x.debuffType == DebuffType.CLEAR_DAMAGE_ADDED || x.debuffType == DebuffType.CLEAR_DAMAGE_RECEIVED) && x.debuffDecrementType == DebuffDecrementType.DIRECT))
        {
            if (debuff.debuffType == DebuffType.CLEAR_DAMAGE_ADDED)
            {
                extraDamageAdded = 0f;
                clearDamageAddedDebuff = true;
            }
            else if (debuff.debuffType == DebuffType.CLEAR_DAMAGE_RECEIVED)
            {
                extraDamageReceived = 0f;
                clearDamageReceivedDebuff = true;
            }
        }
        
        if (clearDamageAddedDebuff)
        {
            ClearDamageAddedDebuffFromDebuffList();
        }
        
        if (clearDamageReceivedDebuff)
        {
            ClearDamageReceivedDebuffFromDebuffList();
        }

        ClearDirectDebuffFromList();
        OnDebuffUpdate?.Invoke(this, EventArgs.Empty);
    }

    private void ClearDamageAddedDebuffFromDebuffList()
    {
         for (int i = debuffList.Count - 1; i >= 0; i--)
         {
             if (debuffList[i].debuffType == DebuffType.EXTRA_DAMAGE_ADDED || debuffList[i].debuffType == DebuffType.DAMAGE_POINT_ADDED_MULTIPLIER)
             {
                 debuffList.RemoveAt(i);
             }
         }
    }
    
    private void ClearDamageReceivedDebuffFromDebuffList()
    {
        for (int i = debuffList.Count - 1; i >= 0; i--)
        {
            if (debuffList[i].debuffType == DebuffType.EXTRA_DAMAGE_RECEIVED || debuffList[i].debuffType == DebuffType.DAMAGE_POINT_RECEIVED_MULTIPLIER)
            {
                debuffList.RemoveAt(i);
            }
        }
    }
     
    public void DisableSelectionButton()
    {
        selectionButton.interactable = false;
    }
    
    public void EnableSelectionButton()
    {
        selectionButton.interactable = true;
    }

    public virtual void ReceiveDamage(float damagePoints)
    {
        damagePoints += GetExtraDamageReceived();
        
        if (_currentArmorPoints > damagePoints)
        {
            _currentArmorPoints -= damagePoints;
        }
        else
        {
            float currentDamagePoint = Mathf.Max(0, damagePoints - _currentArmorPoints);
			_currentArmorPoints = 0f;
			_currentHealthPoints -= currentDamagePoint;
        }

        _isReceivingImpact = true;
        DecrementAttackDebuffList();
    }
    
    public void IncreaseArmor(float extraArmorPoints)
    {
        _currentArmorPoints += extraArmorPoints;
        OnArmorPointUpdate?.Invoke(this, EventArgs.Empty);
        OnArmorIncrease?.Invoke(this, EventArgs.Empty);
    }
    
    public float GetCurrentHealthPoints()
    {
        return _currentHealthPoints;
    }
    
    public float GetHealthPointMax()
    {
        return healthPoints;
    }

    public float GetArmorPoints()
    {
        return _currentArmorPoints;
    }
    
    protected virtual void Update()
    {
        if (!_isReceivingImpact)
            return;
      
        if (_startAnimationTimer <= startAnimationTimerMax)
        {
            _startAnimationTimer += Time.deltaTime;
        }
        else
        {
            OnHealthPointUpdate?.Invoke(this, EventArgs.Empty);
            OnArmorPointUpdate?.Invoke(this, EventArgs.Empty);
            OnDamageReceive?.Invoke(this, EventArgs.Empty);
            OnNextDamageChangeEvent();
            _startAnimationTimer = 0f;
            _isReceivingImpact = false;
        }
    }
}