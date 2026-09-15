using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDataSO", menuName = "SO/Audio/AudioDataSO")]
public class AudioDataSO : ScriptableObject
{
    [Header("SFX")]
    public AudioClip click;
    public AudioClip coin;
    public AudioClip goalReached;
    public AudioClip initJelly;
    public AudioClip merge;
    public AudioClip touch;
}