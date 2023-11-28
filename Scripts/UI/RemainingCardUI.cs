using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RemainingCardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI remainingCardLabel;
    [SerializeField] private TextMeshProUGUI drawPileLabel;
    [SerializeField] private TextMeshProUGUI discardPileLabel;

    private void Start()
    {
        CardManager.Instance.OnIncrementNumberCardPlayer += GameManagerOnIncrementNumberCardPlayer;
        CardManager.Instance.OnCardDraw += CardManagerOnCardDraw;
        UpdateVisual();
    }

    private void CardManagerOnCardDraw(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void GameManagerOnIncrementNumberCardPlayer(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        remainingCardLabel.text = "Energy : "+CardManager.Instance.GetRemainingEnergy();
        drawPileLabel.text = "Draw Pile : " + CardManager.Instance.GetDrawPileCountNumber();
        discardPileLabel.text = "Discard Pile : " + CardManager.Instance.GetDiscardPileCountNumber();
    }
}
