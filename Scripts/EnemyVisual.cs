using System;
using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
   [SerializeField] private EnemyTarget enemyTarget;
   [SerializeField] private Animator enemyTargetAnimator;
   [SerializeField] private ParticleSystem hitEffect;
   
   private static readonly int Slash = Animator.StringToHash("Slash");
   private static readonly int Hurt = Animator.StringToHash("Hurt");
   private static readonly int Death = Animator.StringToHash("Dead");

   private float _startAnimationTimer;
   private bool _isReceivingImpact;
   
   private void Start()
   {
      enemyTarget.OnDamageReceive += EnemyTargetOnDamageReceive;
      enemyTarget.OnAttack += EnemyTargetOnAttackPlayer;
      enemyTarget.OnHealthPointUpdate += EnemyTargetOnHealthPointUpdate;
   }
   
   
   private void EnemyTargetOnHealthPointUpdate(object sender, EventArgs e)
   {
      if (enemyTarget.GetCurrentHealthPoints() <= 0f)
      {
         enemyTargetAnimator.SetTrigger(Death);
      }
   }

   private void EnemyTargetOnAttackPlayer(object sender, EventArgs e)
   {
      enemyTargetAnimator.SetTrigger(Slash);
   }


   private void EnemyTargetOnDamageReceive(object sender, EventArgs e)
   {
      enemyTargetAnimator.SetTrigger(Hurt);
      CameraShaker.Instance.ShakeOnce(4f, 4f, 0, .4f);
      hitEffect.Play();
   }
}
