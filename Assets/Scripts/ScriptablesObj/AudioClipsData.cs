using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "clipsData",menuName = "Data/AudioData/ClipsData",order = 1)]
public class AudioClipsData : ScriptableObject
{
    [SerializeField] private List<ClipData> clipsData;
    public List<ClipData> ClipsData => clipsData;
}

[System.Serializable]
public class ClipData
{
    [SerializeField] public GameDefinitions.SFXClip clipDefinition;
    [SerializeField] public AudioClip clip;
}
