using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyPieceDespawn : Despawn<JellyPieceCtrl>
{
    [Header("Jelly Piece Despawn")]
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

    public override void DoDespawn()
    {
        base.DoDespawn();

        // Reset Slots của JellyPiece
        this.jellyPieceCtrl.JellyPieceConfig.Slots.Clear();

        JellyColor color = this.jellyPieceCtrl.JellyPieceModel.Color;
        ExplosionEffectCtrl explosionEffectCtrl = this.jellyPieceCtrl.ExplosionEffectSpawn.Spawn(color, transform.position);
        explosionEffectCtrl.transform.position += new Vector3(0, 0, -1f);
    }
}
