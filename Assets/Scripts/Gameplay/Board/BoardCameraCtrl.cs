using UnityEngine;

public class BoardCameraCtrl : BaseBehaviour
{
   [SerializeField] private Transform mainCamera;

   protected override void LoadComponent()
   {
      base.LoadComponent();
      this.LoadMainCamera();
   }

   private void LoadMainCamera()
   {
      if (this.mainCamera != null) return;
      this.mainCamera = Camera.main.transform;
      Debug.Log(this.transform.name + ": LoadMainCamera");
   }

   public void CenterOnBoard(int width, int height, float cellSpacing, Vector3 boardOrigin)
   {
      Vector3 boardCenter = boardOrigin + new Vector3(
          (width - 1) * cellSpacing / 2f,
          (height - 1) * cellSpacing / 2f,
          0f
      );

      Quaternion rotation =
          Quaternion.Euler(-15f, 0f, 0f);

      mainCamera.rotation = rotation;
      mainCamera.position =
          boardCenter -
          rotation * Vector3.forward * 10f;
   }
}
