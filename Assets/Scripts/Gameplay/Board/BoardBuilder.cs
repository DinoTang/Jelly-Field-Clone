using UnityEngine;

public class BoardBuilder : BaseBehaviour
{
   [SerializeField] private BoardSlotSpawner boardSlotSpawner;
   [SerializeField] private JellyCellSpawner jellyCellSpawner;
   [SerializeField] private JellyPieceSpawner jellyPieceSpawner;
   [SerializeField] private Vector3 boardOrigin = new(0f, 12f, 0f);
   [SerializeField] private float cellSpacing = 0.725f;
   [SerializeField] private float boardSlotSize = 0.75f;

   private GridModel<BoardSlot> grid;
   public GridModel<BoardSlot> Grid => grid;
   public float CellSpacing => cellSpacing;
   public Vector3 BoardOrigin => boardOrigin;
   protected override void LoadComponent()
   {
      base.LoadComponent();
      this.LoadBoardSlotSpawner();
      this.LoadJellyCellSpawner();
      this.LoadJellyPieceSpawner();
   }

   private void LoadBoardSlotSpawner()
   {
      if (this.boardSlotSpawner != null) return;
      this.boardSlotSpawner = FindAnyObjectByType<BoardSlotSpawner>();
      Debug.Log(this.transform.name + ": LoadBoardSlotSpawner");
   }

   private void LoadJellyCellSpawner()
   {
      if (this.jellyCellSpawner != null) return;
      this.jellyCellSpawner = FindAnyObjectByType<JellyCellSpawner>();
      Debug.Log(this.transform.name + ": LoadJellyCellSpawner");
   }

   private void LoadJellyPieceSpawner()
   {
      if (this.jellyPieceSpawner != null) return;
      this.jellyPieceSpawner = FindAnyObjectByType<JellyPieceSpawner>();
      Debug.Log(this.transform.name + ": LoadJellyPieceSpawner");
   }

   public void Build(JellyLevelSO levelData)
   {
      this.InitGrid(levelData);

      for (int x = 0; x < grid.Width; x++)
      {
         for (int y = 0; y < grid.Height; y++)
         {
            if (!levelData.IsValid(x, y)) continue;

            Vector3 slotPos = this.GetBoardPosition(x, y, levelData);

            BoardSlot slot = boardSlotSpawner.Spawn("BoardSlot", slotPos);
            slot.transform.localScale = Vector3.one * boardSlotSize;

            this.grid.Set(x, y, slot);

            this.BuildJellyCell(slot, levelData, x, y);
         }
      }

      this.SpawnPlayerJellyCell(levelData);
   }

   private void InitGrid(JellyLevelSO levelData)
   {
      this.grid = new GridModel<BoardSlot>(levelData.Width, levelData.Height);
   }

   public Vector3 GetBoardPosition(int x, int y, JellyLevelSO levelData)
   {
      int worldY = levelData.Height - 1 - y;

      return this.boardOrigin + new Vector3(
         x * cellSpacing,
         worldY * cellSpacing
      );
   }

   private void BuildJellyCell(BoardSlot slot, JellyLevelSO levelData, int x, int y)
   {
      JellyCellData jellyCellData = levelData.JellyCells.Find(cell => cell.Position.x == x && cell.Position.y == y);
      if (jellyCellData == null) return;

      // Spawn jellyCell

      Vector3 jellyCellPos = this.GetBoardPosition(x, y, levelData);
      JellyCellCtrl jellyCellCtrl = this.jellyCellSpawner.Spawn("JellyCellCtrl", jellyCellPos);

      // SetData cho jellyCellCtrl
      jellyCellCtrl.JellyCellConfig.SetGridPos(x, y);
      jellyCellCtrl.SetJellyPosition();

      slot.SetJellyCell(jellyCellCtrl);

      // Spawn jellyPiece
      this.BuildJellyPiece(jellyCellData, jellyCellCtrl, jellyCellPos);
   }

   private void BuildJellyPiece(JellyCellData jellyCellData, JellyCellCtrl jellyCellCtrl, Vector3 position)
   {

      foreach (JellyPieceData jellyPieceData in jellyCellData.Pieces)
      {
         JellyPieceCtrl jellyPieceCtrl = this.jellyPieceSpawner.Spawn("JellyPieceCtrl", position);

         // SetData cho jellyPieceCtrl
         jellyPieceCtrl.JellyPieceModel.SetColor(jellyPieceData.Color);
         jellyPieceCtrl.JellyPieceModel.ApplyMaterialByColor();
         jellyPieceCtrl.JellyPieceConfig.SetSlots(jellyPieceData.Slots);

         // Đưa các jellyPiece vào danh sách chứa của jellyCell
         jellyCellCtrl.JellyCellConfig.AddJellyPieces(jellyPieceCtrl);

         // Arrange các jellyPiece
         jellyCellCtrl.JellyCellArrange.ArrangePiece(jellyPieceCtrl);
      }

      jellyCellCtrl.JellyCellDragHandler.CachePieceOffsets();
   }


   [SerializeField] private float spawnJellyY = -3f;
   private Vector3 GetBoardCenter(JellyLevelSO levelData)
   {
      float centerX = (levelData.Width - 1) * cellSpacing * 0.5f;
      float centerY = (levelData.Height - 1) * cellSpacing * 0.5f;


      return boardOrigin + new Vector3(
          centerX,
          centerY,
          0
      );
   }
   private void SpawnPlayerJellyCell(JellyLevelSO levelData)
   {
      Vector3 center = this.GetBoardCenter(levelData);


      Vector3 spawnPosition = center + new Vector3(
         0,
         this.spawnJellyY,
         0
      );


      JellyCellCtrl jellyCellCtrl =
          jellyCellSpawner.Spawn(
              "JellyCellCtrl",
              spawnPosition
          );


      JellyCellData jellyCellData =
          levelData.JellyCells[0];


      this.BuildJellyPiece(
          jellyCellData,
          jellyCellCtrl,
          spawnPosition
      );


      jellyCellCtrl.JellyCellDragHandler.CachePieceOffsets();
      jellyCellCtrl.JellyCellDragHandler.SetIsClocking(false);
   }
}