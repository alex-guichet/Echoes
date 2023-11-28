using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class EnemyActionUI : MonoBehaviour
{
    [SerializeField] private EnemyTarget enemyTarget;
    [SerializeField] private TextMeshProUGUI damageLabel;
    
    private void Start()
    {
        enemyTarget.OnNextDamageChange += TargetControllerOnNextDamageChange;
        enemyTarget.OnDebuffUpdate += TargetControllerOnNextDamageChange;
        GameManager.Instance.OnEndGame += GameManagerOnEndGame;
        enemyTarget.OnHealthPointUpdate += TargetOnHealthPointUpdate;
        UpdateVisual();
    }  
    
    private void TargetOnHealthPointUpdate(object sender, EventArgs e)
    {
        if (enemyTarget.GetCurrentHealthPoints() <= 0f)
        {
            Hide();
        }
    }
    
    private void GameManagerOnEndGame(object sender, EventArgs e)
    {
        Hide();
    }

    private void TargetControllerOnNextDamageChange(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        damageLabel.text = enemyTarget.GetNextEnemyDamage().ToString(CultureInfo.InvariantCulture);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
