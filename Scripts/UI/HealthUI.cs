using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Target target;
    
    [SerializeField] private GameObject healthDisplay;
    [SerializeField] private Image healthProgressBar;
    [SerializeField] private TextMeshProUGUI healthPointLabel;
    
    [SerializeField] private GameObject armorBar;
    [SerializeField] private TextMeshProUGUI armorLabel;

    private void Start()
    {
        target.OnHealthPointUpdate += TargetOnHealthPointUpdate;
        target.OnArmorPointUpdate += TargetOnArmorPointUpdate;
        GameManager.Instance.OnEndGame += GameManagerOnEndGame;
        
        healthDisplay.SetActive(true);
        armorBar.SetActive(false);
        healthPointLabel.text = target.GetCurrentHealthPoints().ToString(CultureInfo.InvariantCulture);
    }

    private void GameManagerOnEndGame(object sender, EventArgs e)
    {
        HideArmorBar();
        HideHealthBar();
    }

    private void TargetOnHealthPointUpdate(object sender, EventArgs e)
    {
        if (target.GetCurrentHealthPoints() > 0f)
        {
            healthProgressBar.fillAmount = target.GetCurrentHealthPoints() / target.GetHealthPointMax();
            healthPointLabel.text = target.GetCurrentHealthPoints().ToString(CultureInfo.InvariantCulture);
        }
        else
        {
            HideHealthBar();
            HideArmorBar();
        }
    }
    
    private void TargetOnArmorPointUpdate(object sender, EventArgs e)
    {
        float currentArmorPoints = target.GetArmorPoints();
        
        if (currentArmorPoints > 0f)
        {
            ShowArmorBar();
            armorLabel.text = target.GetArmorPoints().ToString(CultureInfo.InvariantCulture);
            HideHealthBar();
        }
        else
        {
            HideArmorBar();
            if (target.GetCurrentHealthPoints() > 0f)
            {
                ShowHealthBar();
            }
        }
    }
    
    private void ShowArmorBar()
    {
        armorBar.SetActive(true);
    }

    private void HideArmorBar()
    {
        armorBar.SetActive(false);
    }
    
    private void ShowHealthBar()
    {
        healthDisplay.SetActive(true);
    }

    private void HideHealthBar()
    {
        healthDisplay.SetActive(false);
    }
    
}
