using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JellyCellData
{
   public Vector2Int Position;

   public List<JellyPieceData> Pieces = new();
}