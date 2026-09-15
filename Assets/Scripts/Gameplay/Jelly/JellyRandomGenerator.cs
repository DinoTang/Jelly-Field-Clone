using System.Collections.Generic;
using UnityEngine;

public class JellyRandomGenerator
{
    private readonly JellyColor[] colors =
    {
      JellyColor.Cyan,
      JellyColor.Green,
      JellyColor.Purple,
      JellyColor.Pink,
      JellyColor.Yellow,
   };

    public List<JellyPieceData> Generate()
    {
        int pieceCount = Random.Range(1, 5);

        switch (pieceCount)
        {
            case 1:
                return this.GenerateOnePiece();

            case 2:
                return this.GenerateTwoPieces();

            case 3:
                return this.GenerateThreePieces();

            default:
                return this.GenerateFourPieces();
        }
    }

    private List<JellyPieceData> GenerateOnePiece()
    {
        List<JellyPieceData> pieces = new();

        pieces.Add(this.CreatePiece(
           this.GetRandomColor(),
           new List<JellySlotType>
           {
            JellySlotType.TopLeft,
            JellySlotType.TopRight,
            JellySlotType.BottomLeft,
            JellySlotType.BottomRight
           }
        ));

        return pieces;
    }

    private List<JellyPieceData> GenerateTwoPieces()
    {
        List<JellyPieceData> pieces = new();

        JellyColor colorA = this.GetRandomColor();
        JellyColor colorB = this.GetDifferentColor(colorA);

        if (Random.value < 0.5f)
        {
            // Horizontal
            pieces.Add(this.CreatePiece(
               colorA,
               new List<JellySlotType>
               {
               JellySlotType.TopLeft,
               JellySlotType.TopRight
               }
            ));

            pieces.Add(this.CreatePiece(
               colorB,
               new List<JellySlotType>
               {
               JellySlotType.BottomLeft,
               JellySlotType.BottomRight
               }
            ));
        }
        else
        {
            // Vertical
            pieces.Add(this.CreatePiece(
               colorA,
               new List<JellySlotType>
               {
               JellySlotType.TopLeft,
               JellySlotType.BottomLeft
               }
            ));

            pieces.Add(this.CreatePiece(
               colorB,
               new List<JellySlotType>
               {
               JellySlotType.TopRight,
               JellySlotType.BottomRight
               }
            ));
        }

        return pieces;
    }

    private List<JellyPieceData> GenerateThreePieces()
    {
        List<JellyPieceData> pieces = new();

        JellyColor colorA = this.GetRandomColor();
        JellyColor colorB = this.GetDifferentColor(colorA);
        JellyColor colorC = this.GetDifferentColor(colorA, colorB);

        // Random một trong 4 vị trí để tạo piece 2 slot
        int pairType = Random.Range(0, 4);

        switch (pairType)
        {
            case 0:
                // TL + TR
                pieces.Add(this.CreatePiece(
                   colorA,
                   new List<JellySlotType>
                   {
                  JellySlotType.TopLeft,
                  JellySlotType.TopRight
                   }
                ));

                pieces.Add(this.CreatePiece(
                   colorB,
                   new List<JellySlotType>
                   {
                  JellySlotType.BottomLeft
                   }
                ));

                pieces.Add(this.CreatePiece(
                   colorC,
                   new List<JellySlotType>
                   {
                  JellySlotType.BottomRight
                   }
                ));
                break;

            case 1:
                // BL + BR
                pieces.Add(this.CreatePiece(
                   colorA,
                   new List<JellySlotType>
                   {
                  JellySlotType.BottomLeft,
                  JellySlotType.BottomRight
                   }
                ));

                pieces.Add(this.CreatePiece(
                   colorB,
                   new List<JellySlotType>
                   {
                  JellySlotType.TopLeft
                   }
                ));

                pieces.Add(this.CreatePiece(
                   colorC,
                   new List<JellySlotType>
                   {
                  JellySlotType.TopRight
                   }
                ));
                break;

            case 2:
                // TL + BL
                pieces.Add(this.CreatePiece(
                   colorA,
                   new List<JellySlotType>
                   {
                  JellySlotType.TopLeft,
                  JellySlotType.BottomLeft
                   }
                ));

                pieces.Add(this.CreatePiece(
                   colorB,
                   new List<JellySlotType>
                   {
                  JellySlotType.TopRight
                   }
                ));

                pieces.Add(this.CreatePiece(
                   colorC,
                   new List<JellySlotType>
                   {
                  JellySlotType.BottomRight
                   }
                ));
                break;

            default:
                // TR + BR
                pieces.Add(this.CreatePiece(
                   colorA,
                   new List<JellySlotType>
                   {
                  JellySlotType.TopRight,
                  JellySlotType.BottomRight
                   }
                ));

                pieces.Add(this.CreatePiece(
                   colorB,
                   new List<JellySlotType>
                   {
                  JellySlotType.TopLeft
                   }
                ));

                pieces.Add(this.CreatePiece(
                   colorC,
                   new List<JellySlotType>
                   {
                  JellySlotType.BottomLeft
                   }
                ));
                break;
        }

        return pieces;
    }

    private List<JellyPieceData> GenerateFourPieces()
    {
        List<JellyPieceData> pieces = new();

        List<JellyColor> randomColors = this.GetUniqueColors(4);

        pieces.Add(this.CreatePiece(
           randomColors[0],
           new List<JellySlotType> { JellySlotType.TopLeft }
        ));

        pieces.Add(this.CreatePiece(
           randomColors[1],
           new List<JellySlotType> { JellySlotType.TopRight }
        ));

        pieces.Add(this.CreatePiece(
           randomColors[2],
           new List<JellySlotType> { JellySlotType.BottomLeft }
        ));

        pieces.Add(this.CreatePiece(
           randomColors[3],
           new List<JellySlotType> { JellySlotType.BottomRight }
        ));

        return pieces;
    }

    private JellyPieceData CreatePiece(
       JellyColor color,
       List<JellySlotType> slots)
    {
        JellyPieceData piece = new JellyPieceData();

        piece.Color = color;
        piece.Slots = slots;

        return piece;
    }

    private JellyColor GetRandomColor()
    {
        return this.colors[Random.Range(0, this.colors.Length)];
    }

    private JellyColor GetDifferentColor(JellyColor color)
    {
        JellyColor result;

        do
        {
            result = this.GetRandomColor();
        }
        while (result == color);

        return result;
    }

    private JellyColor GetDifferentColor(
       JellyColor colorA,
       JellyColor colorB)
    {
        JellyColor result;

        do
        {
            result = this.GetRandomColor();
        }
        while (result == colorA || result == colorB);

        return result;
    }

    private List<JellyColor> GetUniqueColors(int count)
    {
        List<JellyColor> available = new(this.colors);
        List<JellyColor> result = new();

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, available.Count);

            result.Add(available[index]);
            available.RemoveAt(index);
        }

        return result;
    }
}