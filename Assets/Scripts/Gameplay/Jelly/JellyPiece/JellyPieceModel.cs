using System.Collections.Generic;
using UnityEngine;

public class JellyPieceModel : JellyPieceAbstract
{
    [Header("JellyPieceModel")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] protected JellyColor color;
    public JellyColor Color => color;
    public MeshRenderer MeshRenderer => meshRenderer;
    public MeshFilter MeshFilter => meshFilter;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMeshRenderer();
        this.LaodMeshFilter();
    }

    private void LoadMeshRenderer()
    {
        if (this.meshRenderer != null) return;
        this.meshRenderer = GetComponent<MeshRenderer>();
        Debug.Log(transform.name + ": LoadMeshRenderer");
    }
    private void LaodMeshFilter()
    {
        if (this.meshFilter != null) return;
        this.meshFilter = GetComponent<MeshFilter>();
        Debug.Log(transform.name + ": LaodMeshFilter");
    }

    public void SetColor(JellyColor color)
    {
        this.color = color;
    }

    public void ApplyMaterialByColor()
    {
        Material material = this.jellyPieceCtrl.JellyPieceConfig.JellyMaterialSO.GetMaterial(this.color);
        this.meshRenderer.sharedMaterial = material;
    }

    public void SetSizeQuarter()
    {
        this.transform.localScale = this.jellyPieceCtrl.JellyPieceConfig.JellyPieceSizeConfig.Quarter;
    }

    public void SetSizeHalfHorizontal()
    {
        this.transform.localScale = this.jellyPieceCtrl.JellyPieceConfig.JellyPieceSizeConfig.HalfHorizontal;
    }

    public void SetSizeHalfVertical()
    {
        this.transform.localScale = this.jellyPieceCtrl.JellyPieceConfig.JellyPieceSizeConfig.HalfVertical;
    }

    public void SetSizeFull()
    {
        this.transform.localScale = this.jellyPieceCtrl.JellyPieceConfig.JellyPieceSizeConfig.Full;
    }
}
