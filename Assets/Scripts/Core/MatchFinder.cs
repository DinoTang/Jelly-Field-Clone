using System.Collections.Generic;
using UnityEngine;

public class MatchFinder
{
    public MatchResult FindMatches(BoardSlot placedSlot, GridModel<BoardSlot> grid)
    {
        MatchResult matchResult = new();

        if (placedSlot == null || placedSlot.IsEmpty()) return matchResult;

        JellyCellCtrl currentCell = placedSlot.CurrentJellyCell;

        Vector2Int currentPos = currentCell.JellyCellConfig.GridPos;

        CheckNeighbor(
            currentCell,
            currentPos + Vector2Int.left,
            JellyDirection.Left,
            grid,
            matchResult);

        CheckNeighbor(
            currentCell,
            currentPos + Vector2Int.down,
            JellyDirection.Up,
            grid,
            matchResult);

        CheckNeighbor(
            currentCell,
            currentPos + Vector2Int.right,
            JellyDirection.Right,
            grid,
            matchResult);

        CheckNeighbor(
            currentCell,
            currentPos + Vector2Int.up,
            JellyDirection.Down,
            grid,
            matchResult);

        return matchResult;
    }

    private void CheckNeighbor(
        JellyCellCtrl currentCell,
        Vector2Int neighborPos,
        JellyDirection direction,
        GridModel<BoardSlot> grid,
        MatchResult matchResult)
    {
        if (!grid.IsInBounds(neighborPos.x, neighborPos.y)) return;

        BoardSlot neighborSlot = grid.Get(neighborPos.x, neighborPos.y);

        if (neighborSlot == null || neighborSlot.IsEmpty()) return;


        JellyCellCtrl neighborCell = neighborSlot.CurrentJellyCell;


        JellySlotType[] currentSlots = this.GetCurrentSideSlots(direction);


        JellySlotType[] neighborSlots = this.GetNeighborSideSlots(direction);


        this.FindMatchingPieces(currentCell, neighborCell, direction, currentSlots, neighborSlots, matchResult);
    }

    private void FindMatchingPieces(
     JellyCellCtrl currentCell,
     JellyCellCtrl neighborCell,
     JellyDirection direction,
     JellySlotType[] currentSlots,
     JellySlotType[] neighborSlots,
     MatchResult matchResult)
    {
        for (int i = 0; i < currentSlots.Length; i++)
        {
            JellySlotType currentSlot = currentSlots[i];
            JellySlotType neighborSlot = neighborSlots[i];

            foreach (JellyPieceCtrl currentPiece in currentCell.JellyCellConfig.JellyPieces)
            {
                if (!currentPiece.JellyPieceConfig.Slots.Contains(currentSlot)) continue;

                foreach (JellyPieceCtrl neighborPiece in neighborCell.JellyCellConfig.JellyPieces)
                {
                    if (!neighborPiece.JellyPieceConfig.Slots.Contains(neighborSlot)) continue;

                    if (!IsSameColor(currentPiece, neighborPiece)) continue;

                    MatchData match = matchResult.GetOrCreateMatch(currentCell, neighborCell, direction);

                    match.AddCurrentPiece(currentPiece);
                    match.AddNeighborPiece(neighborPiece);
                }
            }
        }
    }

    private bool IsSameColor(JellyPieceCtrl a, JellyPieceCtrl b)
    {
        return a.JellyPieceModel.Color == b.JellyPieceModel.Color;
    }


    private JellySlotType[] GetCurrentSideSlots(JellyDirection direction)

    {
        switch (direction)
        {
            case JellyDirection.Left:
                return new[]
                {
                    JellySlotType.TopLeft,
                    JellySlotType.BottomLeft
                };

            case JellyDirection.Up:
                return new[]
                {
                    JellySlotType.TopLeft,
                    JellySlotType.TopRight
                };

            case JellyDirection.Right:
                return new[]
                {
                    JellySlotType.TopRight,
                    JellySlotType.BottomRight
                };

            case JellyDirection.Down:
                return new[]
                {
                    JellySlotType.BottomLeft,
                    JellySlotType.BottomRight
                };
        }

        return new JellySlotType[0];
    }

    private JellySlotType[] GetNeighborSideSlots(
        JellyDirection direction)
    {
        switch (direction)
        {
            case JellyDirection.Left:
                return new[]
                {
                    JellySlotType.TopRight,
                    JellySlotType.BottomRight
                };

            case JellyDirection.Up:
                return new[]
                {
                    JellySlotType.BottomLeft,
                    JellySlotType.BottomRight
                };

            case JellyDirection.Right:
                return new[]
                {
                    JellySlotType.TopLeft,
                    JellySlotType.BottomLeft
                };

            case JellyDirection.Down:
                return new[]
                {
                    JellySlotType.TopLeft,
                    JellySlotType.TopRight
                };
        }

        return new JellySlotType[0];
    }
}