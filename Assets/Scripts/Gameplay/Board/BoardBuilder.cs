using System.Collections.Generic;
using UnityEngine;

public class BoardBuilder : BoardManagerAbstract
{
   [SerializeField] private BoardSlotSpawn boardSlotSpawner;
   [SerializeField] private JellyCellSpawn jellyCellSpawner;
   [SerializeField] private JellyPieceSpawn jellyPieceSpawner;
   [SerializeField] private SpawnPointSpawn spawnPointSpawner;
   [SerializeField] private Vector3 boardOrigin = new(0f, 12f, 0f);
   [SerializeField] private float cellSpacing = 0.725f;
   [SerializeField] private float boardSlotSize = 0.75f;
   [SerializeField] private float boardSlotOffsetZ = 0.32f;

   [SerializeField] private float playerJellyOffsetY = 0.1f;
   [SerializeField] private float playerJellyPieceOffsetZ = 0.32f;


   public float CellSpacing => cellSpacing;
   public Vector3 BoardOrigin => boardOrigin;
   public float PlayerJellyOffsetY => playerJellyOffsetY;
   protected override void LoadComponent()
   {
      base.LoadComponent();
      this.LoadBoardSlotSpawner();
      this.LoadJellyCellSpawner();
      this.LoadJellyPieceSpawner();
      this.LoadSpawnPointSpawn();
   }

   public void Clear()
   {
      this.jellyPieceSpawner.DespawnAll();
      this.jellyCellSpawner.DespawnAll();
      this.boardSlotSpawner.DespawnAll();
      this.spawnPointSpawner.DespawnAll();
   }

   private void LoadBoardSlotSpawner()
   {
      if (this.boardSlotSpawner != null) return;
      this.boardSlotSpawner = FindAnyObjectByType<BoardSlotSpawn>();
      Debug.Log(this.transform.name + ": LoadBoardSlotSpawner");
   }

   private void LoadJellyCellSpawner()
   {
      if (this.jellyCellSpawner != null) return;
      this.jellyCellSpawner = FindAnyObjectByType<JellyCellSpawn>();
      Debug.Log(this.transform.name + ": LoadJellyCellSpawner");
   }

   private void LoadJellyPieceSpawner()
   {
      if (this.jellyPieceSpawner != null) return;
      this.jellyPieceSpawner = FindAnyObjectByType<JellyPieceSpawn>();
      Debug.Log(this.transform.name + ": LoadJellyPieceSpawner");
   }

   private void LoadSpawnPointSpawn()
   {
      if (this.spawnPointSpawner != null) return;
      this.spawnPointSpawner = FindAnyObjectByType<SpawnPointSpawn>();
      Debug.Log(this.transform.name + ": LoadSpawnPointSpawn");
   }


   public void Build()
   {
      for (int x = 0; x < this.boardManager.Grid.Width; x++)
      {
         for (int y = 0; y < this.boardManager.Grid.Height; y++)
         {
            if (!this.boardManager.LevelData.IsValid(x, y)) continue;

            Vector3 slotPos = this.GetBoardPosition(x, y);
            BoardSlot slot = boardSlotSpawner.Spawn("BoardSlot", slotPos);
            slot.ResetData();

            Vector3 slotPosition = slot.transform.position;
            slotPosition.z += this.boardSlotOffsetZ;
            slot.transform.position = slotPosition;

            slot.SetGridPos(x, y);
            slot.transform.localScale = Vector3.one * boardSlotSize;

            this.boardManager.Grid.Set(x, y, slot);

            this.BuildJellyCell(slot, x, y);
         }
      }

      this.SpawnPlayerJellyCells();
   }


   public Vector3 GetBoardPosition(int x, int y)
   {
      int worldY = this.boardManager.LevelData.Height - 1 - y;

      return this.boardOrigin + new Vector3(
         x * cellSpacing,
         worldY * cellSpacing
      );
   }

   private void BuildJellyCell(BoardSlot slot, int x, int y)
   {
      JellyCellData jellyCellData =
      this.boardManager.LevelData.JellyCells.Find(cell => cell.Position.x == x && cell.Position.y == y);
      if (jellyCellData == null) return;

      // Spawn jellyCell

      Vector3 jellyCellPos = this.GetBoardPosition(x, y);
      JellyCellCtrl jellyCellCtrl = this.jellyCellSpawner.Spawn("JellyCellCtrl", jellyCellPos);
      jellyCellCtrl.JellyCellConfig.ResetData();

      // SetData cho jellyCellCtrl
      jellyCellCtrl.JellyCellConfig.SetGridPos(x, y);
      jellyCellCtrl.JellyCellConfig.SetJellyPosition();

      slot.SetJellyCell(jellyCellCtrl);

      // Spawn jellyPiece
      this.BuildJellyPiece(jellyCellData, jellyCellCtrl, jellyCellPos);
   }

