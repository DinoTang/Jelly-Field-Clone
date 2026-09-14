using System.Collections.Generic;
using UnityEngine;

public class JellyPieceAbstract : BaseBehaviour
{
   [Header("Jelly Piece Abstract")]
   [SerializeField] protected JellyPieceCtrl jellyPieceCtrl;
   protected override void LoadComponent()
   {
      base.LoadComponent();
      this.LoadJellyPieceCtrl();
   }

   private void LoadJellyPieceCtrl()
   {
      if (this.jellyPieceCtrl != null) return;
      this.jellyPieceCtrl = GetComponentInParent<JellyPieceCtrl>();
      Debug.Log(transform.name + ": LoadJellyPieceCtrl");
   }
}
