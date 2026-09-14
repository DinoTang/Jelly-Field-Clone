using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JellyLevel", menuName = "SO/JellyLevel")]
public class JellyLevelSO : ScriptableObject
{
    [SerializeField] private int width = 5;
    [SerializeField] private int height = 3;
    [SerializeField] private bool[] validCells;
    [SerializeField] private List<JellySpawnPointData> spawnPoints = new();
    [SerializeField] private List<JellyCellData> jellyCells = new();

    public int Width => width;
    public int Height => height;
    public List<JellySpawnPointData> SpawnPoints => spawnPoints;

    public List<JellyCellData> JellyCells => jellyCells;

    public bool IsValid(int x, int y)
    {
        if (validCells == null)
            return false;

        if (x < 0 || x >= width || y < 0 || y >= height)
            return false;

        int index = y * width + x;

        return index < validCells.Length && validCells[index];
    }

    public bool[] GetValidCells()
    {
        return this.validCells;
    }

    public void SetSize(int newWidth, int newHeight)
    {
        newWidth = Mathf.Max(1, newWidth);
        newHeight = Mathf.Max(1, newHeight);

        if (newWidth == width && newHeight == height)
            return;

        width = newWidth;
        height = newHeight;

        validCells = new bool[width * height];

        for (int i = 0; i < validCells.Length; i++)
            validCells[i] = true;
    }
}