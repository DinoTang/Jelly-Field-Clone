using System.Collections.Generic;
using UnityEngine;

public class JellyPieceCtrl : PoolObj
{
   [Header("Jelly Piece Ctrl")]
   [SerializeField] protected JellyPieceModel jellyPieceModel;
   public JellyPieceModel JellyPieceModel => jellyPieceModel;

   [SerializeField] protected JellyPieceConfig jellyCellConfig;
   public JellyPieceConfig JellyCellConfig => jellyCellConfig;

   public override string GetName()
   {
      return "JellyPieceCtrl";
   }

   protected override void LoadComponent()
   {
      base.LoadComponent();
      this.LoadJellyPieceModel();
      this.LoadJellyCellConfig();
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


   private Vector3 offset;

   public Vector3 Offset => offset;

   public void SetOffset(Vector3 offset)
   {
      this.offset = offset;
   }
}
