
using Assets.Scripts.Entities;
using Assets.Scripts.Models;
using Game.Behaviors;
using SQLiteDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.PackageManager;
using UnityEditor.Playables;
using UnityEngine;


namespace Game.Manager
{
    public class DatabaseManager : MonoBehaviour
    {
        public static class Schema
        {
            public const string DBName = "MyDatabase.db";

            public static class Table
            {

                public static string Actor = "Actor";
            }
        }

        public static class Queries
        {
            public static class Select
            {
                public static class Actor
                {
                    public static string Entities = "SELECT a.Name, a.Description, s.Level, s.MaxHp, s.Strength, s.Vitality, s.Agility, s.Speed, s.Luck, t.Width AS ThumbnailWidth, t.Width AS ThumbnailHeight, t.X AS ThumbnailX, t.Y AS ThumbnailY FROM Actor a INNER JOIN ActorStats ON (a.id = ActorStats.ActorId) INNER JOIN Stats s ON (ActorStats.StatsId = s.Id) INNER JOIN ActorThumbnail ON (a.Id = ActorThumbnail.ActorId) INNER JOIN Thumbnail t ON (t.Id = ActorThumbnail.ThumbnailId);";
                
                
                
                }
            }
        }

        public static class Entities
        {
            public static List<ActorEntity> Actors = new List<ActorEntity>();
        }


        #region Properties
        protected LogManager logManager => GameManager.instance.logManager;
        #endregion

        //Variables
        public const bool autoOverwrite = true; //Used to reinstall app every load...
        private SQLiteDB instance = SQLiteDB.Instance;
  

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
            instance.DBName = Schema.DBName;

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

            Entities.Actors.Clear();

            //reader = instance.GetAllData(Schema.Table.Actor);
            reader = instance.ExecuteReader(Queries.Select.Actor.Entities);
            while (reader != null && reader.Read())
            {
                var x = new ActorEntity()
                {
                    Name = reader.GetStringValue("Name"),
                    Description = reader.GetStringValue("Description"),
                };

                x.stats = new ActorStats()
                {
                    HP = reader.GetFloatValue("MaxHp"),
                    MaxHP = reader.GetFloatValue("MaxHp"),
                    Strength = reader.GetFloatValue("Strength"),
                    Vitality = reader.GetFloatValue("Vitality"),
                    Agility = reader.GetFloatValue("Agility"),
                    Speed = reader.GetFloatValue("Speed"),
                    Luck = reader.GetFloatValue("Luck"),
                };

                x.thumbnail = new ThumbnailSize()
                {
                    Width = reader.GetIntValue("ThumbnailWidth"),
                    Height = reader.GetIntValue("ThumbnailHeight"),
                    X = reader.GetIntValue("ThumbnailX"),
                    Y = reader.GetIntValue("ThumbnailY"),
                };

                //x.Rarity = new Rarity()
                //{
                //    Name = reader.GetStringValue("RarityName"),
                //    Color = ColorHelper.Solid.White,
                //};


                Entities.Actors.Add(x);
                logManager.Info(JsonUtility.ToJson(x));
            };
        }

        public ActorStats GetActorStats(string name)
        {
            return Entities.Actors.Where(x => x.Name == name).FirstOrDefault().stats;
        }


        public ThumbnailSize GetThumbnailSize(string name)
        {
            return Entities.Actors.Where(x => x.Name == name).FirstOrDefault().thumbnail;
        }

    }
}