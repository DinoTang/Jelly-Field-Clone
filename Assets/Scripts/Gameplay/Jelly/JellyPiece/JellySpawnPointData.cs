using UnityEngine;

[System.Serializable]
public class JellySpawnPointData
{
    [SerializeField] private Vector3 position;

    public Vector3 Position => position;
}