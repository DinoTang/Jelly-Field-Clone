using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class JellyPieceFillAnimator : JellyPieceAbstract
{
   [Header("Jelly Piece Fill Animator")]
   [SerializeField] private float fillDuration = 0.2f;
   [SerializeField] private Ease fillEase = Ease.OutQuad;

   private Tween fillTween;

   public void PlayFill(
       JellyCellArrange arrange,
       JellyPieceCtrl jellyPiece,
       List<JellySlotType> targetSlots,
       Action onComplete = null)
   {
      if (arrange == null || jellyPiece == null || targetSlots == null || targetSlots.Count == 0)
      {
         onComplete?.Invoke();
         return;
      }

      List<JellySlotType> finalSlots = new(jellyPiece.JellyPieceConfig.Slots);

      foreach (JellySlotType slot in targetSlots)
      {
         if (!finalSlots.Contains(slot))
            finalSlots.Add(slot);
      }

      arrange.GetArrangeData(finalSlots, out Vector3 targetPosition, out Vector3 targetScale);

      this.KillTween();

      this.fillTween = DOTween.Sequence()
    .Join(jellyPiece.transform.DOMove(targetPosition, this.fillDuration).SetEase(this.fillEase))
    .Join(jellyPiece.JellyPieceModel.transform.DOScale(targetScale, this.fillDuration).SetEase(this.fillEase))
    .OnComplete(() =>
    {
       foreach (JellySlotType slot in targetSlots)
       {
          if (!jellyPiece.JellyPieceConfig.Slots.Contains(slot))
             jellyPiece.JellyPieceConfig.Slots.Add(slot);
       }

       arrange.ArrangePiece(jellyPiece);
       onComplete?.Invoke();
    });
   }

   private void KillTween()
   {
      if (this.fillTween != null && this.fillTween.IsActive())
         this.fillTween.Kill();
   }

   protected override void OnDisable()
   {
      base.OnDisable();
      this.KillTween();
   }
}