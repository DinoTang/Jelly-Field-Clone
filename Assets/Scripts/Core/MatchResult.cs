using System.Collections.Generic;

public class MatchResult
{
    private readonly Dictionary<JellyCellCtrl, List<JellyPieceCtrl>> matchedPieces;
    public Dictionary<JellyCellCtrl, List<JellyPieceCtrl>> MatchedPieces => matchedPieces;

    public bool HasMatch()
    {
        return this.matchedPieces.Count > 0;
    }
    public MatchResult()
    {
        this.matchedPieces = new Dictionary<JellyCellCtrl, List<JellyPieceCtrl>>();
    }

    public void AddMatch(JellyCellCtrl jellyCell, JellyPieceCtrl jellyPiece)
    {
        if (!this.matchedPieces.TryGetValue(jellyCell, out List<JellyPieceCtrl> pieces))
        {
            pieces = new List<JellyPieceCtrl>();
            this.matchedPieces.Add(jellyCell, pieces);
        }

        if (!pieces.Contains(jellyPiece))
            pieces.Add(jellyPiece);
    }

    public List<JellyPieceCtrl> GetMatchedPieces(JellyCellCtrl jellyCell)
    {
        if (this.matchedPieces.TryGetValue(jellyCell, out List<JellyPieceCtrl> pieces))
            return pieces;

        return new List<JellyPieceCtrl>();
    }

    public List<JellyCellCtrl> GetMatchedCells()
    {
        return new List<JellyCellCtrl>(this.matchedPieces.Keys);
    }

    public int GetMatchedPieceCount()
    {
        int count = 0;

        foreach (List<JellyPieceCtrl> pieces in this.matchedPieces.Values)
            count += pieces.Count;

        return count;
    }
}
