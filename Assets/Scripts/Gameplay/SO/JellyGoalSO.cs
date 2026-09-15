using UnityEngine;

[CreateAssetMenu(fileName = "JellyGoal", menuName = "SO/JellyGoal")]
public class JellyGoalSO : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private JellyColor jellyColor;

    public Sprite Icon => icon;
    public JellyColor JellyColor => jellyColor;
}