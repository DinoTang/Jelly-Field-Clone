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
    [SerializeField] private List<JellyGoalData> goals = new();
    [SerializeField] private int coinReward = 0;
    public int Width => width;
    public int Height => height;
    public List<JellySpawnPointData> SpawnPoints => spawnPoints;
    public List<JellyCellData> JellyCells => jellyCells;
    public List<JellyGoalData> Goals => goals;
    public int CoinReward => this.coinReward;

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

        bool[] oldValidCells = this.validCells;
        int oldWidth = width;
        int oldHeight = height;

        width = newWidth;
        height = newHeight;

        validCells = new bool[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int newIndex = y * width + x;

                if (x < oldWidth && y < oldHeight && oldValidCells != null)
                {
                    int oldIndex = y * oldWidth + x;
                    validCells[newIndex] = oldValidCells[oldIndex];
                }
                else
                {
                    validCells[newIndex] = true;
                }
            }
        }
    }
}