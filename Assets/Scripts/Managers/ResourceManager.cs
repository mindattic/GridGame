using Assets.Scripts.Models;
using Game.Behaviors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class ResourceParameter
{
    public string Key;   // The parameter name (e.g., "Role", "Description")
    public string Value; // The parameter entry (e.g., "Tank", "A brave warrior")
}

[System.Serializable]
public class ResourceItem<T>
{
    public T Value;                     // The resource itself (e.g., Sprite, AudioClip)
    public List<ResourceParameter> Parameters; // Parameters loaded from the JSON file
}

public static class ResourceFolder
{
    public static string Backgrounds = "Backgrounds";
    public static string Portraits = "Portraits";
    public static string SoundEffects = "SoundEffects";
    public static string Materials = "Materials";
    public static string Seamless = "Seamless";
    public static string Sprites = "Sprites";
}

public class ResourceManager : MonoBehaviour
{
    #region Properties
    protected LogManager logManager => GameManager.instance.logManager;
    #endregion


    public Dictionary<string, ResourceItem<Sprite>> backgrounds = new Dictionary<string, ResourceItem<Sprite>>();
    public Dictionary<string, ResourceItem<Texture2D>> portraits = new Dictionary<string, ResourceItem<Texture2D>>();
    public Dictionary<string, ResourceItem<AudioClip>> soundEffects = new Dictionary<string, ResourceItem<AudioClip>>();
    public Dictionary<string, ResourceItem<Material>> materials = new Dictionary<string, ResourceItem<Material>>();
    public Dictionary<string, ResourceItem<Sprite>> seamless = new Dictionary<string, ResourceItem<Sprite>>();
    public Dictionary<string, ResourceItem<Sprite>> sprites = new Dictionary<string, ResourceItem<Sprite>>();


    public void Awake()
    {
        List<string> keys = new List<string>();

        keys.SetRange("CandleLitPath");
        backgrounds = LoadResources<Sprite>(ResourceFolder.Backgrounds, keys);

        keys.SetRange("Barbarian", "Bat", "Cleric", "Ninja", "Paladin", "PandaGirl",
                      "Scorpion", "Sentinel", "Slime", "Yeti");
        portraits = LoadResources<Texture2D>(ResourceFolder.Portraits, keys);

        keys.SetRange("Death", "Move1", "Move2", "Move3", "Move4", "Move5", "Move6", "NextTurn",
                      "PlayerGlow", "Portrait", "Rumble", "Select", "Slash1", "Slash2", "Slash3",
                      "Slash4", "Slash5", "Slash6", "Slash7", "Slide");
        soundEffects = LoadResources<AudioClip>(ResourceFolder.SoundEffects, keys);

        keys.SetRange("EnemyParallax", "PlayerParallax");
        materials = LoadResources<Material>(ResourceFolder.Materials, keys);

        keys.SetRange("BlackFire1", "BlackFire2", "Fire1", "RedFire1", "Swords1", "Swords2",
                      "WhiteFire1", "WhiteFire2");
        seamless = LoadResources<Sprite>(ResourceFolder.Seamless, keys);

        keys.SetRange("DottedLine", "DottedLineArrow", "DottedLineTurn", "Footstep", "Pause", 
                      "Paused");
        sprites = LoadResources<Sprite>(ResourceFolder.Sprites, keys);


    }


    public ResourceItem<Sprite> Background(string key)
    {
        if (backgrounds.TryGetValue(key, out var entry))
            return entry;
        else
            logManager.Error($"Failed to retrieve background `{key}` from resource manager.");

        return null;
    }

    public ResourceItem<Texture2D> Portrait(string key)
    {
        if (portraits.TryGetValue(key, out var entry))
            return entry;
        else
            logManager.Error($"Failed to retrieve portrait texture2D `{key}` from resource manager.");

        return null;
    }

    public ResourceItem<AudioClip> SoundEffect(string key)
    {
        if (soundEffects.TryGetValue(key, out var entry))
            return entry;
        else
            logManager.Error($"Failed to retrieve sound effect `{key}` from resource manager.");

        return null;
    }

    public ResourceItem<Material> Material(string key, Texture2D texture = null)
    {
        if (materials.TryGetValue(key, out var entry))
        {
            entry.Value.mainTexture = texture;
            return entry;
        }
        else
            logManager.Error($"Failed to retrieve material `{key}` from resource manager.");

        return null;
    }

    public ResourceItem<Sprite> Seamless(string key)
    {
        if (seamless.TryGetValue(key, out var entry))
            return entry;
        else
            logManager.Error($"Failed to retrieve seamless sprite `{key}` from resource manager.");

        return null;
    }

