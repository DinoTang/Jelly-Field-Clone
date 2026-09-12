using System;
using System.Collections.Generic;

[Serializable]
public class JellyPieceData
{
   public JellyColor color;
   public List<JellySlotType> slots = new();
}