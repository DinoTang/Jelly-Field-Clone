using System.Collections.Generic;

public class MatchResolver
{
    public void Resolve(MatchResult matchResult, GridModel<BoardSlot> grid)
    {
        if (!matchResult.HasMatch()) return;

        this.ClearMatches(matchResult);
        this.FillMatches(matchResult);
    }

    private void ClearMatches(MatchResult matchResult)
    {
        // Lưu các JellyPiece đã được clear để tránh một piece bị clear/despawn nhiều lần
        // trong trường hợp JellyCell tham gia nhiều MatchData.
        HashSet<JellyPieceCtrl> clearedPieces = new();

        // Lưu các JellyCell bị ảnh hưởng bởi match để sau khi clear toàn bộ piece
        // mới kiểm tra xem Cell đó còn piece hay không.
        HashSet<JellyCellCtrl> affectedCells = new();

        foreach (MatchData match in matchResult.Matches)
        {
            JellyCellCtrl currentCell = match.CurrentCell;
            JellyCellCtrl neighborCell = match.NeighborCell;

            // Đánh dấu hai Cell này để kiểm tra trạng thái sau khi clear.
            affectedCells.Add(currentCell);
            affectedCells.Add(neighborCell);

            foreach (JellyPieceCtrl jellyPiece in match.CurrentPieces)
            {
                // Một JellyPiece có thể xuất hiện trong nhiều MatchData,
                // nên chỉ clear piece này một lần.
                if (clearedPieces.Contains(jellyPiece)) continue;

                clearedPieces.Add(jellyPiece);
                this.ClearPiece(currentCell, jellyPiece);
            }

            foreach (JellyPieceCtrl jellyPiece in match.NeighborPieces)
            {
                // Tránh clear/despawn lại cùng một JellyPiece.
                if (clearedPieces.Contains(jellyPiece)) continue;

                clearedPieces.Add(jellyPiece);
                this.ClearPiece(neighborCell, jellyPiece);
            }
        }

        // Chỉ kiểm tra JellyCell sau khi tất cả matched piece đã được clear.
        // Nếu Cell không còn JellyPiece nào thì clear luôn cả JellyCell.
        foreach (JellyCellCtrl jellyCell in affectedCells)
        {
            if (jellyCell == null) continue;

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

    private void FillMatches(MatchResult matchResult)
    {
        foreach (MatchData match in matchResult.Matches)
        {
            this.FillCell(match.CurrentCell, match.Direction);
            this.FillCell(match.NeighborCell, this.GetOppositeDirection(match.Direction));
        }
    }

    private void FillCell(JellyCellCtrl jellyCell, JellyDirection direction)
    {
        if (jellyCell == null) return;

        // Tìm các slot đang trống trong JellyCell.
        List<JellySlotType> emptySlots = this.GetEmptySlots(jellyCell);

        if (emptySlots.Count == 0) return;

        // Tìm Piece phù hợp để fill vào từng slot trống.
        // Piece có ít slot hơn sẽ được ưu tiên trước.
        Dictionary<JellyPieceCtrl, List<JellySlotType>> sourcePieces =
            this.FindFillSourcePieces(jellyCell, emptySlots, direction);

        foreach (KeyValuePair<JellyPieceCtrl, List<JellySlotType>> pair in sourcePieces)
        {
            JellyPieceCtrl piece = pair.Key;
            List<JellySlotType> targetSlots = pair.Value;

            // Animate Piece tràn từ vị trí hiện tại sang các slot mới.
            // Slot chỉ được cập nhật vào Config sau khi animation hoàn thành.
            piece.JellyPieceFillAnimator.PlayFill(
            jellyCell.JellyCellArrange,
            piece,
            targetSlots,
            () =>
            {
                // Cập nhật lại offset để DragHandler không bị lệch
                // sau khi Piece đã thay đổi vị trí và kích thước.
                jellyCell.JellyCellDragHandler.CachePieceOffsets();
            }
        );
        }
    }

    private List<JellySlotType> GetEmptySlots(JellyCellCtrl jellyCell)
    {
        List<JellySlotType> emptySlots = new();

        foreach (JellySlotType slot in System.Enum.GetValues(typeof(JellySlotType)))
        {
            bool occupied = false;

            foreach (JellyPieceCtrl piece in jellyCell.JellyCellConfig.JellyPieces)
            {
                if (piece.JellyPieceConfig.Slots.Contains(slot))
                {
                    occupied = true;
                    break;
                }
            }

            if (!occupied)
            {
                emptySlots.Add(slot);
            }
        }

        return emptySlots;
    }

    private Dictionary<JellyPieceCtrl, List<JellySlotType>> FindFillSourcePieces(
     JellyCellCtrl jellyCell,
     List<JellySlotType> emptySlots,
     JellyDirection direction)
    {
        Dictionary<JellyPieceCtrl, List<JellySlotType>> sourcePieces = new();

        foreach (JellySlotType emptySlot in emptySlots)
        {
            // Tìm Piece phù hợp nhất để fill vào slot trống hiện tại.
            JellyPieceCtrl bestPiece = null;
            int bestPriority = -1;
            int bestSlotCount = int.MaxValue;

            foreach (JellyPieceCtrl piece in jellyCell.JellyCellConfig.JellyPieces)
            {
                if (piece == null) continue;

                int pieceSlotCount = piece.JellyPieceConfig.Slots.Count;

                foreach (JellySlotType sourceSlot in piece.JellyPieceConfig.Slots)
                {
                    // Chỉ Piece có slot nằm cạnh slot trống mới có thể fill.
                    if (!this.IsAdjacentSlot(sourceSlot, emptySlot))
                        continue;

                    // Tính độ ưu tiên dựa trên hướng fill.
                    int priority = this.GetFillPriority(sourceSlot, emptySlot, direction);

                    // Ưu tiên Piece đang chiếm ít slot hơn.
                    if (pieceSlotCount < bestSlotCount)
                    {
                        bestPiece = piece;
                        bestPriority = priority;
                        bestSlotCount = pieceSlotCount;
                        continue;
                    }

                    // Nếu cùng số slot, ưu tiên Piece có hướng fill phù hợp hơn.
                    if (pieceSlotCount == bestSlotCount && priority > bestPriority)
                    {
                        bestPiece = piece;
                        bestPriority = priority;
                    }
                }
            }

            if (bestPiece == null)
                continue;

            // Gom các slot cần fill theo từng Piece.
            if (!sourcePieces.TryGetValue(bestPiece, out List<JellySlotType> targetSlots))
            {
                targetSlots = new List<JellySlotType>();
                sourcePieces.Add(bestPiece, targetSlots);
            }

            if (!targetSlots.Contains(emptySlot))
            {
                targetSlots.Add(emptySlot);
            }
        }

        return sourcePieces;
    }

    private int GetFillPriority(
        JellySlotType sourceSlot,
        JellySlotType targetSlot,
        JellyDirection direction)
    {
        // Ưu tiên Piece fill theo hướng ngược lại với hướng match.
        if (this.CanFillToSlot(
            sourceSlot,
            targetSlot,
            this.GetOppositeDirection(direction)))
        {
            return 2;
        }

        // Piece vẫn có thể fill nhưng không đúng hướng ưu tiên.
        return 1;
    }

    private bool IsAdjacentSlot(JellySlotType sourceSlot, JellySlotType targetSlot)
    {
        return (sourceSlot == JellySlotType.TopLeft && targetSlot == JellySlotType.TopRight) ||
               (sourceSlot == JellySlotType.TopRight && targetSlot == JellySlotType.TopLeft) ||
               (sourceSlot == JellySlotType.BottomLeft && targetSlot == JellySlotType.BottomRight) ||
               (sourceSlot == JellySlotType.BottomRight && targetSlot == JellySlotType.BottomLeft) ||
               (sourceSlot == JellySlotType.TopLeft && targetSlot == JellySlotType.BottomLeft) ||
               (sourceSlot == JellySlotType.BottomLeft && targetSlot == JellySlotType.TopLeft) ||
               (sourceSlot == JellySlotType.TopRight && targetSlot == JellySlotType.BottomRight) ||
               (sourceSlot == JellySlotType.BottomRight && targetSlot == JellySlotType.TopRight);
    }

    private bool CanFillToSlot(JellySlotType sourceSlot, JellySlotType targetSlot, JellyDirection direction)
    {
        switch (direction)
        {
            case JellyDirection.Left:
                return (sourceSlot == JellySlotType.TopRight && targetSlot == JellySlotType.TopLeft) ||
                       (sourceSlot == JellySlotType.BottomRight && targetSlot == JellySlotType.BottomLeft);

            case JellyDirection.Right:
                return (sourceSlot == JellySlotType.TopLeft && targetSlot == JellySlotType.TopRight) ||
                       (sourceSlot == JellySlotType.BottomLeft && targetSlot == JellySlotType.BottomRight);

            case JellyDirection.Up:
                return (sourceSlot == JellySlotType.BottomLeft && targetSlot == JellySlotType.TopLeft) ||
                       (sourceSlot == JellySlotType.BottomRight && targetSlot == JellySlotType.TopRight);

            case JellyDirection.Down:
                return (sourceSlot == JellySlotType.TopLeft && targetSlot == JellySlotType.BottomLeft) ||
                       (sourceSlot == JellySlotType.TopRight && targetSlot == JellySlotType.BottomRight);
        }

        return false;
    }

    private JellyDirection GetOppositeDirection(JellyDirection direction)
    {
        switch (direction)
        {
            case JellyDirection.Left:
                return JellyDirection.Right;

            case JellyDirection.Right:
                return JellyDirection.Left;

            case JellyDirection.Up:
                return JellyDirection.Down;

            case JellyDirection.Down:
                return JellyDirection.Up;
        }

        return direction;
    }
}