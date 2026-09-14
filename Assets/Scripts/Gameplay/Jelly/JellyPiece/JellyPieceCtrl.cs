using System.Collections.Generic;
using UnityEngine;

public class JellyPieceCtrl : PoolObj
{
   [Header("Jelly Piece Ctrl")]
   [SerializeField] protected JellyPieceModel jellyPieceModel;
   [SerializeField] protected JellyPieceConfig jellyCellConfig;
   [SerializeField] protected JellyPieceFillAnimator jellyPieceFillAnimator;
   [SerializeField] protected JellyPieceDespawn jellyPieceDespawn;

   public JellyPieceModel JellyPieceModel => jellyPieceModel;
   public JellyPieceConfig JellyPieceConfig => jellyCellConfig;
   public JellyPieceFillAnimator JellyPieceFillAnimator => jellyPieceFillAnimator;
   public JellyPieceDespawn JellyPieceDespawn => jellyPieceDespawn;
   public override string GetName()
   {
      return "JellyPieceCtrl";
   }

   protected override void LoadComponent()
   {
      base.LoadComponent();
      this.LoadJellyPieceModel();
      this.LoadJellyCellConfig();
      this.LoadJellyPieceFillAnimator();
      this.LoadJellyCellDespawn();
   }

   private void LoadJellyPieceModel()
   {
      if (this.jellyPieceModel != null) return;
      this.jellyPieceModel = GetComponentInChildren<JellyPieceModel>();
      Debug.Log(transform.name + ": LoadJellyPieceModel");
   }

   private void LoadJellyCellConfig()
   {
      if (this.jellyCellConfig != null) return;
      this.jellyCellConfig = GetComponentInChildren<JellyPieceConfig>();
      Debug.Log(transform.name + ": LoadJellyCellConfig");
   }

   private void LoadJellyPieceFillAnimator()
   {
      if (this.jellyPieceFillAnimator != null) return;
      this.jellyPieceFillAnimator = GetComponentInChildren<JellyPieceFillAnimator>();
      Debug.Log(transform.name + ": LoadJellyPieceFillAnimator");
   }

   private void LoadJellyCellDespawn()
   {
      if (this.jellyPieceDespawn != null) return;
      this.jellyPieceDespawn = GetComponentInChildren<JellyPieceDespawn>();
      Debug.Log(transform.name + ": LoadJellyCellDespawn");
   }
}
