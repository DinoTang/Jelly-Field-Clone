using System.Collections.Generic;
using UnityEngine;

public class JellyCellCtrl : PoolObj
{
    [Header("JellyCellCtrl")]
    [SerializeField] protected Vector2Int gridPos;
    [SerializeField] protected List<JellyPieceData> pieces = new();

    [Header("TransformPosition")]
    [SerializeField] protected Transform topLeft;
    [SerializeField] protected Transform topRight;
    [SerializeField] protected Transform bottomLeft;
    [SerializeField] protected Transform bottomRight;
    [SerializeField] private float slotDistance = 0.18f;

    public override string GetName()
    {
        return "JellyCellCtrl";
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadSlotPositions();
        this.UpdateSlotPositions();
    }

    public void SetGridPos(int x, int y)
    {
        this.gridPos = new Vector2Int(x, y);
    }

    public void SetJellyPieces(List<JellyPieceData> pieces)
    {
        this.pieces = new List<JellyPieceData>(pieces);
    }
    protected void LoadSlotPositions()
    {
        if (this.topLeft != null) return;

        this.topLeft = transform.Find("TopLeft_Pos");
        this.topRight = transform.Find("TopRight_Pos");
        this.bottomLeft = transform.Find("BottomLeft_Pos");
        this.bottomRight = transform.Find("BottomRight_Pos");
    }

    protected void UpdateSlotPositions()
    {
        topLeft.localPosition = new Vector3(-slotDistance, slotDistance, 0f);
        topRight.localPosition = new Vector3(slotDistance, slotDistance, 0f);
        bottomLeft.localPosition = new Vector3(-slotDistance, -slotDistance, 0f);
        bottomRight.localPosition = new Vector3(slotDistance, -slotDistance, 0f);
    }

    protected Transform GetSlotTransform(JellySlotType slot)
    {
        return slot switch
        {
            JellySlotType.TopLeft => topLeft,
            JellySlotType.TopRight => topRight,
            JellySlotType.BottomLeft => bottomLeft,
            JellySlotType.BottomRight => bottomRight,
            _ => null
        };
    }

    public void ArrangePiece(JellyPieceCtrl jellyPiece)
    {
        List<JellySlotType> slots = jellyPiece.JellyCellConfig.Slots;

        if (slots.Count == 1)
        {
            Transform target = GetSlotTransform(slots[0]);

            jellyPiece.transform.position = target.position;
            jellyPiece.JellyPieceModel.SetSizeQuarter();
            return;
        }

        if (slots.Count == 2)
        {
            Transform a = GetSlotTransform(slots[0]);
            Transform b = GetSlotTransform(slots[1]);

            Vector3 center = (a.position + b.position) * 0.5f;

            jellyPiece.transform.position = center;

            if (IsHorizontal(slots[0], slots[1]))
                jellyPiece.JellyPieceModel.SetSizeHalfHorizontal();
            else
                jellyPiece.JellyPieceModel.SetSizeHalfVertical();

            return;
        }

        if (slots.Count == 4)
        {
            Vector3 center =
                (topLeft.position +
                 topRight.position +
                 bottomLeft.position +
                 bottomRight.position) / 4f;

            jellyPiece.transform.position = center;
            jellyPiece.JellyPieceModel.SetSizeFull();
        }
    }

    protected bool IsHorizontal(JellySlotType a, JellySlotType b)
    {
        return (a == JellySlotType.TopLeft && b == JellySlotType.TopRight) ||
               (a == JellySlotType.TopRight && b == JellySlotType.TopLeft) ||
               (a == JellySlotType.BottomLeft && b == JellySlotType.BottomRight) ||
               (a == JellySlotType.BottomRight && b == JellySlotType.BottomLeft);
    }
}
