using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<EnemyTarget> enemyTargets;
    [SerializeField] private float timerEndGameMax = 2f;
    [SerializeField] private float secondsBetweenEnemyAttacks = 10f;

    public static GameManager Instance;

    public event EventHandler OnVictory;
    public event EventHandler OnDefeat;
    public event EventHandler OnEndGame;
    public event EventHandler OnStartEnemyAttack;
    public event EventHandler OnEndEnemyAttack;
    
    private Target _currentTargetSelected;
    private bool _endGame;
    private bool _victory;
    private float _timerEndGame;
    private Coroutine _enemyAttackCoroutine;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
    }
    
    private void Start()
    {
        PlayerTarget.Instance.OnHealthPointUpdate += TargetOnHealthPointUpdate;

		foreach (var targetController in enemyTargets)
        {
            targetController.OnHealthPointUpdate += TargetOnHealthPointUpdate;
        }
        
        CardManager.Instance.OnCurrentCardSelectedChange += CardManagerOnCurrentCardSelectedChange;
    }

    private void CardManagerOnCurrentCardSelectedChange(object sender, EventArgs e)
    {
        if (CardManager.Instance.GetCurrentCardSelected().GetCardTargetType() == CardTargetType.ENEMY)
        {
            foreach (var targetController in enemyTargets)
            {
                targetController.EnableSelectionButton();
            }
            PlayerTarget.Instance.DisableSelectionButton();
        }
        else if(CardManager.Instance.GetCurrentCardSelected().GetCardTargetType() == CardTargetType.PLAYER)
        {
            PlayerTarget.Instance.EnableSelectionButton();
            foreach (var targetController in enemyTargets)
            {
                targetController.DisableSelectionButton();
            }
        }
    }

    private void TargetOnHealthPointUpdate(object sender, EventArgs e)
    {
        Target target = (Target)sender;
        if (target.GetCurrentHealthPoints() <= 0)
        {
            if (target is EnemyTarget)
            {
                enemyTargets.Remove((EnemyTarget)target);
                if (enemyTargets.Count == 0)
                {
                    _victory = true;
                    _endGame = true;
                }
            }

            if (target is PlayerTarget)
            {
                _victory = false;
                _endGame = true;
            }

            if (_endGame)
            {
                target.OnHealthPointUpdate -= TargetOnHealthPointUpdate;
                OnEndGame?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void SetCurrentTargetSelected(Target targetController)
    {
        _currentTargetSelected = targetController;
    }
    
    public Target GetCurrentTargetSelected()
    {
        return _currentTargetSelected;
    }
    
    public void ExecuteEnemyAttack()
    {
        if (_enemyAttackCoroutine != null)
        {
            StopCoroutine(_enemyAttackCoroutine);
        }

        OnStartEnemyAttack?.Invoke(this, EventArgs.Empty);
        _enemyAttackCoroutine = StartCoroutine(ExecuteEnemyAttackCoroutine());
    }

    public bool IsEndGame()
    {
        return _endGame;
    }

    public List<EnemyTarget> GetEnemyTargets()
    {
        return enemyTargets;
    }

    public void DisableAllSelectionButtons()
    {
        PlayerTarget.Instance.DisableSelectionButton();

        foreach (var targetController in enemyTargets)
        {
            targetController.DisableSelectionButton();
        }
    }
    
    public void DecrementDebuffTurnForAllTargets()
    {
        PlayerTarget.Instance.DecrementTurnDebuffList();

        foreach (var targetController in enemyTargets)
        {
            targetController.DecrementTurnDebuffList();
        }
    }
    

    public void Update()
    {
        if (!_endGame)
            return;

        if (_timerEndGame < timerEndGameMax)
        {
            _timerEndGame += Time.deltaTime;
        }
        else
        {
            if (_victory)
            {
                OnVictory?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnDefeat?.Invoke(this, EventArgs.Empty);
            }
            _endGame = false;
            _timerEndGame = 0f;
        }
    }

    private IEnumerator ExecuteEnemyAttackCoroutine()
    {
        foreach (var targetController in enemyTargets)
        {
            if (!IsEndGame())
            {
                targetController.DamagePlayer();
            }
            yield return new WaitForSeconds(secondsBetweenEnemyAttacks);
        }

        if (!_endGame)
        {
            OnEndEnemyAttack?.Invoke(this, EventArgs.Empty);
            DecrementDebuffTurnForAllTargets();
        }
    }

    private void OnDestroy()
    {
        if (_enemyAttackCoroutine != null)
        {
            StopCoroutine(_enemyAttackCoroutine);
        }
    }
}
