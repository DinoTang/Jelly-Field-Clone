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
    [SerializeField] private Vector3 spawnPoint;
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
    public void SetSpawnPoint(Vector3 spawnPoint)
    {
        this.spawnPoint = spawnPoint;
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
        this.StartDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.isClocking) return;
        this.UpdateDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (this.isClocking) return;
        this.ProcessDrop();
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
        if (this.jellyCellCtrl.JellyCellConfig.CurrentSlot != null)
        {
            this.jellyCellCtrl.JellyCellConfig.CurrentSlot.RemoveJellyCell();
            this.jellyCellCtrl.JellyCellConfig.ClearCurrentSlot();
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

    private void StartDrag(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioDataSO.touch);

        this.SaveStartPos();

        // bỏ slot cũ khi bắt đầu kéo
        this.RemoveJellyCellFromOldSlot();

        // tính khoảng lệch giữa pos JellyCell và pos chuột lúc bắt đầu kéo
        this.CacheDragOffset(eventData);
    }

    private void UpdateDrag(PointerEventData eventData)
    {
        // Lấy vị trí cần tới
        Vector3 targetPosition = this.GetDragPosition(eventData);

        // Đồng bộ vị trí giữa jellyPiece và jellyCell
        this.SyncPosition(targetPosition);

        this.jellyCellCtrl.JellyCellDropHandler.UpdatePlacementPreview();
    }

    private void ProcessDrop()
    {
        this.isClocking = true;

        GridModel<BoardSlot> grid = BoardManager.Instance.Grid;
        
        AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioDataSO.initJelly);

        bool placed = this.jellyCellCtrl.JellyCellDropHandler.HandleDrop();

        if (!placed)
        {
            this.isClocking = false;
            return;
        }

        BoardManager.Instance.StartCoroutine(BoardManager.Instance.ResolveChain(grid));

        BoardManager.Instance.BoardBuilder.SpawnPlayerJellyCellAfterDrop(this.GetSpawnPointOrigin());
    }

    public void SnapToBoard(BoardSlot targetSlot)
    {
        if (targetSlot == null) return;

        Vector2Int gridPos = targetSlot.GridPos;

        Vector3 boardPosition =
            BoardManager.Instance.BoardBuilder.GetBoardPosition(gridPos.x, gridPos.y);

        // Đưa JellyCell về đúng vị trí Board
        this.jellyCellCtrl.transform.position = boardPosition;
        this.jellyCellCtrl.JellyCellConfig.SetGridPos(gridPos.x, gridPos.y);
        // this.jellyCellCtrl.JellyCellConfig.SetJellyPosition();

        // Đưa JellyPiece về layout chuẩn của Board
        foreach (JellyPieceCtrl jellyPiece in this.jellyCellCtrl.JellyCellConfig.JellyPieces)
        {
            this.jellyCellCtrl.JellyCellArrange.ArrangePiece(jellyPiece);
        }

        // Lưu lại offset mới
        this.CachePieceOffsets();

        // Gắn JellyCell vào BoardSlot
        targetSlot.SetJellyCell(this.jellyCellCtrl);
        this.jellyCellCtrl.JellyCellConfig.SetCurrentSlot(targetSlot);
    }

    private Vector3 GetSpawnPointOrigin()
    {
        float yPos = BoardManager.Instance.BoardBuilder.PlayerJellyOffsetY;
        Vector3 spawnPointOrigin = new Vector3(0, yPos, 0) * -1;
        spawnPointOrigin += this.spawnPoint;

        return spawnPointOrigin;
    }
}
