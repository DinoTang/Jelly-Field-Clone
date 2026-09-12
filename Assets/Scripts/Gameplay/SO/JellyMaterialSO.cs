using UnityEngine;

[CreateAssetMenu(fileName = "JellyMaterialSO", menuName = "SO/JellyMaterialSO")]
public class JellyMaterialSO : ScriptableObject
{
    [SerializeField] private Material cyan;
    [SerializeField] private Material green;
    [SerializeField] private Material purple;
    [SerializeField] private Material pink;
    [SerializeField] private Material yellow;

    public Material GetMaterial(JellyColor color)
    {
        return color switch
        {
            JellyColor.Pink => pink,
            JellyColor.Purple => purple,
            JellyColor.Cyan => cyan,
            JellyColor.Yellow => yellow,
            JellyColor.Green => green,
            _ => null
        };
    }
}