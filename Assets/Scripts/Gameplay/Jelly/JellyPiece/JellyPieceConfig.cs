using System.Collections.Generic;
using UnityEngine;

public class JellyPieceConfig : JellyPieceAbstract
{
   [Header("JellyPieceConfig")]
   [SerializeField] protected JellyMaterialSO jellyMaterialSO;
   [SerializeField] protected JellyPieceSizeConfig jellyPieceSizeConfig;
   [SerializeField] protected List<JellySlotType> slots;
   [SerializeField] protected JellyCellCtrl owner;
   private Vector3 offset;

   public JellyMaterialSO JellyMaterialSO => jellyMaterialSO;
   public JellyPieceSizeConfig JellyPieceSizeConfig => jellyPieceSizeConfig;
   public List<JellySlotType> Slots => slots;
   public Vector3 Offset => offset;
   public JellyCellCtrl Owner => owner;
   public void ResetData()
   {
      this.Slots.Clear();
      this.SetOwner(null);
      this.SetOffset(Vector3.zero);
   }
   public void SetOffset(Vector3 offset)
   {
      this.offset = offset;
   }

   public void SetSlots(List<JellySlotType> slots)
   {
      this.slots = new List<JellySlotType>(slots);
   }

   public void SetOwner(JellyCellCtrl owner)
   {
      this.owner = owner;
   }

   protected override void LoadComponent()
   {
      base.LoadComponent();
      this.LoadJellyMaterialSO();
      this.LoadJellyPieceSizeConfig();
   }

   private void LoadJellyMaterialSO()
   {
      if (this.jellyMaterialSO != null) return;
      this.jellyMaterialSO = Resources.Load<JellyMaterialSO>("JellyMaterialSO");
      Debug.Log(transform.name + ": LoadJellyMaterialSO");
   }

   private void LoadJellyPieceSizeConfig()
   {
      if (this.jellyPieceSizeConfig != null) return;
      this.jellyPieceSizeConfig = transform.GetComponent<JellyPieceSizeConfig>();
      Debug.Log(transform.name + ": LoadJellyMaterialSO");
   }
}
