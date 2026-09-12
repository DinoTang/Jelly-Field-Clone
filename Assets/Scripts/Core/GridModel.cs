using System;

public class GridModel<T>
{
    public int Width { get; }
    public int Height { get; }

    private readonly T[,] cells;
    public T[,] Cells => cells;
    public GridModel(int width, int height)
    {
        Width = width;
        Height = height;
        cells = new T[width, height];
    }

    public T Get(int x, int y)
    {
        if (!IsInBounds(x, y))
            throw new Exception($"Out of bounds: {x},{y}");

        return cells[x, y];
    }

    public void Set(int x, int y, T value)
    {
        if (!IsInBounds(x, y))
            throw new Exception($"Out of bounds: {x},{y}");

        cells[x, y] = value;
    }

    public bool IsInBounds(int x, int y)
    {
        return x >= 0 &&
               y >= 0 &&
               x < Width &&
               y < Height;
    }
}