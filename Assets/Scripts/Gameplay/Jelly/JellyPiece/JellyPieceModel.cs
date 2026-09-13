using System.Collections.Generic;
using UnityEngine;

public class JellyPieceModel : JellyPieceAbstract
{
    [Header("JellyPieceModel")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] protected JellyColor color;
    public JellyColor Color => color;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMeshRenderer();
    }

    private void LoadMeshRenderer()
    {
        if (this.meshRenderer != null) return;
        this.meshRenderer = GetComponent<MeshRenderer>();
        Debug.Log(transform.name + ": LoadMeshRenderer");
    }

    public void SetColor(JellyColor color)
    {
        this.color = color;
    }

    public void ApplyMaterialByColor()
    {
        Material material = this.jellyPieceCtrl.JellyCellConfig.JellyMaterialSO.GetMaterial(this.color);
        this.meshRenderer.sharedMaterial = material;
    }
    public void SetSizeQuarter()
    {
        this.transform.localScale = new Vector3(18, 18, 35);
    }

    public void SetSizeHalfHorizontal()
    {
        this.transform.localScale = new Vector3(36, 18, 35);
    }

    public void SetSizeHalfVertical()
    {
        this.transform.localScale = new Vector3(18, 36, 35);
    }

    public void SetSizeFull()
    {
        this.transform.localScale = new Vector3(36, 36, 35);
    }
}
