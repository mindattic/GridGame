
using SQLiteDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Game.Manager
{
    public class DatabaseManager : ExtendedMonoBehavior
    {
        private const string DATABASE_NAME = "MyDatabase.db";
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
            logManager.error(err);
        }

        void OnApplicationQuit()
        {
            instance.Dispose();
        }

        private void Awake()
        {
            instance.DBLocation = Application.persistentDataPath;
            instance.DBName = DATABASE_NAME;

            if (!instance.Exists)
                instance.CreateDatabase(instance.DBName, isOverWrite: true);

            var isConnected = instance.ConnectToDefaultDatabase(instance.DBName, loadFresh: true);

            if (!isConnected)
                throw new UnityException($"Failed to connect to database: {instance.DBName}");

            Load();
        }

        void Load()
        {
            DBReader reader;

            //TODO: Should only load enemy types that are in current level...
            #region Load ActorStats

            actorStats.Clear();
            reader = instance.GetAllData("ActorStats");
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
                logManager.info(JsonUtility.ToJson(x));

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