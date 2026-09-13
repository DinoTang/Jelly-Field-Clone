using UnityEngine;

public class BoardSlot : PoolObj
{
   [SerializeField] protected JellyCellCtrl currentJellyCell;
   public JellyCellCtrl CurrentJellyCell => currentJellyCell;

   public bool IsEmpty()
   {
      return currentJellyCell == null;
   }

   public void SetJellyCell(JellyCellCtrl jellyCell)
   {
      this.currentJellyCell = jellyCell;

      jellyCell.SetCurrentSlot(this);
   }

   public void RemoveJellyCell()
   {
      this.currentJellyCell = null;
   }
   public void ShowHint()
   {
      BoardManager.Instance.BoardPlacementPreview.Show(transform.position);
   }

   public void HideHint()
   {
      BoardManager.Instance.BoardPlacementPreview.Hide();
   }

   public override string GetName()
   {
      return "BoardSlot";
   }
}
