using UnityEngine;

public class BoardManager : BaseBehaviour
{
   [SerializeField] protected JellyLevelSO levelData;
   [SerializeField] protected BoardBuilder boardBuilder;
   [SerializeField] private BoardCameraCtrl boardCameraCtrl;

   protected override void Start()
   {
      this.boardBuilder.Build(this.levelData);
      this.boardCameraCtrl.CenterOnBoard(this.levelData.Width,
                                          this.levelData.Height,
                                          this.boardBuilder.CellSpacing,
                                          this.boardBuilder.BoardOrigin);
   }
   protected override void LoadComponent()
   {
      base.LoadComponent();
      this.LoadBoardBuilder();
      this.LoadBoardCameraCtrl();
   }

   protected void LoadBoardBuilder()
   {
      if (this.boardBuilder != null) return;
      this.boardBuilder = GetComponentInChildren<BoardBuilder>();
      Debug.Log(this.transform.name + ": LoadBoardBuilder");
   }

   protected void LoadBoardCameraCtrl()
   {
      if (this.boardCameraCtrl != null) return;
      this.boardCameraCtrl = GetComponentInChildren<BoardCameraCtrl>();
      Debug.Log(this.transform.name + ": LoadBoardCameraCtrl");
   }
}
