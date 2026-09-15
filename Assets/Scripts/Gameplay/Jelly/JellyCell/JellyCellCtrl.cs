using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JellyCellCtrl : PoolObj
{
    [Header("Jelly Cell Ctrl")]
    [SerializeField] protected JellyCellArrange jellyCellArrange;
    [SerializeField] protected JellyCellConfig jellyCellConfig;
    [SerializeField] protected JellyCellDragHandler jellyCellDragHandler;
    [SerializeField] protected JellyCellDropHandler jellyCellDropHandler;
    // [SerializeField] protected JellyCellJiggleController jellyCellJiggleController;
    [SerializeField] protected JellyCellDespawn jellyCellDespawn;


    public JellyCellArrange JellyCellArrange => jellyCellArrange;
    public JellyCellConfig JellyCellConfig => jellyCellConfig;
    public JellyCellDragHandler JellyCellDragHandler => jellyCellDragHandler;
    public JellyCellDropHandler JellyCellDropHandler => jellyCellDropHandler;
    // public JellyCellJiggleController JellyCellJiggleController => jellyCellJiggleController;
    public JellyCellDespawn JellyCellDespawn => jellyCellDespawn;


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
        this.LoadJellyCellDropHandler();
        this.LoadJellyCellJiggleController();
        this.LoadJellyCellDespawn();
    }

    private void LoadJellyCellArrange()
    {
        if (this.jellyCellArrange != null) return;
        this.jellyCellArrange = GetComponentInChildren<JellyCellArrange>();
        Debug.Log(transform.name + ": LoadJellyCellArrange");
    }

    private void LoadJellyCellConfig()
    {
        if (this.jellyCellConfig != null) return;
        this.jellyCellConfig = GetComponentInChildren<JellyCellConfig>();
        Debug.Log(transform.name + ": LoadJellyCellConfig");
    }

    private void LoadJellyCellDragHandler()
    {
        if (this.jellyCellDragHandler != null) return;
        this.jellyCellDragHandler = GetComponentInChildren<JellyCellDragHandler>();
        Debug.Log(transform.name + ": LoadJellyCellDragHandler");
    }

    private void LoadJellyCellDropHandler()
    {
        if (this.jellyCellDropHandler != null) return;
        this.jellyCellDropHandler = GetComponentInChildren<JellyCellDropHandler>();
        Debug.Log(transform.name + ": LoadJellyCellDropHandler");
    }

    private void LoadJellyCellJiggleController()
    {
        // if (this.jellyCellJiggleController != null) return;
        // this.jellyCellJiggleController = GetComponentInChildren<JellyCellJiggleController>();
        // Debug.Log(transform.name + ": LoadJellyCellJiggleController");
    }

    private void LoadJellyCellDespawn()
    {
        if (this.jellyCellDespawn != null) return;
        this.jellyCellDespawn = GetComponentInChildren<JellyCellDespawn>();
        Debug.Log(transform.name + ": LoadJellyCellDespawn");
    }
}
