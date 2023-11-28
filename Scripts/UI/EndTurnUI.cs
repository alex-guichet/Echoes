using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EndTurnUI : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    
    private void Start()
    {
        _button.onClick.AddListener(() =>
        {
            GameManager.Instance.ExecuteEnemyAttack();
        });
        
        GameManager.Instance.OnStartEnemyAttack += GameManagerOnStartEnemyAttack;
        GameManager.Instance.OnEndEnemyAttack += GameManagerOnEndEnemyAttack;
        GameManager.Instance.OnEndGame += GameManagerOnEndGame;
    }

    private void GameManagerOnStartEnemyAttack(object sender, EventArgs e)
    {
        DisableButton();
    }
    
    private void GameManagerOnEndEnemyAttack(object sender, EventArgs e)
    {
        EnableButton();
    }

    private void GameManagerOnEndGame(object sender, EventArgs e)
    {
        DisableButton();
    }

    private void DisableButton()
    {
        _button.interactable = false;
    }
    
    private void EnableButton()
    {
        _button.interactable = true;
    }
}
