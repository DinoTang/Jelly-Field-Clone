using UnityEngine;

public class BoardSlot : PoolObj
{
   [SerializeField] protected JellyCellCtrl currentJellyCell;
   public JellyCellCtrl CurrentJellyCell => currentJellyCell;

   [SerializeField] private Vector2Int gridPos;

   public Vector2Int GridPos => gridPos;

   public void ResetData()
   {
      this.currentJellyCell = null;
      this.gridPos = new Vector2Int(-1, -1);
   }

   public void SetGridPos(int x, int y)
   {
      this.gridPos = new Vector2Int(x, y);
   }

   public bool IsEmpty()
   {
      return currentJellyCell == null;
   }

   public void SetJellyCell(JellyCellCtrl jellyCell)
   {
      this.currentJellyCell = jellyCell;

      jellyCell.JellyCellConfig.SetCurrentSlot(this);
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
