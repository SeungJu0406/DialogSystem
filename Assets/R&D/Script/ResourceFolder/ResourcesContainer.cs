using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utility;


public class ResourcesContainer : SingleTon<ResourcesContainer>
{
    // ¶óº§
    private Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();

    private List<Sprite> _loaded = new List<Sprite>();

    [SerializeField] private string _spriteLabel = "Sprites";

    public bool IsLoaded;
    protected override void InitAwake()
    {
        LoadResources();
    }

    public static Sprite GetSprite(string name)
    {
        if (Instance._sprites.TryGetValue(name, out var sprite))
        {
            return sprite;
        }
        else
        {
            Debug.LogWarning($"name '{name}' not found.");
        }
        return null;
    }

    private void LoadResources()
    {
        var handle = Addressables.LoadAssetsAsync<Sprite>(_spriteLabel, sprite => { _loaded.Add(sprite);  });

        handle.Completed += OnLoadCompleted;
        
    }

    private void OnLoadCompleted(AsyncOperationHandle<IList<Sprite>> handle)
    {
        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach(var sprite in _loaded)
            {
                _sprites.Add(sprite.name,sprite);
            }
            IsLoaded = true;
        }
        else
        {
            Debug.LogError("Failed to load sprites.");
        }
    }


}