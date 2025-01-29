using Assets.Scripts.Models;
using Game.Behaviors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class JsonData<T>
{
    public List<T> Items;
}


[Serializable]
public class ActorEntity
{
    public string Character;
    public string Description;
    //public Rarity Rarity; 

    public ActorStats Stats;
    public ThumbnailSettings ThumbnailSettings;
    public ActorDetails Details;
}


[Serializable]
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


[Serializable]
public enum StageCompletionCondition
{
    DefeatAllEnemies,
    CollectCoins,
    SurviveTurns
}

[Serializable]
public class StageEntity
{
    public string Name;
    public string Description;
    public string CompletionCondition;
    public int CompletionValue;
    public List<StageActor> Actors;
    public List<StageDottedLine> DottedLines;
}

[Serializable]
public class StageActor
{
    public string Character;
    public string Team;
    public string Location;
}

[Serializable]
public class StageDottedLine
{
    public string Segment;
    public string Location;
}


public class DataManager : MonoBehaviour
{
    protected LogManager logManager => GameManager.instance.logManager;

    public static class Resource
    {
        public static string Actors = "Actors";
        public static string VisualEffects = "VisualEffects";
        public static string Stages = "Stages";
    }

    public List<ActorEntity> Actors = new List<ActorEntity>();
    public List<VisualEffectEntity> VisualEffects = new List<VisualEffectEntity>();
    public List<StageEntity> Stages = new List<StageEntity>();

  

    public List<T> ParseJson<T>(string resource)
    {
        string filePath = $"Data/{resource}";

        Debug.Log(filePath);
        TextAsset jsonFile = Resources.Load<TextAsset>(filePath);

        if (jsonFile == null)
        {
            logManager.Error($"File {filePath} not found in Resources.");
            return null;
        }

        var collection = JsonConvert.DeserializeObject<JsonData<T>>(jsonFile.text);
        return collection.Items;
    }

    public void Initialize()
    {
        Actors = ParseJson<ActorEntity>(Resource.Actors);
        VisualEffects = ParseJson<VisualEffectEntity>(Resource.VisualEffects);
        Stages = ParseJson<StageEntity>(Resource.Stages);
    }

    public ActorStats GetStats(Character character)
    {
        var data = Actors.Where(x => x.Character == character.ToString()).FirstOrDefault().Stats;
        if (data == null)
            logManager.Error($"Unable to retrieve actor stats for `{character}`");
        return data;
    }


    public ThumbnailSettings GetThumbnailSetting(Character character)
    {
        var data = Actors.Where(x => x.Character == character.ToString()).FirstOrDefault().ThumbnailSettings;
        if (data == null)
            logManager.Error($"Unable to retrieve thumnail settings for `{name}`");
        return data;
    }

    public ActorDetails GetDetails(Character character)
    {
        var data = Actors.Where(x => x.Character == character.ToString()).FirstOrDefault().Details;
        if (data == null)
            logManager.Error($"Unable to retrieve actor details for `{character}`");
        return data;
    }

    public VisualEffectEntity GetVisualEffect(string name)
    {
        var data = VisualEffects.Where(x => x.Name == name).FirstOrDefault();
        if (data == null)
            logManager.Error($"Unable to retrieve visual effect for `{name}`");
        return data;
    }

    public StageEntity GetStage(string name)
    {
        var data = Stages.Where(x => x.Name == name).FirstOrDefault();
        if (data == null)
            logManager.Error($"Unable to retrieve stage for `{name}`");
        return data;
    }

}
