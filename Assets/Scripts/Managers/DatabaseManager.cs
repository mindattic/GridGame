
using Game.Behaviors;
using SQLiteDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Game.Manager
{
  

    public static class DatabaseSchema
    {
        public const string Name = "MyDatabase.db";
        
        public static class Table
        {

            public static string Actor = "Actor";
        }
    }

    public class DatabaseManager : MonoBehaviour
    {
        protected LogManager logManager => GameManager.instance.logManager;

        public const bool autoOverwrite = true; //Used to reinstall app every load...
        private SQLiteDB instance = SQLiteDB.Instance;

        public List<ActorStats> actorStats = new List<ActorStats>();


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

        private void Awake()
        {
            instance.DBLocation = Application.persistentDataPath;
            instance.DBName = DatabaseSchema.Name;

            //Refresh if this is the first load of the application
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

            //TODO: Should only load enemy types that are in current level...
#region Load ActorStats

            actorStats.Clear();
            reader = instance.GetAllData(DatabaseSchema.Table.Actor);
            while (reader != null && reader.Read())
            {
                var x = new ActorStats()
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
                    Luck = reader.GetFloatValue("Luck")
                };
                actorStats.Add(x);
                logManager.Info(JsonUtility.ToJson(x));

#endregion


            }
        }


        public ActorStats GetActorStats(string name)
        {
            var stats = actorStats.Where(x => x.Name == name).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            return stats;
        }


    }
}