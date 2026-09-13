using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
   [SerializeField] protected JellyLevelSO levelData;
   [SerializeField] protected BoardBuilder boardBuilder;
   [SerializeField] protected BoardCameraCtrl boardCameraCtrl;
   [SerializeField] protected BoardPlacementPreview boardPlacementPreview;

   public BoardBuilder BoardBuilder => boardBuilder;
   public BoardPlacementPreview BoardPlacementPreview => boardPlacementPreview;

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
      this.LoadBoardPlacementPreview();
   }

   private void LoadBoardBuilder()
   {
      if (this.boardBuilder != null) return;
      this.boardBuilder = GetComponentInChildren<BoardBuilder>();
      Debug.Log(this.transform.name + ": LoadBoardBuilder");
   }

   private void LoadBoardCameraCtrl()
   {
      if (this.boardCameraCtrl != null) return;
      this.boardCameraCtrl = GetComponentInChildren<BoardCameraCtrl>();
      Debug.Log(this.transform.name + ": LoadBoardCameraCtrl");
   }

   private void LoadBoardPlacementPreview()
   {
      if (this.boardPlacementPreview != null) return;
      this.boardPlacementPreview = GetComponentInChildren<BoardPlacementPreview>();
      Debug.Log(this.transform.name + ": LoadBoardPlacementPreview");
   }
}
