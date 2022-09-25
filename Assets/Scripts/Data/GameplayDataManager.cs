using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public delegate void ClipsDataLoaded(AudioClipsData audioClipsData);
public class GameplayDataManager : MonoBehaviour
{
    [SerializeField] private AssetReference clipsAssetReference;

    private AsyncOperationHandle<AudioClipsData> _clipsOperation;
    public event ClipsDataLoaded OnClipsDataLoaded;
    
    private void Start()
    {
        _clipsOperation = clipsAssetReference.LoadAssetAsync<AudioClipsData>();
        _clipsOperation.Completed += ClipsOperationOnCompleted;
    }

    private void ClipsOperationOnCompleted(AsyncOperationHandle<AudioClipsData> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
            OnClipsDataLoaded?.Invoke(obj.Result);
    }

    private void OnDestroy()
    {
        if(_clipsOperation.IsValid())
            Addressables.Release(_clipsOperation);
    }
}
