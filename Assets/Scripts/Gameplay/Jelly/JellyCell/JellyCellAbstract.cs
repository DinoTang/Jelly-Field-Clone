using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class JellyCellAbstract : BaseBehaviour
{
    [Header("Jelly Cell Abstract")]
    [SerializeField] protected JellyCellCtrl jellyCellCtrl;
    public JellyCellCtrl JellyCellCtrl => jellyCellCtrl;

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
}
