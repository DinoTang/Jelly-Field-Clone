using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JellyCellConfig : JellyCellAbstract
{
    [Header("Jelly Cell Config")]
    [SerializeField] protected Vector2Int gridPos;
    [SerializeField] protected List<JellyPieceCtrl> jellyPieces = new();

    public Vector2Int GridPos => gridPos;
    public List<JellyPieceCtrl> JellyPieces => jellyPieces;

    public void SetGridPos(int x, int y)
    {
        this.gridPos = new Vector2Int(x, y);
    }

    public void AddJellyPieces(JellyPieceCtrl pieces)
    {
        this.jellyPieces.Add(pieces);
    }
}