    public ResourceItem<Sprite> Sprite(string key)
    {
        if (sprites.TryGetValue(key, out var entry))
            return entry;
        else
            logManager.Error($"Failed to retrieve sprite `{key}` from resource manager.");

        return null;
    }

    /// <summary>
    /// Load all resources from a folder and their matching JSON parameters.
    /// </summary>
    //public List<ResourceItem<Sprite>> LoadAllFromFolder(string resourcePath)
    //{
    //    Sprite[] sprites = Resources.LoadAll<Sprite>(resourcePath);
    //    List<ResourceItem<Sprite>> entries = new List<ResourceItem<Sprite>>();

    //    foreach (var sprite in sprites)
    //    {
    //        // Load matching JSON file for parameters
    //        List<ResourceParameter> parameters = LoadParameters(resourcePath, sprite.name);

    //        entries.Add(new ResourceItem<Sprite>
    //        {
    //            Key = sprite.name,
    //            Value = sprite,
    //            Parameters = parameters
    //        });
    //    }

    //    return entries;
    //}

    /// <summary>
    /// Load specific resources by their IDs and matching JSON parameters.
    /// </summary>
    /// 


    public T Load<T>(string resourcePath) where T : UnityEngine.Object
    {
        T resource = Resources.Load<T>(resourcePath);
        if (resource == null)
            logManager.Error($"Failed to load external resource from `{resourcePath}`");

        return resource;
    }


    public Dictionary<string, ResourceItem<T>> LoadResources<T>(string resourcePath, List<string> keys) where T : UnityEngine.Object
    {
        Dictionary<string, ResourceItem<T>> entries = new Dictionary<string, ResourceItem<T>>();

        try
        {
            foreach (var key in keys)
            {
                // Load the sprite
                T value = Resources.Load<T>($"{resourcePath}/{key}");
                if (value == null)
                {
                    logManager.Warning($"Resource `{key}` was not found in folder {resourcePath}");
                    continue;
                }

                // Load the matching JSON file for parameters
                List<ResourceParameter> parameters = LoadParameters(resourcePath, key);

                entries.Add(key, new ResourceItem<T>
                {
                    Value = value,
                    Parameters = parameters
                });
            }
        }
        catch (Exception ex)
        {
            logManager.Error(ex.Message);
        }

        return entries;
    }

    /// <summary>
    /// Load parameters from a JSON file matching the resource name.
    /// </summary>
    private List<ResourceParameter> LoadParameters(string folderPath, string resourceName)
    {
        string jsonPath = Path.Combine(Application.dataPath, "Resources", folderPath, $"{resourceName}.json");

        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            return JsonUtility.FromJson<ParameterList>(json).Parameters;
        }

