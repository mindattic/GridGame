
using Assets.Scripts.Entities;
using Assets.Scripts.Models;
using Game.Behaviors;
using SQLiteDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEditor.Playables;
using UnityEngine;


namespace Game.Manager
{
    public class DatabaseManager : MonoBehaviour
    {
        public static class Schema
        {
            public const string Name = "MyDatabase.db";

            public static class Table
            {

                public static string Actor = "Actor";
            }
        }

        public static class Queries
        {
            public static string ActorEntity 
                = "SELECT                                             "
                + "  a.Id,                                            "
                + "  a.Name,                                          "
                + "  a.Variant,                                       "
                + "  a.Description,                                   "
                + "  a.Level,                                         "
                + "  a.MaxHp,                                         "
                + "  a.Strength,                                      "
                + "  a.Vitality,                                      "
                + "  a.Agility,                                       "
                + "  a.Speed,                                         "
                + "  a.Luck,                                          "
                + "  t.Width as 'ThumbnailWidth',                     "
                + "  t.Width as 'ThumbnailHeight',                    "
                + "	 t.X as 'ThumbnailX',                             "
                + "  t.Y as 'ThumbnailY'                              "
                + "FROM Actor a                                       "
                + "INNER JOIN ActorThumbnail at ON(a.Id = at.ActorId) "
                + "INNER JOIN Thumbnail t ON(t.Id = at.ThumbnailId)   ";
        }


        #region Properties
        protected LogManager logManager => GameManager.instance.logManager;
        #endregion

        //Variables
        public const bool autoOverwrite = true; //Used to reinstall app every load...
        private SQLiteDB instance = SQLiteDB.Instance;
        public List<ActorEntity> actorEntities = new List<ActorEntity>();


        void OnEnable()
        {
            SQLiteEventListener.onError += OnError;
        }

        void OnDisable()
        {
            SQLiteEventListener.onError -= OnError;
        }

        void OnError(string err)
        {
            logManager.Error(err);
        }

        void OnApplicationQuit()
        {
            instance.Dispose();
        }

        //Method which is used for initialization tasks that need to occur before the game starts 
        private void Awake()
        {
            instance.DBLocation = Application.persistentDataPath;
            instance.DBName = Schema.Name;

            //Update if this is the first load of the application
            if (autoOverwrite || !instance.Exists)
                instance.CreateDatabase(instance.DBName, isOverWrite: true);

            var isConnected = instance.ConnectToDefaultDatabase(instance.DBName, loadFresh: true);

            if (!isConnected)
                throw new UnityException($"Failed to connect to database: {instance.DBName}");

            Load(); //TODO: Load data based on current stage???...
        }


        //TODO:
        //Come up with a way to retrieve records piecemeal so
        //that entire database tables don't have to be
        //downloaded for a small subset of data, e.g:
        //Stage 05: ["Slime", "Scorpion", "Bat", "Yeti"]


        void Load()
        {
            DBReader reader;

            actorEntities.Clear();
            //reader = instance.GetAllData(Schema.Table.Actor);
            reader = instance.Select(Queries.ActorEntity);
            while (reader != null && reader.Read())
            {
                var x = new ActorEntity()
                {
                    Id = reader.GetIntValue("Id"),
                    Name = reader.GetStringValue("Name"),
                    Variant = reader.GetStringValue("Variant"),
                    Description = reader.GetStringValue("Description"),
                    Level = reader.GetFloatValue("Level"),
                    HP = reader.GetFloatValue("MaxHp"),
                    MaxHP = reader.GetFloatValue("MaxHp"),
                    Strength = reader.GetFloatValue("Strength"),
                    Vitality = reader.GetFloatValue("Vitality"),
                    Agility = reader.GetFloatValue("Agility"),
                    Speed = reader.GetFloatValue("Speed"),
                    Luck = reader.GetFloatValue("Luck"),

                    ThumbnailWidth = reader.GetIntValue("ThumbnailWidth"),
                    ThumbnailHeight = reader.GetIntValue("ThumbnailHeight"),
                    ThumbnailX = reader.GetIntValue("ThumbnailX"),
                    ThumbnailY = reader.GetIntValue("ThumbnailY"),
                };
                actorEntities.Add(x);
                logManager.Info(JsonUtility.ToJson(x));


            }
        }


        public ActorStats GetActorStats(string name)
        {
            //Retrieve random varient of actor with name
            var entity = actorEntities.Where(x => x.Name == name).OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            var stats = new ActorStats()
            {
                Id = entity.Id,
                Name = entity.Name,
                Variant = entity.Variant,
                Description = entity.Description,
                Level = entity.Level,
                PreviousHP = entity.PreviousHP,
                HP = entity.HP,
                MaxHP = entity.MaxHP,
                PreviousAP = entity.PreviousAP,
                AP = entity.AP,
                MaxAP = entity.MaxAP,
                Strength = entity.Strength,
                Vitality = entity.Vitality,
                Agility = entity.Agility,
                Speed = entity.Speed,
                Luck = entity.Luck
            };

            return stats;
        }


        public ThumbnailSize GetThumbnailSize(string name)
        {
            var entity = actorEntities.Where(x => x.Name == name).OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            var thumbnailSize = new ThumbnailSize()
            {
                Width = entity.ThumbnailWidth,
                Height = entity.ThumbnailHeight,
                X = entity.ThumbnailX,
                Y = entity.ThumbnailY
            };

            return thumbnailSize;
        }

    }
}