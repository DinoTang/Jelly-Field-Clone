using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JellyCellCtrl : PoolObj
{
    [Header("Jelly Cell Ctrl")]
    [SerializeField] protected JellyCellArrange jellyCellArrange;
    [SerializeField] protected JellyCellConfig jellyCellConfig;
    [SerializeField] protected JellyCellDragHandler jellyCellDragHandler;

    public JellyCellArrange JellyCellArrange => jellyCellArrange;
    public JellyCellConfig JellyCellConfig => jellyCellConfig;
    public JellyCellDragHandler JellyCellDragHandler => jellyCellDragHandler;

    public override string GetName()
    {
        return "JellyCellCtrl";
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadJellyCellArrange();
        this.LoadJellyCellConfig();
        this.LoadJellyCellDragHandler();
    }

    protected void LoadJellyCellArrange()
    {
        if (this.jellyCellArrange != null) return;
        this.jellyCellArrange = GetComponentInChildren<JellyCellArrange>();
        Debug.Log(transform.name + ": LoadJellyCellArrange");
    }

    protected void LoadJellyCellConfig()
    {
        if (this.jellyCellConfig != null) return;
        this.jellyCellConfig = GetComponentInChildren<JellyCellConfig>();
        Debug.Log(transform.name + ": LoadJellyCellConfig");
    }

    protected void LoadJellyCellDragHandler()
    {
        if (this.jellyCellDragHandler != null) return;
        this.jellyCellDragHandler = GetComponentInChildren<JellyCellDragHandler>();
        Debug.Log(transform.name + ": LoadJellyCellDragHandler");
    }
}
