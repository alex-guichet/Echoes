using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DebuffUI : MonoBehaviour
{
   [SerializeField] private Target target;
   [SerializeField] private TextMeshProUGUI damageReceivedLabel;
   [SerializeField] private TextMeshProUGUI damageAddedLabel;

   private void Start()
   {
      target.OnDebuffUpdate += TargetOnDebuffUpdate;
      GameManager.Instance.OnEndGame += GameManagerOnEndGame;
      target.OnHealthPointUpdate += TargetOnHealthPointUpdate;
      
      UpdateVisual();
   }
   
   private void TargetOnHealthPointUpdate(object sender, EventArgs e)
   {
      if (target.GetCurrentHealthPoints() <= 0f)
      {
         Hide();
      }
   }
   
   private void GameManagerOnEndGame(object sender, EventArgs e)
   {
      Hide();
   }

   private void TargetOnDebuffUpdate(object sender, EventArgs e)
   {
      UpdateVisual();
   }

   public void UpdateVisual()
   {
      if (target.GetExtraDamageAdded() == 0f)
      {
         damageAddedLabel.gameObject.SetActive(false);
      }
      else
      {
         damageAddedLabel.gameObject.SetActive(true);
         damageAddedLabel.text = "Damage Added : " + target.GetExtraDamageAdded();
      }
      
      if (target.GetExtraDamageReceived() == 0f)
      {
         damageReceivedLabel.gameObject.SetActive(false);
      }
      else
      {
         damageReceivedLabel.gameObject.SetActive(true);
         damageReceivedLabel.text = "Damage Received : " + target.GetExtraDamageReceived();
      }
   }

   public void Hide()
   {
      gameObject.SetActive(false);
   }
}
