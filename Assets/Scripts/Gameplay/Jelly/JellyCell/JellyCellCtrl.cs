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

    public JellyCellArrange JellyCellArrange => jellyCellArrange;
    public JellyCellConfig JellyCellConfig => jellyCellConfig;
    public JellyCellDragHandler JellyCellDragHandler => jellyCellDragHandler;
    public JellyCellDropHandler JellyCellDropHandler => jellyCellDropHandler;

    [SerializeField] protected BoardSlot currentSlot;

    public BoardSlot CurrentSlot => currentSlot;

    [SerializeField] private float jellyOffsetZ = -0.32f;

    public void SetCurrentSlot(BoardSlot slot)
    {
        this.currentSlot = slot;
    }

    public void ClearCurrentSlot()
    {
        this.currentSlot = null;
    }

    // Cái này để áp thêm z cho bên BoardBuilder để lúc sinh jellyCell
    public void SetJellyPosition()
    {
        transform.position += new Vector3(0, 0, this.jellyOffsetZ);
    }

    // Cái này để áp thêm z cho jellyCell lúc Move
    public Vector3 GetJellyPosition(Vector3 slotPosition)
    {
        return slotPosition + new Vector3(0, 0, jellyOffsetZ);
    }

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
}
