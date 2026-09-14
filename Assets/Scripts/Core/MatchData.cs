using System.Collections.Generic;

public class MatchData
{
    public JellyCellCtrl CurrentCell { get; }
    public JellyCellCtrl NeighborCell { get; }
    public JellyDirection Direction { get; }
    public List<JellyPieceCtrl> CurrentPieces { get; }
    public List<JellyPieceCtrl> NeighborPieces { get; }

    public MatchData(JellyCellCtrl currentCell, JellyCellCtrl neighborCell, JellyDirection direction)
    {
        this.CurrentCell = currentCell;
        this.NeighborCell = neighborCell;
        this.Direction = direction;
        this.CurrentPieces = new List<JellyPieceCtrl>();
        this.NeighborPieces = new List<JellyPieceCtrl>();
    }

    public void AddCurrentPiece(JellyPieceCtrl piece)
    {
        if (!this.CurrentPieces.Contains(piece))
            this.CurrentPieces.Add(piece);
    }

    public void AddNeighborPiece(JellyPieceCtrl piece)
    {
        if (!this.NeighborPieces.Contains(piece))
            this.NeighborPieces.Add(piece);
    }
}