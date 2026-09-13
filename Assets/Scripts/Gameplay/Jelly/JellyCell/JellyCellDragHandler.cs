using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider))]
public class JellyCellDragHandler : JellyCellAbstract,
    IPointerDownHandler,
    IDragHandler,
    IPointerUpHandler
{
    [Header("Jelly Cell Drag Handler")]
    [SerializeField] protected Vector3 dragOffset;
    [SerializeField] protected BoxCollider boxCollider;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBoxCollider();
    }

    protected void LoadBoxCollider()
    {
        if (this.boxCollider != null) return;
        this.boxCollider = GetComponent<BoxCollider>();
        this.SetupCollider();
        Debug.Log(transform.name + ": LoadBoxCollider");
    }

    private void SetupCollider()
    {
        this.boxCollider.size = new Vector3(0.74f, 0.73f, 0.735f);
    }

    public void CachePieceOffset()
    {
        foreach (JellyPieceCtrl jellyPiece in this.jellyCellCtrl.JellyCellConfig.JellyPieces)
        {
            Vector3 offset = jellyPiece.transform.position - transform.position;

            jellyPiece.SetOffset(offset);
        }
    }


    public void Move(Vector3 position)
    {
        transform.parent.position = position;

        foreach (JellyPieceCtrl piece in this.jellyCellCtrl.JellyCellConfig.JellyPieces)
        {
            piece.transform.position =
                position + piece.Offset;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 mouseWorld = InputManager.Instance.GetMouseWorldPosition(eventData);

        dragOffset = transform.position - mouseWorld;

        Debug.Log("Start Drag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mouseWorld = InputManager.Instance.GetMouseWorldPosition(eventData);

        Vector3 targetPosition = mouseWorld + dragOffset;

        this.Move(targetPosition);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

}
