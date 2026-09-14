using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JellyCellConfig : JellyCellAbstract
{
    [Header("Jelly Cell Config")]
    [SerializeField] protected BoardSlot currentSlot;
    [SerializeField] protected Vector2Int gridPos;
    [SerializeField] private float jellyOffsetZ = -0.32f;
    [SerializeField] protected List<JellyPieceCtrl> jellyPieces = new();

    public Vector2Int GridPos => gridPos;
    public List<JellyPieceCtrl> JellyPieces => jellyPieces;

    public void SetGridPos(int x, int y)
    {
        this.gridPos = new Vector2Int(x, y);
    }
    public BoardSlot CurrentSlot => currentSlot;

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
    
    public void AddJellyPieces(JellyPieceCtrl pieces)
    {
        this.jellyPieces.Add(pieces);
    }

    public void RemoveJellyPiece(JellyPieceCtrl jellyPiece)
    {
        if (!this.jellyPieces.Contains(jellyPiece)) return;

        this.jellyPieces.Remove(jellyPiece);
    }
}
