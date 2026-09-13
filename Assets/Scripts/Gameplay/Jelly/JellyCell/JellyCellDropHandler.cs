using UnityEngine;

public class JellyCellDropHandler : JellyCellAbstract
{
    [Header("Jelly Cell Drop Handler")]
    [SerializeField] private float detectDistance = 0.5f;
    [SerializeField] private BoardSlot hoverSlot;

    private void PlaceJellyCell(BoardSlot slot)
    {
        Vector3 position = jellyCellCtrl.GetJellyPosition(slot.transform.position);

        this.jellyCellCtrl.JellyCellDragHandler.SyncPosition(position);

        slot.SetJellyCell(jellyCellCtrl);
    }

    public void HandleDrop()
    {
        if (hoverSlot != null) this.PlaceJellyCell(hoverSlot);
        else this.jellyCellCtrl.JellyCellDragHandler.ReturnPosition();

        this.ResetHoverSlot();
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
            ResetHoverSlot();
            return;
        }

        float distance = Vector3.Distance(transform.position, nearestSlot.transform.position);

        if (distance <= detectDistance)
        {
            if (hoverSlot != nearestSlot)
            {
                ResetHoverSlot();

                hoverSlot = nearestSlot;
                hoverSlot.ShowHint();
            }
        }
        else
        {
            ResetHoverSlot();
        }
    }

    private void ResetHoverSlot()
    {
        if (hoverSlot == null) return;

        hoverSlot.HideHint();
        hoverSlot = null;
    }
}
