using System.Collections.Generic;
using UnityEngine;

public class JellyPieceCtrl : PoolObj
{
   [Header("Jelly Piece Ctrl")]
   [SerializeField] protected JellyPieceModel jellyPieceModel;
   [SerializeField] protected JellyPieceConfig jellyCellConfig;
   [SerializeField] protected JellyPieceFillAnimator jellyPieceFillAnimator;
   [SerializeField] protected JellyPieceJiggle jellyPieceJiggle;
   [SerializeField] protected JellyPieceDespawn jellyPieceDespawn;
   [SerializeField] protected ExplosionEffectSpawn explosionEffectSpawn;

   public JellyPieceModel JellyPieceModel => jellyPieceModel;
   public JellyPieceConfig JellyPieceConfig => jellyCellConfig;
   public JellyPieceFillAnimator JellyPieceFillAnimator => jellyPieceFillAnimator;
   public JellyPieceJiggle JellyPieceJiggle => jellyPieceJiggle;
   public JellyPieceDespawn JellyPieceDespawn => jellyPieceDespawn;
   public ExplosionEffectSpawn ExplosionEffectSpawn => explosionEffectSpawn;
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
      this.LoadJellyPieceJiggle();
      this.LoadJellyCellDespawn();
      this.LoadExplosionEffectSpawn();
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

   private void LoadJellyPieceJiggle()
   {
      if (this.jellyPieceJiggle != null) return;
      this.jellyPieceJiggle = GetComponentInChildren<JellyPieceJiggle>();
      Debug.Log(transform.name + ": LoadJellyPieceJiggle");
   }

   private void LoadJellyCellDespawn()
   {
      if (this.jellyPieceDespawn != null) return;
      this.jellyPieceDespawn = GetComponentInChildren<JellyPieceDespawn>();
      Debug.Log(transform.name + ": LoadJellyCellDespawn");
   }

   private void LoadExplosionEffectSpawn()
   {
      if (this.explosionEffectSpawn != null) return;
      this.explosionEffectSpawn = FindAnyObjectByType<ExplosionEffectSpawn>();
      Debug.Log(transform.name + ": LoadExplosionEffectSpawn");
   }
}