   private void BuildJellyPiece(JellyCellData jellyCellData, JellyCellCtrl jellyCellCtrl, Vector3 position)
   {

      foreach (JellyPieceData jellyPieceData in jellyCellData.Pieces)
      {
         JellyPieceCtrl jellyPieceCtrl = this.jellyPieceSpawner.Spawn("JellyPieceCtrl", position);
         jellyPieceCtrl.JellyPieceConfig.ResetData();

         // SetData cho jellyPieceCtrl
         jellyPieceCtrl.JellyPieceModel.SetColor(jellyPieceData.Color);
         jellyPieceCtrl.JellyPieceModel.ApplyMaterialByColor();
         jellyPieceCtrl.JellyPieceConfig.SetSlots(jellyPieceData.Slots);
         jellyPieceCtrl.JellyPieceConfig.SetOwner(jellyCellCtrl);

         // Đưa các jellyPiece vào danh sách chứa của jellyCell
         jellyCellCtrl.JellyCellConfig.AddJellyPieces(jellyPieceCtrl);

         // Arrange các jellyPiece
         jellyCellCtrl.JellyCellArrange.ArrangePiece(jellyPieceCtrl);
      }

      jellyCellCtrl.JellyCellDragHandler.CachePieceOffsets();
   }
   public void SpawnPlayerJellyCellAfterDrop(Vector3 spawnPoint)
   {
      this.SpawnPlayerJellyCell(spawnPoint);
   }

   public void SpawnPlayerJellyCells()
   {
      List<SpawnPoint> spawnPoints = this.SpawnSpawnPoints();
      foreach (SpawnPoint spawnPoint in spawnPoints)
      {
         if (spawnPoint == null) continue;

         this.SpawnPlayerJellyCell(spawnPoint.transform.position);
      }
   }

   private void SpawnPlayerJellyCell(Vector3 spawnPosition)
   {
      spawnPosition.y += this.playerJellyOffsetY;

      JellyCellCtrl jellyCellCtrl = this.jellyCellSpawner.Spawn("JellyCellCtrl", spawnPosition);
      jellyCellCtrl.JellyCellConfig.ResetData();

      // Gắn spawnPoint vào jellyCell để sau này spawn lại ngay tại đó
      jellyCellCtrl.JellyCellDragHandler.SetSpawnPoint(spawnPosition);

      List<JellyPieceData> jellyPieces = this.boardManager.RandomGenerator.Generate();

      jellyCellCtrl.JellyCellConfig.SetGridPos(-1, -1);
      jellyCellCtrl.JellyCellConfig.SetJellyPosition();

      foreach (JellyPieceData jellyPieceData in jellyPieces)
      {
         JellyPieceCtrl jellyPieceCtrl = this.jellyPieceSpawner.Spawn("JellyPieceCtrl", spawnPosition);
         jellyPieceCtrl.JellyPieceConfig.ResetData();

         jellyPieceCtrl.JellyPieceModel.SetColor(jellyPieceData.Color);
         jellyPieceCtrl.JellyPieceModel.ApplyMaterialByColor();
         jellyPieceCtrl.JellyPieceConfig.SetSlots(jellyPieceData.Slots);
         jellyPieceCtrl.JellyPieceConfig.SetOwner(jellyCellCtrl);

         jellyCellCtrl.JellyCellConfig.AddJellyPieces(jellyPieceCtrl);

         // Arrange trước
         jellyCellCtrl.JellyCellArrange.ArrangePiece(jellyPieceCtrl);

         // Chỉ Player Jelly mới có offset Z + bù Y theo camera
         jellyPieceCtrl.transform.position += this.GetPlayerJellyPieceOffset();
      }

      jellyCellCtrl.JellyCellDragHandler.CachePieceOffsets();
      jellyCellCtrl.JellyCellDragHandler.SetIsClocking(false);
   }

   private List<SpawnPoint> SpawnSpawnPoints()
   {
      List<SpawnPoint> spawnPoints = new();
      foreach (JellySpawnPointData spawnPointData in this.boardManager.LevelData.SpawnPoints)
      {
         SpawnPoint spawnPoint = this.spawnPointSpawner.Spawn("SpawnPoint", spawnPointData.Position);
         spawnPoints.Add(spawnPoint);
      }
      return spawnPoints;
   }
   private Vector3 GetPlayerJellyPieceOffset()
   {
      float angle = Mathf.Abs(this.boardManager.BoardCameraCtrl.CameraRotationX); ;
      float yOffset = this.playerJellyPieceOffsetZ * Mathf.Tan(angle * Mathf.Deg2Rad);

      return new Vector3(
          0f,
          yOffset,
          this.playerJellyPieceOffsetZ
      );
   }

}