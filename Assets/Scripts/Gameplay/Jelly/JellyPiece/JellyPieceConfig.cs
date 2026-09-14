using System.Collections.Generic;
using UnityEngine;

public class JellyPieceConfig : JellyPieceAbstract
{
   [Header("JellyPieceConfig")]
   [SerializeField] protected JellyMaterialSO jellyMaterialSO;
   [SerializeField] protected List<JellySlotType> slots;

   private Vector3 offset;

   public JellyMaterialSO JellyMaterialSO => jellyMaterialSO;
   public List<JellySlotType> Slots => slots;
   public Vector3 Offset => offset;

   public void SetOffset(Vector3 offset)
   {
      this.offset = offset;
   }

   public void SetSlots(List<JellySlotType> slots)
   {
      this.slots = new List<JellySlotType>(slots);
   }

   protected override void LoadComponent()
   {
      base.LoadComponent();
      this.LoadJellyMaterialSO();
   }

   private void LoadJellyMaterialSO()
   {
      if (this.jellyMaterialSO != null) return;
      this.jellyMaterialSO = Resources.Load<JellyMaterialSO>("JellyMaterialSO");
      Debug.Log(transform.name + ": LoadJellyMaterialSO");
   }
}
