using System.Collections.Generic;
using UnityEngine;

public class JellyPieceCtrl : PoolObj
{
   [Header("JellyPieceCtrl")]
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

   protected void LoadJellyPieceModel()
   {
      if (this.jellyPieceModel != null) return;
      this.jellyPieceModel = GetComponentInChildren<JellyPieceModel>();
      Debug.Log(transform.name + ": LoadJellyPieceModel");
   }

   protected void LoadJellyCellConfig()
   {
      if (this.jellyCellConfig != null) return;
      this.jellyCellConfig = GetComponentInChildren<JellyPieceConfig>();
      Debug.Log(transform.name + ": LoadJellyCellConfig");
   }
}
