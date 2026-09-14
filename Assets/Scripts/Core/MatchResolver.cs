using System.Collections.Generic;

public class MatchResolver
{
    public void Resolve(MatchResult matchResult, GridModel<BoardSlot> grid)
    {
        if (!matchResult.HasMatch()) return;

        this.ClearMatches(matchResult);
    }

    private void ClearMatches(MatchResult matchResult)
    {
        foreach (var pair in matchResult.MatchedPieces)
        {
            JellyCellCtrl jellyCell = pair.Key;
            List<JellyPieceCtrl> matchedPieces = pair.Value;

            foreach (var jellyPiece in matchedPieces)
            {
                this.ClearPiece(jellyCell, jellyPiece);
            }

            if (jellyCell.JellyCellConfig.JellyPieces.Count == 0)
            {
                this.ClearCell(jellyCell);
            }
        }
    }

    private void ClearPiece(JellyCellCtrl jellyCell, JellyPieceCtrl jellyPiece)
    {
        jellyCell.JellyCellConfig.RemoveJellyPiece(jellyPiece);
        jellyPiece.JellyPieceDespawn.DoDespawn();
    }

    private void ClearCell(JellyCellCtrl jellyCell)
    {
        jellyCell.JellyCellDespawn.DoDespawn();
    }

    private void ResolveCells(MatchResult matchResult, GridModel<BoardSlot> grid)
    {
        // resolve cell sau khi clear
    }
}