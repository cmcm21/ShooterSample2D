using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class AudioManager
{
    private static List<GOAudioSource> _goAudioSources;
    private static AudioClipsData _audioClipsData;
    private static Dictionary<GameDefinitions.SFXClip, AudioClip> _audioClipsDictionary;

    public static void Initialize(AudioClipsData audioClipsData)
    {
        _goAudioSources = new List<GOAudioSource> { CreateAudioSource() };
        _audioClipsDictionary = new Dictionary<GameDefinitions.SFXClip, AudioClip>();
        _audioClipsData = audioClipsData;
        MapDictionaryWithAudioData();
    }

    private static void MapDictionaryWithAudioData()
    {
        foreach (var clipData in _audioClipsData.ClipsData)
            _audioClipsDictionary[clipData.clipDefinition] = clipData.clip;
    }

    private static GOAudioSource CreateAudioSource()
    {
        var gameObject = new GameObject();
        gameObject.name = "audioObject";
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        return new GOAudioSource(audioSource, gameObject);
    }

    private static GOAudioSource CanPlay()
    {
        foreach(var goAudioSource in _goAudioSources)
            if (!goAudioSource.AudioSource.isPlaying)
                return goAudioSource;
        return null;
    }

    public static void PlayPositionalAudio(GameDefinitions.SFXClip clip, Vector3 position)
    {
        var audioSource = GetOrCreateGoAudioSource();
        if(_audioClipsDictionary.TryGetValue(clip,out var audioClip)) 
            PlayPositionalAudio(audioSource, audioClip, position);
    }

    private static GOAudioSource GetOrCreateGoAudioSource()
    {
        var audioSource = CanPlay();
        if (audioSource == null)
        {
            audioSource = CreateAudioSource();
            _goAudioSources.Add(audioSource);
        }

        return audioSource;
    }

    private static void PlayPositionalAudio(GOAudioSource audioSource, AudioClip clip, Vector3 position)
    {
        audioSource.GameObject.transform.position = position;
        audioSource.AudioSource.clip = clip;
        audioSource.AudioSource.Play();
    }

    public static void Play2DAudio(GameDefinitions.SFXClip clip)
    {
        var audioSource = GetOrCreateGoAudioSource();
        if(_audioClipsDictionary.TryGetValue(clip,out var audioClip))
            Play2DAudio(audioSource,audioClip);
    }

    private static void Play2DAudio(GOAudioSource audioSource, AudioClip clip)
    {
        audioSource.AudioSource.PlayOneShot(clip);
    }
}
public class GOAudioSource
{
    public GOAudioSource(AudioSource audioSource, GameObject gameObject)
    {
        this.AudioSource = audioSource;
        this.GameObject = gameObject;
    }
    public AudioSource AudioSource;
    public GameObject GameObject;
}