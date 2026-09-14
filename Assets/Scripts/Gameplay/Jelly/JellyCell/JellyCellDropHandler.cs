using UnityEngine;

public class JellyCellDropHandler : JellyCellAbstract
{
    [Header("Jelly Cell Drop Handler")]
    [SerializeField] private float detectDistance = 0.5f;
    [SerializeField] private BoardSlot hoverSlot;

    private void PlaceJellyCell(BoardSlot slot)
    {
        Vector3 position = jellyCellCtrl.JellyCellConfig.GetJellyPosition(slot.transform.position);

        this.jellyCellCtrl.JellyCellDragHandler.SyncPosition(position);

        jellyCellCtrl.JellyCellConfig.SetGridPos(slot.GridPos.x, slot.GridPos.y);

        this.jellyCellCtrl.JellyCellDragHandler.SnapToBoard(slot);
    }

    public bool HandleDrop()
    {
        if (this.hoverSlot == null)
        {
            this.jellyCellCtrl.JellyCellDragHandler.ReturnPosition();
            this.ResetHoverSlot();
            return false;
        }

        this.PlaceJellyCell(this.hoverSlot);
        this.ResetHoverSlot();
        return true;
    }

    private BoardSlot FindNearestSlot(Vector3 position)
    {
        BoardSlot[] slots = FindObjectsByType<BoardSlot>();

        BoardSlot nearest = null;

        float minDistance = Mathf.Infinity;


        foreach (BoardSlot slot in slots)
        {
            if (!slot.IsEmpty()) continue;

            float distance = Vector3.Distance(position, slot.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = slot;
            }
        }


        return nearest;
    }
    public void UpdatePlacementPreview()
    {
        BoardSlot nearestSlot = FindNearestSlot(jellyCellCtrl.transform.position);

        if (nearestSlot == null)
        {
            this.ResetHoverSlot();
            return;
        }

        float distance = Vector3.Distance(transform.position, nearestSlot.transform.position);

        if (distance <= detectDistance)
        {
            if (hoverSlot != nearestSlot)
            {
                this.ResetHoverSlot();

                hoverSlot = nearestSlot;
                hoverSlot.ShowHint();
            }
        }
        else
        {
            this.ResetHoverSlot();
        }
    }

    private void ResetHoverSlot()
    {
        if (hoverSlot == null) return;

        hoverSlot.HideHint();
        hoverSlot = null;
    }
}
