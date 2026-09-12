using System.Collections.Generic;
using UnityEngine;

public class JellyPieceConfig : JellyPieceAbstract
{
   [Header("JellyPieceConfig")]
   [SerializeField] protected JellyMaterialSO jellyMaterialSO;
   public JellyMaterialSO JellyMaterialSO => jellyMaterialSO;
   [SerializeField] protected List<JellySlotType> slots;
   public List<JellySlotType> Slots => slots;

   public void SetSlots(List<JellySlotType> slots)
   {
      this.slots = new List<JellySlotType>(slots);
   }

   protected override void LoadComponent()
   {
      base.LoadComponent();
      this.LoadJellyMaterialSO();
   }

   protected void LoadJellyMaterialSO()
   {
      if (this.jellyMaterialSO != null) return;
      this.jellyMaterialSO = Resources.Load<JellyMaterialSO>("JellyMaterialSO");
      Debug.Log(transform.name + ": LoadJellyMaterialSO");
   }
}