        //Debug.LogWarning($"Parameters file not found for resource {resourceName} at {jsonPath}");
        return new List<ResourceParameter>();
    }

    // Helper class for deserializing parameter lists
    [System.Serializable]
    private class ParameterList
    {
        public List<ResourceParameter> Parameters = new List<ResourceParameter>();
    }
































    //Variables
    //[SerializeField] public List<MaterialResource> materials = new List<MaterialResource>();
    [SerializeField] public List<ActorDetails> actorDetails = new List<ActorDetails>();
    //[SerializeField] public List<StatusSprite> statusSprites = new List<StatusSprite>();
    //[SerializeField] public List<PropSprite> propSprites = new List<PropSprite>();
    //[SerializeField] public List<SeamlessSprite> seamLessSprites = new List<SeamlessSprite>();
    //[SerializeField] public List<SoundEffect> soundEffects = new List<SoundEffect>();
    [SerializeField] public List<MusicTrack> musicTracks = new List<MusicTrack>();
    [SerializeField] public List<VisualEffect> visualEffects = new List<VisualEffect>();
    [SerializeField] public List<WeaponTypeResource> weaponTypes = new List<WeaponTypeResource>();
    //[SerializeField] public List<SpriteResource> sprites = new List<SpriteResource>();
    //[SerializeField] public List<DottedLineSegmentResource> dottedLines = new List<DottedLineSegmentResource>();

    //[SerializeField] public List<PrefabResource> prefabs = new List<PrefabResource>();
    //[SerializeField] public List<ShaderResource> shaders = new List<ShaderResource>();


    //public ActorSprite ActorSprite(string key)
    //{
    //    try
    //    {
    //        return actorSprites.First(x => x.key.Equals(key));
    //    }
    //    catch (Exception ex)
    //    {
    //        logManager.Error($"Failed to retrieve actor sprite `{key}` from resource manager. | Error: {ex.Message}");
    //    }

    //    return null;
    //}

    //public Material Material(string key, Texture2D texture = null)
    //{
    //    try
    //    {
    //        var material = materials.First(x => x.key.Equals(key)).material;
    //        if (texture != null)
    //            material.mainTexture = texture;
    //        return material;
    //    }
    //    catch (Exception ex)
    //    {
    //        logManager.Error($"Failed to retrieve actor material `{key}` from resource manager. | Error: {ex.Message}");
    //    }

    //    return null;
    //}


    public string ActorDetails(string id)
    {
        try
        {
            var details = actorDetails.FirstOrDefault(x => x.id.Equals(id))?.details ?? null;
            if (details != null)
                return details;
        }
        catch (Exception ex)
        {
            logManager.Error($"Failed to retrieve actor details `{id}` from resource manager. | Error: {ex.Message}");
        }

        return null;
    }

    //public Sprite Prop(string id)
    //{
    //    try
    //    {
    //        return propSprites.First(x => x.id.Equals(id)).sprite;
    //    }
    //    catch (Exception ex)
    //    {
    //        logManager.Error($"Failed to retrieve prop sprite `{id}` from resource manager. | Error: {ex.Message}");
    //    }

    //    return null;
    //}


    //public Sprite Seamless(string id)
    //{
    //    try
    //    {
    //        return seamLessSprites.First(x => x.id.Equals(id)).sprite;
    //    }
    //    catch (Exception ex)
    //    {
    //        logManager.Error($"Failed to retrieve seamless sprite `{id}` from resource manager. | Error: {ex.Message}");
    //    }

    //    return null;
    //}


    //public Sprite StatusThumbnail(string id)
    //{
    //    try
    //    {
    //        return statusSprites.First(x => x.id.Equals(id)).thumbnail;
    //    }
    //    catch (Exception ex)
    //    {
    //        logManager.Error($"Failed to retrieve status thumbnail `{id}` from resource manager. | Error: {ex.Message}");
    //    }

    //    return null;
    //}


    //public AudioClip SoundEffect(string key)
    //{
    //    try
    //    {
    //        return soundEffects[key].Value;
    //    }
    //    catch (Exception ex)
    //    {
    //        logManager.Error($"Failed to retrieve sound effect `{key}` from resource manager. | Error: {ex.Message}");
    //    }

    //    return null;
    //}


    public AudioClip MusicTrack(string id)
    {
        try
        {
            return musicTracks.First(x => x.id.Equals(id)).audio;
        }
        catch (Exception ex)
        {
            logManager.Error($"Failed to retrieve music track `{id}` from resource manager. | Error: {ex.Message}");
        }

        return null;
    }


    public VisualEffect VisualEffect(string id)
    {
        try
        {
            return visualEffects.First(x => x.id.Equals(id));
        }
        catch (Exception ex)
        {
            logManager.Error($"Failed to retrieve visual effect `{id}` from resource manager. | Error: {ex.Message}");
        }

        return null;
    }

    public WeaponTypeResource WeaponType(WeaponType type)
    {
        try
        {
            return weaponTypes.First(x => x.Type.Equals(type));
        }
        catch (Exception ex)
        {
            logManager.Error($"Failed to retrieve weapon type resource `{type}` from resource manager. | Error: {ex.Message}");
        }

        return null;
    }


    //public SpriteResource Sprite(string id)
    //{
    //    try
    //    {
    //        return sprites.First(x => x.id.Equals(id));
    //    }
    //    catch (Exception ex)
    //    {
    //        logManager.Error($"Failed to retrieve sprite `{id}` from resource manager. | Error: {ex.Message}");
    //    }

    //    return null;
    //}

    //public DottedLineSegmentResource DottedLine(string id)
    //{
    //    try
    //    {
    //        return dottedLines.First(x => x.id.Equals(id));
    //    }
    //    catch (Exception ex)
    //    {
    //        logManager.Error($"Failed to retrieve dotted line `{id}` from resource manager. | Error: {ex.Message}");
    //    }

    //    return null;
    //}

    //public PrefabResource Prefab(string key)
    //{
    //    try
    //    {
    //        return prefabs.First(x => x.key.Equals(key));
    //    }
    //    catch (Exception ex)
    //    {
    //        logManager.Error($"Failed to retrieve prefab `{key}` from resource manager. | Error: {ex.Message}");
    //    }

    //    return null;
    //}


    //public ShaderResource Shader(string key)
    //{
    //    try
    //    {
    //        return shaders.First(x => x.key.Equals(key));
    //    }
    //    catch (Exception ex)
    //    {
    //        logManager.Error($"Failed to retrieve shader `{key}` from resource manager. | Error: {ex.Message}");
    //    }

    //    return null;
    //}

}
