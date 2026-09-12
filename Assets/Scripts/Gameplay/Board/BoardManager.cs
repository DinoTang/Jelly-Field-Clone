using UnityEngine;

public class BoardManager : BaseBehaviour
{
   [SerializeField] protected BoardSlotSpawner boardSlotSpawner;
   [SerializeField] protected JellyLevelSO levelData;
   [SerializeField] protected Vector3 boardOrigin = new(0, 12f, 0);
   [SerializeField] private float cellSpacing = 0.7f;
   private GridModel<BoardSlot> grid;
   public GridModel<BoardSlot> Grid => grid;
   [SerializeField] private Transform cameraTarget;

   private float boardSlotSize = 0.75f;

   protected override void Start()
   {
      this.BuildBoard();
      this.UpdateCamera();
   }
   protected override void LoadComponent()
   {
      base.LoadComponent();
      this.LoadBoardSlotSpawner();
      this.LoadCameraTarget();
   }

   protected void InitGrid()
   {
      this.grid = new GridModel<BoardSlot>(this.levelData.Width, this.levelData.Height);
   }

   public Vector3 GetBoardPosition(int x, int y)
   {
      return this.boardOrigin + new Vector3(
            x * cellSpacing,
            y * cellSpacing,
            0f
        );
   }

   protected void LoadBoardSlotSpawner()
   {
      if (this.boardSlotSpawner != null) return;
      this.boardSlotSpawner = FindAnyObjectByType<BoardSlotSpawner>();
      Debug.Log(this.transform.name + ": LoadBoardSlotSpawner");
   }

   protected void LoadCameraTarget()
   {
      if (this.cameraTarget != null) return;
      this.cameraTarget = FindAnyObjectByType<MainCamera>().transform;
      Debug.Log(this.transform.name + ": LoadCameraTarget");
   }

   private void BuildBoard()
   {
      this.InitGrid();

      for (int x = 0; x < this.grid.Width; x++)
      {
         for (int y = 0; y < this.grid.Height; y++)
         {
            if (!levelData.IsValid(x, y)) continue;

            BoardSlot slot = this.boardSlotSpawner.Spawn("BoardSlot", this.GetBoardPosition(x, y));
            slot.transform.localScale = new Vector3(this.boardSlotSize, this.boardSlotSize, this.boardSlotSize);
            this.grid.Set(x, y, slot);
         }
      }
   }

   private void UpdateCamera()
   {
      Vector3 boardCenter = boardOrigin + new Vector3(
          (levelData.Width - 1) * cellSpacing / 2f,
          (levelData.Height - 1) * cellSpacing / 2f,
          0f
      );

      this.cameraTarget.position = boardCenter;

      Vector3 direction = Quaternion.Euler(-10f, 0f, 0f) * Vector3.back;

      this.cameraTarget.transform.position = boardCenter + direction * 10f;

      this.cameraTarget.transform.LookAt(boardCenter);
   }

}
