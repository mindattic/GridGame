using Assets.Scripts.Models;
using Game.Behaviors;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DataCollection<T>
{
    public List<T> Items;
}

[System.Serializable]
public class ActorEntity
{
    public string Name;
    public string Description;
    //public Rarity Rarity; 

    public ActorStats Stats;
    public ThumbnailSettings ThumbnailSettings;
    public ActorDetails Details;
}

[System.Serializable]
public class VisualEffectEntity
{
    public string Name;
    public string RelativeOffset;
    public string AngularRotation;
    public string RelativeScale;
    public float Delay;
    public float Duration;
    public bool IsLoop;
}

public class DataManager : MonoBehaviour
{
    protected LogManager logManager => GameManager.instance.logManager;


    public static class Entities
    {
        public static List<ActorEntity> Actors = new List<ActorEntity>();
        public static List<VisualEffectEntity> VisualEffects = new List<VisualEffectEntity>();

    }

    public static class Resource
    {
        public static string Actors = "Actors";
        public static string VisualEffects = "VisualEffects";

    }

    public void Awake()
    {
    }



    public void Initialize()
    {
        Entities.Actors = ParseJson<ActorEntity>(Resource.Actors);
        Entities.VisualEffects = ParseJson<VisualEffectEntity>(Resource.VisualEffects);
    }


    public List<T> ParseJson<T>(string resource)
    {
        string filePath = $"Data/{resource}";
        TextAsset jsonFile = Resources.Load<TextAsset>(filePath);

        if (jsonFile == null)
        {
            logManager.Error($"File {filePath} not found in Resources.");
            return null;
        }

        var collection = JsonUtility.FromJson<DataCollection<T>>(jsonFile.text);
        return collection.Items;
    }


    public ActorStats GetStats(string name)
    {
        var data = Entities.Actors.Where(x => x.Name == name).FirstOrDefault().Stats;
        if (data == null)
            logManager.Error($"Unable to retrieve actor stats for `{name}`");
        return data;
    }


    public ThumbnailSettings GetThumbnailSetting(string name)
    {
        var data = Entities.Actors.Where(x => x.Name == name).FirstOrDefault().ThumbnailSettings;
        if (data == null)
            logManager.Error($"Unable to retrieve thumnail settings for `{name}`");
        return data;
    }

    public ActorDetails GetDetails(string name)
    {
        var data = Entities.Actors.Where(x => x.Name == name).FirstOrDefault().Details;
        if (data == null)
            logManager.Error($"Unable to retrieve actor details for `{name}`");
        return data;
    }

    public VisualEffectEntity GetVisualEffect(string name)
    {
        var data = Entities.VisualEffects.Where(x => x.Name == name).FirstOrDefault();
        if (data == null)
            logManager.Error($"Unable to retrieve visual effect for `{name}`");
        return data;
    }



}
