using System.Collections;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
   [Header("Board Manager")]
   [SerializeField] protected JellyLevelSO levelData;
   [SerializeField] protected BoardBuilder boardBuilder;
   [SerializeField] protected BoardCameraCtrl boardCameraCtrl;
   [SerializeField] protected BoardPlacementPreview boardPlacementPreview;

   private readonly MatchFinder matchFinder = new();
   private readonly MatchResult matchResult = new();
   private readonly MatchResolver matchResolver = new();
   private readonly JellyRandomGenerator randomGenerator = new();

   public JellyLevelSO LevelData => levelData;
   public BoardBuilder BoardBuilder => boardBuilder;
   public BoardCameraCtrl BoardCameraCtrl => boardCameraCtrl;
   public BoardPlacementPreview BoardPlacementPreview => boardPlacementPreview;
   public MatchFinder MatchFinder => matchFinder;
   public MatchResult MatchResult => matchResult;
   public MatchResolver MatchResolver => matchResolver;
   public JellyRandomGenerator RandomGenerator => randomGenerator;
   private GridModel<BoardSlot> grid;
   public GridModel<BoardSlot> Grid => grid;
   private void InitGrid()
   {
      this.grid = new GridModel<BoardSlot>(this.levelData.Width, this.levelData.Height);
   }

   protected override void Start()
   {
      this.levelData = GameManager.Instance.LevelData;
      this.InitGrid();
      this.boardBuilder.Build();
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


   public IEnumerator ResolveChain(GridModel<BoardSlot> grid)
   {
      while (true)
      {
         // Tìm toàn bộ Match hiện tại trên Board.
         MatchResult matchResult = this.MatchFinder.FindAllMatches(grid);

         // Không còn Match nào thì Chain Resolve kết thúc.
         if (!matchResult.HasMatch()) yield break;

         bool resolveComplete = false;

         // Resolve một batch Match.
         // Callback chỉ được gọi sau khi toàn bộ Fill animation hoàn thành.
         this.MatchResolver.Resolve(
             matchResult,
             grid,
             () =>
             {
                resolveComplete = true;
             }
         );

         // Chờ Clear + Fill + Animation hoàn thành.
         yield return new WaitUntil(() => resolveComplete);
         yield return new WaitForSeconds(0.25f);

         // Sau khi Fill xong, vòng while chạy lại
         // và tìm Match mới để tiếp tục Chain.
      }
   }

   public void ResetBoard()
   {
      this.StopAllCoroutines();

      this.levelData = GameManager.Instance.LevelData;

      this.boardBuilder.Clear();

      this.grid = new GridModel<BoardSlot>(
          this.levelData.Width,
          this.levelData.Height
      );

      this.boardBuilder.Build();

      this.boardCameraCtrl.CenterOnBoard(
          this.levelData.Width,
          this.levelData.Height,
          this.boardBuilder.CellSpacing,
          this.boardBuilder.BoardOrigin
      );
   }
}
