using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum CardTargetType
{
    PLAYER,
    ENEMY
}

public class Card : MonoBehaviour
{
    [SerializeField] private CardTargetType cardTargetType;
    [SerializeField] private string cardName;
    [SerializeField] private int cardEnergy;
    [SerializeField] private List<CardEffect> cardEffectList;

    private Button _cardButton;

    private void Awake()
    {
        _cardButton = GetComponent<Button>();
    }

    private void Start()
    {
        _cardButton.onClick.AddListener(() =>
        {
            CardManager.Instance.SelectCard(this);
        });
    }

    public CardTargetType GetCardTargetType()
    {
        return cardTargetType;
    }

    public void ExecuteCurrentCardEffects()
    {
        Target target = GameManager.Instance.GetCurrentTargetSelected();

        foreach (var cardEffect in cardEffectList)
        {
            cardEffect.Use(target);
        }
    }

    public void DisableCardButton()
    {
        _cardButton.interactable = false;
    }
    
    public void EnableCardButton()
    {
        _cardButton.interactable = true;
    }
    

    public string GetCardName()
    {
        return cardName;
    }

    public int GetCardEnergy()
    {
        return cardEnergy;
    }

    public bool HasStrike()
    {
        foreach (var cardEffect in cardEffectList.Where(x => x is AttackCardEffect))
        {
            AttackCardEffect attackCardEffect = (AttackCardEffect)cardEffect;
            if (attackCardEffect.HasStrike())
            {
                return true;
            }
        }
        return false;
    }

    public bool HasAttackEffects()
    {
        if (cardEffectList.FindIndex(x => x is AttackCardEffect) != -1)
        {
            return true;
        }
        return false;
    }
}
