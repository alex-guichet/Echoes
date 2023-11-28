using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardUI : MonoBehaviour
{
    [SerializeField] private Card card;
    [SerializeField] private TextMeshProUGUI energyLabel;
    [SerializeField] private TextMeshProUGUI titleLabel;
    [SerializeField] private TextMeshProUGUI descriptionLabel;
    [SerializeField] private TextMeshProUGUI effectsLabel;

    private void Start()
    {
        energyLabel.text = card.GetCardEnergy().ToString();
        titleLabel.text = card.GetCardName();
    }
}
