using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JellyLevelList", menuName = "SO/JellyLevelList")]
public class JellyLevelListSO : ScriptableObject
{
    [SerializeField] private List<JellyLevelSO> levels = new();

    public List<JellyLevelSO> Levels => this.levels;

    public int LevelCount => this.levels.Count;

    public JellyLevelSO GetLevel(int index)
    {
        if (index < 0 || index >= this.levels.Count)
            return null;

        return this.levels[index];
    }
}