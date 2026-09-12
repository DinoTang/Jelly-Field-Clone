using System;
using System.Collections.Generic;

[Serializable]
public class JellyPieceData
{
   public JellyColor Color;
   public List<JellySlotType> Slots = new();
}