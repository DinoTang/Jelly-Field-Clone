using System.Collections.Generic;
using UnityEngine;

public class JellyPieceAbstract : BaseBehaviour
{
   [Header("JellyPieceAbstract")]
   [SerializeField] protected JellyPieceCtrl jellyPieceCtrl;
   protected override void LoadComponent()
   {
      base.LoadComponent();
      this.LoadJellyPieceCtrl();
   }

   protected void LoadJellyPieceCtrl()
   {
      if (this.jellyPieceCtrl != null) return;
      this.jellyPieceCtrl = GetComponentInParent<JellyPieceCtrl>();
      Debug.Log(transform.name + ": LoadJellyPieceCtrl");
   }
}
