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
    [SerializeField] protected bool isClocking = true;
    [SerializeField] protected Vector3 dragOffset;
    [SerializeField] protected BoxCollider boxCollider;

    public BoxCollider BoxCollider => boxCollider;
    private Vector3 dragStartPosition;

    private void SetupCollider()
    {
        this.boxCollider.size = new Vector3(0.74f, 0.73f, 0.735f);
    }
    public void SetIsClocking(bool isClocking)
    {
        this.isClocking = isClocking;
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadBoxCollider();
    }

    private void LoadBoxCollider()
    {
        if (this.boxCollider != null) return;
        this.boxCollider = GetComponent<BoxCollider>();
        this.SetupCollider();
        Debug.Log(transform.name + ": LoadBoxCollider");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (this.isClocking) return;

        this.SaveStartPos();

        // bỏ slot cũ khi bắt đầu kéo
        this.RemoveJellyCellFromOldSlot();

        // tính khoảng lệch giữa pos JellyCell và pos chuột lúc bắt đầu kéo
        this.CacheDragOffset(eventData);

        Debug.Log("Start Drag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.isClocking) return;

        // Lấy vị trí cần tới
        Vector3 targetPosition = this.GetDragPosition(eventData);

        // Đồng bộ vị trí giữa jellyPiece và jellyCell
        this.SyncPosition(targetPosition);

        this.jellyCellCtrl.JellyCellDropHandler.UpdatePlacementPreview();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (this.isClocking) return;

        this.jellyCellCtrl.JellyCellDropHandler.HandleDrop();
    }


    public void SaveStartPos()
    {
        this.dragStartPosition = transform.parent.position;
    }
    public void ReturnPosition()
    {
        this.jellyCellCtrl.JellyCellDragHandler.SyncPosition(this.dragStartPosition);
    }
    public void CachePieceOffsets()
    {
        foreach (JellyPieceCtrl jellyPiece in this.jellyCellCtrl.JellyCellConfig.JellyPieces)
        {
            Vector3 offset = jellyPiece.transform.position - transform.position;

            jellyPiece.JellyPieceConfig.SetOffset(offset);
        }
    }

    public void SyncPosition(Vector3 position)
    {
        this.jellyCellCtrl.transform.position = position;

        foreach (JellyPieceCtrl piece in this.jellyCellCtrl.JellyCellConfig.JellyPieces)
        {
            piece.transform.position =
                position + piece.JellyPieceConfig.Offset;
        }
    }
    private void RemoveJellyCellFromOldSlot()
    {
        if (this.jellyCellCtrl.CurrentSlot != null)
        {
            this.jellyCellCtrl.CurrentSlot.RemoveJellyCell();
            this.jellyCellCtrl.ClearCurrentSlot();
        }
    }

    private void CacheDragOffset(PointerEventData eventData)
    {
        Vector3 mouseWorld = InputManager.Instance.GetMouseWorldPosition(eventData);

        this.dragOffset = transform.parent.position - mouseWorld;
    }

    private Vector3 GetDragPosition(PointerEventData eventData)
    {
        Vector3 mouseWorld = InputManager.Instance.GetMouseWorldPosition(eventData);

        return mouseWorld + this.dragOffset;
    }

}
