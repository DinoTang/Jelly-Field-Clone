using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JellyCellData
{
   public Vector2Int position;

   public List<JellyPieceData> pieces = new();
}