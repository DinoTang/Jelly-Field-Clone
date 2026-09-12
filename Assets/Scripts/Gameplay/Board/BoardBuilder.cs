using UnityEngine;

public class BoardBuilder : BaseBehaviour
{
   [SerializeField] private BoardSlotSpawner boardSlotSpawner;
   [SerializeField] private JellyCellSpawner jellyCellSpawner;
   [SerializeField] private JellyPieceSpawner jellyPieceSpawner;
   [SerializeField] private Vector3 boardOrigin = new(0f, 12f, 0f);
   [SerializeField] private float cellSpacing = 0.7f;
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

   protected void LoadBoardSlotSpawner()
   {
      if (this.boardSlotSpawner != null) return;
      this.boardSlotSpawner = FindAnyObjectByType<BoardSlotSpawner>();
      Debug.Log(this.transform.name + ": LoadBoardSlotSpawner");
   }

   protected void LoadJellyCellSpawner()
   {
      if (this.jellyCellSpawner != null) return;
      this.jellyCellSpawner = FindAnyObjectByType<JellyCellSpawner>();
      Debug.Log(this.transform.name + ": LoadJellyCellSpawner");
   }

   protected void LoadJellyPieceSpawner()
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
            if (!levelData.IsValid(x, y))
               continue;

            Vector3 position = this.GetBoardPosition(x, y, levelData);

            BoardSlot slot = boardSlotSpawner.Spawn("BoardSlot", position);
            slot.transform.localScale = Vector3.one * boardSlotSize;
            this.grid.Set(x, y, slot);

            this.BuildJellyCell(levelData, x, y, position);
         }
      }
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
          worldY * cellSpacing,
          0f
      );
   }

   private void BuildJellyCell(JellyLevelSO levelData, int x, int y, Vector3 position)
   {
      // Spawn jellyCell
      JellyCellData jellyCellData = levelData.JellyCells.Find(cell => cell.Position.x == x && cell.Position.y == y);
      if (jellyCellData == null) return;

      JellyCellCtrl jellyCellCtrl = this.jellyCellSpawner.Spawn("JellyCellCtrl", position);

      // SetData cho jellyCellCtrl
      jellyCellCtrl.SetGridPos(x, y);
      jellyCellCtrl.SetJellyPieces(jellyCellData.Pieces);

      this.BuildJellyPiece(jellyCellData, jellyCellCtrl, position);
   }

   private void BuildJellyPiece(JellyCellData jellyCellData, JellyCellCtrl jellyCellCtrl, Vector3 position)
   {
      // Spawn jellyPiece
      foreach (JellyPieceData jellyPieceData in jellyCellData.Pieces)
      {
         JellyPieceCtrl jellyPieceCtrl = this.jellyPieceSpawner.Spawn("JellyPieceCtrl", position);

         // SetData cho jellyPieceCtrl
         jellyPieceCtrl.JellyPieceModel.SetColor(jellyPieceData.Color);
         jellyPieceCtrl.JellyPieceModel.ApplyMaterialByColor();
         jellyPieceCtrl.JellyCellConfig.SetSlots(jellyPieceData.Slots);

         // Arrange các jellyPiece
         jellyCellCtrl.ArrangePiece(jellyPieceCtrl);
      }
   }
}