using System.Collections.Generic;

public class MatchResult
{
    private readonly List<MatchData> matches;

    public List<MatchData> Matches => this.matches;

    public MatchResult()
    {
        this.matches = new List<MatchData>();
    }

    public bool HasMatch()
    {
        return this.matches.Count > 0;
    }

    public MatchData GetOrCreateMatch(JellyCellCtrl currentCell, JellyCellCtrl neighborCell, JellyDirection direction)
    {
        foreach (MatchData match in this.matches)
        {
            if (match.CurrentCell == currentCell &&
                match.NeighborCell == neighborCell &&
                match.Direction == direction)
            {
                return match;
            }
        }

        MatchData newMatch = new(currentCell, neighborCell, direction);
        this.matches.Add(newMatch);

        return newMatch;
    }

    public int GetMatchedPieceCount()
    {
        int count = 0;

        foreach (MatchData match in this.matches)
        {
            count += match.CurrentPieces.Count;
            count += match.NeighborPieces.Count;
        }

        return count;
    }
}
