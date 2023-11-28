using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] slashSound;
    [SerializeField] private AudioSource[] swordMetalSound;
    [SerializeField] private AudioSource[] playerHurtSound;
    [SerializeField] private AudioSource[] enemyHurtSound;
    [SerializeField] private AudioSource[] playerArmorSound;
    [SerializeField] private AudioSource[] enemyArmorSound;
    [SerializeField] private AudioSource shieldActivationSound;
    [SerializeField] private AudioSource magicSound;
    

    private void Start()
    {
        PlayerTarget.Instance.OnDamageReceive += PlayerTargetOnDamageReceive;
        PlayerTarget.Instance.OnAttack += TargetOnAttack;
        PlayerTarget.Instance.OnArmorIncrease += TargetOnArmorIncrease;
        PlayerTarget.Instance.OnMagic += TargetOnMagic;

        foreach (var enemyTarget in GameManager.Instance.GetEnemyTargets())
        {
            enemyTarget.OnDamageReceive += EnemyTargetOnDamageReceive;
            enemyTarget.OnAttack += TargetOnAttack;
            enemyTarget.OnArmorIncrease += TargetOnArmorIncrease;
            enemyTarget.OnMagic += TargetOnMagic;
        }
        swordMetalSound[0].Play();
        swordMetalSound[1].Play();
    }

    private void TargetOnMagic(object sender, EventArgs e)
    {
        magicSound.Play();
    }

    private void TargetOnArmorIncrease(object sender, EventArgs e)
    {
        shieldActivationSound.Play();
    }

    private void TargetOnAttack(object sender, EventArgs e)
    {
        slashSound[Random.Range(0, slashSound.Length)].Play();
    }

    private void PlayerTargetOnDamageReceive(object sender, EventArgs e)
    {
        swordMetalSound[Random.Range(0, swordMetalSound.Length)].Play();
        playerHurtSound[Random.Range(0, playerHurtSound.Length)].Play();
        playerArmorSound[Random.Range(0, playerArmorSound.Length)].Play();
    }

    private void EnemyTargetOnDamageReceive(object sender, EventArgs e)
    {
        swordMetalSound[Random.Range(0, swordMetalSound.Length)].Play();
        enemyHurtSound[Random.Range(0, enemyHurtSound.Length)].Play();
        enemyArmorSound[Random.Range(0, enemyArmorSound.Length)].Play();
    }
    
    
}
