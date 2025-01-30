using Assets.Scripts.Models;
using Game.Behaviors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
public class ActorData
{

    public string Character;
    public string Description;
    //public Rarity Rarity; 

    public ActorStats Stats;
    public ThumbnailSettings ThumbnailSettings;
    public ActorDetails Details;
}


[Serializable]
public class VisualEffectData
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
public class StageData
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

//public class StageActorConverter : JsonConverter
//{
//    public override bool CanConvert(Type objectType)
//    {
//        return objectType == typeof(StageActor);
//    }

//    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//    {
//        JObject obj = JObject.Load(reader);

//        Character character = Enum.TryParse<Character>(obj["Character"]?.ToString(), out character) ? character : Character.Unknown;
//        Team team = Enum.TryParse<Team>(obj["Team"]?.ToString(), out team) ? team : Team.Neutral;
//        Vector2Int location
//            = !string.IsNullOrWhiteSpace(obj["Location"]?.ToString())
//            ? obj["Location"].ToString().ToVector2Int()
//            : Random.UnoccupiedLocation;

//        return new StageActor
//        {
//            Character = character, 
//            Team = team,         
//            Location = location
//        };
//    }

//    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//    {
//        var stageActor = (StageActor)value;

//        JObject obj = new JObject
//        {
//            ["Character"] = stageActor.Character.ToString(),
//            ["Team"] = stageActor.Team.ToString(),
//            ["Location"] = stageActor.Location.HasValue ? $"({stageActor.Location.Value.x}, {stageActor.Location.Value.y})" : null
//        };

//        obj.WriteTo(writer);
//    }
//}

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

    public List<ActorData> Actors = new List<ActorData>();
    public List<VisualEffectData> VisualEffects = new List<VisualEffectData>();
    public List<StageData> Stages = new List<StageData>();

  

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

        //Convert Location values from string (1, 0.5) to Vector2Int...
        //Convert Character values from string to Character Enum here...


        return collection.Items;
    }

    public void Initialize()
    {
        Actors = ParseJson<ActorData>(Resource.Actors);
        VisualEffects = ParseJson<VisualEffectData>(Resource.VisualEffects);
        Stages = ParseJson<StageData>(Resource.Stages);
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

    public VisualEffectData GetVisualEffect(string name)
    {
        var data = VisualEffects.Where(x => x.Name == name).FirstOrDefault();
        if (data == null)
            logManager.Error($"Unable to retrieve visual effect for `{name}`");
        return data;
    }

    public StageData GetStage(string name)
    {
        var data = Stages.Where(x => x.Name == name).FirstOrDefault();
        if (data == null)
            logManager.Error($"Unable to retrieve stage for `{name}`");
        return data;
    }

}
