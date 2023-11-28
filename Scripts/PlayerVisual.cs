using System;
using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
   [SerializeField] private Animator playerTargetAnimator;
   [SerializeField] private ParticleSystem hitEffect;
   
   private static readonly int Slash = Animator.StringToHash("Slash");
   private static readonly int Hurt = Animator.StringToHash("Hurt");
   private static readonly int Death = Animator.StringToHash("Dead");
   
   private void Start()
   {
      PlayerTarget.Instance.OnAttack += PlayerTargetOnAttack;
      PlayerTarget.Instance.OnDamageReceive += PlayerTargetOnDamageReceive;
      PlayerTarget.Instance.OnHealthPointUpdate += PlayerTargetOnHealthPointUpdate;
   }

   private void PlayerTargetOnHealthPointUpdate(object sender, EventArgs e)
   {
      if (PlayerTarget.Instance.GetCurrentHealthPoints() <= 0f)
      {
         playerTargetAnimator.SetTrigger(Death);
      }
   }

   private void PlayerTargetOnAttack(object sender, EventArgs e)
   {
      playerTargetAnimator.SetTrigger(Slash);
   }
   
   
   private void PlayerTargetOnDamageReceive(object sender, EventArgs e)
   {
      playerTargetAnimator.SetTrigger(Hurt);
      CameraShaker.Instance.ShakeOnce(4f, 4f, 0, .4f);
      hitEffect.Play();
   }
}
