using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<Card> cardList;
    [SerializeField] private List<Card> baseCardList;
    [SerializeField] private int numberCardInHand = 5;
    [SerializeField] private int numberEnergyMax = 3;
    
    private int _currentNumberEnergyPlayed;
    
    private List<Card> _cardsInHand = new();
    private List<Card> _cardsInDrawPile = new();
    private List<Card> _cardsInDiscardPile = new();
    private List<Card> _cardsExhausted = new();
    
    public static CardManager Instance;
    
    private Card _currentCardSelected;
    private Card _lastCardAttackingEnemy;

    public event EventHandler OnCurrentCardSelectedChange;
    public event EventHandler OnIncrementNumberCardPlayer;
    public event EventHandler OnCardDraw;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;

        foreach (Transform cardTransform in transform)
        {
            cardList.Add(cardTransform.GetComponent<Card>());
        }

        for (int i = 0; i < numberCardInHand; i++)
        {
            if (cardList.Count <= 0)
            {
                return;
            }
            
            int randomNumber = Random.Range(0, cardList.Count);
            _cardsInHand.Add(cardList[randomNumber]);
            cardList[randomNumber].gameObject.SetActive(true);
            cardList.RemoveAt(randomNumber);
        }

        foreach (var card in cardList)
        {
            _cardsInDrawPile.Add(card);
            card.gameObject.SetActive(false);
        }
    }

    public void Start()
    {
        GameManager.Instance.OnEndGame += GameManagerOnEndGame;
        GameManager.Instance.OnStartEnemyAttack += GameManagerOnStartEnemyAttack;
        GameManager.Instance.OnEndEnemyAttack += GameManagerOnEndEnemyAttack;
    }
    
    
    private void GameManagerOnStartEnemyAttack(object sender, EventArgs e)
    {
        ClearCardsInHand();
    }
    
    private void GameManagerOnEndEnemyAttack(object sender, EventArgs e)
    {
        DrawCards();
        ResetEnergy();
    }

    private void GameManagerOnEndGame(object sender, EventArgs e)
    {
        DisableAllCardsInHands();
    }

    public void RemoveCardFromHandList()
    {
        _cardsInHand.Remove(_currentCardSelected);
        _cardsInDiscardPile.Add(_currentCardSelected);
        _currentCardSelected.EnableCardButton();
        OnCardDraw?.Invoke(this, EventArgs.Empty);
    }

    public void ExhaustCardPlayed()
    {
		_cardsInHand.Remove(_currentCardSelected);
		_cardsExhausted.Add(_currentCardSelected);
		_currentCardSelected.EnableCardButton();
	}

    public void ClearCardsInHand()
    {
        foreach (var card in _cardsInHand)
        {
            _cardsInDiscardPile.Add(card);
            card.gameObject.SetActive(false);
            card.EnableCardButton();
        }
        
        _cardsInHand.Clear();
        
    }
    
    public void DrawCards()
    {
        for (int i = 0; i < numberCardInHand; i++)
        {
            if (_cardsInDrawPile.Count > 0)
            {
                int randomNumber = Random.Range(0, _cardsInDrawPile.Count);
                _cardsInHand.Add(_cardsInDrawPile[randomNumber]);
                _cardsInDrawPile[randomNumber].gameObject.SetActive(true);
                _cardsInDrawPile.RemoveAt(randomNumber);
            }
            else
            {
                int randomNumber = Random.Range(0, _cardsInDiscardPile.Count);
                _cardsInHand.Add(_cardsInDiscardPile[randomNumber]);
                _cardsInDiscardPile[randomNumber].gameObject.SetActive(true);
                _cardsInDiscardPile.RemoveAt(randomNumber);
            }
        }

        if (_cardsInDrawPile.Count <= 0)
        {
            foreach (var card in _cardsInDiscardPile)
            {
                _cardsInDrawPile.Add(card);
            }
            _cardsInDiscardPile.Clear();
        }
        OnCardDraw?.Invoke(this, EventArgs.Empty);
    }
    
    public void DisableAllCardsInHands()
    {
        foreach (var card in _cardsInHand)
        {
            card.DisableCardButton();
        }
    }
    
    public void DisableCardsInHandBasedOnEnergyLeft()
    {
        foreach (var card in _cardsInHand)
        {
            if (card.GetCardEnergy() > GetRemainingEnergy())
            {
                card.DisableCardButton();
            }
        }
    }
    
    private void IncrementMaxCardPlayNumber(int cardEnergy)
    {
        _currentNumberEnergyPlayed += cardEnergy;
        OnIncrementNumberCardPlayer?.Invoke(this, EventArgs.Empty);
        DisableCardsInHandBasedOnEnergyLeft();
    }
    
    public int GetRemainingEnergy()
    {
        return numberEnergyMax - _currentNumberEnergyPlayed;
    }

    public void ResetEnergy()
    {
        _currentNumberEnergyPlayed = 0;
        OnIncrementNumberCardPlayer?.Invoke(this, EventArgs.Empty);
    }
    
    public void ExecuteCurrentCard()
    {
        IncrementMaxCardPlayNumber(_currentCardSelected.GetCardEnergy());
        _currentCardSelected.ExecuteCurrentCardEffects();
        _currentCardSelected.gameObject.SetActive(false);
    }
    

    public void SelectCard(Card cardSelected)
    {
        _currentCardSelected = cardSelected;
        OnCurrentCardSelectedChange?.Invoke(this, EventArgs.Empty);
    }

    public Card GetCurrentCardSelected()
    {
        return _currentCardSelected;
    }
    
    public void SetCurrentCardSelected(Card cardSelected)
    {
        _currentCardSelected = cardSelected;
    }

    public Card GetEnemyBaseCard()
    {
        return baseCardList[Random.Range(0, baseCardList.Count)];
    }

    public int GetEnergyCurrentCard()
    {
		return _currentCardSelected.GetCardEnergy();
    }

    public int GetDrawPileCountNumber()
    {
        return _cardsInDrawPile.Count;
    }
    
    public int GetDiscardPileCountNumber()
    {
        return _cardsInDiscardPile.Count;
    }

}
