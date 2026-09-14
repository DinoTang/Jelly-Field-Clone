using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyCellDespawn : Despawn<JellyCellCtrl>
{
    [Header("Jelly Cell Despawn")]
    [SerializeField] protected JellyCellCtrl jellyCellCtrl;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadJellyCellCtrl();
    }

    private void LoadJellyCellCtrl()
    {
        if (this.jellyCellCtrl != null) return;
        this.jellyCellCtrl = GetComponentInParent<JellyCellCtrl>();
        Debug.Log(transform.name + ": LoadJellyCellCtrl");
    }
    public override void DoDespawn()
    {
        base.DoDespawn();

        // Reset gridPos của JellyCell
        this.jellyCellCtrl.JellyCellConfig.SetGridPos(-1, -1);

        // Reset currentBoardSlot của jellyCell
        BoardSlot currentSlot = this.jellyCellCtrl.JellyCellConfig.CurrentSlot;
        if (currentSlot != null)
        {
            this.jellyCellCtrl.JellyCellConfig.ClearCurrentSlot();
            currentSlot.RemoveJellyCell();
        }
    }
}
