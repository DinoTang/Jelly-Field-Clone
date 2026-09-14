using UnityEngine;

public class BoardManagerAbstract : BaseBehaviour
{
   [SerializeField] protected BoardManager boardManager;

   protected override void LoadComponent()
   {
      base.LoadComponent();
      this.LoadBoardManager();
   }

   private void LoadBoardManager()
   {
      if (this.boardManager != null) return;
      this.boardManager = GetComponentInParent<BoardManager>();
      Debug.Log(this.transform.name + ": LoadBoardManager");
   }
}
